//
//███████╗██╗░██████╗░███╗░░░███╗░█████╗░  ░█████╗░░█████╗░███╗░░██╗██╗░░░██╗███████╗██████╗░████████╗███████╗██████╗░
//██╔════╝██║██╔════╝░████╗░████║██╔══██╗  ██╔══██╗██╔══██╗████╗░██║██║░░░██║██╔════╝██╔══██╗╚══██╔══╝██╔════╝██╔══██╗
//█████╗░░██║██║░░██╗░██╔████╔██║███████║  ██║░░╚═╝██║░░██║██╔██╗██║╚██╗░██╔╝█████╗░░██████╔╝░░░██║░░░█████╗░░██████╔╝
//██╔══╝░░██║██║░░╚██╗██║╚██╔╝██║██╔══██║  ██║░░██╗██║░░██║██║╚████║░╚████╔╝░██╔══╝░░██╔══██╗░░░██║░░░██╔══╝░░██╔══██╗
//██║░░░░░██║╚██████╔╝██║░╚═╝░██║██║░░██║  ╚█████╔╝╚█████╔╝██║░╚███║░░╚██╔╝░░███████╗██║░░██║░░░██║░░░███████╗██║░░██║
//╚═╝░░░░░╚═╝░╚═════╝░╚═╝░░░░░╚═╝╚═╝░░╚═╝  ░╚════╝░░╚════╝░╚═╝░░╚══╝░░░╚═╝░░░╚══════╝╚═╝░░╚═╝░░░╚═╝░░░╚══════╝╚═╝░░╚═╝
//
//███████╗░█████╗░██████╗░  ██╗░░░██╗███╗░░██╗██╗████████╗██╗░░░██╗
//██╔════╝██╔══██╗██╔══██╗  ██║░░░██║████╗░██║██║╚══██╔══╝╚██╗░██╔╝
//█████╗░░██║░░██║██████╔╝  ██║░░░██║██╔██╗██║██║░░░██║░░░░╚████╔╝░
//██╔══╝░░██║░░██║██╔══██╗  ██║░░░██║██║╚████║██║░░░██║░░░░░╚██╔╝░░
//██║░░░░░╚█████╔╝██║░░██║  ╚██████╔╝██║░╚███║██║░░░██║░░░░░░██║░░░
//╚═╝░░░░░░╚════╝░╚═╝░░╚═╝  ░╚═════╝░╚═╝░░╚══╝╚═╝░░░╚═╝░░░░░░╚═╝░░░
//
using DA_Assets.FCU.Drawers;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;
using System;
using UnityEngine;

#pragma warning disable CS0649

namespace DA_Assets.FCU
{
    [Serializable]
    public sealed class FigmaConverterUnity : MonoBehaviour
    {
        [SerializeField] ProjectImporter importController;
        [DASerialization(nameof(importController))]
        public ProjectImporter ProjectImporter => importController.SetMonoBehaviour(this);

        [SerializeField] CanvasDrawer canvasDrawer;
        [DASerialization(nameof(canvasDrawer))]
        public CanvasDrawer CanvasDrawer => canvasDrawer.SetMonoBehaviour(this);

#if UITKPLUGIN_EXISTS
        [SerializeField] UITK_Converter uitkConverter;
        [DASerialization(nameof(uitkConverter))]
        public UITK_Converter UITK_Converter => uitkConverter.SetMonoBehaviour(this);
#endif
        [SerializeField] ProjectCacher projectCacher;
        [DASerialization(nameof(projectCacher))]
        public ProjectCacher ProjectCacher => projectCacher.SetMonoBehaviour(this);

        [SerializeField] ProjectDownloader projectDownloader;
        [DASerialization(nameof(projectDownloader))]
        public ProjectDownloader ProjectDownloader => projectDownloader.SetMonoBehaviour(this);

        [SerializeField] ImageTypeSetter imageTypeSetter;
        [DASerialization(nameof(imageTypeSetter))]
        public ImageTypeSetter ImageTypeSetter => imageTypeSetter.SetMonoBehaviour(this);

        [SerializeField] DuplicateFinder duplicateFinder;
        [DASerialization(nameof(duplicateFinder))]
        public DuplicateFinder DuplicateFinder => duplicateFinder.SetMonoBehaviour(this);

        [SerializeField] DelegateHolder delegateHolder;
        [DASerialization(nameof(delegateHolder))]
        public DelegateHolder DelegateHolder => delegateHolder.SetMonoBehaviour(this);

        [SerializeField] SettingsBinder settings;
        [DASerialization(nameof(settings))]
        public SettingsBinder Settings => settings.SetMonoBehaviour(this);

        [SerializeField] FcuEventHandlers eventHandlers;
        [DASerialization(nameof(eventHandlers))]
        public FcuEventHandlers EventHandlers => eventHandlers.SetMonoBehaviour(this);

        [SerializeField] FcuEvents events;
        [DASerialization(nameof(events))]
        public FcuEvents Events => events.SetMonoBehaviour(this);

        [SerializeField] PrefabCreator prefabCreator;
        [DASerialization(nameof(prefabCreator))]
        public PrefabCreator PrefabCreator => prefabCreator.SetMonoBehaviour(this);

        [SerializeField] InspectorDrawer inspectorDrawer;
        [DASerialization(nameof(inspectorDrawer))]
        public InspectorDrawer InspectorDrawer => inspectorDrawer.SetMonoBehaviour(this);

        [SerializeField] Authorizer authController;
        [DASerialization(nameof(authController))]
        public Authorizer AuthController => authController.SetMonoBehaviour(this);

        [SerializeField] RequestSender requestSender;
        [DASerialization(nameof(requestSender))]
        public RequestSender RequestSender => requestSender.SetMonoBehaviour(this);

        [SerializeField] HashGenerator hashGenerator;
        [DASerialization(nameof(hashGenerator))]
        public HashGenerator HashGenerator => hashGenerator.SetMonoBehaviour(this);

        [SerializeField] NameHumanizer nameHumanizer;
        [DASerialization(nameof(nameHumanizer))]
        public NameHumanizer NameHumanizer => nameHumanizer.SetMonoBehaviour(this);

        [SerializeField] FontDownloader fontDownloader;
        [DASerialization(nameof(fontDownloader))]
        public FontDownloader FontDownloader => fontDownloader.SetMonoBehaviour(this);

        [SerializeField] FontLoader fontLoader;
        [DASerialization(nameof(fontLoader))]
        public FontLoader FontLoader => fontLoader.SetMonoBehaviour(this);

        [SerializeField] TagSetter tagSetter;
        [DASerialization(nameof(tagSetter))]
        public TagSetter TagSetter => tagSetter.SetMonoBehaviour(this);

        [SerializeField] SpriteWorker spriteWorker;
        [DASerialization(nameof(spriteWorker))]
        public SpriteWorker SpriteWorker => spriteWorker.SetMonoBehaviour(this);

        [SerializeField] AssetTools tools;
        [DASerialization(nameof(tools))]
        public AssetTools AssetTools => tools.SetMonoBehaviour(this);

        [SerializeField] ComponentsParser сomponentsParser;
        [DASerialization(nameof(сomponentsParser))]
        public ComponentsParser ComponentsParser => сomponentsParser.SetMonoBehaviour(this);

        [SerializeField] SyncHelpers syncHelper;
        [DASerialization(nameof(syncHelper))]
        public SyncHelpers SyncHelpers => syncHelper.SetMonoBehaviour(this);

        [SerializeField] TransformSetter transformSetter;
        [DASerialization(nameof(transformSetter))]
        public TransformSetter TransformSetter => transformSetter.SetMonoBehaviour(this);

        [SerializeField] CurrentProject currentProject;
        [DASerialization(nameof(currentProject))]
        public CurrentProject CurrentProject => currentProject.SetMonoBehaviour(this);

        [SerializeField] SpriteGenerator spriteGenerator;
        [DASerialization(nameof(spriteGenerator))]
        public SpriteGenerator SpriteGenerator => spriteGenerator.SetMonoBehaviour(this);

        [SerializeField] SpriteColorizer spriteColorizer;
        [DASerialization(nameof(spriteColorizer))]
        public SpriteColorizer SpriteColorizer => spriteColorizer.SetMonoBehaviour(this);

        [SerializeField] SpritePathSetter spritePathSetter;
        [DASerialization(nameof(spritePathSetter))]
        public SpritePathSetter SpritePathSetter => spritePathSetter.SetMonoBehaviour(this);

        [SerializeField] SpriteDownloader spriteDownloader;
        [DASerialization(nameof(spriteDownloader))]
        public SpriteDownloader SpriteDownloader => spriteDownloader.SetMonoBehaviour(this);

        [SerializeField] FigmaSession figmaSession;
        [DASerialization(nameof(figmaSession))]
        public FigmaSession FigmaSession => figmaSession.SetMonoBehaviour(this);

        [SerializeField] string guid;
        public string Guid => guid.CreateShortGuid(out guid);
    }
}