using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.gui;

namespace Zapoctak.game.events
{
    public class EventProcesser
    {
        private Queue<Event> queue = new Queue<Event>();
        private Event current; //not in queue

        public void addAttackEvent(Entity source, Entity target)
        {
            queue.Enqueue(new Event(source, target, new AttackEvent()));
        }

        public void addMagicEvent(Entity source, Entity target, Magic magic)
        {
            queue.Enqueue(new Event(source, target, new MagicEvent(magic)));
        }

        public void Enqueue(Event ev)
        {
            queue.Enqueue(ev);
        }

        public Event getCurEvent()
        {
            return current;
        }

        public void update(double dt)
        {
            if (current == null)
            {
                if (queue.Count > 0)
                {
                    current = queue.Dequeue();
                }
                else return;
            }
            if (current != null)
            {
                if (current.data.update(dt))
                {
                    processEffect(current);
                    current = null;
                }
            }
        }

        public void processEffect(Event ev)
        {
            Effect ef = ev.data.getEffect(ev.source);
            Entity target = ev.target;

            //recompute
            if (ef.damageHeal == DamageHeal.DAMAGE && ef.type == DamageType.AD)
            {
                ef.amount -= target.stats.armor;
                ef.amount = Math.Max(ef.amount, 1);
            }
            else if (ef.damageHeal == DamageHeal.DAMAGE && ef.type == DamageType.AP)
            {
                ef.amount -= target.stats.resist;
                ef.amount = Math.Max(ef.amount, 1);
            }

            //apply
            double value = ef.amount * (ef.damageHeal == DamageHeal.DAMAGE ? -1 : 1);
            if (ef.target == Target.HP) target.hp = U.Clamp(target.hp + value, 0, target.stats.maxhp);
            if (ef.target == Target.MP) target.mp = U.Clamp(target.mp + value, 0, target.stats.maxmp);

            //add floater
            Log.D("Adding floating text for Effect: " + ef);
            target.getTexts().Add(FloatingText.FromEffect(ef));

            //reset time loader
            ev.source.TimeReset();
        }
    }
}
