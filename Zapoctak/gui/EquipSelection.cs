using System;
using System.Windows.Forms;
using System.Drawing;
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

        private Character curChar;
        private EquipType curType = EquipType.NULL;

        public UserControl1 control;

        public void init()
        {
            weaponPanel.Tag = EquipType.WEAPON;
            weaponPanel.Click += new EventHandler(onPanelClicked);
            armorPanel.Tag = EquipType.ARMOR;
            armorPanel.Click += new EventHandler(onPanelClicked);

            purchaseWeapons = new FlowLayoutPanel();
            purchaseArmors = new FlowLayoutPanel();
            Panel[] panels = { purchaseWeapons, purchaseArmors };
            Equip[][] equips = { Equip.allWeapons, Equip.allArmors };

            for (int i = 0; i < 2; i++)
            {
                ((FlowLayoutPanel)panels[i]).FlowDirection = FlowDirection.TopDown;
                panels[i].AutoSize = true;
                panels[i].AutoScroll = true;
                panels[i].Width = purchasePanel.Width - 25;

                Label none = new Label();
                none.Text = "          (None)"; //trolling lvl = over 8000
                none.Width = 100;
                none.Height = 20;
                none.Tag = null;
                none.Click += new EventHandler(onEquipClicked);
                panels[i].Controls.Add(none);

                foreach (Equip e in equips[i])
                {
                    FlowLayoutPanel row = new FlowLayoutPanel();
                    row.FlowDirection = FlowDirection.LeftToRight;
                    row.Width = panels[i].Width;
                    row.Height = 20;
                    row.Tag = e;
                    row.Click += new EventHandler(onEquipClicked);

                    PictureBox box = new PictureBox();
                    box.Image = e.image;
                    box.Width = box.Height = row.Height;
                    box.SizeMode = PictureBoxSizeMode.StretchImage;
                    box.Tag = e;
                    box.Click += new EventHandler(onEquipClicked);
                    row.Controls.Add(box);

                    Label name = new Label();
                    name.Text = e.name;
                    name.Width = 100;
                    name.Height = row.Height;
                    name.Tag = e;
                    name.Click += new EventHandler(onEquipClicked);
                    row.Controls.Add(name);

                    Label cost = new Label();
                    cost.Text = e.cost.ToString();
                    cost.AutoSize = true;
                    cost.Tag = e;
                    cost.Click += new EventHandler(onEquipClicked);
                    row.Controls.Add(cost);

                    panels[i].Controls.Add(row);
                }
            }

            purchaseNothing = new Panel();
            Label nothing = new Label();
            nothing.Text = "Select a character first";
            nothing.Width = 150;
            nothing.Height = 30;
            purchaseNothing.Controls.Add(nothing);
            purchaseNothing.AutoSize = true;

            purchasePanel.Controls.Add(purchaseNothing);
        }

        private void onPanelClicked(object sender, EventArgs args)
        {
            if (curChar != null)
                setEquipType((EquipType)((Control)sender).Tag);
            Log.d("Equip panel selected");
        }

        private void onEquipClicked(object sender, EventArgs args)
        {
            if (curType == EquipType.WEAPON)
                curChar.weapon = (Equip)((Control)sender).Tag;
            else
                curChar.armor = (Equip)((Control)sender).Tag;
            setCharacter(curChar);
            Log.d("Equip selected");
        }

        public void setEquipType(EquipType type)
        {
            this.curType = type;
            purchasePanel.Controls.Remove(purchaseNothing);
            purchasePanel.Controls.Remove(purchaseWeapons);
            purchasePanel.Controls.Remove(purchaseArmors);
            switch (type)
            {
                case EquipType.NULL: purchasePanel.Controls.Add(purchaseNothing); break;
                case EquipType.WEAPON: purchasePanel.Controls.Add(purchaseWeapons); break;
                case EquipType.ARMOR: purchasePanel.Controls.Add(purchaseArmors); break;
            }
            control.Refresh();
        }

        private void fillRow(int row, Image def, Equip equip) {
            if (equip != null)
            {
                images[row].Image = equip.image;
                allLabels[row, COL_NAME].Text = equip.name;
                for (int i = 0; i < 6; i++)
                    allLabels[row, i + 1].Text = format(equip.stats.getStat(i));
                allLabels[row, COL_COST].Text = format(equip.cost);
            }
            else
            {
                images[row].Image = def;
                for (int i = 0; i < 8; i++)
                    allLabels[row, i].Text = defText;
            }
        }

        public void setCharacter(Character charac)
        {
            if (charac == null)
            {
                setEquipType(EquipType.NULL);
                curChar = null;

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
                if (curChar == null) setEquipType(EquipType.WEAPON);
                curChar = charac;

                allLabels[ROW_CHAR, COL_NAME].Text = charac.info.name;
                images[ROW_CHAR].Image = charac.info.image;
                charac.recomputeStats();

                for (int i = 0; i < 6; i++)
                {
                    allLabels[ROW_CHAR, i + 1].Text = format(charac.info.stats.getStat(i));
                    allLabels[ROW_SUM, i + 1].Text = format(charac.stats.getStat(i));
                }

                fillRow(ROW_WEAPON, TextureManager.getOtherTexture(defWeapon), charac.weapon);
                fillRow(ROW_ARMOR, TextureManager.getOtherTexture(defArmor), charac.armor);

                allLabels[ROW_SUM, COL_COST].Text =
                    ((charac.weapon == null ? 0 : charac.weapon.cost) +
                    (charac.armor == null ? 0 : charac.armor.cost)) + "";
            }
            control.Refresh();
        }

        public static string format(double d)
        {
            return d.ToString("0.#");
        }
    }
}
