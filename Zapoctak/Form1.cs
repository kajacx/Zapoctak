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
using Zapoctak.game.monsters;
using Zapoctak.gui;

namespace Zapoctak
{
    public partial class Form1 : Form
    {
        private CharacterSelection charSel = new CharacterSelection();
        private EquipSelection equipSel = new EquipSelection();
        private UserControl1 control;
        private Game game;

        public Form1()
        {
            InitializeComponent();
            control = new UserControl1();
            initStuff();

            Controls.Add(control);
            this.Focus();
        }

        private void initStuff()
        {
            //load data from files
            TextureManager.initAll();
            CharacterInfo.allInfos = FileLineLoader.LoadCharInfos();
            Equip.allWeapons = FileLineLoader.LoadWeapons();
            Equip.allArmors = FileLineLoader.LoadArmors();
            Magic.Init();

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

            //play button
            control.getPlayButton().Click += new EventHandler(playPressed);

            //monster infos
            MonsterInfo.allMonsterInfos = FileLineLoader.LoadMonstersInfos();

            //drawing panels
            DisplayPanel.Init();
            RowPanel.Init();
        }

        private void playPressed(object sender, EventArgs args)
        {
            Controls.Remove(control);

            game = new Game(charSel.gatherChars());
            game.form = this;

            Controls.Add(new RenderPanel(game));
            Refresh();

            game.Start();

            this.Invalidate();
            this.Refresh();
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            Log.D("Key pressed IN CMD: " + keyData);
            game.selector.KeyPressed(keyData);
            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            /*Log.D("Key pressed IN FORN: " + e.KeyCode);
            if(game!=null)
            game.selector.OnKeyPress(sender, e);*/
        }

    }
}
