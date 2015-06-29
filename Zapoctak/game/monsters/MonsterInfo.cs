using System;
using System.Drawing;
using System.Collections.Generic;

namespace Zapoctak.game.monsters
{
    public class MonsterInfo
    {
        public static MonsterInfo[] allMonsterInfos;

        public string name;
        public Stats stats = new Stats();
        public List<Plan> plans = new List<Plan>();
        public double spawnProb;
        public double difficulty;
        public Image texture;

        private double probSum;

        public MonsterInfo()
        {
            Attack atk = new Attack();
            atk.prob = 10;
            atk.target = Plan.Target.FOE;
            plans.Add(atk);
        }

        public void countProbSum()
        {
            probSum = 0;
            foreach (Plan p in plans)
                probSum += p.prob;
        }

        public Plan randPlan(double mp)
        {
            double rem = U.ran.NextDouble()*probSum;
            Plan plan = null;
            foreach (Plan p in plans)
            {
                rem -= p.prob;
                if (rem < 0)
                {
                    plan = p;
                    break;
                }
            }
            if (plan == null)
            {
                Log.E("Failure in random plan selection");
                return null;
            }
            if (plan is UseMagic && (plan as UseMagic).magic.manaCost > mp) {
                Log.B("Monster drawed magic plan with low mana, drawing attack instead");
                return plans[0];
            }
            return plan;
        }
    }
}
