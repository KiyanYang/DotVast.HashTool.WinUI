// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using DotVast.HashTool.WinUI.Models;

namespace DotVast.HashTool.WinUI.Contracts.Services;

public interface IComputeHashService
{
    /// <summary>
    /// Computes the hash value asynchronously.
    /// </summary>
    /// <param name="hashTask">The hash task with input, mode, algorithm and other info (like progress, state).</param>
    /// <param name="mres">The <see cref="ManualResetEventSlim"/> instance that controls the pause and resume of the computation.</param>
    /// <param name="cancellationToken">The <see cref="CancellationToken"/> instance that can be used to cancel the computation.</param>
    /// <returns>A task that represents the asynchronous computation.</returns>
    Task ComputeHashAsync(HashTask hashTask, ManualResetEventSlim mres, CancellationToken cancellationToken);
}
