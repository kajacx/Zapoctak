using System;
using Zapoctak.graphics;
using Zapoctak.game.events;
using Zapoctak.game.monsters;

namespace Zapoctak.game
{
    public class Magic
    {
        public double manaCost;
        public Effect effect;
        public int goldCost;
        public Sprite animation;

        public bool update(double dt)
        {
            return true;
        }
    }
}
