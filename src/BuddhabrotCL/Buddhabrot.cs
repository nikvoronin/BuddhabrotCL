using System;
using System.Runtime.InteropServices;

using Cloo;

namespace BuddhabrotCL
{
    public class Buddhabrot
    {
        public const string DEFAULT_KERNEL_BUDDHABROT = "buddhabrot";

        public ComputePlatform clPlatform;
        public ComputeContext clContext;
        public ComputeContextPropertyList clProperties;
        public ComputeKernel clkBbrot;
        public ComputeProgram clProgram;
        public ComputeCommandQueue clCommands;
        public ComputeEventList clEvents;

        public ComputeBuffer<Vector4> cbuf_Rng;
        public ComputeBuffer<RGBA> cbuf_Result;
        public RGBA[] h_resultBuffer;
        private GCHandle gc_resultBuffer;

        BrotParams bp;

        public Buddhabrot(ComputePlatform cPlatform, string kernelSource, BrotParams bp)
        {
            this.bp = bp;

            clPlatform = cPlatform;
            clProperties = new ComputeContextPropertyList(clPlatform);
            clContext = new ComputeContext(clPlatform.Devices, clProperties, null, IntPtr.Zero);
            clCommands = new ComputeCommandQueue(clContext, clContext.Devices[0], ComputeCommandQueueFlags.None);
            clEvents = new ComputeEventList();
            clProgram = new ComputeProgram(clContext, new string[] { kernelSource });

            h_resultBuffer = new RGBA[bp.width * bp.height];
            gc_resultBuffer = GCHandle.Alloc(h_resultBuffer, GCHandleType.Pinned);
        }

        public void BuildKernels()
        {
            clProgram.Build(null, null, null, IntPtr.Zero);
            clkBbrot = clProgram.CreateKernel(DEFAULT_KERNEL_BUDDHABROT);
        }

        public void AllocateBuffers()
        {
            Random rnd = new Random((int)DateTime.UtcNow.Ticks);

            Vector4[] seeds = new Vector4[bp.workers];
            for (int i = 0; i < bp.workers; i++)
                seeds[i] =
                    new Vector4
                    {
                        x = (ushort)rnd.Next(),
                        y = (ushort)rnd.Next(),
                        z = (ushort)rnd.Next(),
                        w = (ushort)rnd.Next()
                    };

            cbuf_Rng =
                new ComputeBuffer<Vector4>(
                    clContext,
                    ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer,
                    seeds);

            cbuf_Result =
                new ComputeBuffer<RGBA>(
                    clContext,
                    ComputeMemoryFlags.ReadWrite,
                    bp.width * bp.height);
        }

        public void ConfigureKernel()
        {
            clkBbrot.SetValueArgument(0, bp.reMin);
            clkBbrot.SetValueArgument(1, bp.reMax);
            clkBbrot.SetValueArgument(2, bp.imMin);
            clkBbrot.SetValueArgument(3, bp.imMax);
            clkBbrot.SetValueArgument(4, (uint)bp.iterMin);
            clkBbrot.SetValueArgument(5, (uint)bp.iterMax);
            clkBbrot.SetValueArgument(6, (uint)bp.width);
            clkBbrot.SetValueArgument(7, (uint)bp.height);
            clkBbrot.SetValueArgument(8, bp.escapeOrbit);
            clkBbrot.SetValueArgument(9, bp.minColor);
            clkBbrot.SetValueArgument(10, bp.maxColor);
            clkBbrot.SetValueArgument(11, bp.isGrayscale ? 1u : 0u);
            clkBbrot.SetValueArgument(12, bp.IsHackMode ? 1u : 0u);
            clkBbrot.SetMemoryArgument(13, cbuf_Rng);
            clkBbrot.SetMemoryArgument(14, cbuf_Result);
        }

        public void ExecuteKernel_Buddhabrot()
        {
            clCommands.Execute(clkBbrot, null, new long[] { bp.workers }, null, clEvents);
        }


        public void ReadResult()
        {
            clCommands.Read(cbuf_Result, true, 0, bp.width * bp.height, gc_resultBuffer.AddrOfPinnedObject(), clEvents);
            clCommands.Finish();
        }
    }
}
