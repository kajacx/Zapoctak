using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game.monsters
{
    public class Monster : Entity
    {
        public MonsterInfo info;
        public void boost(double coef)
        {
            for (int i = 0; i < 6; i++) stats.setStat(i, stats.getStat(i) * coef);
        }
    }
}
