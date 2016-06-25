using System;
using System.Runtime.InteropServices;
using Cloo;
using System.Collections.Generic;

namespace BuddhabrotCL
{
    public class Buddhabrot
    {
        public ComputePlatform clPlatform;
        public ComputeContext clContext;
        public ComputeContextPropertyList clProperties;
        public ComputeKernel clKernel;
        public ComputeProgram clProgram;
        public ComputeCommandQueue clCommands;
        public ComputeEventList clEvents;

        public ComputeBuffer<Vector4> cbuf_Rng;
        public ComputeBuffer<Vector4> cbuf_Result;
        public Vector4[] h_resultBuf;
        private GCHandle gc_resultBuffer;

        List<string> functions = new List<string>();

        RenderParameters bp;

        public Buddhabrot(ComputePlatform cPlatform, string kernelSource, RenderParameters bp)
        {
            this.bp = bp;

            clPlatform = cPlatform;
            clProperties = new ComputeContextPropertyList(clPlatform);
            clContext = new ComputeContext(clPlatform.Devices, clProperties, null, IntPtr.Zero);
            clCommands = new ComputeCommandQueue(clContext, clContext.Devices[0], ComputeCommandQueueFlags.None);
            clEvents = new ComputeEventList();
            clProgram = new ComputeProgram(clContext, new string[] { kernelSource });

            h_resultBuf = new Vector4[bp.width * bp.height];
            gc_resultBuffer = GCHandle.Alloc(h_resultBuf, GCHandleType.Pinned);

            int i = kernelSource.IndexOf("__kernel");
            if (i > -1)
            {
                int j = kernelSource.IndexOf("(", i);
                if (j > -1)
                {
                    string raw = kernelSource.Substring(i + 8, j - i - 8);
                    string[] parts = raw.Trim().Split(' ');
                    for(int k = parts.Length - 1; k != 0; k--)
                    {
                        if(!string.IsNullOrEmpty(parts[k]))
                        {
                            functions.Add(parts[k]);
                            break;
                        }
                    }
                }
            }
        }

        public void BuildKernels()
        {
            string msg = null;
            foreach (string fn in functions)
            {
                try
                {
                    clProgram.Build(null, null, null, IntPtr.Zero);
                    clKernel = clProgram.CreateKernel(fn);
                    break;
                }
                catch(Exception ex)
                {
                    msg = ex.Message;
                }
            }

            if (clKernel == null)
                throw new Exception(msg);
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
                new ComputeBuffer<Vector4>(
                    clContext,
                    ComputeMemoryFlags.ReadOnly,
                    bp.width * bp.height);
        }

        public void ConfigureKernel()
        {
            clKernel.SetValueArgument(0, bp.reMin);
            clKernel.SetValueArgument(1, bp.reMax);
            clKernel.SetValueArgument(2, bp.imMin);
            clKernel.SetValueArgument(3, bp.imMax);
            clKernel.SetValueArgument(4, (uint)bp.iterMin);
            clKernel.SetValueArgument(5, (uint)bp.iterMax);
            clKernel.SetValueArgument(6, (uint)bp.width);
            clKernel.SetValueArgument(7, (uint)bp.height);
            clKernel.SetValueArgument(8, bp.escapeOrbit);
            clKernel.SetValueArgument(9, bp.C.Value);
            clKernel.SetValueArgument(10, bp.minColor);
            clKernel.SetValueArgument(11, bp.maxColor);
            clKernel.SetValueArgument(12, bp.isGrayscale ? 1u : 0u);
            clKernel.SetMemoryArgument(13, cbuf_Rng);
            clKernel.SetMemoryArgument(14, cbuf_Result);
        }

        public void ExecuteKernel_Buddhabrot()
        {
            clCommands.Execute(clKernel, null, new long[] { bp.workers }, null, clEvents);
        }


        public void ReadResult()
        {
            clCommands.Read(cbuf_Result, true, 0, bp.width * bp.height, gc_resultBuffer.AddrOfPinnedObject(), clEvents);
            clCommands.Finish();
        }
    }
}
