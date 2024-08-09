using GameTools.NPCAccess.SqlServer.Context.SqlModels;

namespace GameTools.NPCAccess.SqlServer.Transformers
{
    internal static class ModelExtensions
    {
        public static NpcRowModel ToRowModel(this NpcAccessModel dto)
        {
            NpcRowModel model = new NpcRowModel();

            model.NpcId = dto.NpcId.GetValueOrDefault();
            model.UserId = dto.UserId;
            model.IsPublic = dto.IsPublic;
            model.SpeciesName = dto.Species;
            model.VocationName = dto.Vocation;
            model.CharacterDetails = dto.CharacterDetails;

            // Not going to touch the model Date props.
            // We'll do that in the actual method that is writing the NPC.

            return model;
        }

        public static NpcRowModel ApplyInboundChanges(this NpcRowModel model, NpcAccessModel dto)
        {
            // Here's where we update the Model instance with whatever came in
            // from the application.  We're only going to update the User-manageable
            // properties.

            model.IsPublic = dto.IsPublic;
            model.SpeciesName = dto.Species;
            model.VocationName = dto.Vocation;
            model.CharacterDetails = dto.CharacterDetails;

            return model;
        }

        public static NpcAccessModel ToDto(this NpcRowModel model)
        {
            NpcAccessModel dto = new NpcAccessModel();

            dto.NpcId = model.NpcId;
            dto.UserId = model.UserId;
            dto.IsPublic = model.IsPublic;
            dto.Species = model.SpeciesName;
            dto.Vocation = model.VocationName;
            dto.CharacterDetails = model.CharacterDetails;

            return dto;
        }
    }
}
