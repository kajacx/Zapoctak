﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game
{
    public class Effect
    {
        public double amount;
        public DamageType type;
        public Target target;
        public DamageHeal damageHeal;

        public Effect Clone()
        {
            Effect res = new Effect();
            res.amount = amount;
            res.type = type;
            res.target = target;
            res.damageHeal = damageHeal;
            return res;
        }

        public override string ToString()
        {
            return String.Format("Effect[amnt:{0}, type:{1}, target:{2}, dmg:{3}]",
                amount, type, target, damageHeal);
        }
    }

    public enum DamageType { AD, AP, TRUE };
    public enum Target { HP, MP };
    public enum DamageHeal { DAMAGE, HEAL };
}
