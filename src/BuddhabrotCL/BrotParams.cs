using System;
using System.ComponentModel;

namespace BuddhabrotCL
{
    public class BrotParams
    {
        [Category("Fractal")]
        public bool IsGrayscale { get; set; } = false;
        public bool isGrayscale;

        [Category("Filter")]
        public float Factor { get; set; } = 1.0f;
        [Category("Filter")]
        public float Overexposure { get; set; } = 1.0f;
        [Category("Filter")]
        public FxFilter Filter { get; set; } = FxFilter.Sqrt;
        [Category("Filter")]
        public float Limit_Red { get; set; } = .33f;
        [Category("Filter")]
        public float Limit_Green { get; set; } = .66f;

        [Category("View")]
        [Description(@"ReMin; ReMax; ImMin; ImMax

Try this:
Deep region 1: -1.05; -0.9; -0.3; -0.225;
Deep region 2: -1.22; -1.0; 0.16; 0.32;
Smallbrot: -1.8; -1.73; -0.028; 0.028;
Fullbrot: -2.0; 2.0; -2.0; 2.0;")]
        public string Region
        {
            get
            {
                return $"{ReMin}; {ReMax}; {ImMin}; {ImMax}";
            }

            set
            {
                string[] parts = value.Split(';');
                if (parts.Length == 4)
                {
                    if (float.TryParse(parts[0], out reMin))
                        ReMin = reMin;
                    if (float.TryParse(parts[1], out reMax))
                        ReMax = reMax;
                    if (float.TryParse(parts[2], out imMin))
                        ImMin = imMin;
                    if (float.TryParse(parts[3], out imMax))
                        ImMax = imMax;
                }
            }
        }
        [Category("View")]
        public float ReMin { get; set; } = -2f;
        [Category("View")]
        public float ReMax { get; set; } =  2f;
        [Category("View")]
        public float ImMin { get; set; } = -2f;
        [Category("View")]
        public float ImMax { get; set; } =  2f;
        [Category("View")]
        public bool IsUpdate { get; set; } = true;

        public float reMin;
        public float reMax;
        public float imMin;
        public float imMax;

        [Category("Fractal")]
        public float EscapeOrbit { get; set; } = 4f;
        public float escapeOrbit;
        [Category("Fractal")]
        public bool IsHackMode { get; set; }

        [Category("Fractal")]
        public int IterationsMin { get; set; } = 20;
        [Category("Fractal")]
        public int IterationsMax { get; set; } = 2000;
        public int iterMin;
        public int iterMax;



        [Category("Image")]
        public uint Size
        {
            get {
                return Math.Max(Width, Height);
            }

            set {
                Width = Height = value;
            }
        }
        [Category("Image")]
        public uint Width { get; set; } = 1000;
        [Category("Image")]
        public uint Height { get; set; } = 1000;
        public int width;
        public int height;

        public RGBA minColor;
        public RGBA maxColor;

        public void RecalculateColors()
        {
            minColor.r = (uint)(iterMin);
            maxColor.r = (uint)(iterMin + (iterMax - iterMin) * Limit_Red);

            minColor.g = maxColor.r;
            maxColor.g = (uint)(iterMin + (iterMax - iterMin) * Limit_Green);

            minColor.b = maxColor.g;
            maxColor.b = (uint)(iterMax);
        }
    }
}
