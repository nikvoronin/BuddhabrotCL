using System.Runtime.InteropServices;

namespace BuddhabrotCL
{
    public enum FxFilter
    {
        Linear,
        Sqrt,
        Log,
        Log10,
        Exp
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2
    {
        public uint X;
        public uint Y;
    };

    [StructLayout(LayoutKind.Sequential)]
    public struct RGBA
    {
        public uint r;
        public uint g;
        public uint b;
        public uint a;
    };
}
