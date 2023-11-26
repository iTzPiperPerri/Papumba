using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0649

namespace DA_Assets.FCU.Drawers.CanvasDrawers
{
    [Serializable]
    public class AutoLayoutDrawer : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Draw(FObject fobject)
        {
            foreach (int index in fobject.Data.ChildIndexes)
            {
                FObject child = monoBeh.CurrentProject.GetByIndex(index);
                this.LayoutElementDrawer.Draw(child);
            }

            if (fobject.Data.GameObject.TryGetComponent(out LayoutGroup oldLayoutGroup))
            {
                oldLayoutGroup.Destroy();
            }

            if (fobject.LayoutWrap == "WRAP")
            {
                this.GridLayoutDrawer.Draw(fobject);
            }
            else if (fobject.LayoutMode == "HORIZONTAL")
            {
                this.HorLayoutDrawer.Draw(fobject);
            }
            else if (fobject.LayoutMode == "VERTICAL")
            {
                this.VertLayoutDrawer.Draw(fobject);
            }
        }



        [SerializeField] GridLayoutDrawer gridLayoutDrawer;
        [DASerialization(nameof(gridLayoutDrawer))]
        public GridLayoutDrawer GridLayoutDrawer => gridLayoutDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] VertLayoutDrawer vertLayoutDrawer;
        [DASerialization(nameof(vertLayoutDrawer))]
        public VertLayoutDrawer VertLayoutDrawer => vertLayoutDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] HorLayoutDrawer horLayoutDrawer;
        [DASerialization(nameof(horLayoutDrawer))]
        public HorLayoutDrawer HorLayoutDrawer => horLayoutDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] FittersDrawer fittersDrawer;
        [DASerialization(nameof(fittersDrawer))]
        public FittersDrawer FittersDrawer => fittersDrawer.SetMonoBehaviour(monoBeh);

        [SerializeField] LayoutElementDrawer layoutElementDrawer;
        [DASerialization(nameof(layoutElementDrawer))]
        public LayoutElementDrawer LayoutElementDrawer => layoutElementDrawer.SetMonoBehaviour(monoBeh);
    }
}
