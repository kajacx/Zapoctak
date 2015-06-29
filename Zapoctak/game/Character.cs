using System;
using System.Collections.Generic;

namespace Zapoctak.game
{
    public class Character : Entity
    {
        public CharacterInfo info;
        public int id;

        public Equip weapon;
        public Equip armor;

        public List<Magic> magic = new List<Magic>();
        public string msg;

        public void recomputeStats()
        {
            Stats w = weapon != null ? weapon.stats : Stats.ALL_ZERO;
            Stats a = armor != null ? armor.stats : Stats.ALL_ZERO;

            for (int i = 0; i < 6; i++)
            {
                stats.setStat(i, info.stats.getStat(i) + w.getStat(i) + a.getStat(i));
            }
        }

        public void unequip()
        {
            weapon = armor = null;
        }

        override public void TimeReady()
        {
            msg = "Ready";
            game.charReady(this);
        }

        public override string ToString()
        {
            return "[Char] Class: " + info.name
               + ", w: " + U.ToString(weapon)
               + ", a: " + U.ToString(armor);
        }
    }
}
