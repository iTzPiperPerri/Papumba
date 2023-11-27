using System.Collections.Generic;
using System.Runtime.Serialization;

namespace DA_Assets.FCU
{
    public struct FontItem
    {
        [DataMember(Name = "family")] public string Family { get; set; }
        [DataMember(Name = "variants")] public List<string> Variants { get; set; }
        [DataMember(Name = "subsets")] public List<string> Subsets { get; set; }
        [DataMember(Name = "version")] public string Version { get; set; }
        [DataMember(Name = "lastModified")] public string LastModified { get; set; }
        [DataMember(Name = "files")] public Dictionary<string, string> Files { get; set; }
        [DataMember(Name = "category")] public string Category { get; set; }
        [DataMember(Name = "kind")] public string Kind { get; set; }
        [DataMember(Name = "menu")] public string Menu { get; set; }
    }

    public struct FontRoot
    {
        [DataMember(Name = "kind")] public string Kind { get; set; }
        [DataMember(Name = "items")] public List<FontItem> Items { get; set; }
    }
}
