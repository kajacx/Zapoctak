using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game.monsters
{
    public class Plan
    {
        public double prob;
        public Target target;

        public enum Target
        {
            SELF, ALLY, FOE, ALL
        }
    }
}
