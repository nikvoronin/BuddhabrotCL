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
        public FxFilter Filter { get; set; } = FxFilter.Log10;
        [Category("Filter")]
        public float Limit_Red { get; set; } = .33f;
        [Category("Filter")]
        public float Limit_Green { get; set; } = .66f;

        [Category("View")]
        [Description(@"ReMin; ReMax; ImMin; ImMax

Try this:
Deep region 1: -1.05f; -0.9f; -0.3f; -0.225f;
Deep region 2: -1.22f; -1.0f; 0.16f; 0.32f;
Smallbrot: -1.8f; -1.73f; -0.028f; 0.028f;
Fullbrot: -2.0f; 2.0f; -2.0f; 2.0f;")]
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
        public int IterationsMax { get; set; } = 200;
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
