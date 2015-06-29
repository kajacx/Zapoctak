using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game.events;

namespace Zapoctak.game.monsters
{
    public class Monster : Entity
    {
        public MonsterInfo info;
        public void boost(double coef)
        {
            for (int i = 0; i < 6; i++) stats.setStat(i, stats.getStat(i) * coef);
        }

        override public void TimeReady()
        {
            game.monsterReady(this);
            Log.B(this + " ready");
        }

        public Event DrawRandomEvent()
        {
            Plan plan = info.randPlan(mp);

            Entity target = null;
            switch (plan.target)
            {
                case Plan.Target.SELF: target = this; break;
                case Plan.Target.ALLY: target = game.monsters[U.ran.Next(game.monsters.Length)]; break;
                case Plan.Target.FOE: target = game.characters[U.ran.Next(game.characters.Length)]; break;
                case Plan.Target.ALL: target = game.entities[U.ran.Next(game.entities.Length)]; break;
            }

            EventData data = plan.toEventData();

            return new Event(this, target, data);
        }

        public override string ToString()
        {
            return "Monster [" + info.name + "] entId: " + entityId;
        }
    }
}
