using System;
using System.Drawing;

namespace Zapoctak.game
{
    public class Equip
    {
        public static Equip[] allWeapons;
        public static Equip[] allArmors;

        public string name;
        public int cost;
        public Stats stats = new Stats();
        public Image image;
        public EquipType type;

        public override string ToString()
        {
            return name;
        }
    }

    public enum EquipType {NULL, WEAPON, ARMOR};
}
