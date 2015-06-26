using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Zapoctak.game;

namespace Zapoctak.gui
{
    class FloatingText
    {
        private const double duration = 2; //in seconds

        public double progress; //0-1
        public string text;
        public Color color;

        public bool update(double dt) //true if ended
        {
            progress += dt / duration;
            if (progress >= 1)
            {
                progress = 1;
                return true;
            }
            return false;
        }

        public static FloatingText FromEffect(Effect ef)
        {
            FloatingText ret = new FloatingText();

            //color
            if (ef.damageHeal == DamageHeal.DAMAGE && ef.target == Target.HP)
                ret.color = Color.Red;
            else if (ef.damageHeal == DamageHeal.DAMAGE && ef.target == Target.MP)
                ret.color = Color.MediumPurple;
            else if (ef.damageHeal == DamageHeal.HEAL && ef.target == Target.HP)
                ret.color = Color.LightGreen;
            else if (ef.damageHeal == DamageHeal.HEAL && ef.target == Target.MP)
                ret.color = Color.LightBlue;
            else
                Log.E("Invalid setting in effect: " + ef);

            //text
            if (ef.damageHeal == DamageHeal.DAMAGE)
                ret.text = "-";
            else
                ret.text = "+";

            ret.text = ret.text + (int)ef.amount;

            if (ef.target == Target.HP)
                ret.text = ret.text + "HP";
            else
                ret.text = ret.text + "MP";

            //return
            return ret;
        }
    }
}
