using DA_Assets.FCU.Model;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using DA_Assets.Shared;
using DA_Assets.Shared.Extensions;

#pragma warning disable CS0649

namespace DA_Assets.FCU
{
    [CreateAssetMenu(menuName = DAConstants.Publisher + "/FcuConfig")]
    public class FcuConfig : SingletoneScriptableObject<FcuConfig>
    {
        public const string ProductName = "Figma Converter for Unity";

        public const string DestroyChilds = "Destroy childs";
        public const string SetFcuToSyncHelpers = "Set current FCU to SyncHelpers";
        public const string CompareTwoObjects = "Compare two selected objects";
        public const string DestroyLastImported = "Destroy last imported frames";
        public const string DestroySyncHelpers = "Destroy SyncHelpers";
        public const string CreatePrefabs = "Create Prefabs";
        public const string ResetToPrefabState = "Reset to prefab state";
        public const string ResetAllComponents = "Reset all components to prefab state";
        public const string Create = "Create";

        private static string apiLink = "https://api.figma.com/v1/files/{0}?geometry=paths&plugin_data=shared";
        private static string clientId = "LaB1ONuPoY7QCdfshDbQbT";
        private static string clientSecret = "E9PblceydtAyE7Onhg5FHLmnvingDp";
        private static string redirectUri = "http://localhost:1923/";
        private static string authUrl = "https://www.figma.com/api/oauth/token?client_id={0}&client_secret={1}&redirect_uri={2}&code={3}&grant_type=authorization_code";
        private static string oAuthUrl = "https://www.figma.com/oauth?client_id={0}&redirect_uri={1}&scope=file_read&state={2}&response_type=code";

        public static string ApiLink => apiLink;
        public static string ClientId => clientId;
        public static string ClientSecret => clientSecret;
        public static string RedirectUri => redirectUri;
        public static string AuthUrl => authUrl;
        public static string OAuthUrl => oAuthUrl;

        #region FIELDS

        [SerializeField] string productVersion;

        [Space]

        [SerializeField] List<TagConfig> tags;
        [SerializeField] List<DependencyItem> dependencies;

        [Header("File names")]
        [SerializeField] string webLogFileName;
        public string WebLogFileName => webLogFileName;

        [SerializeField] string localizationFileName;
        public string LocalizationFileName => localizationFileName;

        [Header("Formats")]
        [SerializeField] string dateTimeFormat1;
        public string DateTimeFormat1 => dateTimeFormat1;
        [SerializeField] string dateTimeFormat2;
        public string DateTimeFormat2 => dateTimeFormat2;

        [Header("GameObject names")]
        [SerializeField] string canvasGameObjectName;
        public string CanvasGameObjectName => canvasGameObjectName;
        [SerializeField] string i2LocGameObjectName;
        public string I2LocGameObjectName => i2LocGameObjectName;

        [Header("Values")]
        [SerializeField] int maxCachedFilesCount;
        [SerializeField] int maxLogFilesCount;

        [Tooltip("Enable this setting to automatically compress Assets during import.")]
        [SerializeField] bool compressAssetsOnImport;

#if UNITY_EDITOR
        [Header("TextureImporter Settings")]
        [SerializeField] bool generateMipMaps = false;
        public bool GenerateMipMaps => generateMipMaps;

        [SerializeField] bool crunchedCompression;
        public bool CrunchedCompression => crunchedCompression;

        [Tooltip("This value is used only when flag 'CrunchedCompression' is active.")]
        [SerializeField] int crunchedCompressionQuality;
        public int CrunchedCompressionQuality => crunchedCompressionQuality;

        [SerializeField] UnityEditor.TextureImporterCompression textureImporterCompression;
        public UnityEditor.TextureImporterCompression TextureImporterCompression => textureImporterCompression;
#endif

        [Header("Api")]
        [SerializeField] bool https;
        [SerializeField] int apiRequestsCountLimit = 2;
        [SerializeField] int apiTimeoutSec = 5;
        [SerializeField] int chunkSizeGetSpriteLinks;
        [SerializeField] int chunkSizeDownloadSprites;
        [SerializeField] string gFontsApiKey;

        [Header("Other")]
        [SerializeField] char realTagSeparator = '-';
        [SerializeField] int maxRenderSize = 4096;
        [SerializeField] string rateMePrefsKey = "fcuRateMeShown";
        public string RateMePrefsKey => rateMePrefsKey;
        [SerializeField] int renderUpscaleFactor = 2;
        public int RenderUpscaleFactor => renderUpscaleFactor;

        #endregion
        #region PROPERTIES


        public int MaxCachedFilesCount => maxCachedFilesCount;
        public int MaxLogFilesCount => maxLogFilesCount;
        public int ChunkSizeGetSpriteLinks => chunkSizeGetSpriteLinks;
        public int ChunkSizeDownloadSprites => chunkSizeDownloadSprites;
        public string GoogleFontsApiKey { get => gFontsApiKey; set => gFontsApiKey = value; }
        public List<DependencyItem> AssemblyConfigs => dependencies;
        public List<TagConfig> TagConfigs => tags;
        public bool Https => https;
        public string ProductVersion => productVersion;
        public char RealTagSeparator => realTagSeparator;
        public int MaxRenderSize => maxRenderSize;
        public int ApiRequestsCountLimit => apiRequestsCountLimit;
        public int ApiTimeoutSec => apiTimeoutSec;

        private string logPath;
        public string LogPath
        {
            get
            {
                if (string.IsNullOrWhiteSpace(logPath))
                    logPath = Path.Combine(Directory.GetParent(Application.dataPath).FullName, "Logs");

                if (logPath.Contains("/"))
                    logPath = logPath.Replace("/", "\\");

                return logPath;
            }
        }

        private string cachePath = null;
        public string CachePath
        {
            get
            {
                if (cachePath.IsEmpty())
                {
                    string tempFolder = Path.GetTempPath();
                    cachePath = Path.Combine(tempFolder, "FCU Cache");
                    cachePath = cachePath.Replace("/", "\\");
                }

                cachePath.CreateFolderIfNotExists();

                return cachePath;
            }
        }
        #endregion
    }
}