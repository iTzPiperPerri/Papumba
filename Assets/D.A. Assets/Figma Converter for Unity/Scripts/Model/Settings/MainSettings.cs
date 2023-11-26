﻿using DA_Assets.FCU.Extensions;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using System.Net.Sockets;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
    [Serializable]
    public class MainSettings : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] UIFramework uiFramework = UIFramework.UGUI;
        public UIFramework UIFramework { get => uiFramework; set => SetValue(ref uiFramework, value); }

        [SerializeField] ImageFormat imageFormat = ImageFormat.PNG;
        public ImageFormat ImageFormat { get => imageFormat; set => SetValue(ref imageFormat, value); }

        [SerializeField] float imageScale = 4.0f;
        public float ImageScale { get => imageScale; set => SetValue(ref imageScale, value); }

        [SerializeField] int goLayer = 5;
        public int GameObjectLayer { get => goLayer; set => SetValue(ref goLayer, value); }

        [SerializeField] string spritesPath = "Assets\\Sprites";
        public string SpritesPath { get => spritesPath; set => SetValue(ref spritesPath, value); }

        [SerializeField] string uguiOutputPath = "Assets\\UGUI Output";
        public string UGUIOutputPath { get => uguiOutputPath; set => SetValue(ref uguiOutputPath, value); }

        [SerializeField] bool redownloadSprites = false;
        public bool RedownloadSprites { get => redownloadSprites; set => SetValue(ref redownloadSprites, value); }

        [SerializeField] bool rawImport = false;
        public bool RawImport
        {
            get => rawImport;
            set
            {
                if (value && value != rawImport)
                {
                    DALogger.LogError(FcuLocKey.log_dev_function_enabled.Localize(FcuLocKey.label_raw_import.Localize()));
                }

                SetValue(ref rawImport, value);
            }
        }

        [SerializeField] bool windowMode = false;
        public bool WindowMode { get => windowMode; set => SetValue(ref windowMode, value); }

        [SerializeField] bool useI2Localization = false;
        public bool UseI2Localization
        {
            get => useI2Localization;
            set
            {
#if I2LOC_EXISTS
                SetValue(ref useI2Localization, value);
#else
                if (value == true)
                {
                    DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize("I2Localization"));
                    SetValue(ref useI2Localization, value);
                }
#endif
            }
        }

        [SerializeField] string projectUrl;
        public string ProjectUrl
        {
            get => projectUrl;
            set
            {
                string _value = value;

                try
                {
                    if ((_value?.Contains("/")).ToBoolNullFalse())
                    {
                        string[] splited = value.Split('/');
                        _value = splited[4];
                    }
                }
                catch
                {
                    Debug.LogError(FcuLocKey.log_incorrent_project_url.Localize());
                }

                SetValue(ref projectUrl, _value);
            }
        }

        [SerializeField] string[] componentsUrls = new string[5];
        public string[] ComponentsUrls
        {
            get
            {
                if (componentsUrls.IsEmpty())
                {
                    componentsUrls = new string[5];
                }

                return componentsUrls;
            }
            set
            {
                SetValue(ref componentsUrls, value);
            }
        }
    }
}
