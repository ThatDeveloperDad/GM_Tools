using System;

namespace GameTools.Framework
{
    public sealed class CharacterAppearance
    {

        public CharacterAppearance()
        {
            Gender = string.Empty;
            Complexion = string.Empty;
            BodyMarkings = string.Empty;
            IntegumentKind = IntegumentKind.None;
            BodyStyle = string.Empty;
            EyeColor = string.Empty;
            Description = string.Empty;
        }


        public int HeightCm { get; set; }

        public int WeightKg { get; set; }

        public string Gender {get;set;}

        public string Complexion {get;set;}

        /// <summary>
        /// This refers to body adornments such as tattoos, scars and so on.
        /// </summary>
        public string BodyMarkings{get;set;}

        /// <summary>
        /// What kind of "covering" has the character got on their body?
        /// eg: Hair, Scales, Feathers, Bark, None
        /// </summary>
        public IntegumentKind IntegumentKind {get; set;}

        public string Integument => IntegumentKind.ToString();

        /// <summary>
        /// What color are the integuments?
        /// </summary>
        public string? IntegumentColor {get;set;}
        
        /// <summary>
        /// How are the integuments styled?
        /// </summary>
        public string? IntegumentStyle {get;set;}

        /// <summary>
        /// Bipedal, amorphous, winged, octopedal, spectral, etc...
        /// </summary>
        public string BodyStyle {get;set;}

        public int EyeCount {get;set;}

        public string EyeColor {get;set;}

        public string Description {get;set;}
            
    }
}