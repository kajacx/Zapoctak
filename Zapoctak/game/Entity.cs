using System;
using System.Collections.Generic;
using Zapoctak.gui;

namespace Zapoctak.game
{
    public abstract class Entity
    {
        public int entityId;
        public Stats stats = new Stats();
        public double hp, mp;
        public double time; //0-1 

        public Entity up, right, down, left; //other entites or self

        public bool isDead;

        private bool timeReady;
        public Game game;
        private List<FloatingText> texts = new List<FloatingText>();

        internal List<FloatingText> getTexts()
        {
            return texts;
        }

        public void replenish()
        {
            hp = stats.maxhp;
            mp = stats.maxmp;
        }

        public void update(double dt)
        {
            if (isDead)
            {
                Log.W("Updating dead entity: "+this);
                return;
            }

            //action
            if (!timeReady)
            {
                time += dt * Game.timeLoadSpeed;
                if (time >= 1)
                {
                    timeReady = true;
                    TimeReady();
                }
            }

            //flaoting texts
            for (int i = 0; i < texts.Count; i++)
            {
                if (texts[i].update(dt))
                {
                    texts.RemoveAt(i);
                    i--;
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
