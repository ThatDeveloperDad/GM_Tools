using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ThatDeveloperDad.AIWorkloadManager.Contracts
{
    public interface IAiWorkloadManager
    {
        string TestLLM();

        string DescribeNPC(string npcJson);
    }
}
