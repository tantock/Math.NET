using MathDotNET.LinearAlgebra;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MathDotNET.NumericalOptimizer
{
    public class NonlinearConjugateGradient
    {
        public double c1Wolfe = 1e-4;
        public double c2Wolfe = 0.9;
        public double GradientTolerance = 1e-8;
        public int numStepsToConverge = 0;

        private Vector<double> solution;
        public Vector<double> Solution { get { return solution; } }
        public Func<Vector<double>, double> FuncToMinimize;
        public Func<Vector<double>, Vector<double>> FuncGrad;
        Action<Vector<double>, double> callbackMethod;

        public NonlinearConjugateGradient(Func<Vector<double>, double> _FuncToMinimize, Func<Vector<double>, Vector<double>> _FuncGrad, Action<Vector<double>, double> callback = null)
        {
            FuncToMinimize = _FuncToMinimize;
            FuncGrad = _FuncGrad;
            callbackMethod = callback;
        }

        public Task Minimize(Vector<double> initialX, CancellationTokenSource ctSource)
        {
            CancellationToken ct = ctSource.Token;
            var task = Task.Run(() => {
                var initialGrad = FuncGrad(initialX);
                Vector<double> deltaX = -initialGrad/initialGrad.EuclidLength;
                double a = backtrackingLineSearch(initialX, deltaX, initialGrad);
                Vector<double> nextX = initialX + (a * deltaX);
                double normGrad = FuncGrad(nextX).EuclidLength;
                var s = new Vector<double>(deltaX);
                while (normGrad > GradientTolerance)
                {
                    if (ct.IsCancellationRequested)
                    {
                        // Clean up here, then...
                        solution = nextX;
                        ct.ThrowIfCancellationRequested();
                    }
                    var deltaNextX = -FuncGrad(nextX);
                    var B = Fletcher_Reeves(deltaNextX, deltaX);
                    s = deltaNextX + B * s;
                    a = backtrackingLineSearch(nextX, deltaNextX/deltaNextX.EuclidLength, -deltaNextX);
                    nextX = nextX + a * s;
                    normGrad = FuncGrad(nextX).EuclidLength;
                    numStepsToConverge++;
                    if (callbackMethod != null)
                    {
                        if (ct.IsCancellationRequested)
                        {
                            // Clean up here, then...
                            solution = nextX;
                            ct.ThrowIfCancellationRequested();
                        }
                        callbackMethod(new Vector<double>(nextX), normGrad);
                    }
                }

                solution = nextX;
            }, ctSource.Token);

            return task;
        }

        private double Fletcher_Reeves(Vector<double> deltaX, Vector<double> prevDeltaX)
        {
            return (deltaX * deltaX) / (prevDeltaX * prevDeltaX);
        }

        

        private double backtrackingLineSearch(Vector<double> X, Vector<double> directionUnitVector, Vector<double> gradF_of_x, double alphaInit = 1)
        {
            double tau = 0.5;
            double c = 0.5;
            double alpha = alphaInit;
            double m = gradF_of_x * directionUnitVector;
            double t = -c * m;
            while(FuncToMinimize(X) - FuncToMinimize(X + alpha*directionUnitVector) < alpha * t)
            {
                alpha = tau * alpha;
            }



            return alpha;
        }
    }
}
