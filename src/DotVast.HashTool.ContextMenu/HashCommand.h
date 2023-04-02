#pragma once
#include <winrt/Windows.Data.Json.h>
#include "BaseCommand.h"

using namespace winrt::Windows::Data::Json;

struct HashCommand : public BaseCommand
{
public:
    HashCommand(winrt::hstring hashName);
    STDMETHODIMP GetTitle(
        _In_opt_ IShellItemArray* psiItemArray,
        _Outptr_ LPWSTR* ppszName) override;
    STDMETHODIMP Invoke(
        _In_opt_ IShellItemArray* psiItemArray,
        _In_opt_ IBindCtx* pbc) override;

private:
    winrt::hstring m_hashName;
};
