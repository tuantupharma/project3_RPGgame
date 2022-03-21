using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAdventure
{
    public interface IAttackAnimListener
    {

        // this method is called by animation event

        void MeleeAttackStart();

        // this method is called by animation event
        void MeleeAttackEnd();
        
    }
}
