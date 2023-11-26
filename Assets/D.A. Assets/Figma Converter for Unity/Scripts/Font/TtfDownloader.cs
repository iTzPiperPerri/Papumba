using DA_Assets.FCU.Extensions;
using DA_Assets.Shared;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

namespace DA_Assets.FCU
{
    [Serializable]
    public class TtfDownloader : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public IEnumerator Download(List<FontMetadata> figmaFonts)
        {
            UnityFonts unityTtfFonts = monoBeh.FontDownloader.FindUnityFonts(figmaFonts, monoBeh.FontLoader.TtfFonts);

            if (unityTtfFonts.Missing.Count == 0)
                yield break;

            yield return monoBeh.FontDownloader.GFontsApi.GetGoogleFontsBySubset(monoBeh.FontDownloader.GFontsApi.FontSubsets);

            List<FontStruct> downloaded = new List<FontStruct>();
            List<FontStruct> notDownloaded = new List<FontStruct>();

            foreach (FontStruct missingFont in unityTtfFonts.Missing)
            {
                DownloadFont(missingFont, @return =>
                {
                    downloaded.Add(@return.Result);

                    if (@return.Success == false)
                    {
                        notDownloaded.Add(missingFont);
                    }

                }).StartDARoutine(monoBeh);
            }

            int tempCount = -1;
            while (DALogger.WriteLogBeforeEqual(downloaded, unityTtfFonts.Missing, FcuLocKey.log_downloading_fonts, ref tempCount))
            {
                yield return WaitFor.Delay1();
            }

            if (notDownloaded.Count > 0)
            {
                monoBeh.FontDownloader.PrintFontNames(FcuLocKey.cant_download_fonts, notDownloaded);
            }

            yield return SaveTtfFonts(downloaded);

            yield return monoBeh.FontLoader.AddToTtfFontsList();
        }


        public IEnumerator DownloadFont(FontStruct missingFont, Return<FontStruct, string> @return)
        {
            FontItem fontItem = monoBeh.FontDownloader.GFontsApi.GetFontItem(missingFont.FontMetadata, missingFont.FontSubset);

            if (fontItem.IsDefault())
            {
                @return.Invoke(new RoutineResult<FontStruct, string>
                {
                    Error = "Font not found in Google Fonts",
                    Success = false
                });

                yield break;
            }

            string fontUrl = monoBeh.FontDownloader.GFontsApi.GetUrlByWeight(
                fontItem,
                missingFont.FontMetadata.Weight,
                missingFont.FontMetadata.FontStyle);

            Request request = new Request
            {
                RequestType = RequestType.GetFile,
                Query = fontUrl
            };

            yield return monoBeh.RequestSender.SendRequest<byte[]>(request, @return2 =>
            {
                if (@return2.Success)
                {
                    FontStruct res = missingFont;
                    res.Bytes = @return2.Result;

                    @return.Invoke(new RoutineResult<FontStruct, string>
                    {
                        Result = res,
                        Success = true
                    });
                }
                else
                {
                    @return.Invoke(new RoutineResult<FontStruct, string>
                    {
                        Error = @return2.Error.Error,
                        Success = false
                    });
                }
            });
        }

        public IEnumerator SaveTtfFonts(List<FontStruct> downloadedFonts)
        {
#if UNITY_EDITOR
            foreach (FontStruct fs in downloadedFonts)
            {
                if (fs.Bytes == null || fs.Bytes.Length < 1)
                {
                    continue;
                }

                try
                {
                    string baseFontName = monoBeh.FontDownloader.GetBaseFileName(fs);

                    string ttfPath = Path.Combine(monoBeh.FontLoader.TtfFontsPath, $"{baseFontName}.ttf");

                    Directory.CreateDirectory(monoBeh.FontLoader.TtfFontsPath);
                    File.WriteAllBytes(ttfPath, fs.Bytes);
                }
                catch (Exception ex)
                {
                    DALogger.LogError(ex);
                }

                yield return null;
            }

            UnityEditor.AssetDatabase.Refresh();
#endif
            yield return null;
        }
    }
}
