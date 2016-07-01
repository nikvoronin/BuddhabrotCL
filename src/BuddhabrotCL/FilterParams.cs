using System.ComponentModel;

namespace BuddhabrotCL
{
    public class FilterParams
    {
        [Category("Filter")]
        public float Factor { get; set; } = 1.0f;
        [Category("Filter")]
        public float Exposure { get; set; } = 1.0f;
        [Category("Filter")]
        public FxFilter Type { get; set; } = FxFilter.Sqrt;
        [Category("Filter")]
        public Tint Tint { get; set; } = Tint.RGB;
    }
}
