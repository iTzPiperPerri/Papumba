using DA_Assets.FCU.Model;
using DA_Assets.Shared.Extensions;
using System.Linq;
using UnityEngine;

#if TextMeshPro
using TMPro;
#endif

namespace DA_Assets.FCU.Extensions
{
    public static class TextExtensions
    {
        public static string GetText(this GameObject go)
        {
#if TextMeshPro
            if (go.TryGetComponent<TMP_Text>(out TMP_Text tmpText))
            {
                return tmpText.text;
            }
#endif
            if (go.TryGetComponent<UnityEngine.UI.Text>(out UnityEngine.UI.Text unityText))
            {
                return unityText.text;
            }

            return null;
        }

        public static void SetText(this GameObject go, string text)
        {
            bool set = false;
#if TextMeshPro
            if (go.TryGetComponent<TMP_Text>(out TMP_Text tmpText))
            {
                tmpText.text = text;
                set = true;
            }
#endif
            if (set)
                return;

            if (go.TryGetComponent<UnityEngine.UI.Text>(out UnityEngine.UI.Text unityText))
            {
                unityText.text = text;
            }
        }

        /// <summary>
        /// https://drafts.csswg.org/css-fonts/#font-weight-numeric-values
        /// </summary>
        public static string FontWeightToString(this int weight)
        {
            switch (weight)
            {
                case 100: return "Thin";
                case 200: return "ExtraLight";
                case 300: return "Light";
                case 400: return "Normal";
                case 500: return "Medium";
                case 600: return "SemiBold";
                case 700: return "Bold";
                case 800: return "ExtraBold";
                case 900: return "Black";
            }

            return weight.ToString();
        }

        public static string FontNameToString(this FontMetadata fontMetadata,
            bool includeWeight = true,
            bool includeItalic = true,
            FontSubset? fontSubset = null,
            bool format = false)
        {
            string fullName = fontMetadata.Family;

            if (includeWeight)
                fullName += $"-{fontMetadata.Weight.FontWeightToString()}";

            if (includeItalic)
                if (fontMetadata.FontStyle == FontStyle.Italic)
                    fullName += $"-{FontStyle.Italic}";

            if (fontSubset != null)
                fullName += $"-{fontSubset}";

            if (format)
            {
                fullName = fullName.FormatFontName();
            }

            return fullName;
        }

        public static FontMetadata GetFontMetadata(this FObject fobject)
        {
            FontMetadata fm = new FontMetadata
            {
                Family = fobject.StyleOverrideTable.IsEmpty() ? fobject.Style.FontFamily : fobject.StyleOverrideTable.First().Value.FontFamily,
                Weight = fobject.StyleOverrideTable.IsEmpty() ? fobject.Style.FontWeight : fobject.StyleOverrideTable.First().Value.FontWeight,
                FontStyle = fobject.Style.Italic.ToBoolNullFalse() ? FontStyle.Italic : FontStyle.Normal,
            };

            return fm;
        }

        public static string FontNameToString(this FObject fobject,
            bool includeWeight = true,
            bool includeItalic = true,
            FontSubset? fontSubset = null,
            bool format = false)
        {
            FontMetadata fm = fobject.GetFontMetadata();
            string fn = fm.FontNameToString(includeWeight, includeItalic, fontSubset, format);
            return fn;
        }

        public static Color GetTextColor(this FObject text)
        {
            if (text.Fills.IsEmpty() == false)
            {
                if (text.Fills[0].Type.Contains("GRADIENT"))
                {
                    return text.Fills[0].GradientStops[0].Color;
                }
                else
                {
                    return text.Fills[0].Color;
                }
            }

            return default;
        }
    }
}