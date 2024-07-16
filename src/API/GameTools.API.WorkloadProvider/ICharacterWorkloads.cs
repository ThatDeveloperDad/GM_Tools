using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.API.WorkloadProvider
{
    public interface ICharacterWorkloads
    {
        string GenerateNPC(bool includeAI = false);
    }
}
