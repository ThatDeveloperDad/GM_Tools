using GameTools.TownsfolkManager.Contracts;

namespace GameTools.BlazorClient.Services
{
    public class DisplayNPCViewModel
    {
        private readonly Townsperson _npc;

        public DisplayNPCViewModel():this(new Townsperson()) { }

        public DisplayNPCViewModel(Townsperson npc)
        {
            _npc = npc;
        }

        internal Townsperson NpcModel => _npc;

        public string Species => _npc.Species;
        public string Gender => _npc.Appearance.Gender;
        public string Pronouns => _npc.Pronouns;
        public int Age => _npc.AgeYears;

        public int HeightCm => _npc.Appearance.HeightCm;

        public int WeightKg => _npc.Appearance.WeightKg;

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

        public string History => _npc.Background.Name;

        public string Profession => _npc.Vocation.Name;

        public string TextDescription { get; private set; }

        public bool DescriptionIsAI { get; private set; }

        public void AddAiDescription(string description)
        {
            TextDescription = description;
            DescriptionIsAI = true;
        }

        // Ignore this for now.  We want to do it eventually, but not today.
        //public void SetUserEditedDescription(string description)
        //{
        //    TextDescription = description;
        //    DescriptionIsAI = false;
        //}
    }
}
