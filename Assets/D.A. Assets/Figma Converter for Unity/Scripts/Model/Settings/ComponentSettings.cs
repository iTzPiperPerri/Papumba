﻿using DA_Assets.Shared;
using System;
using UnityEngine;

#pragma warning disable CS0162

namespace DA_Assets.FCU.Model
{
    [Serializable]
    public class ComponentSettings : MonoBehaviourBinder<FigmaConverterUnity>
    {
        [SerializeField] ImageComponent imageComponent = ImageComponent.UnityImage;
        [SerializeField] ShadowComponent shadowComponent = ShadowComponent.Figma;
        [SerializeField] TextComponent textComponent = TextComponent.UnityText;
        [SerializeField] ButtonComponent buttonComponent = ButtonComponent.UnityButton;

        public ImageComponent ImageComponent
        {
            get
            {
                return imageComponent;
            }
            set
            {
                switch (value)
                {
                    case ImageComponent.Shape:
#if SHAPES_EXISTS == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(ImageComponent.Shape)));
                        SetValue(ref imageComponent, ImageComponent.UnityImage);
                        return;
#endif
                        break;
                    case ImageComponent.MPImage:
#if MPUIKIT_EXISTS == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(ImageComponent.MPImage)));
                        SetValue(ref imageComponent, ImageComponent.UnityImage);
                        return;
#endif
                        break;
                    case ImageComponent.ProceduralImage:
#if PUI_EXISTS == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(ImageComponent.ProceduralImage)));
                        SetValue(ref imageComponent, ImageComponent.UnityImage);
                        return;
#endif
                        break;
                }

                SetValue(ref imageComponent, value);
            }
        }
        public ShadowComponent ShadowComponent
        {
            get
            {
                return shadowComponent;
            }
            set
            {
                switch (value)
                {
                    case ShadowComponent.TrueShadow:
#if TRUESHADOW_EXISTS == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(ShadowComponent.TrueShadow)));
                        SetValue(ref shadowComponent, ShadowComponent.Figma);
                        return;
#endif
                        break;
                }

                SetValue(ref shadowComponent, value);
            }
        }
        public TextComponent TextComponent
        {
            get
            {
                return textComponent;
            }
            set
            {
                switch (value)
                {
                    case TextComponent.TextMeshPro:
#if TextMeshPro == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(TextComponent.TextMeshPro)));
                        textComponent = TextComponent.UnityText;
                        return;
#endif
                        break;
                }

                SetValue(ref textComponent, value);
            }
        }
        public ButtonComponent ButtonComponent
        {
            get
            {
                return buttonComponent;
            }
            set
            {
                switch (value)
                {
                    case ButtonComponent.DAButton:
#if DABUTTON_EXISTS == false
                        DALogger.LogError(FcuLocKey.log_asset_not_imported.Localize(nameof(ButtonComponent.DAButton)));
                        buttonComponent = ButtonComponent.UnityButton;
                        return;
#endif
                        break;
                }

                SetValue(ref buttonComponent, value);
            }
        }
    }
}
