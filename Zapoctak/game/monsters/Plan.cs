using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game.events;

namespace Zapoctak.game.monsters
{
    public abstract class Plan
    {
        public double prob;
        public Target target;

        public enum Target
        {
            SELF, ALLY, FOE, ALL
        }

        public abstract EventData toEventData();
    }
}
