#include "pch.h"
#include "EnumCommand.h"

EnumCommand::EnumCommand(IExplorerCommandList& commands)
{
    m_commands = commands;
    m_current = m_commands.cbegin();
}

STDMETHODIMP EnumCommand::Next(
    _In_ ULONG celt,
    _Out_ IExplorerCommand** pUICommand,
    _Out_opt_ ULONG* pceltFetched)
{
    ULONG fetched = 0;

    for (ULONG i = 0; (i < celt) && (m_current != m_commands.cend()); ++i)
    {
        m_current->copy_to(&pUICommand[0]);
        ++m_current;
        ++fetched;
    }

    if (pceltFetched)
    {
        *pceltFetched = fetched;
    }

    return (fetched == celt) ? S_OK : S_FALSE;
}

STDMETHODIMP EnumCommand::Skip(
    _In_ ULONG celt)
{
    if (m_current + celt > m_commands.cend())
    {
        return S_FALSE;
    }

    m_current += celt;
    return S_OK;
}

STDMETHODIMP EnumCommand::Reset()
{
    m_current = m_commands.cbegin();
    return S_OK;
}

STDMETHODIMP EnumCommand::Clone(
    _Out_ IEnumExplorerCommand** ppenum)
{
    *ppenum = nullptr;
    return E_NOTIMPL;
}
