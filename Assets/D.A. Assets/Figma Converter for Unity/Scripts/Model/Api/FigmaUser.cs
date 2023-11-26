using System;
using UnityEngine;

namespace DA_Assets.FCU.Model
{
    [Serializable]
    public struct FigmaUser
    {
        [SerializeField] public string id;
        [SerializeField] public string email;
        [SerializeField] public string handle;
        [SerializeField] public string img_url;

        public string Id => id;
        public string Name => handle;
        public string Email => email;
        public string ImgUrl => img_url;
    }
}