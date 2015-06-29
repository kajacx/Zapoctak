using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game;

namespace Zapoctak.game.events
{
    class MagicEvent : EventData
    {
        public Magic magic;
        public double progress; //0 - duration
        public const double duration = 1.5;

        public MagicEvent(Magic magic)
        {
            this.magic = magic;
        }

        public bool update(double dt)
        {
            progress += dt;
            if (progress >= duration)
            {
                progress = duration;
                return true;
            }
            return false;
        }

        public Effect getEffect(Entity caster)
        {
            Effect effect = magic.effect.Clone();
            effect.amount += caster.stats.magic;
            return effect;
        }
    }
}
