#include "pch.h"
#include "EnumCommand.h"
#include <wil/common.h>

EnumCommand::EnumCommand(std::vector<ComPtr<IExplorerCommand>>& commands)
{
    m_commands = commands;
    m_current = m_commands.cbegin();
}

IFACEMETHODIMP EnumCommand::Next(ULONG celt, IExplorerCommand** apUICommand, ULONG* pceltFetched)
{
    ULONG fetched{ 0 };
    wil::assign_to_opt_param(pceltFetched, 0ul);

    for (ULONG i = 0; (i < celt) && (m_current != m_commands.cend()); i++)
    {
        m_current->CopyTo(&apUICommand[0]);
        m_current++;
        fetched++;
    }

    wil::assign_to_opt_param(pceltFetched, fetched);
    return (fetched == celt) ? S_OK : S_FALSE;
}

IFACEMETHODIMP EnumCommand::Skip(ULONG celt)
{
    if (m_current + celt > m_commands.cend())
    {
        return S_FALSE;
    }

    m_current += celt;
    return S_OK;
}

IFACEMETHODIMP EnumCommand::Reset()
{
    m_current = m_commands.cbegin();
    return S_OK;
}

IFACEMETHODIMP EnumCommand::Clone(IEnumExplorerCommand** ppenum)
{
    *ppenum = nullptr;
    return E_NOTIMPL;
}
