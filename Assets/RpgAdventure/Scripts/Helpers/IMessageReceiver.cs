
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RpgAdventure
{

    public enum MessageType
    {
        DAMAGE,
        DEAD

    }
    public interface IMessageReceiver
    {
        //Damageable sender,
       // Damageable.DamageMessage message


        void OnReceiveMessage(MessageType type, 
            object sender,
            object message);

    }
}
