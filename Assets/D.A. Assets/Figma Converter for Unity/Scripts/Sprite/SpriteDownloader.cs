using DA_Assets.FCU.Extensions;
using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;

namespace DA_Assets.FCU
{
    [Serializable]
    public class SpriteDownloader : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public IEnumerator SetMissingImageLinks(List<FObject> fobjects, List<IdLink> idLinks)
        {
            int settedLinksCount = 0;

            for (int i = 0; i < fobjects.Count(); i++)
            {
                for (int j = 0; j < idLinks.Count(); j++)
                {
                    if (fobjects[i].Id == idLinks[j].Id)
                    {
                        if (idLinks[j].Link.IsEmpty())
                        {
                            DALogger.LogError($"Can't get link, please check the following component: {fobjects[i].Data.Hierarchy}");
                            fobjects[i].Data.NeedDownload = false;
                        }
                        else
                        {
                            fobjects[i].Data.Link = idLinks[j].Link;
                        }

                        settedLinksCount++;
                    }
                }
            }

            DALogger.Log(FcuLocKey.log_links_added.Localize(settedLinksCount, idLinks.Count()));
            yield return null;
        }

        public IEnumerator GetMissingSpriteLinks(List<FObject> fobjects, Action<List<IdLink>> callback)
        {
            List<string> fobjectIds = fobjects.Select(x => x.Id).ToList();

            List<List<string>> idChunks = fobjectIds.Split(FcuConfig.Instance.ChunkSizeGetSpriteLinks);
            List<List<IdLink>> linkChunks = new List<List<IdLink>>();

            foreach (List<string> chunk in idChunks)
            {
                Request request = RequestCreator.CreateImageLinksRequest(monoBeh.Settings.MainSettings.ProjectUrl, chunk, monoBeh);

                GetChunkImageLinks(request, result =>
                {
                    if (result.Success)
                    {
                        linkChunks.Add(result.Result);
                    }
                    else
                    {
                        monoBeh.ProjectImporter.ImportErrorCount++;
                        linkChunks.Add(default);

                        switch (result.Error.Status)
                        {
                            case 404:
                                DALogger.LogError(FcuLocKey.log_cant_get_images.Localize(FcuLocKey.label_figma_comp.Localize()));
                                break;
                            default:
                                DALogger.LogError(FcuLocKey.log_cant_get_image_links.Localize(result.Error.Error, result.Error.Status));
                                break;
                        }
                    }
                }).StartDARoutine(monoBeh);
            }

            int tempCount = -1;
            while (DALogger.WriteLogBeforeEqual(idChunks, linkChunks, FcuLocKey.log_getting_links, linkChunks.FromChunks(), fobjectIds, ref tempCount))
            {
                yield return WaitFor.Delay1();
            }

            List<IdLink> missingImageLinks = linkChunks.FromChunks();

            if (FcuConfig.Instance.Https == false)
            {
                missingImageLinks.ForEach(x => x.Link = x.Link?.Replace("https://", "http://"));
            }

            callback.Invoke(missingImageLinks);
        }

        public IEnumerator GetChunkImageLinks(Request request, Return<List<IdLink>, FigmaError> @return)
        {
            RoutineResult<List<IdLink>, FigmaError> routineResult = new RoutineResult<List<IdLink>, FigmaError>();

            yield return monoBeh.RequestSender.SendRequest<FigmaImageRequest>(request, result =>
            {
                if (result.Success)
                {
                    List<IdLink> chunkImageLinks = new List<IdLink>();

                    foreach (KeyValuePair<string, string> image in result.Result.images)
                    {
                        chunkImageLinks.Add(new IdLink
                        {
                            Id = image.Key,
                            Link = image.Value
                        });
                    }

                    routineResult.Success = true;
                    routineResult.Result = chunkImageLinks;
                }
                else
                {
                    routineResult.Success = false;
                    routineResult.Error = result.Error;
                }
            });

            @return.Invoke(routineResult);
        }

        public IEnumerator DownloadSprites(List<FObject> fobjects)
        {
            List<FObject> needDownload = fobjects.Where(x => x.Data.NeedDownload).ToList();

            if (needDownload.IsEmpty())
            {
                yield break;
            }

            List<IdLink> missingImageLinks = new List<IdLink>();

            yield return GetMissingSpriteLinks(needDownload, @return => missingImageLinks = @return);
            yield return SetMissingImageLinks(needDownload, missingImageLinks);

            DALogger.Log(FcuLocKey.log_start_download_images.Localize());

            int requestCount = 0;

            foreach (List<FObject> chunk in needDownload.Split(FcuConfig.Instance.ChunkSizeDownloadSprites))
            {
                ConcurrentBag<DownloadedSprite> images = new ConcurrentBag<DownloadedSprite>();

                foreach (FObject item in chunk)
                {
                    Request request = new Request
                    {
                        RequestType = RequestType.GetFile,
                        Query = item.Data.Link
                    };

                    if (request.Query.IsEmpty())
                    {
                        DALogger.LogError(
                            FcuLocKey.log_malformed_url.Localize(item.Data.Hierarchy,
                            FcuLocKey.label_components_settings.Localize()));

                        images.Add(default);
                        continue;
                    }

                    monoBeh.RequestSender.SendRequest<byte[]>(request, (result) =>
                    {
                        if (result.Success)
                        {
                            images.Add(new DownloadedSprite
                            {
                                FObject = item,
                                SpriteBytes = result.Result
                            });
                        }
                        else
                        {
                            monoBeh.ProjectImporter.ImportErrorCount++;
                            images.Add(default);

                            switch (result.Error.Status)
                            {
                                case 909:
                                    DALogger.LogError(FcuLocKey.log_ssl_error.Localize(result.Error.Error, result.Error.Status));
                                    monoBeh.Events.OnImportFail?.Invoke(monoBeh);
                                    monoBeh.AssetTools.StopImport();
                                    break;
                                default:
                                    DALogger.LogError(FcuLocKey.cant_download_sprite.Localize(item.Data.Hierarchy, result.Error.Error, result.Error.Status));
                                    break;
                            }
                        }

                        requestCount++;
                    }).StartDARoutine(monoBeh);
                }

                int tempCount = -1;
                while (DALogger.WriteLogBeforeEqual(images, chunk, FcuLocKey.log_downloading_images, ref tempCount))
                {
                    yield return WaitFor.Delay1();
                }

                foreach (DownloadedSprite image in images)
                {
                    if (image.IsDefault() || image.SpriteBytes == null || image.SpriteBytes.Length < 1)
                    {
                        continue;
                    }

                    File.WriteAllBytes(image.FObject.Data.SpritePath, image.SpriteBytes);
                }

                if (requestCount != 0 && requestCount % FcuConfig.Instance.ApiRequestsCountLimit == 0)
                {
                    DALogger.Log(FcuLocKey.log_api_waiting.Localize(FcuConfig.Instance.ApiTimeoutSec, needDownload.Count() - requestCount, requestCount, needDownload.Count()));
                    yield return WaitFor.Delay(FcuConfig.Instance.ApiTimeoutSec);
                }
            }
        }
    }
    public struct FigmaImageRequest
    {
        [DataMember(Name = "err")] public string error;
        [DataMember(Name = "images")] public Dictionary<string, string> images;
    }

    public struct DownloadedSprite
    {
        public FObject FObject;
        public byte[] SpriteBytes;
    }

    public struct IdLink
    {
        public string Id;
        public string Link;
    }
}
