// dllmain.cpp : 定义 DLL 应用程序的入口点。
#include "pch.h"
#include <wrl/module.h>
#include <wrl/implements.h>
#include <wrl/client.h>
#include <shobjidl_core.h>
#include <wil/resource.h>
#include <string>
#include <vector>
#include <sstream>
#include <wil/filesystem.h>

#include <winrt/base.h>
#include <winrt/Windows.Storage.h>
#include <winrt/Windows.Data.Json.h>
#include <winrt/Windows.Foundation.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.ApplicationModel.h>
#include <filesystem>
#include <shlwapi.h>

#include "BaseCommand.h"
#include "EnumCommand.h"
#include "HashCommand.h"

using namespace Microsoft::WRL;

using namespace winrt::Windows::ApplicationModel;
using namespace winrt::Windows::Data::Json;

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD  ul_reason_for_call,
    LPVOID lpReserved
)
{
    switch (ul_reason_for_call)
    {
    case DLL_PROCESS_ATTACH:
    case DLL_THREAD_ATTACH:
    case DLL_THREAD_DETACH:
    case DLL_PROCESS_DETACH:
        break;
    }
    return TRUE;
}

#ifdef _DEBUG
class __declspec(uuid("C8085C38-E65F-4DA6-BBBA-A47246499B6D"))
#else
class __declspec(uuid("412FE3A3-833F-4EDE-BE03-D2F510B1AE59"))
#endif
    HashToolContextMenuCommand final : public BaseCommand
{
public:
    HashToolContextMenuCommand()
    {
        m_title = winrt::Windows::ApplicationModel::AppInfo::Current().DisplayInfo().DisplayName();
        auto a = winrt::Windows::Storage::ApplicationData::Current().LocalSettings().Values().Lookup(L"HashOptions");
        JsonArray result;
        if (JsonArray::TryParse(winrt::unbox_value<winrt::hstring>(a), result))
        {
            for (const auto& obj : result)
            {
                auto command = Make<HashCommand>(obj.GetObjectW());
                if (command.Get()->IsEnabled)
                {
                    m_commands.push_back(command);
                }
            }
        }
    }

    IFACEMETHODIMP GetTitle(_In_opt_ IShellItemArray* items, _Outptr_result_nullonfailure_ PWSTR* name) override
    {
        *name = nullptr;
        return SHStrDup(m_title.data(), name);
    }

    IFACEMETHODIMP GetIcon(_In_opt_ IShellItemArray*, _Outptr_result_nullonfailure_ PWSTR* icon) override
    {
        *icon = nullptr;
        std::wstring iconResourcePath = winrt::Windows::ApplicationModel::Package::Current().EffectiveLocation().Path().c_str();
        iconResourcePath += L"\\Assets\\Icon.ico";
        return SHStrDup(iconResourcePath.c_str(), icon);
    }

    IFACEMETHODIMP GetFlags(_Out_ EXPCMDFLAGS* flags) override
    {
        *flags = ECF_HASSUBCOMMANDS;
        return S_OK;
    }

    IFACEMETHODIMP EnumSubCommands(_COM_Outptr_ IEnumExplorerCommand** enumCommands) override
    {
        *enumCommands = nullptr;
        auto e = Make<EnumCommand>(m_commands);
        return e->QueryInterface(IID_PPV_ARGS(enumCommands));
    }

private:
    winrt::hstring m_title;
    std::vector<ComPtr<IExplorerCommand>> m_commands;
};


CoCreatableClass(HashToolContextMenuCommand)
CoCreatableClassWrlCreatorMapInclude(HashToolContextMenuCommand)

STDAPI DllGetActivationFactory(_In_ HSTRING activatableClassId, _COM_Outptr_ IActivationFactory** factory)
{
    return Module<ModuleType::InProc>::GetModule().GetActivationFactory(activatableClassId, factory);
}

STDAPI DllCanUnloadNow()
{
    return Module<InProc>::GetModule().GetObjectCount() == 0 ? S_OK : S_FALSE;
}

STDAPI DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _COM_Outptr_ void** instance)
{
    return Module<InProc>::GetModule().GetClassObject(rclsid, riid, instance);
}
