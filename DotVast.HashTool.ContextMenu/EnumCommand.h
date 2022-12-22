#pragma once
#include <wrl/implements.h>
#include <shobjidl_core.h>
#include <vector>

using namespace Microsoft::WRL;

class EnumCommand : public RuntimeClass<RuntimeClassFlags<ClassicCom>, IEnumExplorerCommand>
{
public:
    EnumCommand(std::vector<ComPtr<IExplorerCommand>>& commands);
    IFACEMETHODIMP Next(ULONG celt, __out_ecount_part(celt, *pceltFetched) IExplorerCommand** apUICommand, __out_opt ULONG* pceltFetched);
    IFACEMETHODIMP Skip(ULONG celt);
    IFACEMETHODIMP Reset();
    IFACEMETHODIMP Clone(__deref_out IEnumExplorerCommand** ppenum);

private:
    std::vector<ComPtr<IExplorerCommand>> m_commands;
    std::vector<ComPtr<IExplorerCommand>>::const_iterator m_current;
};
