using DA_Assets.Shared;
using DA_Assets.FCU;
using UnityEngine;

namespace DA_Assets.Shared
{
    internal class Footer
    {
        public static DAInspector gui => DAInspector.Instance;
        public static void DrawFooter()
        {
            gui.Space30();

            if (gui.LinkButton(FcuLocKey.label_made_by.Localize(DAConstants.Publisher)))
            {
                Application.OpenURL(DAConstants.SiteLink);
            }
        }
    }
}
