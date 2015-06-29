using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zapoctak.gui;

namespace Zapoctak
{
    public partial class UserControl1 : UserControl
    {
        public UserControl1()
        {
            InitializeComponent();
        }

        //apply holy water to your eyes after looking at following binding methods

        public void bind(CharacterSelection sel)
        {
            sel.addRemove[0] = button1;
            sel.addRemove[1] = button2;
            sel.addRemove[2] = button3;
            sel.addRemove[3] = button4;

            sel.prev[0] = button5;
            sel.prev[1] = button6;
            sel.prev[2] = button7;
            sel.prev[3] = button8;

            sel.next[0] = button9;
            sel.next[1] = button10;
            sel.next[2] = button11;
            sel.next[3] = button12;

            sel.pictures[0] = pictureBox1;
            sel.pictures[1] = pictureBox2;
            sel.pictures[2] = pictureBox3;
            sel.pictures[3] = pictureBox4;

            sel.classLabel[0] = label2;
            sel.classLabel[1] = label3;
            sel.classLabel[2] = label4;
            sel.classLabel[3] = label5;
        }

        public void bind(EquipSelection sel)
        {
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_NAME] = label17;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_HP] = label18;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_ATK] = label19;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_DEF] = label20;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_MP] = label21;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_MAG] = label22;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_RES] = label23;
            sel.allLabels[EquipSelection.ROW_CHAR, EquipSelection.COL_COST] = label24;

            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_NAME] = label32;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_HP] = label31;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_ATK] = label30;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_DEF] = label29;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_MP] = label28;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_MAG] = label27;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_RES] = label26;
            sel.allLabels[EquipSelection.ROW_WEAPON, EquipSelection.COL_COST] = label25;

            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_NAME] = label40;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_HP] = label39;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_ATK] = label38;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_DEF] = label37;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_MP] = label36;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_MAG] = label35;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_RES] = label34;
            sel.allLabels[EquipSelection.ROW_ARMOR, EquipSelection.COL_COST] = label33;

            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_NAME] = label48;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_HP] = label47;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_ATK] = label46;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_DEF] = label45;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_MP] = label44;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_MAG] = label43;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_RES] = label42;
            sel.allLabels[EquipSelection.ROW_SUM, EquipSelection.COL_COST] = label41;

            sel.images[EquipSelection.ROW_CHAR] = pictureBox5;
            sel.images[EquipSelection.ROW_WEAPON] = pictureBox6;
            sel.images[EquipSelection.ROW_ARMOR] = pictureBox7;
            sel.images[EquipSelection.ROW_SUM] = pictureBox8;

            sel.purchasePanel = panel1;
            sel.weaponPanel = panel2;
            sel.armorPanel = panel3;
        }

        public void bind(Shop shop)
        {
            shop.moneyLabel = label7;
        }

        public Button getPlayButton()
        {
            return playButton;
        }
    }
}
