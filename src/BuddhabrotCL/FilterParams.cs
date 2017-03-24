using ProtoBuf;
using System.ComponentModel;

namespace BuddhabrotCL
{
    [ProtoContract]
    public class FilterParams
    {
        [Category("Filter")]
        [ProtoMember(1)]
        public float Factor { get; set; } = 1.0f;
        [Category("Filter")]
        [ProtoMember(2)]
        public float Exposure { get; set; } = 1.0f;
        [Category("Filter")]
        [ProtoMember(3)]
        public FxFilter Type { get; set; } = FxFilter.Sqrt;
        [Category("Filter")]
        [ProtoMember(4)]
        public Tint Tint { get; set; } = Tint.RGB;
        [Category("Filter")]
        [ProtoMember(5)]
        public bool Grayscale { get; set; } = false;
    }
}
