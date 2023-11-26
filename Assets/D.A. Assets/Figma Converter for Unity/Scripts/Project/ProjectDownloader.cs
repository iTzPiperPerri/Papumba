using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;

namespace DA_Assets.FCU
{
    [Serializable]
    public class ProjectDownloader : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public IEnumerator DownloadProject()
        {
            monoBeh.InspectorDrawer.SelectableFrames.Clear();

            if (monoBeh.FigmaSession.IsAuthed() == false)
            {
                DALogger.LogError(FcuLocKey.log_need_auth.Localize());
                monoBeh.Events.OnProjectDownloadFail?.Invoke(monoBeh);
                yield break;
            }

            monoBeh.Events.OnProjectDownloadStart?.Invoke(monoBeh);

            RoutineResult<FigmaProject, FigmaError> result = default;

            yield return DownloadProject(monoBeh.Settings.MainSettings.ProjectUrl, x => result = x);

            if (result.Success)
            {
                monoBeh.CurrentProject.FigmaProject = result.Result;

                yield return monoBeh.InspectorDrawer.FillSelectableFramesArray(fromCache: false);

                DALogger.Log(FcuLocKey.log_project_downloaded.Localize());

                monoBeh.Events.OnProjectDownloaded?.Invoke(monoBeh);
            }
            else
            {
                switch (result.Error.Status)
                {
                    case 403:
                        DALogger.LogError(FcuLocKey.log_need_auth.Localize());
                        break;
                    case 404:
                        DALogger.LogError(FcuLocKey.log_project_not_found.Localize());
                        break;
                    default:
                        DALogger.LogError(FcuLocKey.log_unknown_error.Localize(result.Error.Error, result.Error.Status));
                        break;
                }

                monoBeh.Events.OnProjectDownloadFail?.Invoke(monoBeh);
                monoBeh.AssetTools.StopImport();
            }
        }

        public IEnumerator DownloadProject(string projectUrl, Return<FigmaProject, FigmaError> @return)
        {
            Request projectRequest = RequestCreator.CreateProjectRequest(
                monoBeh.FigmaSession.Token,
                projectUrl);

            yield return monoBeh.RequestSender.SendRequest(projectRequest, @return);
        }
    }
}
