using System.Runtime.InteropServices;
using System.Text;

namespace DotVast.HashTool.WinUI.Helpers;

public sealed class RuntimeHelper
{
    [DllImport("kernel32.dll", CharSet = CharSet.Unicode, SetLastError = true)]
    private static extern int GetCurrentPackageFullName(ref int packageFullNameLength, StringBuilder? packageFullName);

    static RuntimeHelper()
    {
        var length = 0;

        IsMSIX = GetCurrentPackageFullName(ref length, null) != 15700L;
    }

    public static bool IsMSIX { get; }
}
