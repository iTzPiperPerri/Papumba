using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    [Serializable]
    public class MaskDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject)
        {
            FObject target = monoBeh.CurrentProject.GetByIndex(fobject.Data.ParentIndex);

            GameObject targetGo;

            if (fobject.IsObjectMask())
            {
                targetGo = target.Data.GameObject;
            }
            else
            {
                targetGo = fobject.Data.GameObject;
            }

            if (fobject.IsFrameMask() || fobject.IsClipMask())
            {
                targetGo.TryAddComponent(out RectMask2D unityMask);
            }
            else if (fobject.IsObjectMask())
            {
                monoBeh.CanvasDrawer.ImageDrawer.Draw(fobject, targetGo);
                targetGo.TryAddComponent(out Mask unityMask);
                unityMask.showMaskGraphic = false;
            }
        }
    }
}
