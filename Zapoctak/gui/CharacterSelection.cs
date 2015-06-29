using System;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Drawing;
using Zapoctak.game;
using Zapoctak.resources;

namespace Zapoctak.gui
{
    public class CharacterSelection
    {
        public delegate void CharacterChanged(Character newChar); //can be null
        public event CharacterChanged characterChangedEvent;

        public const int maxChars = 4;
        const string addString = "Add";
        const string removeString = "Remove";
        const string undefinedString = "Unknown";

        private Color unselectedCol = Color.Transparent;
        private Color selectedCol = Color.Red;

        public Button[] addRemove = new Button[maxChars];
        public Button[] prev = new Button[maxChars];
        public Button[] next = new Button[maxChars];
        public Label[] classLabel = new Label[maxChars];
        public PictureBox[] pictures = new PictureBox[maxChars];
        public Character[] currentSelection = new Character[maxChars];
        public bool[] selected = new bool[maxChars];

        public UserControl1 control;

        public CharacterSelection()
        {
            for(int i = 0; i<maxChars; i++) {
                currentSelection[i] = new Character();
            }
        }

        public Character[] gatherChars()
        {
            List<Character> chars = new List<Character>();
            for (int i = 0; i < maxChars; i++)
                if (selected[i]) chars.Add(currentSelection[i]);
            return chars.ToArray();
        }

        public void bindHadlers()
        {
            for (int i = 0; i < maxChars; i++)
            {
                addRemove[i].Tag = i;
                addRemove[i].Click += new EventHandler(addRemoveHandler);

                prev[i].Tag = i;
                prev[i].Click += new EventHandler(prevHandler);

                next[i].Tag = i;
                next[i].Click += new EventHandler(nextHandler);

                classLabel[i].Tag = pictures[i].Tag = i;
                classLabel[i].Click += new EventHandler(focusHandler);
                pictures[i].Click += new EventHandler(focusHandler);
            }
        }

        private void updateByCurrent(int id)
        {
            for (int i = 0; i < maxChars; i++) classLabel[i].BackColor = unselectedCol;
            classLabel[id].BackColor = selectedCol;

            if (!selected[id])
            {
                addRemove[id].Text = addString;
                classLabel[id].Text = undefinedString;
                pictures[id].Image = TextureManager.getOtherTexture(TextureManager.noChar);
            }
            else
            {
                addRemove[id].Text = removeString;
                classLabel[id].Text = currentSelection[id].info.name;
                pictures[id].Image = currentSelection[id].info.image;
            }
            characterChangedEvent(selected[id] ? currentSelection[id] : null);
            Shop.shop.recompute();
            control.Refresh();
        }

        private void addRemoveHandler(object Sender, EventArgs args)
        {
            int id = (int) ((Control)Sender).Tag;
            if (!selected[id])
            {
                currentSelection[id].info = CharacterInfo.allInfos[0];
                selected[id] = true;
            }
            else
            {
                selected[id] = false;
            }
            updateByCurrent(id);
        }

        private void prevHandler(object Sender, EventArgs args)
        {
            int id = (int)((Control)Sender).Tag;
            if (selected[id])
            {
                currentSelection[id].info = CharacterInfo.next(currentSelection[id].info, -1);
                updateByCurrent(id);
            }
        }

        private void nextHandler(object Sender, EventArgs args)
        {
            int id = (int)((Control)Sender).Tag;
            if (selected[id])
            {
                currentSelection[id].info = CharacterInfo.next(currentSelection[id].info, 1);
                updateByCurrent(id);
            }
        }

        private void focusHandler(object Sender, EventArgs args)
        {
            int id = (int)((Control)Sender).Tag;
            updateByCurrent(id);
        }
    }
}
