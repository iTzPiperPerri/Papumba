using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;

namespace DA_Assets.FCU
{
    [Serializable]
    public class FcuEventHandlers : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public void Auth_OnClick()
        {
            monoBeh.AuthController.Auth().StartDARoutine(monoBeh);
        }

        public void DownloadProject_OnClick()
        {
            monoBeh.AssetTools.StopImport();
            monoBeh.ProjectDownloader.DownloadProject().StartDARoutine(monoBeh);
        }

        public void ImportSelectedFrames_OnClick()
        {
            if (monoBeh.CurrentProject.FigmaProject.Document.Children.IsEmpty())
            {
                DALogger.LogError(FcuLocKey.log_project_empty.Localize());
                return;
            }

            monoBeh.AssetTools.StopImport();
            monoBeh.ProjectImporter.StartImport().StartDARoutine(monoBeh);
        }

        public void CreatePrefabs_OnClick()
        {
            monoBeh.PrefabCreator.CreatePrefabs().StartDARoutine(monoBeh);
        }

        public void DestroyLastImportedFrames_OnClick()
        {
            monoBeh.AssetTools.DestroyLastImportedFrames().StartDARoutine(monoBeh);
        }

        public void DestroyChilds_OnClick()
        {
            monoBeh.AssetTools.DestroyChilds().StartDARoutine(monoBeh);
        }

        public void DestroySyncHelpers_OnClick()
        {
            monoBeh.SyncHelpers.DestroySyncHelpers().StartDARoutine(monoBeh);
        }

        public void SetFcuToSyncHelpers_OnClick()
        {
            monoBeh.SyncHelpers.SetFcuToAllSyncHelpers().StartDARoutine(monoBeh);
        }

        public static void CreateFcu_OnClick()
        {
            AssetTools.CreateFcuOnScene();
        }

        public void StopImport_OnClick()
        {
            monoBeh.AssetTools.StopImport();
        }
    }
}