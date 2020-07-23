using System;
using System.Collections.Generic;
using System.Text;

namespace MathDotNET
{
    public interface IMathOperators<T>
    {
        T add(T a, T b);
        T subtract(T a, T b);
        T multiply(T a, T b);
        T divide(T a, T b);

        T sqrt(T a);

        T GetZeroValue();

    }
}
