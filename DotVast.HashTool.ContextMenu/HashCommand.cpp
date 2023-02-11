#include "pch.h"
#include "HashCommand.h"
#include <shellapi.h>

HashCommand::HashCommand(JsonObject hashOption)
{
    HashName = hashOption.GetNamedString(L"Hash", L"");
    IsEnabled = hashOption.GetNamedBoolean(L"IsEnabled", false);
}

HashCommand::HashCommand(winrt::hstring hashName, bool isEnabled)
{
    HashName = hashName;
    IsEnabled = isEnabled;
}

IFACEMETHODIMP HashCommand::GetTitle(IShellItemArray* items, PWSTR* name)
{
    *name = nullptr;
    return SHStrDup(HashName.c_str(), name);
}

IFACEMETHODIMP HashCommand::Invoke(IShellItemArray* selection, IBindCtx*) noexcept try
{
    HWND parent = nullptr;
    if (m_site)
    {
        ComPtr<IOleWindow> oleWindow;
        RETURN_IF_FAILED(m_site.As(&oleWindow));
        RETURN_IF_FAILED(oleWindow->GetWindow(&parent));
    }

    if (selection)
    {
        DWORD count;
        RETURN_IF_FAILED(selection->GetCount(&count));

        std::wostringstream args;

        args << L"--hash";
        args << L" \"" << HashName << L'"';
        args << L" --path";

        for (DWORD i = 0; i < count; i++)
        {
            IShellItem* shellItem;
            LPWSTR path;
            selection->GetItemAt(i, &shellItem);
            shellItem->GetDisplayName(SIGDN_FILESYSPATH, &path);
            shellItem->Release();
            args << L" \"" << path << L" \""; // 在路径后添加空格, 防止出现 `"C:\"`, 使得后一个引号被转义
        }

#ifdef _DEBUG
        auto appPath = L"shell:AppsFolder\\DotVast.HashTool.WinUI.Dev_5xsw0t1dxcp4g!App";
#else
        auto appPath = L"shell:AppsFolder\\DotVast.HashTool.WinUI_5xsw0t1dxcp4g!App";
#endif // _DEBUG

        ShellExecute(NULL, L"open", appPath, args.str().c_str(), NULL, 0);
    }
    else
    {
        MessageBox(parent, L"No selected items.", L"DotVast.HashTool.WinUI", MB_OK);
    }

    return S_OK;
} CATCH_RETURN();
