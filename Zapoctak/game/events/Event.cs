using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game;

namespace Zapoctak.game.events
{
    public class Event
    {
        public Entity source, target;
        public EventData data;

        public Event(Entity source, Entity target, EventData data)
        {
            this.source = source;
            this.target = target;
            this.data = data;
        }

        public override string ToString()
        {
            return String.Format("Event[from:{0}, to:{1}, data:{2}]", source, target, data);
        }
    }
}
