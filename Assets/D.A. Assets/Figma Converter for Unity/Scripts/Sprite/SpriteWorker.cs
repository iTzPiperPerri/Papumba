using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

#pragma warning disable CS0162

namespace DA_Assets.FCU
{
    [Serializable]
    public class SpriteWorker : MonoBehaviourBinder<FigmaConverterUnity>
    {
        public IEnumerator MarkAsSprites(List<FObject> fobjects)
        {
            List<FObject> fobjectWithSprite = fobjects.Where(x => x.Data.SpritePath != null).ToList();

            int allCount = fobjectWithSprite.Count();
            int count = 0;

            DACycles.ForEach(fobjectWithSprite, fobject =>
            {
                SetImgTypeSprite(fobject, () =>
                {
                    count++;
                }).StartDARoutine(monoBeh);
            }, WaitFor.Delay001().WaitTimeF, 100).StartDARoutine(monoBeh);

            int tempCount = -1;
            while (DALogger.WriteLogBeforeEqual(
                ref count,
                ref allCount, 
                FcuLocKey.log_mark_as_sprite.Localize(count, allCount),
                ref tempCount))
            {
                yield return WaitFor.Delay1();
            }

            yield return null;
        }

        public Sprite GetSprite(FObject fobject)
        {
#if UNITY_EDITOR
            Sprite sprite = (Sprite)UnityEditor.AssetDatabase.LoadAssetAtPath(fobject.Data.SpritePath, typeof(Sprite));
            return sprite;
#endif
            return null;
        }



        public IEnumerator SetImgTypeSprite(FObject fobject, Action callback)
        {
#if UNITY_EDITOR
            while (true)
            {
                bool success = SetTextureSettings(fobject);

                if (success)
                {
                    callback.Invoke();
                    break;
                }

                yield return WaitFor.Delay01();
            }
#endif
            yield return null;
        }

        private bool SetTextureSettings(FObject fobject)
        {
            try
            {
#if UNITY_EDITOR
                UnityEditor.TextureImporter importer = UnityEditor.AssetImporter.GetAtPath(fobject.Data.SpritePath) as UnityEditor.TextureImporter;
                importer.GetTextureSize(out int width, out int height);
                fobject.Data.SpriteSize = new Vector2Int(width, height);

                if (importer.isReadable == true &&
                    importer.textureType == UnityEditor.TextureImporterType.Sprite &&
                    importer.crunchedCompression == FcuConfig.Instance.CrunchedCompression &&
                    importer.mipmapEnabled == FcuConfig.Instance.GenerateMipMaps)
                {
                    if (importer.crunchedCompression)
                    {
                        if (importer.compressionQuality == FcuConfig.Instance.CrunchedCompressionQuality)
                        {
                            return true;
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return true;
                    }
                }

                importer.isReadable = true;
                importer.textureType = UnityEditor.TextureImporterType.Sprite;
                importer.crunchedCompression = FcuConfig.Instance.CrunchedCompression;
                importer.textureCompression = FcuConfig.Instance.TextureImporterCompression;
                importer.mipmapEnabled = FcuConfig.Instance.GenerateMipMaps;

                if (importer.crunchedCompression)
                {
                    importer.compressionQuality = FcuConfig.Instance.CrunchedCompressionQuality;
                }

                importer.SetMaxTextureSize(width, height);

                UnityEditor.AssetDatabase.WriteImportSettingsIfDirty(fobject.Data.SpritePath);
                UnityEditor.AssetDatabase.Refresh();

                return false;
#else
                throw new NotImplementedException();
#endif
            }
            catch
            {
                //DALogger.Log(FcuLocKey.cant_load_sprite.Localize(fobject.Data.SpritePath, ex.Message));
                return true;
            }
        }
    }
}
