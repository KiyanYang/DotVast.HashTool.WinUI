#include "pch.h"
#include "BaseCommand.h"

STDMETHODIMP BaseCommand::EnumSubCommands(
    _Outptr_ IEnumExplorerCommand** ppEnum)
{
    *ppEnum = nullptr;
    return E_NOTIMPL;
}

STDMETHODIMP BaseCommand::GetCanonicalName(
    _Out_ GUID* pguidCommandName)
{
    *pguidCommandName = GUID_NULL;
    return S_OK;
}

STDMETHODIMP BaseCommand::GetFlags(
    _Out_ EXPCMDFLAGS* pFlags)
{
    *pFlags = ECF_DEFAULT;
    return S_OK;
}

STDMETHODIMP BaseCommand::GetIcon(
    _In_opt_ IShellItemArray* psiItemArray,
    _Outptr_ LPWSTR* ppszIcon)
{
    *ppszIcon = nullptr;
    return E_NOTIMPL;
}

STDMETHODIMP BaseCommand::GetState(
    _In_opt_ IShellItemArray* psiItemArray,
    _In_ BOOL fOkToBeSlow,
    _Outptr_ EXPCMDSTATE* pCmdState)
{
    *pCmdState = ECS_ENABLED;
    return S_OK;
}

STDMETHODIMP BaseCommand::GetTitle(
    _In_opt_ IShellItemArray* psiItemArray,
    _Outptr_ LPWSTR* ppszName)
{
    *ppszName = nullptr;
    return E_NOTIMPL;
}

STDMETHODIMP BaseCommand::GetToolTip(
    _In_opt_ IShellItemArray* psiItemArray,
    _Outptr_ LPWSTR* ppszInfotip)
{
    *ppszInfotip = nullptr;
    return E_NOTIMPL;
}

STDMETHODIMP BaseCommand::Invoke(
    _In_opt_ IShellItemArray* psiItemArray,
    _In_opt_ IBindCtx* pbc)
{
    return S_OK;
}
