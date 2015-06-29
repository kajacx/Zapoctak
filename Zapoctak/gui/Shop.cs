using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zapoctak.game;

namespace Zapoctak.gui
{
    public class Shop
    {
        public const int MoneyPerCharacter = 500;
        public Label moneyLabel;
        public int money;
        public CharacterSelection selection;

        public static Shop shop = new Shop();

        public void recompute()
        {
            Character[] chars = selection.gatherChars();
            money = chars.Length * MoneyPerCharacter;
            foreach (var charac in chars)
            {
                money -= charac.weapon == null ? 0 : charac.weapon.cost;
                money -= charac.armor == null ? 0 : charac.armor.cost;
            }
            moneyLabel.Text = money + "";
        }
    }
}
