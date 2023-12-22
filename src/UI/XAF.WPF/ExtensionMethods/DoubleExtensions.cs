using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace XAF.UI.WPF.ExtensionMethodes;
public static class DoubleExtensions
{
    public static T Clamp<T>(this T value, T minValue, T maxValue)
        where T : INumber<T>
    {
        return value > maxValue ? maxValue : value < minValue ? minValue : value;
    }
}
