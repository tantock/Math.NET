using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MathDotNET.Collections.Statistics;
using MathDotNET.LinearAlgebra;

namespace MathDotNET.NumericalOptimizer
{
    public class BFGSMinimizer
    {
        private const double TwoWayBacktrackingLineSearchStepSizeInit = 1;

        public Func<DoubleVector, double> FuncToMinimize;
        public Func<DoubleVector, DoubleVector> FuncGrad;
        public double c1Wolfe = 1e-4;
        public double c2Wolfe = 0.9;

        Action<DoubleVector, double> callbackMethod;
        public double GradientTolerance = 1e-8;

        private double initialStepSize = 1;

        public double smallestDouble { get { return double.Epsilon; } }

        private DoubleVector solution;

        public int numStepsToConverge = 0;
        public DoubleVector Solution { get { return solution; } }

        /// <summary>
        /// Creates a BFGS minimizer class to solve for the parameters that minimize a function
        /// </summary>
        /// <param name="_FuncToMinimize">The function to minimize given an array of parameters</param>
        /// <param name="_FuncGrad">The gradient of the function given the current parameter used</param>
        /// <param name="callback">A callback method with the current solution and gradient norm as the argument</param>
        public BFGSMinimizer(Func<DoubleVector, double> _FuncToMinimize, Func<DoubleVector, DoubleVector> _FuncGrad, Action<DoubleVector, double> callback = null)
        {
            FuncToMinimize = _FuncToMinimize;
            FuncGrad = _FuncGrad;
            callbackMethod = callback;
        }

        public Task<DoubleVector> Minimize(DoubleVector initialX, CancellationTokenSource ctSource)
        {
            CancellationToken ct = ctSource.Token;
            ct.ThrowIfCancellationRequested();
            double prevStepSize = initialStepSize;
            return Task.Run(() =>
            {
                numStepsToConverge = 0;
                DoubleVector X = new DoubleVector(initialX);

                DoubleMatrix B_inv = new DoubleMatrix(DoubleMatrix.Identity(initialX.Size));//MatrixBuilder<double>.Identity(initialX.Size);

                DoubleVector grad = new DoubleVector(FuncGrad(initialX));

                //B_inv /= grad.EuclidLength;

                DoubleVector gradNextX = new DoubleVector(initialX.Size);


                //double normGrad = grad.EuclidLength;
                double gradNorm = grad.EuclidLength;

                List<double> normGradList = new List<double>();
                normGradList.Add(gradNorm);

                double stepLength;

                DoubleVector s;

                DoubleVector y;
                //DoubleVector p_init = (B_inv * -grad);
                initialStepSize = 1024;
                int numGradSamples = 5;

                while (!double.IsNaN(gradNorm) && gradNorm > GradientTolerance)
                {

                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        solution = X;
                        ct.ThrowIfCancellationRequested();
                    }
                    //B_inv /= grad.Norm;

                    DoubleVector p = (B_inv * -grad);
                    //p /= p.EuclidLength;

                    //DoubleMatrix p_unit = p / p.EuclidNorm;

                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        solution = X;
                        ct.ThrowIfCancellationRequested();
                    }

                    stepLength = backtrackingLineSearch(X, FuncToMinimize, grad, p, prevStepSize, ctSource);//backtrackingLineSearch(X, p, grad, initialStepSize);

                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        //solution = X;
                        ct.ThrowIfCancellationRequested();
                    }

                    prevStepSize = stepLength;
                    if (stepLength < 1e-16 || double.IsNaN(stepLength))
                    {
                        break;
                    }

                    s = stepLength * p;

                    X = X + s;

                    gradNextX = FuncGrad(X);

                    y = gradNextX - grad;



                    double sT_y = s * y;


                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        //solution = X;
                        ct.ThrowIfCancellationRequested();
                    }

                    double yT_Binv_y = y * B_inv * y;

                    DoubleMatrix secondTerm = ((sT_y + yT_Binv_y) / (sT_y * sT_y)) * (new DoubleMatrix(s, true) * new DoubleMatrix(s, false));

                    DoubleMatrix thirdTerm = (((B_inv * new DoubleMatrix(y, true)) * new DoubleMatrix(s, false)) + ((new DoubleMatrix(s, true) * new DoubleMatrix(y, false)) * B_inv)) / sT_y;

                    B_inv += (secondTerm - thirdTerm);

                    grad = gradNextX;
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        //solution = X;
                        ct.ThrowIfCancellationRequested();
                    }

                    gradNorm = grad.EuclidLength;

                    if (double.IsNaN(gradNorm))
                    {
                        X -= s;
                        break;
                    }

                    normGradList.Add(gradNorm);
                    if (normGradList.Count() > numGradSamples)
                    {
                        normGradList.Remove(0);
                    }
                    if (callbackMethod != null)
                    {
                        callbackMethod(new DoubleVector(X), gradNorm);
                    }
                    numStepsToConverge++;
                    if(gradNorm < GradientTolerance)
                    {
                        break;
                    }
                    // Poll on this property if you have to do
                    // other cleanup before throwing.
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        solution = X;
                        ct.ThrowIfCancellationRequested();
                    }
                }

                solution = X;

                return solution;
            });
        }
        /*
        public Task MinimizeGPU(DoubleVector initialX, CancellationTokenSource ctSource)
        {
            CancellationToken ct = ctSource.Token;
            var task = Task.Run(() =>
            {
                ct.ThrowIfCancellationRequested();
                numStepsToConverge = 0;
                DoubleVector X = new DoubleVector(initialX);

                DoubleMatrix B_inv = MatrixBuilder<double>.Identity(initialX.Size);

                DoubleVector grad = new DoubleVector(FuncGrad(initialX));

                //B_inv /= grad.EuclidLength;

                DoubleVector gradNextX = new DoubleVector(initialX.Size);


                //double normGrad = grad.EuclidLength;

                List<double> normGradList = new List<double>();
                normGradList.Add(grad.EuclidLength);

                double stepLength;

                DoubleVector s;

                DoubleVector y;
                //DoubleVector p_init = (B_inv * -grad);
                initialStepSize = 1024;
                int numGradSamples = 5;
                using (var clMultiply = new DoubleMatrixOpenCL(0))
                {
                    while (!double.IsNaN(normGradList.Last()) && normGradList.Average() + 2 * normGradList.StandardDeviationP() > GradientTolerance)
                    {

                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = X;
                            ct.ThrowIfCancellationRequested();
                        }
                        //B_inv /= grad.Norm;

                        DoubleVector p = (B_inv * -grad);

                        //DoubleMatrix p_unit = p / p.EuclidNorm;

                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = X;
                            ct.ThrowIfCancellationRequested();
                        }

                        stepLength = backtrackingLineSearch(X, p, grad, initialStepSize);

                        s = stepLength * p;

                        X = X + s;

                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = X;
                            ct.ThrowIfCancellationRequested();
                        }

                        gradNextX = FuncGrad(X);

                        y = gradNextX - grad;



                        double sT_y = s * y;


                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = X;
                            ct.ThrowIfCancellationRequested();
                        }

                        double yT_Binv_y = y * B_inv * y;

                        DoubleMatrix secondTerm = ((sT_y + yT_Binv_y) / (sT_y * sT_y)) * (new DoubleMatrix(s, true) * new DoubleMatrix(s, false));

                        DoubleMatrix thirdTerm = (clMultiply.Multiply((B_inv * new DoubleMatrix(y, true)), new DoubleMatrix(s, false)) + clMultiply.Multiply(new DoubleMatrix(s, true) * new DoubleMatrix(y, false), B_inv)) / sT_y;

                        B_inv += (secondTerm - thirdTerm);

                        grad = gradNextX;
                        normGradList.Add(grad.EuclidLength);
                        if (normGradList.Count() > numGradSamples)
                        {
                            normGradList.Remove(0);
                        }
                        if (callbackMethod != null)
                        {
                            callbackMethod(new DoubleVector(X), normGradList.Last());
                        }
                        numStepsToConverge++;
                        // Poll on this property if you have to do
                        // other cleanup before throwing.
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = X;
                            ct.ThrowIfCancellationRequested();
                        }
                    }
                }

                solution = X;
            }, ctSource.Token);

            return task;
        }
        */
        //public void backtrackingGD(DoubleVector initialX)
        //{
        //    DoubleMatrix gradF_of_x = new DoubleMatrix(FuncGrad(initialX));
        //    double normGrad = gradF_of_x.EuclidNorm;
        //    DoubleMatrix X = new DoubleMatrix(initialX);
        //    double stepSize = 100;

        //    while (normGrad > GradientTolerance)
        //    {
        //        var directionUnitVector = -gradF_of_x / normGrad;
        //        stepSize = backtrackingLineSearch(X, directionUnitVector, gradF_of_x, stepSize);
        //        X += stepSize * directionUnitVector;
        //        gradF_of_x = new DoubleMatrix(FuncGrad(X.Arr));
        //        normGrad = gradF_of_x.EuclidNorm;

        //        Debug.WriteLine(normGrad);
        //    }

        //    solution = X;
        //}

        private bool[] IsWolfeCondiMet(DoubleVector X_k, DoubleVector direction_k, double stepLength_k, DoubleVector gradF_of_x = null)
        {
            DoubleVector nextX = (X_k + stepLength_k * direction_k);
            double f_of_x = FuncToMinimize(new DoubleVector(X_k));

            if (gradF_of_x == null)
            {
                gradF_of_x = FuncGrad(X_k);
            }

            double f_of_nextX = FuncToMinimize(nextX);
            DoubleVector gradF_of_nextX = FuncGrad(nextX);
            double p_t_timesGradofX = direction_k * gradF_of_x;
            double p_t_timesGradofnextX = direction_k * gradF_of_nextX;

            //if (f_of_nextX <= f_of_x + c1Wolfe * stepLength_k * p_t_timesGradofX)
            //{
            //    if (-p_t_timesGradofnextX <= -c2Wolfe * p_t_timesGradofX)
            //    {
            //        return new bool[] { true, true };
            //    }
            //    else
            //    {
            //        return new bool[] { true, false };
            //    }
            //}

            return new bool[] { f_of_nextX <= f_of_x + c1Wolfe * stepLength_k * p_t_timesGradofX, -p_t_timesGradofnextX <= -c2Wolfe * p_t_timesGradofX }; // { false, false };//
        }

        private double backtrackingLineSearch(DoubleVector X,Func<DoubleVector,double> f, DoubleVector grad_F_atX, DoubleVector p, double alphaPrev, CancellationTokenSource ctSource)
        {
            double c = 0.5;
            double tau = 0.5;
            double m = grad_F_atX * p;

            var f_of_x = f(X);

            double alpha = alphaPrev;

            double t = -c * m;

            int j = 0; //iteration counter

            if(f_of_x - f(X + alpha*p) >= alpha * t)
            {
                while(f_of_x - f(X + alpha * p) >= alpha * t && alpha <= initialStepSize && !ctSource.IsCancellationRequested)
                {
                    alpha = alpha / tau;
                    j++;
                    System.Diagnostics.Debug.WriteLine("Backtracking iteration count:" + j);
                }
            }
            else
            {
                while (f_of_x - f(X + (alpha * p)) < alpha * t && !ctSource.IsCancellationRequested)
                {
                    alpha = alpha * tau;
                    j++;
                    System.Diagnostics.Debug.WriteLine("Backtracking iteration count:" + j);
                }
            }
            return alpha;
        }

        private double backtrackingLineSearchOld(DoubleVector X, DoubleVector direction, DoubleVector gradF_of_x, double alphaInit = 1)
        {
            double tau = 0.5;
            double c = 0.5;
            double m = gradF_of_x * direction;
            double t = -c * m;
            double alpha = alphaInit;
            int numRetries = 0;
            int totalTries = 0;
            bool[] bothCondi = IsWolfeCondiMet(X, direction, alpha, gradF_of_x);
            while (!bothCondi[0] || !bothCondi[1])
            {
                if (!bothCondi[0])
                {
                    alpha *= tau;
                }
                //else
                //{
                //    numRetries++;
                //    alpha = alphaInit * Math.Pow(3, numRetries);
                //}

                bothCondi = IsWolfeCondiMet(X, direction, alpha, gradF_of_x);
                //if (numRetries > 5)
                //{
                //    initialStepSize = alpha;
                //}
                totalTries++;
            }
            //while(FuncToMinimize(X) - FuncToMinimize(X + alpha * direction) > alpha * t)
            //{
            //    alpha *= 10;
            //}
            //while(FuncToMinimize(X) - FuncToMinimize(X + alpha*direction) < alpha * t)
            //{
            //    alpha *= tau;
            //}

            return alpha;
        }
    }
}
