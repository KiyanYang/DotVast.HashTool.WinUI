#pragma once
#include <winrt/Windows.Data.Json.h>
#include "BaseCommand.h"

using namespace winrt::Windows::Data::Json;

struct HashCommand : public BaseCommand
{
public:
    HashCommand(JsonObject hashOption);
    HashCommand(winrt::hstring hashName, bool isEnabled);
    STDMETHODIMP GetTitle(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszName) override;
    STDMETHODIMP Invoke(
        _In_opt_ IShellItemArray* psiItemArray,
        _In_opt_ IBindCtx* pbc) override;

    winrt::hstring HashName;
    bool IsEnabled;
};
