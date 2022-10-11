namespace DotVast.HashTool.WinUI.Models;

public sealed class LogsOptions
{
    public string? FilePath { get; set; }

    public string FullPath =>
        Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                     FilePath ?? "Logs/app-.log");
}
