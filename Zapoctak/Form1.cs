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

            //play button
            control.getPlayButton().Click += new EventHandler(playPressed);
        }

        private void playPressed(object sender, EventArgs args)
        {
            Controls.Remove(control);

            Game game = createGame();

            Controls.Add(new RenderPanel(game));
            Refresh();
        }

        private Game createGame()
        {
            Game game = new Game();
            game.characters = charSel.gatherChars();
            game.players = game.characters.Length;
            return game;
        }

    }
}
