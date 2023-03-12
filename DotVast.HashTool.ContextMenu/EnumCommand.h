#pragma once

using IExplorerCommandList = std::vector<winrt::com_ptr<IExplorerCommand>>;
using IExplorerCommandIterator = IExplorerCommandList::const_iterator;

struct EnumCommand : public winrt::implements<EnumCommand, IEnumExplorerCommand>
{
public:
    EnumCommand(IExplorerCommandList& commands);
    STDMETHODIMP Next(
        _In_ ULONG celt,
        _Out_ IExplorerCommand** pUICommand,
        _Out_opt_ ULONG* pceltFetched);
    STDMETHODIMP Skip(
        _In_ ULONG celt);
    STDMETHODIMP Reset();
    STDMETHODIMP Clone(
        _Out_ IEnumExplorerCommand** ppenum);

private:
    IExplorerCommandList m_commands;
    IExplorerCommandIterator m_current;
};
