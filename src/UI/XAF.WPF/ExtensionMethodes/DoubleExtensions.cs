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
    //TODO: Remove
    public static double Clip(this double value, double minValue, double maxValue)
    {
        return value > maxValue ? maxValue : value < minValue ? minValue : value;
    }

    public static T Clamp<T>(this T value, T minValue, T maxValue)
        where T : INumber<T>
    {
        return value > maxValue ? maxValue : value < minValue ? minValue : value;
    }
}
