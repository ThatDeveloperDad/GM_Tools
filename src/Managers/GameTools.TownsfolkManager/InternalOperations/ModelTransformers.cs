using GameTools.NPCAccess;
using GameTools.TownsfolkManager.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ThatDeveloperDad.Framework.Serialization;

namespace GameTools.TownsfolkManager.InternalOperations
{
    internal static class ModelTransformers
    {
        public static NpcAccessModel ToNpcAccessModel(this Townsperson app)
        {
            var model = new NpcAccessModel();

            model.NpcId = app.Id;
            model.UserId = app.UserId;
            model.Species = app.Species;
            model.Vocation = app.Vocation.Name;
            model.CharacterDetails = app.SerializeForOutput();

            return model;
        }

        public static Townsperson ToAppModel(this NpcAccessModel resource)
        {
            var app = new Townsperson();

            // Not gonna sweat this right now.

            return app;
        }

        public static NpcAccessFilter ToNpcAccessFilter(this TownspersonFilter filter)
        {
            var model = new NpcAccessFilter()
            { 
                Species = filter.Species,
                Vocation = filter.Vocation
            };

            return model;
        }

        public static FilteredTownsperson ToManagerModel(this NpcAccessFilterResult accessModel)
        {
            var mgrModel = new FilteredTownsperson
                (npcId: accessModel.Id,
                 name: accessModel.Name,
                 species: accessModel.Species,
                 vocation: accessModel.Vocation);

            return mgrModel;
        }
    }
}
