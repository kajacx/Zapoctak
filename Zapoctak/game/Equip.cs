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
    }

    public enum EquipType {WEAPON, ARMOR};
}
