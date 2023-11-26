using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DA_Assets.Shared.Extensions;
using UnityEngine.UI;
using DA_Assets.FCU.Extensions;

#if SHAPES_EXISTS
using Shapes2D;
using Shape = Shapes2D.Shape;
#endif

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    public class Shapes2DDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject, Sprite sprite, GameObject target)
        {
#if SHAPES_EXISTS
            target.TryAddGraphic(out Image img);
            target.TryAddComponent(out Shape shape);

            img.material = null;
            img.sprite = sprite;

            img.type = monoBeh.Settings.Shapes2DSettings.Type;
            img.raycastTarget = monoBeh.Settings.Shapes2DSettings.RaycastTarget;
            img.preserveAspect = monoBeh.Settings.Shapes2DSettings.PreserveAspect;
#if UNITY_2020_1_OR_NEWER
            img.raycastPadding = monoBeh.Settings.Shapes2DSettings.RaycastPadding;
#endif
            switch (fobject.Data.FcuImageType)
            {
                case FcuImageType.Generative:
                case FcuImageType.Drawable:
                    SetColor(fobject, img, shape);
                    monoBeh.CanvasDrawer.ImageDrawer.UnityImageDrawer.SetUnityImageColor(fobject, img);
                    break;
            }

            if (fobject.Type == "ELLIPSE")
            {
                shape.settings.shapeType = ShapeType.Ellipse;
            }
            else
            {
                shape.settings.shapeType = ShapeType.Rectangle;

                if (fobject.CornerRadiuses != null)
                {
                    Vector4 cr = fobject.GetCornerRadius();

                    shape.settings.roundnessPerCorner = true;

                    shape.settings.roundnessBottomLeft = cr.x;
                    shape.settings.roundnessBottomRight = cr.y;
                    shape.settings.roundnessTopRight = cr.z;
                    shape.settings.roundnessTopLeft = cr.w;
                }
                else if (fobject.CornerRadius.ToFloat() != 0)
                {
                    shape.settings.roundnessPerCorner = true;

                    shape.settings.roundnessBottomLeft = fobject.CornerRadius.ToFloat();
                    shape.settings.roundnessBottomRight = fobject.CornerRadius.ToFloat();
                    shape.settings.roundnessTopRight = fobject.CornerRadius.ToFloat();
                    shape.settings.roundnessTopLeft = fobject.CornerRadius.ToFloat();

                    //for new versions only
                    //shape.settings.roundnessPerCorner = false;
                    //shape.settings.roundness = source.CornerRadius;
                }
            }

            foreach (Effect effect in fobject.Effects)
            {
                if (effect.Visible == false)
                {
                    continue;
                }

                if (effect.Type == "LAYER_BLUR")
                {
                    shape.settings.blur = effect.Radius;
                    break;
                }
            }

            img.type = monoBeh.Settings.Shapes2DSettings.Type;
            img.raycastTarget = monoBeh.Settings.Shapes2DSettings.RaycastTarget;
#endif
        }

#if SHAPES_EXISTS
        private void SetColor(FObject fobject, Image img, Shape shape)
        {
            bool hasFill = fobject.TryGetFills(monoBeh, out Paint solidFill, out Paint gradientFill);
            bool hasStroke = fobject.TryGetStrokes(monoBeh, out Paint solidStroke, out Paint gradientStroke);

            shape.settings.fillType = FillType.SolidColor;
            shape.settings.fillColor = Color.white;

            if (gradientFill.IsDefault() == false)
            {
                shape.settings.blur = 0.1f;
            }

            if (hasStroke)
            {
                shape.settings.outlineSize = fobject.StrokeWeight;

                if (gradientStroke.IsDefault())
                {
                    shape.settings.outlineColor = solidStroke.Color.SetFigmaAlpha(solidStroke.Opacity);
                }
                else
                {
                    List<GradientColorKey> gradientColorKeys = monoBeh.CanvasDrawer.ImageDrawer.GetGradientColorKeys(gradientStroke);
                    shape.settings.outlineColor = gradientColorKeys.First().color;
                }
            }
            else
            {
                shape.settings.outlineSize = 0;
            }
        }
#endif

    }
}
