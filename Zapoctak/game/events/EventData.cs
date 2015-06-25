using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Zapoctak.game.events
{
    public interface EventData
    {
        bool update(double dt); //true if finished
        Effect getEffect(Entity caster);
    }
}
