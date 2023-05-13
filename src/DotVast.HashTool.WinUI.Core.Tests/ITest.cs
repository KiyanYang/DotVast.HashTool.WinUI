// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

namespace DotVast.HashTool.WinUI.Core.Tests;

public interface ITest<T> where T : ITest<T>
{
    static IEnumerable<object[]> TestData() => T.TestDataCore().Select(T.ConvertDataItem);

    static abstract IEnumerable<object[]> TestDataCore();
    static abstract object[] ConvertDataItem(object[] item);
}
