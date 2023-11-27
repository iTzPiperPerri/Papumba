using DA_Assets.DAG;
using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GradientStop = DA_Assets.FCU.Model.GradientStop;

#pragma warning disable CS0649

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    [Serializable]
    public class ImageDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject, GameObject customGameObject = null)
        {
            GameObject target = customGameObject == null ? fobject.Data.GameObject : customGameObject;

            if (fobject.Data.GameObject.IsPartOfAnyPrefab() == false)
            {
                if (target.TryGetComponent(out Graphic oldGraphic))
                {
                    oldGraphic.Destroy();
                }
            }

            Sprite sprite = null;

            switch (fobject.Data.FcuImageType)
            {
                case FcuImageType.Downloadable:
                case FcuImageType.Generative:
                    sprite = monoBeh.SpriteWorker.GetSprite(fobject);
                    break;
            }

            if (monoBeh.UsingUnityImage() || fobject.IsObjectMask())
            {
                this.UnityImageDrawer.Draw(fobject, sprite, target);
            }
            else if (monoBeh.UsingShapes2D())
            {
                this.Shapes2DDrawer.Draw(fobject, sprite, target);
            }
            else if (monoBeh.UsingProceduralUI_Image())
            {
                this.ProceduralUiImageDrawer.Draw(fobject, sprite, target);
            }
            else if (monoBeh.UsingMPUIKit())
            {
                this.MPUIKitDrawer.Draw(fobject, sprite, target);
            }

            if (fobject.Data.FcuImageType != FcuImageType.Downloadable)
            {
                if (fobject.IsAllPaintsDisabled())
                {
                    target.GetComponent<Graphic>().enabled = false;
                }
            }
        }

        public List<GradientColorKey> GetGradientColorKeys(Paint gradient)
        {
            List<GradientColorKey> gradientColorKeys = new List<GradientColorKey>();

            foreach (GradientStop gradientStop in gradient.GradientStops)
            {
                gradientColorKeys.Add(new GradientColorKey
                {
                    color = gradientStop.Color,
                    time = gradientStop.Position
                });
            }

            return gradientColorKeys;
        }

        public void AddDaGradient(Paint gradientColor, GameObject go)
        {
            go.TryAddComponent(out DAGradient gradient);

            List<GradientColorKey> gradientColorKeys = GetGradientColorKeys(gradientColor);

            gradient.BlendMode = ColorBlendMode.Multiply;
            gradient.Gradient.colorKeys = gradientColorKeys.ToArray();
            gradient.Angle = CalculateAngleInDegrees(gradientColor.GradientHandlePositions);
        }

        public float CalculateAngleInDegrees(List<Vector2> gradientHandlePositions)
        {
            float radians = Mathf.Atan2(
                gradientHandlePositions[2].y - gradientHandlePositions[0].y,
                gradientHandlePositions[2].x - gradientHandlePositions[0].x
            );

            float degrees = radians * (180 / Mathf.PI);
            degrees = (degrees + 360) % 360; // Normalize angle between 0 and 360 degrees

            return degrees;
        }

        [SerializeField] UnityImageDrawer unityImageDrawer;
        [DASerialization(nameof(unityImageDrawer))]
        public UnityImageDrawer UnityImageDrawer => unityImageDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] Shapes2DDrawer shapes2DDrawer;
        [DASerialization(nameof(shapes2DDrawer))]
        public Shapes2DDrawer Shapes2DDrawer => shapes2DDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] ProceduralUiImageDrawer puiDrawer;
        [DASerialization(nameof(puiDrawer))]
        public ProceduralUiImageDrawer ProceduralUiImageDrawer => puiDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] MPUIKitDrawer mpuikitDrawer;
        [DASerialization(nameof(mpuikitDrawer))]
        public MPUIKitDrawer MPUIKitDrawer => mpuikitDrawer.SetMonoBehaviour(monoBeh);
    }
}