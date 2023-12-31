﻿using DA_Assets.Shared;
using DA_Assets.FCU;
using DA_Assets.FCU.Extensions;
using System;
using System.Linq;
using UnityEngine;

#pragma warning disable CS0649

namespace DA_Assets.Shared
{
    internal class UpdateChecker
    {
        private static VersionCache cachedVersionData = default;

        private static DAInspector gui = DAInspector.Instance;

        internal static void DrawVersionLine(AssetType assetType)
        {
            if (cachedVersionData.IsDefault())
            {
                WriteVersionData(Version.Parse(FcuConfig.Instance.ProductVersion), assetType);
            }

            gui.DrawGroup(new Group
            {
                GroupType = GroupType.Horizontal,
                Body = () =>
                {
                    gui.FlexibleSpace();

                    if (gui.LinkLabel(
                        new GUIContent(cachedVersionData.CurrentVersionText, cachedVersionData.CurrentVersionTooltip),
                        WidthType.Option,
                        cachedVersionData.CurrentVersionStyle))
                    {
                        UnityEditor.PackageManager.UI.Window.Open(cachedVersionData.Asset.Name);
                    }

                    if (cachedVersionData.IsLatestVersion == false)
                    {
                        gui.Space5();
                        gui.Label10px("—", widthType: WidthType.Option);
                        gui.Space5();

                        if (gui.LinkLabel(
                            new GUIContent(cachedVersionData.LatestVersionText, cachedVersionData.LatestVersionTooltip),
                            WidthType.Option,
                            cachedVersionData.LatestVersionStyle))
                        {
                            UnityEditor.PackageManager.UI.Window.Open(cachedVersionData.Asset.Name);
                        }
                    }
                }
            });
        }


        private static void WriteVersionData(Version currentVersion, AssetType assetType)
        {
            try
            {
                if (DAWebConfig.HasWebConfig == false)
                {
                    throw new NullReferenceException();
                }

                Asset assetInfo = DAWebConfig.WebConfig.Assets.First(x => x.Type == assetType);
                cachedVersionData.Asset = assetInfo;

                AssetVersion? current = null;
                AssetVersion? last = null;

                try
                {
                    current = GetVersion(assetType, currentVersion);

                    if (current == null)
                    {
                        throw new NullReferenceException();
                    }

                    if (current.Value.VersionType == VersionType.buggy)
                    {
                        cachedVersionData.CurrentVersionStyle = GuiStyle.RedLabel10px;
                    }
                    else
                    {
                        cachedVersionData.CurrentVersionStyle = GuiStyle.Label10px;
                    }

                    cachedVersionData.CurrentVersionText = $"{currentVersion} [current, {current.Value.VersionType}]";
                    cachedVersionData.CurrentVersionTooltip = current.Value.Description;
                }
                catch
                {
                    cachedVersionData.CurrentVersionText = $"{currentVersion} [current]";
                    cachedVersionData.CurrentVersionStyle = GuiStyle.LinkLabel10px;
                    cachedVersionData.CurrentVersionTooltip = "No description.";
                }

                try
                {
                    last = assetInfo.Versions.Last();

                    if (current.Equals(last))
                    {
                        cachedVersionData.IsLatestVersion = true;
                    }

                    if (last == null)
                    {
                        throw new NullReferenceException();
                    }

                    DateTime lastDt = DateTime.ParseExact(last.Value.ReleaseDate, "MMM d, yyyy", new System.Globalization.CultureInfo("en-US"));
                    DateTime nowDt = DateTime.Now;

                    int differenceInDays = Mathf.Abs((int)(lastDt - nowDt).TotalDays);

                    if (differenceInDays == 0)
                    {
                        cachedVersionData.LatestVersionText = "[latest, today]";
                    }
                    else
                    {
                        cachedVersionData.LatestVersionText = $"{last.Value.Version} [latest, {differenceInDays} {Pluralize("day", differenceInDays)} ago, {last.Value.VersionType}]";
                    }

                    if (last.Value.VersionType == VersionType.buggy)
                    {
                        cachedVersionData.LatestVersionStyle = GuiStyle.RedLabel10px;
                    }
                    else if (differenceInDays >= assetInfo.OldVersionDaysCount)
                    {
                        cachedVersionData.LatestVersionStyle = GuiStyle.BlueLabel10px;
                    }
                    else
                    {
                        cachedVersionData.LatestVersionStyle = GuiStyle.Label10px;
                    }

                    cachedVersionData.LatestVersionTooltip = last.Value.Description;
                }
                catch
                {
                    cachedVersionData.LatestVersionText = $"no data [latest]";
                    cachedVersionData.LatestVersionStyle = GuiStyle.LinkLabel10px;
                    cachedVersionData.LatestVersionTooltip = "No description.";
                }
            }
            catch
            {

            }
        }

        private static AssetVersion? GetVersion(AssetType assetType, Version currentVersion)
        {
            try
            {
                Asset assetInfo = DAWebConfig.WebConfig.Assets.First(x => x.Type == assetType);

                foreach (AssetVersion assetVersion in assetInfo.Versions)
                {
                    Version ver = Version.Parse(assetVersion.Version);

                    int res = currentVersion.CompareTo(ver);

                    if (res == 0)
                    {
                        return assetVersion;
                    }
                }
            }
            catch
            {

            }

            return null;
        }

        private static string Pluralize(string source, int count)
        {
            if (count == 1)
            {
                return source;
            }
            else
            {
                return source + "s";
            }
        }
    }

    internal struct VersionCache
    {
        internal Asset Asset { get; set; }
        internal string CurrentVersionText { get; set; }
        internal string CurrentVersionTooltip { get; set; }
        internal string LatestVersionText { get; set; }
        internal string LatestVersionTooltip { get; set; }
        internal GuiStyle CurrentVersionStyle { get; set; }
        internal GuiStyle LatestVersionStyle { get; set; }
        internal bool IsLatestVersion { get; set; } 
    }
}