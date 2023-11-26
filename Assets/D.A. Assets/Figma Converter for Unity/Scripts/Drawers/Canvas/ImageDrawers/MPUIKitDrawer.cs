using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DA_Assets.Shared.Extensions;
using DA_Assets.FCU.Extensions;
using System.Reflection;

#pragma warning disable CS0649

#if MPUIKIT_EXISTS
using MPUIKIT;
#endif


namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    public class MPUIKitDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject, Sprite sprite, GameObject target)
        {
#if MPUIKIT_EXISTS
            target.TryAddGraphic(out MPImage img);

            img.sprite = sprite;

            SetColor(fobject, img);

            img.type = monoBeh.Settings.MPUIKitSettings.Type;
            img.raycastTarget = monoBeh.Settings.MPUIKitSettings.RaycastTarget;
            img.preserveAspect = monoBeh.Settings.MPUIKitSettings.PreserveAspect;
#if UNITY_2020_1_OR_NEWER
            img.raycastPadding = monoBeh.Settings.MPUIKitSettings.RaycastPadding;
#endif
            if (fobject.Type == "ELLIPSE")
            {
                img.DrawShape = DrawShape.Circle;
                img.Circle = new Circle
                {
                    FitToRect = true
                };
            }
            else
            {
                img.DrawShape = DrawShape.Rectangle;

                img.Rectangle = new Rectangle
                {
                    CornerRadius = fobject.GetCornerRadius()
                };
            }

            img.type = monoBeh.Settings.MPUIKitSettings.Type;
            img.raycastTarget = monoBeh.Settings.MPUIKitSettings.RaycastTarget;
            img.FalloffDistance = monoBeh.Settings.MPUIKitSettings.FalloffDistance;

            MethodInfo initMethod = typeof(MPImage).GetMethod("Init", BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
            initMethod.Invoke(img, null);
#endif
        }

#if MPUIKIT_EXISTS
        public void SetColor(FObject fobject, MPImage img)
        {
            bool hasFills = fobject.TryGetFills(monoBeh, out Paint solidFill, out Paint gradientFill);
            bool hasStroke = fobject.TryGetStrokes(monoBeh, out Paint solidStroke, out Paint gradientStroke);

            monoBeh.Log($"SetUnityImageColor | {fobject.Data.Hierarchy} | {fobject.Data.FcuImageType} | hasFills: {hasFills} | hasStroke: {hasStroke}");

            img.GradientEffect = new GradientEffect
            {
                Enabled = false,
                GradientType = MPUIKIT.GradientType.Linear,
                Gradient = null
            };

            if (fobject.IsDrawableType())
            {
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
                        AddGradient(gradientFill, img);
                    }
                }

                if (hasStroke)
                {
                    img.OutlineWidth = fobject.StrokeWeight;

                    if (gradientStroke.IsDefault())
                    {
                        img.OutlineColor = solidStroke.Color.SetFigmaAlpha(solidStroke.Opacity);
                    }
                    else
                    {
                        List<GradientColorKey> gradientColorKeys = monoBeh.CanvasDrawer.ImageDrawer.GetGradientColorKeys(gradientStroke);
                        img.OutlineColor = gradientColorKeys.First().color;
                    }
                }
                else
                {
                    img.OutlineWidth = 0;
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


        public void AddGradient(Paint gradientColor, MPImage img)
        {
            Gradient gradient = new Gradient
            {
                mode = GradientMode.Blend,
            };

            img.GradientEffect = new GradientEffect
            {
                Enabled = true,
                GradientType = MPUIKIT.GradientType.Linear,
                Gradient = gradient,
                Rotation = monoBeh.CanvasDrawer.ImageDrawer.CalculateAngleInDegrees(gradientColor.GradientHandlePositions)
            };

            List<GradientColorKey> gradientColorKeys = monoBeh.CanvasDrawer.ImageDrawer.GetGradientColorKeys(gradientColor);
            gradient.colorKeys = gradientColorKeys.ToArray();
        }
        #endif
    }
}
