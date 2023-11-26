using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    public class UnityImageDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject, Sprite sprite, GameObject target)
        {
            target.TryAddGraphic(out Image img);

            img.sprite = sprite;

            SetUnityImageColor(fobject, img);

            img.type = monoBeh.Settings.UnityImageSettings.Type;
            img.raycastTarget = monoBeh.Settings.UnityImageSettings.RaycastTarget;
            img.preserveAspect = monoBeh.Settings.UnityImageSettings.PreserveAspect;
            img.maskable = monoBeh.Settings.UnityImageSettings.Maskable;
#if UNITY_2020_1_OR_NEWER
            img.raycastPadding = monoBeh.Settings.UnityImageSettings.RaycastPadding;
#endif
        }

        public void SetUnityImageColor(FObject fobject, Image img)
        {
            bool hasFills = fobject.TryGetFills(monoBeh, out Paint solidFill, out Paint gradientFill);
            bool hasStroke = fobject.TryGetStrokes(monoBeh, out Paint solidStroke, out Paint gradientStroke);

            monoBeh.Log($"SetUnityImageColor | {fobject.Data.Hierarchy} | {fobject.Data.FcuImageType} | hasFills: {hasFills} | hasStroke: {hasStroke}");

            if (fobject.IsDrawableType())
            {
                if (hasFills && hasStroke)
                {
                    AddUnityOutline(fobject, img, solidStroke, gradientStroke);
                }

                if (hasFills)
                {
                    if (solidFill.IsDefault() == false)
                    {
                        img.color = solidFill.Color.SetFigmaAlpha(solidFill.Opacity);
                    }
                    else
                    {
                        img.color = Color.white;
                    }

                    if (gradientFill.IsDefault() == false)
                    {
                        monoBeh.CanvasDrawer.ImageDrawer.AddDaGradient(gradientFill, img.gameObject);
                    }
                }
                else if (hasStroke)
                {
                    if (solidStroke.IsDefault() == false)
                    {
                        img.color = solidStroke.Color.SetFigmaAlpha(solidFill.Opacity);
                    }
                    else
                    {
                        img.color = Color.white;
                    }

                    if (gradientStroke.IsDefault() == false)
                    {
                        monoBeh.CanvasDrawer.ImageDrawer.AddDaGradient(gradientStroke, img.gameObject);
                    }
                }
                else
                {
                    fobject.Data.GameObject.TryDestroyComponent<Outline>();
                }
            }
            else if (fobject.IsGenerativeType())
            {
                if (hasFills && hasStroke)//no need colorize
                {
                    if (fobject.StrokeAlign == "OUTSIDE")
                    {
                        AddUnityOutline(fobject, img, solidStroke, gradientStroke);
                    }
                }
                else if (hasFills)
                {
                    if (solidFill.IsDefault() == false)
                    {
                        img.color = solidFill.Color.SetFigmaAlpha(solidFill.Opacity);
                    }
                    else
                    {
                        img.color = Color.white;
                    }

                    if (gradientFill.IsDefault() == false)
                    {
                        monoBeh.CanvasDrawer.ImageDrawer.AddDaGradient(gradientFill, img.gameObject);
                    }
                }
                else if (hasStroke)
                {
                    if (solidStroke.IsDefault() == false)
                    {
                        img.color = solidStroke.Color.SetFigmaAlpha(solidFill.Opacity);
                    }
                    else
                    {
                        img.color = Color.white;
                    }

                    if (gradientStroke.IsDefault() == false)
                    {
                        monoBeh.CanvasDrawer.ImageDrawer.AddDaGradient(gradientStroke, img.gameObject);
                    }
                }
            }
            else if (fobject.IsDownloadableType())
            {
                if (fobject.Data.SingleColor.IsDefault() == false)
                {
                    img.color = fobject.Data.SingleColor;
                }
                else
                {
                    img.color = Color.white;
                }
            }
        }
        private void AddUnityOutline(FObject fobject, Image img, Paint solidStroke, Paint gradientStroke)
        {
            img.gameObject.TryAddComponent(out Outline outline);
            outline.effectDistance = new Vector2(fobject.StrokeWeight, -fobject.StrokeWeight);

            if (solidStroke.IsDefault() == false)
            {
                outline.effectColor = solidStroke.Color.SetFigmaAlpha(solidStroke.Opacity);
            }
            else if (gradientStroke.IsDefault() == false)
            {
                List<GradientColorKey> gradientColorKeys = monoBeh.CanvasDrawer.ImageDrawer.GetGradientColorKeys(gradientStroke);
                outline.effectColor = gradientColorKeys.First().color;
            }
            else
            {
                outline.effectColor = default;
            }
        }

    }
}