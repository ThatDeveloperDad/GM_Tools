using GameTools.Framework.Concepts;
using GameTools.Framework.Converters;
using GameTools.TownsfolkManager.Contracts;
using System.Text.Json.Serialization;

namespace GameTools.BlazorClient.Services
{
    public class NpcClientModel
    {
        private readonly Townsperson _npc;
        private bool _useMetricMeasurements = false;
        public NpcClientModel() : this(new Townsperson()) { }

        public NpcClientModel(Townsperson npc)
        {
            _npc = npc;
            TextDescription = string.Empty;
        }

        internal Townsperson NpcModel => _npc;

        internal void SetOwner(string ownerId)
        {
            if(string.IsNullOrWhiteSpace(_npc.UserId) == false &&  _npc.UserId != ownerId)
            {
                // Note:  If we ever allow NPCs to be shared as read-only
                // but do allow another user to make a "copy" of the NPC,
                // This is where we'll make that change.  Probably.
                return;
            }
            _npc.SetOwner(ownerId);
        }

        public string OwnerId => _npc.UserId;
        public string Species => _npc.Species;
        public string Gender => _npc.Appearance.Gender;
        public string Pronouns => _npc.Pronouns;
        public int Age => _npc.AgeYears;

        public string Height => 
            _useMetricMeasurements 
                ? _npc.Appearance.HeightCm.AsMeters()
                : _npc.Appearance.HeightCm.AsFeetInches();
            

        public string Weight => 
            _useMetricMeasurements
            ? $"{_npc.Appearance.WeightKg}Kg"
            : _npc.Appearance.WeightKg.AsPounds();

        public string Appearance
        {
            get 
            {
                string appearance = string.Empty;

                string integument = $"{_npc.Appearance.IntegumentStyle} {_npc.Appearance.IntegumentColor} {_npc.Appearance.Integument}.";
                string complexion = string.Empty;
                if (string.IsNullOrWhiteSpace(_npc.Appearance.Complexion))
                {
                    // no visible skin.  Integument is the only consideration.
                }
                else
                {
                    // Assume complxion + integument.
                    complexion = $"a {_npc.Appearance.Complexion} complexion, and ";
                }

                appearance = $"Has {complexion}{integument}";

                return appearance;
            }
        }

        [JsonIgnore]
        public string GenAppearance => _npc.Appearance.Description.ToString();

        public string History => _npc.Background.Name;

        [JsonIgnore]
        public string GenHistory => _npc.Background.Description.ToString();

        public string Profession => _npc.Vocation.Name;

        [JsonIgnore]
        public string GenProfession => _npc.Vocation.Description.ToString();

        [JsonIgnore]
        public string NpcName => _npc.FullName.ToString();

        

        public string PersonalityDescription => _npc.PersonalityDescription.ToString();

        public string TextDescription { get; private set; }

        public bool DescriptionIsAI { get; private set; }

        public void AddAiDescription(string description)
        {
            TextDescription = description;
            DescriptionIsAI = true;
        }

    }
}
