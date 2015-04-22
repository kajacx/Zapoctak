using System;
using System.Drawing;

namespace Zapoctak.game
{
    public class CharacterInfo
    {
        public static CharacterInfo[] allInfos;

        public int id;
        public string name;
        public double maxhp, attack, armor, maxmp, magic, resist;
        public Image image;

        public static CharacterInfo next(CharacterInfo info, int offset)
        {
            int id = (info.id + offset) % allInfos.Length;
            if (id < 0) id += allInfos.Length;
            return allInfos[id];
        }
    }
}
