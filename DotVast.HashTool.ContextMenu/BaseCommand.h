#pragma once

struct BaseCommand : public winrt::implements<BaseCommand, IExplorerCommand>
{
public:
    STDMETHODIMP EnumSubCommands(
        _Outptr_ IEnumExplorerCommand** ppEnum);
    STDMETHODIMP GetCanonicalName(
        _Out_ GUID* pguidCommandName);
    STDMETHODIMP GetFlags(
        _Out_ EXPCMDFLAGS* pFlags);
    STDMETHODIMP GetIcon(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszIcon);
    STDMETHODIMP GetState(
        _In_opt_ IShellItemArray* psiItemArray,
        _In_ BOOL fOkToBeSlow,
        _Outptr_ EXPCMDSTATE* pCmdState);
    STDMETHODIMP GetTitle(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszName);
    STDMETHODIMP GetToolTip(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszInfotip);
    STDMETHODIMP Invoke(
        _In_opt_ IShellItemArray* psiItemArray,
        _In_opt_ IBindCtx* pbc);
};
