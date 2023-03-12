#include "pch.h"

#include <winrt/Windows.Storage.h>
#include <winrt/Windows.Foundation.Collections.h>
#include <winrt/Windows.ApplicationModel.h>

#include "BaseCommand.h"
#include "EnumCommand.h"
#include "HashCommand.h"

BOOL APIENTRY DllMain(HMODULE hModule,
    DWORD ul_reason_for_call,
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

struct ContextMenuCommand : public BaseCommand
{
public:
    const bool DefaultContextMenusEnabled = true;
    const winrt::hstring ContextMenusEnabledKey = L"FileExplorerContextMenusEnabled";
    const winrt::hstring HashOptionsKey = L"HashOptions";

    ContextMenuCommand()
    {
        m_title = winrt::Windows::ApplicationModel::AppInfo::Current().DisplayInfo().DisplayName();

        auto values = winrt::Windows::Storage::ApplicationData::Current().LocalSettings().Values();

        m_enabled = DefaultContextMenusEnabled;
        if (values.HasKey(ContextMenusEnabledKey))
        {
            auto contextMenusEnabled = winrt::unbox_value<winrt::hstring>(values.Lookup(ContextMenusEnabledKey));
            JsonValue enabled = JsonValue::CreateNullValue();
            if (JsonValue::TryParse(contextMenusEnabled, enabled) && enabled.ValueType() == JsonValueType::Boolean)
            {
                m_enabled = enabled.GetBoolean();
            }
        }

        if (values.HasKey(HashOptionsKey))
        {
            auto hashOptions = winrt::unbox_value<winrt::hstring>(values.Lookup(HashOptionsKey));
            JsonArray result;
            if (JsonArray::TryParse(hashOptions, result))
            {
                for (const auto& obj : result)
                {
                    auto command = winrt::make_self<HashCommand>(obj.GetObjectW());
                    if (command.get()->IsEnabled)
                    {
                        m_commands.push_back(command);
                    }
                }
            }
        }

        if (m_commands.size() == 0)
        {
            const winrt::hstring DefaultHashNames[] = { L"MD5", L"SHA1", L"SHA256", L"SHA384" , L"SHA512" };
            for (const auto& hashName : DefaultHashNames)
            {
                auto command = winrt::make_self<HashCommand>(hashName, true);
                m_commands.push_back(command);
            }
        }
    }

    STDMETHODIMP GetTitle(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszName) override
    {
        *ppszName = nullptr;
        return SHStrDup(m_title.c_str(), ppszName);
    }

    STDMETHODIMP GetIcon(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszIcon) override
    {
        *ppszIcon = nullptr;
        auto packagePath = winrt::Windows::ApplicationModel::Package::Current().EffectivePath();
        auto iconResourcePath = packagePath + L"\\Assets\\Icon.ico";
        return SHStrDup(iconResourcePath.c_str(), ppszIcon);
    }

    STDMETHODIMP GetState(
        _In_opt_ IShellItemArray* psiItemArray,
        _In_ BOOL fOkToBeSlow,
        _Outptr_ EXPCMDSTATE* pCmdState) override
    {
        *pCmdState = m_enabled ? ECS_ENABLED : ECS_HIDDEN;
        return S_OK;
    }

    STDMETHODIMP GetFlags(
        _Out_ EXPCMDFLAGS* pFlags) override
    {
        *pFlags = ECF_HASSUBCOMMANDS;
        return S_OK;
    }

    STDMETHODIMP EnumSubCommands(
        _Outptr_ IEnumExplorerCommand** ppEnum) override
    {
        *ppEnum = nullptr;
        auto e = winrt::make<EnumCommand>(m_commands);
        return e->QueryInterface(IID_PPV_ARGS(ppEnum));
    }

private:
    bool m_enabled;
    winrt::hstring m_title;
    IExplorerCommandList m_commands;
};

#ifdef _DEBUG
struct DECLSPEC_UUID("C8085C38-E65F-4DA6-BBBA-A47246499B6D")
#else
struct DECLSPEC_UUID("412FE3A3-833F-4EDE-BE03-D2F510B1AE59")
#endif // _DEBUG
ContextMenuCommandFactory: winrt::implements<ContextMenuCommandFactory, IClassFactory>
{
    STDMETHODIMP CreateInstance(
        _In_opt_ IUnknown * pUnkOuter,
        _In_ REFIID riid,
        _COM_Outptr_ void** ppvObject) noexcept override
    {
        try
        {
            return winrt::make<ContextMenuCommand>()->QueryInterface(riid, ppvObject);
        }
        catch (...)
        {
            return winrt::to_hresult();
        }
    }

    STDMETHODIMP LockServer(
        _In_ BOOL fLock) noexcept override
    {
       if (fLock)
       {
           ++winrt::get_module_lock();
       }
       else
       {
           --winrt::get_module_lock();
       }
        return S_OK;
    }
};

STDAPI DllCanUnloadNow(void)
{
    if (winrt::get_module_lock())
    {
        return S_FALSE;
    }

    winrt::clear_factory_cache();
    return S_OK;
}

STDAPI DllGetClassObject(_In_ REFCLSID rclsid, _In_ REFIID riid, _Outptr_ LPVOID FAR* ppv)
{
    try
    {
        *ppv = nullptr;

        if (rclsid == __uuidof(ContextMenuCommandFactory))
        {
            return winrt::make<ContextMenuCommandFactory>()->QueryInterface(riid, ppv);
        }

        return winrt::hresult_class_not_available().to_abi();
    }
    catch (...)
    {
        return winrt::to_hresult();
    }
}
