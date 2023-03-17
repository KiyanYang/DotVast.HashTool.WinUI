using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Microsoft.Xaml.Interactivity;

using Windows.UI;

namespace DotVast.HashTool.WinUI.Behaviors;

public class TextBlockHighlightTextBehavior : Behavior<TextBlock>
{
    private long? _textPropertyToken;
    private static readonly Brush s_highlightBrush = new SolidColorBrush((Color)App.Current.Resources["SystemAccentColorLight3"]);
    private TextHighlighter? _highlighter;

    #region HighlightText

    public string HighlightText
    {
        get => (string)GetValue(HighlightTextProperty);
        set => SetValue(HighlightTextProperty, value);
    }

    public static readonly DependencyProperty HighlightTextProperty =
        DependencyProperty.Register(nameof(HighlightText), typeof(string), typeof(TextBlockHighlightTextBehavior), new PropertyMetadata(null, OnHighlightTextChanged));

    private static void OnHighlightTextChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
    {
        if (string.Equals(e.OldValue as string, e.NewValue as string))
        {
            return;
        }

        var b = d as TextBlockHighlightTextBehavior;
        b?.UpdateHighlighter();
    }

    #endregion HighlightText

    protected override void OnAttached()
    {
        base.OnAttached();

        _textPropertyToken = AssociatedObject?.RegisterPropertyChangedCallback(TextBlock.TextProperty, (sender, e) => UpdateHighlighter());

        UpdateHighlighter();
    }

    protected override void OnDetaching()
    {
        base.OnDetaching();

        if (_textPropertyToken is long token)
        {
            AssociatedObject?.UnregisterPropertyChangedCallback(TextBlock.TextProperty, token);
        }
    }

    private void UpdateHighlighter()
    {
        if (AssociatedObject is null)
        {
            return;
        }

        _highlighter ??= new() { Background = s_highlightBrush };
        _highlighter.Ranges.Clear();
        AssociatedObject.TextHighlighters.Clear();

        if (string.IsNullOrEmpty(HighlightText))
        {
            return;
        }

        var highlightTextIndex = AssociatedObject.Text.IndexOf(HighlightText, StringComparison.OrdinalIgnoreCase);
        if (highlightTextIndex == -1)
        {
            return;
        }

        _highlighter.Ranges.Add(new TextRange(highlightTextIndex, HighlightText.Length));
        AssociatedObject.TextHighlighters.Add(_highlighter);
    }
}
