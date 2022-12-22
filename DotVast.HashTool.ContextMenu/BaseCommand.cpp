#include "pch.h"
#include "BaseCommand.h"

IFACEMETHODIMP BaseCommand::GetTitle(_In_opt_ IShellItemArray* items, _Outptr_result_nullonfailure_ PWSTR* name)
{
    *name = nullptr;
    return E_NOTIMPL;
}

IFACEMETHODIMP BaseCommand::GetIcon(_In_opt_ IShellItemArray* items, _Outptr_result_nullonfailure_ PWSTR* icon)
{
    *icon = nullptr;
    return E_NOTIMPL;
}

IFACEMETHODIMP BaseCommand::GetToolTip(_In_opt_ IShellItemArray*, _Outptr_result_nullonfailure_ PWSTR* infoTip)
{
    *infoTip = nullptr;
    return E_NOTIMPL;
}

IFACEMETHODIMP BaseCommand::GetCanonicalName(_Out_ GUID* guidCommandName)
{
    *guidCommandName = GUID_NULL;
    return S_OK;
}

IFACEMETHODIMP BaseCommand::GetState(_In_opt_ IShellItemArray* selection, _In_ BOOL okToBeSlow, _Out_ EXPCMDSTATE* cmdState)
{
    *cmdState = ECS_ENABLED;
    return S_OK;
}

IFACEMETHODIMP BaseCommand::Invoke(_In_opt_ IShellItemArray* selection, _In_opt_ IBindCtx*) noexcept try
{
    return S_OK;
} CATCH_RETURN();

IFACEMETHODIMP BaseCommand::GetFlags(_Out_ EXPCMDFLAGS* flags)
{
    *flags = ECF_DEFAULT;
    return S_OK;
}

IFACEMETHODIMP BaseCommand::EnumSubCommands(IEnumExplorerCommand** enumCommands)
{
    *enumCommands = nullptr;
    return E_NOTIMPL;
}

IFACEMETHODIMP BaseCommand::SetSite(_In_ IUnknown* site) noexcept
{
    m_site = site;
    return S_OK;
}

IFACEMETHODIMP BaseCommand::GetSite(_In_ REFIID riid, _COM_Outptr_ void** site) noexcept
{
    RETURN_IF_FAILED(m_site.CopyTo(riid, site));
    return S_OK;
}
