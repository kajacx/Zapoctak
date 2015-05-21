using System;


namespace Zapoctak.game
{
    public class Entity
    {
        public int entityId;
        public Stats stats = new Stats();
        public double hp, mp;
        public void replenish()
        {
            hp = stats.maxhp;
            mp = stats.maxmp;
        }
    }
}
