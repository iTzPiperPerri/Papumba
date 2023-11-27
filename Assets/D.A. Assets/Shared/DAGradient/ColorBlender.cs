using UnityEngine;
using DA_Assets.Shared.Extensions;

namespace DA_Assets.DAG
{
    internal class ColorBlender
    {
        internal static Color Blend(Color c1, Color c2, ColorBlendMode mode, float intensity)
        {
            switch (mode)
            {
                case ColorBlendMode.Difference:
                    return c1.Difference(c2, intensity);
                case ColorBlendMode.Overlay:
                    return c1.Overlay(c2, intensity);
                default:
                    return c1.Multiply(c2, intensity);
            }
        }
    }
}
