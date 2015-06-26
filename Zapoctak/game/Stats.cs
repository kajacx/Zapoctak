using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game
{
    public class Stats
    {
        public static Stats ALL_ZERO = new Stats();

        public double maxhp, attack, armor, maxmp, magic, resist;

        public double getStat(int id)
        {
            switch (id) { 
                case 0: return maxhp;
                case 1: return attack;
                case 2: return armor;
                case 3: return maxmp;
                case 4: return magic;
                case 5: return resist;
            }
            Log.E("Invalid stat id: "+id);
            return -1;
        }

        public void setStat(int id, double val)
        {
            switch (id)
            {
                case 0: maxhp = val; break;
                case 1: attack = val; break;
                case 2: armor = val; break;
                case 3: maxmp = val; break;
                case 4: magic = val; break;
                case 5: resist = val; break;
                default: Log.E("Invalid stat id: " + id); break;
            }
        }

        public Stats Clone()
        {
            Stats ret = new Stats();
            for (int i = 0; i < 6; i++) ret.setStat(i, getStat(i));
            return ret;
        }
    }
}
