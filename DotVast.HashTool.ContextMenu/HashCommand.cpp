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

STDMETHODIMP HashCommand::GetTitle(
    _In_opt_ IShellItemArray* psiItemArray,
    _Outptr_ LPWSTR* ppszName)
{
    *ppszName = nullptr;
    return SHStrDup(HashName.c_str(), ppszName);
}

STDMETHODIMP HashCommand::Invoke(
    _In_opt_ IShellItemArray* psiItemArray,
    _In_opt_ IBindCtx* pbc)
{
    HWND parent = nullptr;

    if (psiItemArray)
    {
        DWORD count;
        if (FAILED(psiItemArray->GetCount(&count)))
        {
            return S_FALSE;
        }

        std::wostringstream args;

        args << L"--hash";
        args << L" \"" << HashName << L'"';
        args << L" --path";

        for (DWORD i = 0; i < count; ++i)
        {
            winrt::com_ptr<IShellItem> shellItem;
            LPWSTR displayName;
            psiItemArray->GetItemAt(i, shellItem.put());
            shellItem->GetDisplayName(SIGDN_FILESYSPATH, &displayName);
            args << L" \"" << displayName << L" \""; // 在路径后添加空格, 防止出现 `"C:\"`, 使得后一个引号被转义
            ::CoTaskMemFree(displayName);
        }

#ifdef _DEBUG
        const auto AppPath = L"shell:AppsFolder\\DotVast.HashTool.WinUI.Dev_5xsw0t1dxcp4g!App";
#else
        const auto AppPath = L"shell:AppsFolder\\DotVast.HashTool.WinUI_5xsw0t1dxcp4g!App";
#endif // _DEBUG

        ShellExecute(NULL, L"open", AppPath, args.str().c_str(), NULL, 0);
    }

    return S_OK;
}
