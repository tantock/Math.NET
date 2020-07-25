using System;
using MathDotNET.LinearAlgebra;

namespace MathDotNET.NumericalOptimizer
{
    public class BFGSMinimizer
    {
        public Func<Vector<double>, double> FuncToMinimize;
        public Func<Vector<double>, Vector<double>> FuncGrad;
        public double c1Wolfe = 1e-4;
        public double c2Wolfe = 0.9;

        Action<Vector<double>, double> callbackMethod;
        public double GradientTolerance = 1e-8;

        public double smallestDouble { get { return double.Epsilon; } }

        private Matrix<double> solution;

        public int numStepsToConverge = 0;
        public Matrix<double> Solution { get { return solution; } }

        /// <summary>
        /// Creates a BFGS minimizer class to solve for the parameters that minimize a function
        /// </summary>
        /// <param name="_FuncToMinimize">The function to minimize given an array of parameters</param>
        /// <param name="_FuncGrad">The gradient of the function given the current parameter used</param>
        /// <param name="callback">A callback method with the current solution and gradient norm as the argument</param>
        public BFGSMinimizer(Func<Vector<double>, double> _FuncToMinimize, Func<Vector<double>, Vector<double>> _FuncGrad, Action<Vector<double>, double> callback = null)
        {
            FuncToMinimize = _FuncToMinimize;
            FuncGrad = _FuncGrad;
            callbackMethod = callback;
        }

        public void Minimize(Vector<double> initialX)
        {
            numStepsToConverge = 0;
            Matrix<double> X = new Matrix<double>(initialX, true);

            Matrix<double> B_inv = MatrixBuilder<double>.Identity(initialX.Size);

            Matrix<double> grad = new Matrix<double>(FuncGrad(initialX), true);

            Matrix<double> gradNextX = new Matrix<double>(initialX.Size, 1);


            double normGrad = grad.Norm;

            double stepLength;

            Matrix<double> s;

            Matrix<double> y;

            while (normGrad > GradientTolerance)
            {
                Matrix<double> p = (B_inv * -grad);

                //Matrix<double> p_unit = p / p.EuclidNorm;

                stepLength = backtrackingLineSearch(X, p, grad);

                s = stepLength * p;

                X = X + s;

                gradNextX = new Matrix<double>(FuncGrad(new Vector<double>(X)),true);

                y = gradNextX - grad;



                double sT_y = (s.T * y).Get(0,0);
                double yT_Binv_y = (y.T * B_inv * y).Get(0,0);

                Matrix<double> secondTerm = ((sT_y + yT_Binv_y) / (sT_y * sT_y)) * (s * s.T);

                Matrix<double> thirdTerm = (B_inv * y * s.T + s * y.T * B_inv) / sT_y;

                B_inv += (secondTerm - thirdTerm);

                grad = gradNextX;
                normGrad = grad.Norm;
                if (callbackMethod != null)
                {
                    callbackMethod(new Vector<double>(X), normGrad);
                }
                numStepsToConverge++;
            }

            solution = X;
        }

        //public void backtrackingGD(Vector<double> initialX)
        //{
        //    Matrix<double> gradF_of_x = new Matrix<double>(FuncGrad(initialX));
        //    double normGrad = gradF_of_x.EuclidNorm;
        //    Matrix<double> X = new Matrix<double>(initialX);
        //    double stepSize = 100;

        //    while (normGrad > GradientTolerance)
        //    {
        //        var directionUnitVector = -gradF_of_x / normGrad;
        //        stepSize = backtrackingLineSearch(X, directionUnitVector, gradF_of_x, stepSize);
        //        X += stepSize * directionUnitVector;
        //        gradF_of_x = new Matrix<double>(FuncGrad(X.Arr));
        //        normGrad = gradF_of_x.EuclidNorm;

        //        Debug.WriteLine(normGrad);
        //    }

        //    solution = X;
        //}

        private bool[] IsWolfeCondiMet(Matrix<double> X_k, Matrix<double> direction_k, double stepLength_k, Matrix<double> gradF_of_x = null)
        {
            Matrix<double> nextX = (X_k + stepLength_k * direction_k);
            double f_of_x = FuncToMinimize(new Vector<double>(X_k));

            if (gradF_of_x == null)
            {
                gradF_of_x = new Matrix<double>(FuncGrad(new Vector<double>(X_k)),true);
            }

            double f_of_nextX = FuncToMinimize(new Vector<double>(nextX));
            Matrix<double> gradF_of_nextX = new Matrix<double>(FuncGrad(new Vector<double>(nextX)),true);
            double p_t_timesGradofX = (direction_k.T * gradF_of_x).Get(0,0);
            double p_t_timesGradofnextX = (direction_k.T * gradF_of_nextX).Get(0,0);

            if (f_of_nextX <= f_of_x + c1Wolfe * stepLength_k * p_t_timesGradofX)
            {
                if (-p_t_timesGradofnextX <= -c2Wolfe * p_t_timesGradofX)
                {
                    return new bool[] { true, true };
                }
                else
                {
                    return new bool[] { true, false };
                }
            }

            return new bool[] { false, false };
        }

        private double backtrackingLineSearch(Matrix<double> X, Matrix<double> direction, Matrix<double> gradF_of_x, double alphaInit = 1)
        {
            double tau = 0.5;
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
                else
                {
                    numRetries++;
                    alpha = alphaInit * Math.Pow(10, numRetries);
                }

                bothCondi = IsWolfeCondiMet(X, direction, alpha, gradF_of_x);
                totalTries++;
            }

            return alpha;
        }
    }
}
