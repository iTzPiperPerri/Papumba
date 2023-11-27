﻿using DA_Assets.FCU.Model;
using DA_Assets.Shared;
using System;
using UnityEngine;

#pragma warning disable CS0649

namespace DA_Assets.FCU
{
    [Serializable]
    public class SettingsBinder : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] MainSettings mainSettings;
        [DASerialization(nameof(mainSettings))]
        public MainSettings MainSettings => mainSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] ComponentSettings componentSettings;
        [DASerialization(nameof(componentSettings))]
        public ComponentSettings ComponentSettings => componentSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] PuiSettings puiSettings;
        [DASerialization(nameof(puiSettings))]
        public PuiSettings PuiSettings => puiSettings.SetMonoBehaviour(monoBeh); 

        [SerializeField] MPUIKitSettings mpuikitSettings;
        [DASerialization(nameof(mpuikitSettings))]
        public MPUIKitSettings MPUIKitSettings => mpuikitSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] UnityImageSettings unityImageSettings;
        [DASerialization(nameof(unityImageSettings))]
        public UnityImageSettings UnityImageSettings => unityImageSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] Shapes2DSettings shapes2D_Settings;
        [DASerialization(nameof(shapes2D_Settings))]
        public Shapes2DSettings Shapes2DSettings => shapes2D_Settings.SetMonoBehaviour(monoBeh);

        [SerializeField] TextMeshSettings textMeshSettings;
        [DASerialization(nameof(textMeshSettings))]
        public TextMeshSettings TextMeshSettings => textMeshSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] UnityTextSettings unityTextSettings;
        [DASerialization(nameof(unityTextSettings))]
        public UnityTextSettings UnityTextSettings => unityTextSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] DebugSettings debugSettings;
        [DASerialization(nameof(debugSettings))]
        public DebugSettings DebugSettings => debugSettings.SetMonoBehaviour(monoBeh);

        [SerializeField] PrefabSettings prefabSettings;
        [DASerialization(nameof(prefabSettings))]
        public PrefabSettings PrefabSettings => prefabSettings.SetMonoBehaviour(monoBeh);
    }
}