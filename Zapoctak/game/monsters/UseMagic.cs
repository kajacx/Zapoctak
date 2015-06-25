using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game.events;

namespace Zapoctak.game.monsters
{
    class UseMagic : Plan
    {
        private Magic magic;

        public UseMagic(Magic magic)
        {
            this.magic = magic;
        }

        public override EventData toEventData()
        {
            return new MagicEvent(magic);
        }
    }
}
