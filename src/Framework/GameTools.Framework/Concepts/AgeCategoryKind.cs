using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameTools.Framework.Concepts
{
    public enum AgeCategoryKind
    {
        /// <summary>
        /// The average age at which a member of this species is considered an adult.
        /// </summary>
        Adulthood, 
        
        /// <summary>
        /// The average age at which a member of the species is considered an Elder
        /// </summary>
        Retirement, 

        /// <summary>
        /// The average maximum lifespan for members of this species
        /// </summary>
        Lifespan
    }
}
