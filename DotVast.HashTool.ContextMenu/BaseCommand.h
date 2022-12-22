﻿#pragma once
#include <wrl/implements.h>
#include <wil/nt_result_macros.h>
#include <wil/resource.h>
#include <wil/stl.h>
#include <shobjidl_core.h>
#include <shlwapi.h>
#include <sstream>

using namespace Microsoft::WRL;

class BaseCommand : public RuntimeClass<RuntimeClassFlags<ClassicCom>, IExplorerCommand, IObjectWithSite>
{
public:
    // IExplorerCommand
    IFACEMETHODIMP GetTitle(_In_opt_ IShellItemArray* items, _Outptr_result_nullonfailure_ PWSTR* name);
    IFACEMETHODIMP GetIcon(_In_opt_ IShellItemArray*, _Outptr_result_nullonfailure_ PWSTR* icon);
    IFACEMETHODIMP GetToolTip(_In_opt_ IShellItemArray*, _Outptr_result_nullonfailure_ PWSTR* infoTip);
    IFACEMETHODIMP GetCanonicalName(_Out_ GUID* guidCommandName);
    IFACEMETHODIMP GetState(_In_opt_ IShellItemArray* selection, _In_ BOOL okToBeSlow, _Out_ EXPCMDSTATE* cmdState);
    IFACEMETHODIMP Invoke(_In_opt_ IShellItemArray* selection, _In_opt_ IBindCtx*) noexcept;
    IFACEMETHODIMP GetFlags(_Out_ EXPCMDFLAGS* flags);
    IFACEMETHODIMP EnumSubCommands(_COM_Outptr_ IEnumExplorerCommand** enumCommands);

    // IObjectWithSite
    IFACEMETHODIMP SetSite(_In_ IUnknown* site) noexcept;
    IFACEMETHODIMP GetSite(_In_ REFIID riid, _COM_Outptr_ void** site) noexcept;

protected:
    ComPtr<IUnknown> m_site;
};
