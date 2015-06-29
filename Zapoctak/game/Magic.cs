using System;
using System.Drawing;
using System.Collections.Generic;
using Zapoctak.gui;
using Zapoctak.game.events;
using Zapoctak.game.monsters;
using Zapoctak.resources;

namespace Zapoctak.game
{
    public class Magic
    {
        public static Magic[] allMagic;
        public static Dictionary<string, Magic> magicMap = new Dictionary<string, Magic>(); //lower case key

        public string name;
        public double manaCost;
        public Effect effect;
        public int goldCost;
        public double frameDuration;
        public List<Image> frames = new List<Image>();

        public static void Init()
        {
            allMagic = FileLineLoader.LoadMagic();
            foreach (Magic m in allMagic)
                magicMap.Add(m.name.ToLower(), m);
        }
    }
}
