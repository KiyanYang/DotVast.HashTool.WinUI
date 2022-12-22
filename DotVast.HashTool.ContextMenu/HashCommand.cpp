#include "pch.h"
#include "HashCommand.h"
#include <shellapi.h>

HashCommand::HashCommand(JsonObject hashOption)
{
    HashName = hashOption.GetNamedString(L"Hash", L"");
    IsEnabled = hashOption.GetNamedBoolean(L"IsEnabled", false);
}

IFACEMETHODIMP HashCommand::GetTitle(IShellItemArray* items, PWSTR* name)
{
    *name = nullptr;
    return SHStrDup(HashName.data(), name);
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

        args << "--hash";
        args << ' ' << '"' << HashName << '"';
        args << ' ' << "--path";

        IShellItem* pShellItem = nullptr;
        PWSTR pPath = nullptr;
        for (DWORD i = 0; i < count; i++)
        {
            selection->GetItemAt(i, &pShellItem);
            pShellItem->GetDisplayName(SIGDN_FILESYSPATH, &pPath);
            pShellItem->Release();
            args << ' ' << '"' << pPath << '"';
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
        MessageBox(parent, L"No selected items.", L"HashTool", MB_OK);
    }

    return S_OK;
} CATCH_RETURN();
