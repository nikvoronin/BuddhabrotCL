using System;
using System.Runtime.InteropServices;

using Cloo;

namespace BuddhabrotCL
{
    public class Buddhabrot
    {
        public const string DEFAULT_KERNEL_BUDDHABROT = "buddhabrot";
        public const string DEFAULT_KERNEL_RNG = "rng";

        public ComputePlatform clPlatform;
        public ComputeContext clContext;
        public ComputeContextPropertyList clProperties;
        public ComputeKernel clkBbrot;
        public ComputeKernel clkRng;
        public ComputeProgram clProgram;
        public ComputeCommandQueue clCommands;
        public ComputeEventList clEvents;

        public ComputeBuffer<Vector2> cbuf_Rng;
        public ComputeBuffer<RGBA>  cbuf_Result;
        public RGBA[] h_resultBuffer;
        private GCHandle gc_resultBuffer;

        public static int workerCount = 1000000;
        private Random rnd;
        BrotParams bp;

        public Buddhabrot(ComputePlatform cPlatform, string kernelSource, string rngSource, BrotParams bp)
        {
            this.bp = bp;

            clPlatform = cPlatform;
            clProperties = new ComputeContextPropertyList(clPlatform);
            clContext = new ComputeContext(clPlatform.Devices, clProperties, null, IntPtr.Zero);
            clCommands = new ComputeCommandQueue(clContext, clContext.Devices[0], ComputeCommandQueueFlags.None);
            clEvents = new ComputeEventList();
            clProgram = new ComputeProgram(clContext, new string[] { kernelSource + rngSource });

            h_resultBuffer = new RGBA[bp.width * bp.height];
            gc_resultBuffer = GCHandle.Alloc(h_resultBuffer, GCHandleType.Pinned);

            rnd = new Random((int)DateTime.UtcNow.Ticks);
        }

        public void BuildKernels()
        {
            clProgram.Build(null, null, null, IntPtr.Zero);
            clkBbrot = clProgram.CreateKernel(DEFAULT_KERNEL_BUDDHABROT);
            clkRng = clProgram.CreateKernel(DEFAULT_KERNEL_RNG);
        }

        public void AllocateBuffers()
        {
            cbuf_Rng = new ComputeBuffer<Vector2>(clContext, ComputeMemoryFlags.ReadWrite, workerCount);
            cbuf_Result  = new ComputeBuffer<RGBA>(clContext, ComputeMemoryFlags.ReadWrite, bp.width * bp.height);
        }

        public void ConfigureKernel()
        {
            clkRng.SetValueArgument<uint>(0, 0);
            clkRng.SetValueArgument<uint>(1, 0);
            clkRng.SetValueArgument(2, workerCount);
            clkRng.SetMemoryArgument(3, cbuf_Rng);

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

        public void ExecuteKernel_Rng()
        {            
            clkRng.SetValueArgument(0, (uint)rnd.Next());
            clkRng.SetValueArgument(1, (uint)rnd.Next());
            clCommands.Execute(clkRng, null, new long[] { 1 }, null, clEvents);
        }

        public void ExecuteKernel_Buddhabrot()
        {
            clCommands.Execute(clkBbrot, null, new long[] { workerCount }, null, clEvents);
        }


        public void ReadResult()
        {
            clCommands.Read(cbuf_Result, true, 0, bp.width * bp.height, gc_resultBuffer.AddrOfPinnedObject(), clEvents);
            clCommands.Finish();
        }
    }
}
