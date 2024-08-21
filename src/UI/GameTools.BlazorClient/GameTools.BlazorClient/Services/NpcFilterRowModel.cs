namespace GameTools.BlazorClient.Services
{
    public record NpcFilterRowModel
    {

        public NpcFilterRowModel(int npcId, string name, string species, string vocation)
        {
            NpcId = npcId;
            Name = name;
            Species = species;
            Vocation = vocation;
        }

        public int NpcId { get; init; }

        public string Name { get; init; }

        public string Species { get; init; }

        public string Vocation { get; init; }
    }
}
