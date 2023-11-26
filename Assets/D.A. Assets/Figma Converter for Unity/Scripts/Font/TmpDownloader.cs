using DA_Assets.FCU.Extensions;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using DA_Assets.FCU.Model;
using DA_Assets.Shared.CssNet;
using UnityEngine;

#if TextMeshPro
using TMPro;
#endif

namespace DA_Assets.FCU
{
    [Serializable]
    public class TmpDownloader : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] int samplingPointSize = 90;
        [SerializeField] int atlasPadding = 5;
        [SerializeField] UnityEngine.TextCore.LowLevel.GlyphRenderMode renderMode = UnityEngine.TextCore.LowLevel.GlyphRenderMode.SDFAA;
        [SerializeField] int atlasWidth = 512;
        [SerializeField] int atlasHeight = 512;
#if TextMeshPro
        [SerializeField] AtlasPopulationMode atlasPopulationMode = AtlasPopulationMode.Dynamic;
#endif
        [SerializeField] bool enableMultiAtlasSupport = true;

        [Space]

        [SerializeField] UnityFonts unityTmpFonts;

        public IEnumerator CreateFonts(List<FontMetadata> figmaFonts)
        {
#if TextMeshPro
            unityTmpFonts = monoBeh.FontDownloader.FindUnityFonts(figmaFonts, monoBeh.FontLoader.TmpFonts);
#endif
            if (unityTmpFonts.Missing.Count == 0)
            {
                yield return SetFallbackFontAssets(figmaFonts);
                yield break;
            }

            List<FontStruct> generated = new List<FontStruct>();
            List<FontStruct> notGenerated = new List<FontStruct>();

            foreach (FontStruct missingFont in unityTmpFonts.Missing)
            {
                GenerateFont(missingFont, @return =>
                {
                    generated.Add(@return.Result);

                    if (@return.Success == false)
                    {
                        notGenerated.Add(missingFont);
                    }

                }).StartDARoutine(monoBeh);
            }

            int tempCount = -1;
            while (DALogger.WriteLogBeforeEqual(generated, unityTmpFonts.Missing, FcuLocKey.log_generating_tmp_fonts, ref tempCount))
            {
                yield return WaitFor.Delay1();
            }

            if (notGenerated.Count > 0)
            {
                monoBeh.FontDownloader.PrintFontNames(FcuLocKey.cant_generate_fonts, notGenerated);
            }

#if UNITY_EDITOR
            UnityEditor.AssetDatabase.Refresh();
#endif
            yield return monoBeh.FontLoader.AddToTmpMeshFontsList();

            yield return SetFallbackFontAssets(figmaFonts);
        }

        private IEnumerator SetFallbackFontAssets(List<FontMetadata> figmaFonts)
        {
#if TextMeshPro
            unityTmpFonts = monoBeh.FontDownloader.FindUnityFonts(figmaFonts, monoBeh.FontLoader.TmpFonts);

            if (unityTmpFonts.Existing.Count == 0)
                yield break;

            FontStruct latinFont = unityTmpFonts.Existing.FirstOrDefault(x => x.FontSubset == FontSubset.Latin);

            if (latinFont.IsDefault())
                yield break;


            TMP_FontAsset latinFontAsset = latinFont.Font as TMP_FontAsset;

            foreach (FontStruct fontStruct in unityTmpFonts.Existing)
            {
                if (fontStruct.FontSubset == FontSubset.Latin)
                    continue;

                TMP_FontAsset tmpFontAsset = fontStruct.Font as TMP_FontAsset;

                if (latinFontAsset.fallbackFontAssetTable.IsEmpty())
                {
                    latinFontAsset.fallbackFontAssetTable = new List<TMP_FontAsset>();
                }

                latinFontAsset.fallbackFontAssetTable.Add(tmpFontAsset);
            }


            latinFontAsset.fallbackFontAssetTable = latinFontAsset.fallbackFontAssetTable.Where(x => x != null).ToList();
            latinFontAsset.fallbackFontAssetTable = latinFontAsset.fallbackFontAssetTable.Distinct().ToList();
#endif

            yield return null;
        }

        private IEnumerator GenerateFont(FontStruct fs, Return<FontStruct, string> @return)
        {
#if UNITY_EDITOR
            RoutineResult<string, string> unicodeRangeResult = default;
            yield return GetUnicodeRange(fs.FontMetadata.Family, fs.FontSubset, x => unicodeRangeResult = x);

            string unicodeRange = null;

            if (unicodeRangeResult.Success == false)
            {
                @return.Invoke(new RoutineResult<FontStruct, string>
                {
                    Error = unicodeRangeResult.Error,
                    Success = false
                });

                yield break;
            }
            else
            {
                unicodeRange = unicodeRangeResult.Result;
            }

            string baseFontName = monoBeh.FontDownloader.GetBaseFileName(fs);

            string tmpPath = Path.Combine(monoBeh.FontLoader.TmpFontsPath, $"{baseFontName}.asset");
            Directory.CreateDirectory(monoBeh.FontLoader.TmpFontsPath);

            Font ttfFont = monoBeh.FontLoader.TtfFonts.FirstOrDefault(x => baseFontName == x.name.FormatFontName());

            if (ttfFont.IsDefault())
            {
                @return.Invoke(new RoutineResult<FontStruct, string>
                {
                    Error = "Can't find ttf font.",
                    Success = false
                });

                yield break;
            }
#if TextMeshPro
            try
            {
                TMP_FontAsset tmpFontAsset = TMP_FontAsset.CreateFontAsset(
                    ttfFont,
                    samplingPointSize,
                    atlasPadding,
                    renderMode,
                    atlasWidth,
                    atlasHeight,
                    atlasPopulationMode
#if UNITY_2020_1_OR_NEWER
                    , enableMultiAtlasSupport);
#else
                    );
#endif

                UnityEditor.AssetDatabase.CreateAsset(tmpFontAsset, tmpPath.ToRelativePath());

                tmpFontAsset.material.name = $"{baseFontName} Atlas Material";
                tmpFontAsset.atlasTexture.name = $"{baseFontName} Atlas";

                UnityEditor.AssetDatabase.AddObjectToAsset(tmpFontAsset.material, tmpFontAsset);
                UnityEditor.AssetDatabase.AddObjectToAsset(tmpFontAsset.atlasTexture, tmpFontAsset);

                UnityEditor.EditorUtility.SetDirty(tmpFontAsset);

                UnityEditor.AssetDatabase.SaveAssets();

                tmpFontAsset.TryAddCharacters(unicodeRange, out string __);

                UnityEditor.EditorUtility.SetDirty(tmpFontAsset);
                UnityEditor.AssetDatabase.SaveAssets();

                @return.Invoke(new RoutineResult<FontStruct, string>
                {
                    Result = fs,
                    Success = true
                });

            }
            catch (Exception)
            {
                @return.Invoke(new RoutineResult<FontStruct, string>
                {
                    Error = "Can't create font asset.",
                    Success = false
                });

                yield break;
            }
#endif
#endif
            yield return null;
        }

        private IEnumerator GetUnicodeRange(string family, FontSubset fontSubset, Return<string, string> @return)
        {
            string url = $"https://fonts.googleapis.com/css2?family=" + family;

            Request request = new Request
            {
                RequestType = RequestType.Get,
                Query = url,
                RequestHeader = new RequestHeader
                {
                    Name = "User-Agent",
                    Value = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/114.0.0.0 Safari/537.36"
                }
            };

            string css = null;
            yield return monoBeh.RequestSender.SendRequest<string>(request, @return2 => css = @return2.Result);

            try
            {
                CssStylesheet ss = new CssStylesheet(css);

                string unicodeRange = null;

                foreach (CssSelector item in ss.selectors)
                {
                    string formatedName = item.Name
                            .Replace("}", "")
                            .Replace("{", "")
                            .Replace(" ", "")
                            .Replace("/*", "")
                            .Replace("*/", "")
                            .Replace("@font-face", "")
                            .Replace("-", "")
                            .ToLower()
                            .Trim();

                    if (formatedName == fontSubset.ToLower())
                    {
                        unicodeRange = item.rules.First(x => x.rule == "unicode-range").objectValue
                            .ToString()
                            .Replace("U+", "")
                            .Replace(" ", "")
                            .Trim();

                        break;
                    }
                }

                @return.Invoke(new RoutineResult<string, string>
                {
                    Success = unicodeRange.IsEmpty() == false,
                    Result = unicodeRange
                });
            }
            catch (Exception ex)
            {
                DALogger.LogError(ex);

                @return.Invoke(new RoutineResult<string, string>
                {
                    Success = false,
                    Error = ex.Message
                });
            }
        }

        public int SamplingPointSize { get => samplingPointSize; set => SetValue(ref samplingPointSize, value); }
        public int AtlasPadding { get => atlasPadding; set => SetValue(ref atlasPadding, value); }
        public UnityEngine.TextCore.LowLevel.GlyphRenderMode RenderMode { get => renderMode; set => SetValue(ref renderMode, value); }
        public int AtlasWidth { get => atlasWidth; set => SetValue(ref atlasWidth, value); }
        public int AtlasHeight { get => atlasHeight; set => SetValue(ref atlasHeight, value); }
#if TextMeshPro
        public AtlasPopulationMode AtlasPopulationMode { get => atlasPopulationMode; set => SetValue(ref atlasPopulationMode, value); }
#endif
        public bool EnableMultiAtlasSupport { get => enableMultiAtlasSupport; set => SetValue(ref enableMultiAtlasSupport, value); }
    }

    [Serializable]
    public struct UnityFonts
    {
        [SerializeField] List<FontStruct> missing;
        [SerializeField] List<FontStruct> existing;

        public List<FontStruct> Existing { get => existing; set => existing = value; }
        public List<FontStruct> Missing { get => missing; set => missing = value; }
    }
}