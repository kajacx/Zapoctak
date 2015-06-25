using System;


namespace Zapoctak.game
{
    public abstract class Entity
    {
        public int entityId;
        public Stats stats = new Stats();
        public double hp, mp;
        public double time; //0-1 

        private bool timeReady;
        public Game game;

        public void replenish()
        {
            hp = stats.maxhp;
            mp = stats.maxmp;
        }

        public void update(double dt)
        {
            if (!timeReady)
            {
                time += dt * Game.timeLoadSpeed;
                if (time >= 1)
                {
                    timeReady = true;
                    TimeReady();
                }
            }
        }

        public abstract void TimeReady();

        public void TimeReset()
        {
            time = 0;
            timeReady = false;
        }
    }
}
