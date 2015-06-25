using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game.events;

namespace Zapoctak.game.monsters
{
    class Attack : Plan
    {
        public override EventData toEventData()
        {
            return new AttackEvent();
        }
    }
}
