using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    [Serializable]
    public class UnityTextDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public Text Draw(FObject fobject)
        {
            fobject.Data.GameObject.TryAddGraphic(out Text text);

            SetDefaultTextStyle(text, fobject);
            SetFont(text, fobject);
            SetDefaultTextFontStyle(text, fobject);
            SetDefaultTextAligment(text, fobject);

            return text;
        }

        private void SetFont(Text text, FObject fobject)
        {
            Font font = monoBeh.FontLoader.GetFontFromArray(fobject, monoBeh.FontLoader.TtfFonts);
            text.font = font;
        }

        private void SetDefaultTextStyle(Text text, FObject fobject)
        {
            text.resizeTextForBestFit = monoBeh.Settings.UnityTextSettings.BestFit;
            text.text = fobject.Characters;
            text.resizeTextMinSize = 1;

            text.resizeTextMaxSize = Convert.ToInt32(fobject.Style.FontSize);
            text.fontSize = Convert.ToInt32(fobject.Style.FontSize);

            text.verticalOverflow = monoBeh.Settings.UnityTextSettings.VerticalWrapMode;
            text.horizontalOverflow = monoBeh.Settings.UnityTextSettings.HorizontalWrapMode;
            text.lineSpacing = monoBeh.Settings.UnityTextSettings.FontLineSpacing;

            if (fobject.Fills[0].GradientStops != null)
            {
                text.color = fobject.Fills[0].GradientStops[0].Color;
            }
            else
            {
                text.color = fobject.Fills[0].Color;
            }
        }

        private void SetDefaultTextFontStyle(Text text, FObject fobject)
        {
            string fontStyleRaw = fobject.Style.FontPostScriptName;

            if (fontStyleRaw != null)
            {
                if (fontStyleRaw.Contains(FontStyle.Bold.ToString()))
                {
                    if (fobject.Style.Italic.ToBoolNullFalse())
                    {
                        text.fontStyle = FontStyle.BoldAndItalic;
                    }
                    else
                    {
                        text.fontStyle = FontStyle.Bold;
                    }
                }
                else if (fobject.Style.Italic.ToBoolNullFalse())
                {
                    text.fontStyle = FontStyle.Italic;
                }
                else
                {
                    text.fontStyle = FontStyle.Normal;
                }
            }
        }

        private void SetDefaultTextAligment(Text text, FObject fobject)
        {
            string textAligment = fobject.Style.TextAlignVertical + " " + fobject.Style.TextAlignHorizontal;

            switch (textAligment)
            {
                case "BOTTOM CENTER":
                    text.alignment = TextAnchor.LowerCenter;
                    break;
                case "BOTTOM LEFT":
                    text.alignment = TextAnchor.LowerLeft;
                    break;
                case "BOTTOM RIGHT":
                    text.alignment = TextAnchor.LowerRight;
                    break;
                case "CENTER CENTER":
                    text.alignment = TextAnchor.MiddleCenter;
                    break;
                case "CENTER LEFT":
                    text.alignment = TextAnchor.MiddleLeft;
                    break;
                case "CENTER RIGHT":
                    text.alignment = TextAnchor.MiddleRight;
                    break;
                case "TOP CENTER":
                    text.alignment = TextAnchor.UpperCenter;
                    break;
                case "TOP LEFT":
                    text.alignment = TextAnchor.UpperLeft;
                    break;
                case "TOP RIGHT":
                    text.alignment = TextAnchor.UpperRight;
                    break;
                default:
                    text.alignment = TextAnchor.MiddleCenter;
                    break;
            }
        }
    }
}
