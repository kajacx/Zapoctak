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

        public Plan randPlan()
        {
            double rem = U.ran.NextDouble()*probSum;
            foreach (Plan p in plans)
            {
                rem -= p.prob;
                if (rem < 0) return p;
            }
            Log.e("Failure in random plan selection");
            return null;
        }
    }
}
