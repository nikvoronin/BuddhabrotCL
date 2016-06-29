using System;
using System.ComponentModel;

namespace BuddhabrotCL
{
    public class Render
    {
        [Category("OpenCL")]
        [DisplayName("Workers Count")]
        public uint Workers { get; set; } = 1000000;
        public uint workers;

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
        [DisplayName("Re Min")]
        public float ReMin { get; set; } = -2f;
        [Category("View")]
        [DisplayName("Re Max")]
        public float ReMax { get; set; } =  2f;
        [Category("View")]
        [DisplayName("Im Min")]
        public float ImMin { get; set; } = -2f;
        [Category("View")]
        [DisplayName("Im Max")]
        public float ImMax { get; set; } =  2f;
        [Category("View")]

        public float reMin;
        public float reMax;
        public float imMin;
        public float imMax;

        [Category("Fractal")]
        [DisplayName("Grayscale Mode")]
        public bool IsGrayscale { get; set; } = false;
        public bool isGrayscale;
        [Category("Fractal")]
        [DisplayName("C-constant")]
        public Vector2F C { get; set; } = new Vector2F { x = -0.75f, y = 0.27015f };
        [Category("Fractal")]
        [DisplayName("Floor Low")]
        public float Limit_Lo { get; set; } = .33f;
        [Category("Fractal")]
        [DisplayName("Floor Mid")]
        public float Limit_Mid { get; set; } = .66f;

        [Category("Fractal")]
        [DisplayName("Escape Orbit")]
        public float EscapeOrbit { get; set; } = 4f;
        public float escapeOrbit;

        [Category("Fractal")]
        [DisplayName("Iterations Min")]
        public int IterationsMin { get; set; } = 0;
        [Category("Fractal")]
        [DisplayName("Iterations Max")]
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

        public Vector4 minColor;
        public Vector4 maxColor;

        public void RecalculateColors()
        {
            minColor.x = 0;
            maxColor.x = (uint)((iterMax - iterMin) * Limit_Lo);

            minColor.y = maxColor.x;
            maxColor.y = (uint)(iterMin + (iterMax - iterMin) * Limit_Mid);

            minColor.z = maxColor.y;
            maxColor.z = (uint)(iterMax);
        }
    }
}
