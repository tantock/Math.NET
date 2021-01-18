using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using Cloo;
namespace MathDotNET.LinearAlgebra
{
    public class DoubleMatrixOpenCL :IDisposable
    {
        const string clProgramSourceMultiplyMatrix = @"
        kernel void MatrixMultiply(
                                    global  read_only double* inputA,
                                    global  read_only double* inputB,
                                    global write_only double* outputC,
                                    int widthA,
                                    int heightA,
                                    int widthB,
                                    int heightB){
            int row = get_global_id(1);
            int col = get_global_id(0);
            double sum = 0.0;
            for(int i = 0; i < widthA; i++){
                sum += inputA[row*widthA + i] * inputB[i*widthB + col];
            }
            outputC[row*widthB + col] = sum;
        }";
        ComputeKernel kernel;
        ComputeProgram program;
        ComputeContext context;
        ComputeCommandQueue commands;
        public DoubleMatrixOpenCL(int deviceID = 0)
        {
            Func<int, string, Version, bool> deviceSelector = (i, n, v) => i == deviceID;
            deviceSelector = deviceSelector ?? ((i, d, v) => true);
            var device = ComputePlatform.Platforms.SelectMany(p => p.Devices).Where((d, i) => deviceSelector(i, $"{d.Name} {d.DriverVersion}", d.Version)).First();
            var properties = new ComputeContextPropertyList(device.Platform);
            context = new ComputeContext(new[] { device }, properties, null, IntPtr.Zero);
            // Create the kernel function and set its arguments.
            program = new ComputeProgram(context, clProgramSourceMultiplyMatrix);

            // Create and build the opencl program.
            program.Build(null, null, null, IntPtr.Zero);
            kernel = program.CreateKernel("MatrixMultiply");

            // Create the command queue. This is used to control kernel execution and manage read/write/copy operations.
            commands = new ComputeCommandQueue(context, context.Devices[0], ComputeCommandQueueFlags.None);
        }

        public Matrix<double> Multiply(Matrix<double> L, Matrix<double> R)
        {
            if (R.M != L.N)
            {
                throw new InvalidOperationException("Invalid dimensions for matrix multiplication");
            }
            else
            {
                Matrix<double> C = new Matrix<double>(L.M, R.N);
                var result = MatrixMultiply(L, R);
                for (int i = 0; i < C.M; i++)
                {
                    for (int j = 0; j < C.N; j++)
                    {
                        C.Set(i, j, result[i * C.N + j]);
                    }
                }

                return C;
            }

        }

        private double[] MatrixMultiply(Matrix<double> L, Matrix<double> R)
        {

            var L_array = L.To1D();
            var R_array = R.To1D();
            var O_array = new double[L.M * R.N];

            ComputeBuffer<double> a = new ComputeBuffer<double>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, L_array);
            ComputeBuffer<double> b = new ComputeBuffer<double>(context, ComputeMemoryFlags.ReadOnly | ComputeMemoryFlags.CopyHostPointer, R_array);
            ComputeBuffer<double> c = new ComputeBuffer<double>(context, ComputeMemoryFlags.WriteOnly, O_array.Length);

            kernel.SetMemoryArgument(0, a);
            kernel.SetMemoryArgument(1, b);
            kernel.SetMemoryArgument(2, c);
            kernel.SetValueArgument(3, L.N);
            kernel.SetValueArgument(4, L.M);
            kernel.SetValueArgument(5, R.N);
            kernel.SetValueArgument(6, R.M);

            commands.Execute(kernel, null, new long[] { R.N, L.M }, null, null);

            commands.ReadFromBuffer(c, ref O_array, true, null);

            commands.Finish();

            // cleanup buffers
            a.Dispose();
            b.Dispose();
            c.Dispose();
            return O_array;
        }

        public void Dispose()
        {
            program.Dispose();
            context.Dispose();

            kernel.Dispose();

            commands.Dispose();
        }
    }
}
