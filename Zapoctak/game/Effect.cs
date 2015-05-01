using System;
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
    }

    public enum DamageType { AD, AP, TRUE };
    public enum Target { HP, MP };
    public enum DamageHeal { DAMAGE, HEAL };
}
