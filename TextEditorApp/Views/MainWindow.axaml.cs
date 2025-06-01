using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using System;
using System.IO;
using System.Text.RegularExpressions;

namespace TextEditorApp;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OpenFile(object? sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters =
            {
                new FileDialogFilter { Name = "RTF Files", Extensions = { "rtf" } },
                new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } }
            }
        };

        var result = await dlg.ShowAsync(this);
        if (result?.Length > 0)
        {
            var text = await File.ReadAllTextAsync(result[0]);
            Editor.Text = text;
        }
    }

    private async void SaveFile(object? sender, RoutedEventArgs e)
    {
        var dlg = new SaveFileDialog
        {
            Filters =
            {
                new FileDialogFilter { Name = "Text Files", Extensions = { "txt" } }
            }
        };

        var result = await dlg.ShowAsync(this);
        if (!string.IsNullOrWhiteSpace(result))
        {
            await File.WriteAllTextAsync(result, Editor.Text);
        }
    }

    private async void SaveRTF(object? sender, RoutedEventArgs e)
    {
        var dlg = new SaveFileDialog
        {
            Filters =
            {
                new FileDialogFilter { Name = "RTF Files", Extensions = { "rtf" } }
            }
        };

        var result = await dlg.ShowAsync(this);
        if (!string.IsNullOrWhiteSpace(result))
        {
            string rtf = "{\\rtf1\\ansi\\deff0 " + Editor.Text.Replace("\n", "\\par ") + " }";
            await File.WriteAllTextAsync(result, rtf);
        }
    }

    private void AlignLeft(object? sender, RoutedEventArgs e) =>
        Editor.TextAlignment = TextAlignment.Left;

    private void AlignCenter(object? sender, RoutedEventArgs e) =>
        Editor.TextAlignment = TextAlignment.Center;

    private void AlignRight(object? sender, RoutedEventArgs e) =>
        Editor.TextAlignment = TextAlignment.Right;

    private async void InsertImage(object? sender, RoutedEventArgs e)
    {
        var dlg = new OpenFileDialog
        {
            AllowMultiple = false,
            Filters =
            {
                new FileDialogFilter { Name = "Images", Extensions = { "png", "jpg", "jpeg" } }
            }
        };

        var result = await dlg.ShowAsync(this);
        if (result?.Length > 0)
        {
            var imageText = $"[Image: {result[0]}]";
            var caret = Editor.CaretIndex;
            Editor.Text = Editor.Text.Insert(caret, imageText);
        }
    }

    private void HighlightSyntax(object? sender, RoutedEventArgs e)
    {
        var text = Editor.Text;
        var highlighted = Regex.Replace(text, @"\b(int|return|if|else|printf|#include)\b",
            m => $"**{m.Value}**");
        Editor.Text = highlighted;
    }

    private void ChangeFont(object? sender, RoutedEventArgs e)
    {
        Editor.FontFamily = new FontFamily("Courier New");
        Editor.FontSize = 16;
    }

    private void SetEnglish(object? sender, RoutedEventArgs e)
    {
        this.Title = "Text Editor";
        // TODO: Update menu headers to English
    }

    private void SetUkrainian(object? sender, RoutedEventArgs e)
    {
        this.Title = "Текстовий редактор";
        // TODO: Update menu headers to Ukrainian
    }
}
