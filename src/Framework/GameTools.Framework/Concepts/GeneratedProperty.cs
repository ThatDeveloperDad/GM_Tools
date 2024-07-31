﻿namespace GameTools.Framework.Concepts
{
    /// <summary>
    /// Use this class to express a property whose value can be generated by 
    /// an LLM.
    /// </summary>
    public class GeneratedProperty
    {
        public GeneratedProperty() : this(string.Empty)
        {

        }

        public GeneratedProperty(string value, bool isAiGenerated = false)
        {
            Value = value;
        }

        public string Value { get; set; }

        public bool IsAiGenerated { get; private set; }

        public void SetAiValue(string value)
        {
            Value = value;
            IsAiGenerated = true;
        }

        public void ApplyUserEdit(string value)
        {
            Value = value;
            IsAiGenerated = false;
        }

        public override string ToString()
        {
            string aiTag = IsAiGenerated ? "(AI): " : string.Empty;
            string thisAsString = $"{aiTag}{Value}";

            return thisAsString;
        }
    }
}
