using System;
using System.Runtime.Serialization;

namespace DA_Assets.FCU.Model
{
    [Serializable]
    public struct FigmaError
    {
        public FigmaError(int status, string err)
        {
            this.Status = status;
            this.Error = err;
        }

        [DataMember(Name = "status")] public int Status { get; set; }
        [DataMember(Name = "err")] public string Error { get; set; }
    }
}