using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game.monsters
{
    class MonsterSpawner
    {
        public static Monster[] spawn(int players)
        {
            int count = 2 * players - 1 + U.ran.Next(3);
            Log.D("Number of monsters: "+count);
            Monster[] ret = new Monster[count];

            double infosProbSum = 0;
            foreach (var mi in MonsterInfo.allMonsterInfos) infosProbSum += mi.spawnProb;

            double totalDiff = 0;
            for (int i = 0; i < count; i++)
            {
                Monster m = new Monster();
                m.info = randInfo(infosProbSum);
                m.stats = m.info.stats.Clone();
                m.entityId = players + i;
                ret[i] = m;
                totalDiff += m.info.difficulty;
            }

            double boost = players / totalDiff;
            foreach (Monster m in ret) { m.boost(boost); m.replenish(); }

            return ret;
        }

        private static MonsterInfo randInfo(double probSum)
        {
            double choice = U.ran.NextDouble() * probSum;
            foreach (var mi in MonsterInfo.allMonsterInfos)
            {
                choice -= mi.spawnProb;
                if (choice < 0) return mi;
            }
            Log.E("Random monster info selection failed");
            return null;
        }
    }
}
