using System;
using System.Runtime.InteropServices;
using Cloo;

namespace BuddhabrotCL
{
    public class Render : IDisposable
    {
        public ComputeDevice clDevice;
        public ComputeContext clContext;
        public ComputeContextPropertyList clProperties;
        public ComputeKernel clKernel;
        public ComputeProgram clProgram;
        public ComputeCommandQueue clCommands;

        public ComputeBuffer<Vector4> cbuf_Rng;
        public ComputeBuffer<Vector4> cbuf_Result;
        private GCHandle gc_resultBuffer;

        string kernelFunc = "main";

        RenderParams rp;

        public Render(ComputeDevice cDevice, string kernelSource)
        {
            clDevice = cDevice;
            clProperties = new ComputeContextPropertyList(clDevice.Platform);
            clContext = new ComputeContext(clDevice.Platform.Devices, clProperties, null, IntPtr.Zero);
            clCommands = new ComputeCommandQueue(clContext, clDevice, ComputeCommandQueueFlags.None);
            clProgram = new ComputeProgram(clContext, new string[] { kernelSource });

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
                            kernelFunc = parts[k];
                            break;
                        } // if
                    } // for k
                } // if j
            } // if i
        }

        public void BuildKernels()
        {
            string msg = null;
            try
            {
                clProgram.Build(null, "-I kernel/lib", null, IntPtr.Zero);
                clKernel = clProgram.CreateKernel(kernelFunc);
            }
            catch(Exception ex)
            {
                msg = ex.Message;
            }

            if (clKernel == null)
                throw new Exception(msg);
        }

        public void AllocateBuffers(ParameterBox pbox)
        {
            rp = pbox.rp;

            Random rnd = new Random((int)DateTime.UtcNow.Ticks);

            Vector4[] seeds = new Vector4[rp.workers];
            for (int i = 0; i < rp.workers; i++)
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

            gc_resultBuffer = GCHandle.Alloc(pbox.h_resultBuf, GCHandleType.Pinned);

            if (pbox.h_resultBuf == null)
            {
                cbuf_Result =
                    new ComputeBuffer<Vector4>(
                        clContext,
                        ComputeMemoryFlags.ReadOnly,
                        rp.width * rp.height);
            }
            else
            {
                cbuf_Result =
                    new ComputeBuffer<Vector4>(
                        clContext,
                        ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer,
                        pbox.h_resultBuf);
            } // elseif
        }

        public void ConfigureKernel()
        {
            clKernel.SetValueArgument(0, rp.reMin);
            clKernel.SetValueArgument(1, rp.reMax);
            clKernel.SetValueArgument(2, rp.imMin);
            clKernel.SetValueArgument(3, rp.imMax);
            clKernel.SetValueArgument(4, (uint)rp.iterMin);
            clKernel.SetValueArgument(5, (uint)rp.iterMax);
            clKernel.SetValueArgument(6, (uint)rp.width);
            clKernel.SetValueArgument(7, (uint)rp.height);
            clKernel.SetValueArgument(8, rp.escapeOrbit);
            clKernel.SetValueArgument(9, rp.C.Value);
            clKernel.SetValueArgument(10, rp.Color);
            clKernel.SetMemoryArgument(11, cbuf_Rng);
            clKernel.SetMemoryArgument(12, cbuf_Result);
        }

        public void ExecuteKernel()
        {
            clCommands.Execute(clKernel, null, new long[] { rp.workers }, null, null);
        }

        public void FinishKernel()
        {
            clCommands.Finish();
        }

        public void ReadResultAndFinishKernel()
        {
            clCommands.Read(cbuf_Result, true, 0, rp.width * rp.height, gc_resultBuffer.AddrOfPinnedObject(), null);
            clCommands.Finish();
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                clCommands.Dispose();
                clKernel.Dispose();
                clProgram.Dispose();
                clContext.Dispose();
                cbuf_Result.Dispose();
                cbuf_Rng.Dispose();
            }
        }
    }
}
