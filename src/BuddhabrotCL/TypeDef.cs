using ProtoBuf;
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace BuddhabrotCL
{
    public enum AppStatus
    {
        Loading,
        Ready,
        Rendering,
        Polishing
    }

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
        RGB,
        GRB,
        BGR,
        GBR,
        BRG,
        RBG
    }

    [StructLayout(LayoutKind.Sequential)]
    [ProtoContract]
    public struct Vector4
    {
        [ProtoMember(1)]
        public uint x;
        [ProtoMember(2)]
        public uint y;
        [ProtoMember(3)]
        public uint z;
        [ProtoMember(4)]
        public uint w;
    };

    [TypeConverter(typeof(ExpandableObjectConverter))]
    public class Vector2F
    {
        [DisplayName("X Re")]
        public float x { get; set; }
        [DisplayName("Y Im")]
        public float y { get; set; }

        [Browsable(false)]
        public Vector2f Value
        {
            get
            {
                return new Vector2f { x = x, y = y };
            }
        }

        public override string ToString()
        {
            return $"{x}; {y}";
        }
    }

    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2f
    {
        public float x;
        public float y;
    };

    [ProtoContract]
    public class ParameterBox
    {
        [ProtoMember(1)]
        public RenderParams rp = new RenderParams();
        [ProtoMember(2)]
        public FilterParams fp = new FilterParams();
        [ProtoMember(3)]
        public Vector4[] h_resultBuf;

        public bool ShouldRestore = false;
    }
}
