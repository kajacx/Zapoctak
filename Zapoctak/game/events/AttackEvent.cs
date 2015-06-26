using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing.Drawing2D;

namespace Zapoctak.game.events
{
    public class AttackEvent : EventData
    {
        private const double duration = 1.5; //duration of an attack
        public double progress; //0-1: forward, 1-2 backward

        public bool update(double dt)
        {
            progress += 2 * dt / duration;
            if (progress >= 2)
            {
                progress = 2;
                return true;
            }
            return false;
        }

        public Matrix positionTrans(float xs, float ys, float xd, float yd)
        {
            float coef = (float) (progress > 1 ? 2 - progress : progress);

            float x = coef * xd + (1 - coef) * xs;
            float y = coef * yd + (1 - coef) * ys;

            float yp = ((float)coef * 2 - 1) * U.SQRT2DIV2; //map
            yp = (float)Math.Sqrt(1 - yp * yp) - U.SQRT2DIV2;
            yp *= 200;

            Matrix ret = new Matrix();
            ret.Translate(x, y - yp);

            return ret;
        }

        public Effect getEffect(Entity caster)
        {
            Effect effect = new Effect();
            effect.amount = caster.stats.attack;
            effect.damageHeal = DamageHeal.DAMAGE;
            effect.target = Target.HP;
            effect.type = DamageType.AD;
            return effect;
        }
    }
}
