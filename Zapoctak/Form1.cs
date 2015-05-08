using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Zapoctak.resources;
using Zapoctak.game;
using Zapoctak.gui;

namespace Zapoctak
{
    public partial class Form1 : Form
    {
        private CharacterSelection charSel = new CharacterSelection();
        private EquipSelection equipSel = new EquipSelection();
        private UserControl1 control;

        public Form1()
        {
            InitializeComponent();
            control = new UserControl1();
            initStuff();

            MyPanel panel = new MyPanel();
            panel.Size = new Size(512,512);
            panel.BackColor = Color.Azure;

            //Controls.Add(panel);

            Controls.Add(control);

        }

        private void initStuff()
        {
            //load data from files
            TextureManager.initAll();
            CharacterInfo.allInfos = FileLineLoader.LoadCharInfos();
            Equip.allWeapons = FileLineLoader.LoadWeapons();
            Equip.allArmors = FileLineLoader.LoadArmors();

            //character selection
            control.bind(charSel);
            charSel.control = control;
            charSel.bindHadlers();

            //equip selection
            control.bind(equipSel);
            equipSel.control = control;
            equipSel.init();

            //bind character and equip
            charSel.characterChangedEvent += new CharacterSelection.CharacterChanged(equipSel.setCharacter);
        }

    }

    class MyPanel : Panel
    {
        protected override void OnPaint(PaintEventArgs pe)
        {
            base.OnPaint(pe);
            Graphics g= pe.Graphics;

            g.TranslateTransform(256, 256);

            //now rotate the image
            g.RotateTransform(30);
        
            g.TranslateTransform(-256, -256);

            Image img = TextureManager.getOtherTexture("test_measure.png");
            //Console.WriteLine(img.Width);
            //Console.WriteLine(img.Height);
            g.DrawImage(img, 0, 0, 512, 512);
            //g.DrawImage(img, 0, 0);
        }
    }
}
