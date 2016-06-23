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

    public enum Tint
    {
        BGR,
        BRG,
        RGB,
        RBG,
        GRB,
        GBR
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector4
    {
        public uint x;
        public uint y;
        public uint z;
        public uint w;
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
