using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.Xaml.Interactivity;

namespace DotVast.HashTool.WinUI.Behaviors;

internal sealed class ControlIsEnabledToVisibilityBehavior : Behavior<Control>
{
    private long? _isEnabledToken;

    protected override void OnAttached()
    {
        base.OnAttached();

        _isEnabledToken = AssociatedObject.RegisterPropertyChangedCallback(Control.IsEnabledProperty, OnIsEnabledChanged);

        UpdateVisibility(AssociatedObject);
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (_isEnabledToken is long token)
        {
            AssociatedObject.UnregisterPropertyChangedCallback(Control.IsEnabledProperty, token);
        }
    }

    private void OnIsEnabledChanged(DependencyObject sender, DependencyProperty dp)
    {
        UpdateVisibility(sender as Control);
    }

    private static void UpdateVisibility(Control? control)
    {
        if (control is null)
        {
            return;
        }

        control.Visibility = control.IsEnabled ? Visibility.Visible : Visibility.Collapsed;
    }
}
