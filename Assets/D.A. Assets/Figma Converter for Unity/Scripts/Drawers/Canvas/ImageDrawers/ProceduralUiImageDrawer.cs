using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using UnityEngine;

#if PUI_EXISTS
using UnityEngine.UI.ProceduralImage;
#endif

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    public class ProceduralUiImageDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject, Sprite sprite, GameObject target)
        {
#if PUI_EXISTS            
            target.TryAddGraphic(out ProceduralImage img);
            img.sprite = sprite;

            img.type = monoBeh.Settings.PuiSettings.Type;
            img.raycastTarget = monoBeh.Settings.PuiSettings.RaycastTarget;
            img.preserveAspect = monoBeh.Settings.PuiSettings.PreserveAspect;
#if UNITY_2020_1_OR_NEWER
            img.raycastPadding = monoBeh.Settings.PuiSettings.RaycastPadding;
#endif
            if (fobject.Type == "ELLIPSE")
            {
                target.TryAddComponent(out RoundModifier roundModifier);
            }
            else
            {
                if (fobject.CornerRadiuses != null)
                {
                    target.TryAddComponent(out FreeModifier freeModifier);

                    freeModifier.Radius = fobject.GetCornerRadius();
                }
                else
                {
                    target.TryAddComponent(out UniformModifier uniformModifier);
                    uniformModifier.Radius = fobject.CornerRadius.ToFloat();
                }
            }

            SetColor(fobject, img);

            switch (fobject.Data.FcuImageType)
            {
                case FcuImageType.Generative:
                case FcuImageType.Drawable:          
                    monoBeh.CanvasDrawer.ImageDrawer.UnityImageDrawer.SetUnityImageColor(fobject, img);
                    break;
            }

            img.type = monoBeh.Settings.PuiSettings.Type;
            img.raycastTarget = monoBeh.Settings.PuiSettings.RaycastTarget;
            img.FalloffDistance = monoBeh.Settings.PuiSettings.FalloffDistance;
#endif
        }

#if PUI_EXISTS
        private void SetColor(FObject fobject, ProceduralImage img)
        {
            if (fobject.IsDownloadableType())
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
#endif
    }
}
