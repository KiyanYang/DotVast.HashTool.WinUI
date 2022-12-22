#pragma once
#include "BaseCommand.h"
#include <winrt/Windows.Data.Json.h>

using namespace winrt::Windows::Data::Json;

class HashCommand final : public BaseCommand
{
public:
    HashCommand(JsonObject hashOption);
    IFACEMETHODIMP GetTitle(_In_opt_ IShellItemArray* items, _Outptr_result_nullonfailure_ PWSTR* name) override;
    IFACEMETHODIMP Invoke(_In_opt_ IShellItemArray* selection, _In_opt_ IBindCtx*) noexcept override;

public:
    winrt::hstring HashName;
    bool IsEnabled;
};
