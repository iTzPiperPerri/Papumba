using System;

namespace DA_Assets.Shared
{
    public class DASerializationAttribute : Attribute
    {
        public DASerializationAttribute(string fieldName)
        {
            this.fieldName = fieldName;
        }

        private string fieldName;
        public string FieldName => fieldName;
    }
}