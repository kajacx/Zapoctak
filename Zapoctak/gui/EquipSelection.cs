using System;
using System.Windows.Forms;
using Zapoctak.game;
using Zapoctak.resources;

namespace Zapoctak.gui
{
    public class EquipSelection
    {
        public const int ROW_CHAR = 0, ROW_WEAPON = 1, ROW_ARMOR = 2, ROW_SUM = 3;
        public const int COL_NAME = 0, COL_HP = 1, COL_ATK = 2, COL_DEF = 3,
            COL_MP = 4, COL_MAG = 5, COL_RES = 6, COL_COST = 7;

        public Label[,] allLabels = new Label[4, 8];
        public PictureBox[] images = new PictureBox[4];
        public Panel purchasePanel, weaponPanel, armorPanel;

        private const string defChar = "generic_character.png",
            defWeapon = "generic_weapon.png", defArmor = "generic_armor.png";
        private const string defText = "-", sumText = "Sum";

        private Panel purchaseWeapons, purchaseArmors, purchaseNothing;

        public void init()
        {
            purchaseWeapons = new FlowLayoutPanel();
            purchaseArmors = new FlowLayoutPanel();
            Panel[] panels = { purchaseWeapons, purchaseArmors };
            Equip[][] equips = { Equip.allWeapons, Equip.allArmors };

            for (int i = 0; i < 2; i++)
            {
                ((FlowLayoutPanel)panels[i]).FlowDirection = FlowDirection.TopDown;
                panels[i].AutoSize = true;
                panels[i].Width = purchasePanel.Width - 25;

                Label none = new Label();
                none.Text = "          (None)"; //trolling lvl = over 8000
                none.Width = 100;
                none.Height = 20;
                panels[i].Controls.Add(none);

                foreach (Equip e in equips[i])
                {
                    FlowLayoutPanel row = new FlowLayoutPanel();
                    row.FlowDirection = FlowDirection.LeftToRight;
                    row.Width = panels[i].Width;
                    row.Height = 20;

                    PictureBox box = new PictureBox();
                    box.Image = e.image;
                    box.Width = box.Height = row.Height;
                    box.SizeMode = PictureBoxSizeMode.StretchImage;
                    row.Controls.Add(box);

                    Label name = new Label();
                    name.Text = e.name;
                    name.Width = 100;
                    name.Height = row.Height;
                    row.Controls.Add(name);

                    Label cost = new Label();
                    cost.Text = e.cost.ToString();
                    cost.AutoSize = true;
                    row.Controls.Add(cost);

                    panels[i].Controls.Add(row);
                }
            }

            purchasePanel.Controls.Add(panels[1]);
        }

        public void setCharacter(Character charac)
        {
            if (charac == null)
            {
                for (int i = 0; i < 4; i++)
                    for (int j = 0; j < 8; j++)
                        allLabels[i, j].Text = defText;

                images[ROW_CHAR].Image = TextureManager.getOtherTexture(defChar);
                images[ROW_WEAPON].Image = TextureManager.getOtherTexture(defWeapon);
                images[ROW_ARMOR].Image = TextureManager.getOtherTexture(defArmor);

                allLabels[ROW_SUM, COL_NAME].Text = sumText;
            }
            else
            {
                allLabels[ROW_CHAR, COL_NAME].Text = charac.info.name;
                images[ROW_CHAR].Image = charac.info.image;
                charac.recomputeStats();

                for (int i = 0; i < 6; i++)
                {
                    allLabels[ROW_CHAR, i + 1].Text = format(charac.info.stats.getStat(i));
                    allLabels[ROW_SUM, i + 1].Text = format(charac.stats.getStat(i));
                }

                allLabels[ROW_SUM, COL_COST].Text =
                    ((charac.weapon == null ? 0 : charac.weapon.cost) +
                    (charac.armor == null ? 0 : charac.armor.cost)) + "";
            }
        }

        public static string format(double d)
        {
            return d.ToString("0.#");
        }
    }
}
