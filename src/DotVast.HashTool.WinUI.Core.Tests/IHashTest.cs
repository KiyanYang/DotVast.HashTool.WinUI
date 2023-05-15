// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using System.Security.Cryptography;

namespace DotVast.HashTool.WinUI.Core.Tests;

public interface IHashTest<T> : ITest<T> where T : IHashTest<T>
{
    static abstract HashAlgorithm Create();
}
