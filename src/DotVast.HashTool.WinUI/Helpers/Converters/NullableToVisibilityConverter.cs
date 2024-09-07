// Copyright (c) Kiyan Yang.
// Licensed under the MIT License.

using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace DotVast.HashTool.WinUI.Helpers.Converters;

internal sealed partial class NullableToVisibilityConverter : IValueConverter
{
    public object Convert(object value, Type targetType, object parameter, string language)
    {
        return value is null ? Visibility.Collapsed : Visibility.Visible;
    }

    public object ConvertBack(object value, Type targetType, object parameter, string language) => throw new NotImplementedException();
}
