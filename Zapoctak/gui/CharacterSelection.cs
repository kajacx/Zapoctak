using System;
using System.Windows.Forms;
using Zapoctak.game;
using Zapoctak.resources;

namespace Zapoctak.gui
{
    public class CharacterSelection
    {
        const int maxChars = 4;
        const string addString = "Add";
        const string removeString = "Remove";
        const string undefinedString = "Undefined";

        public Button[] addRemove = new Button[maxChars];
        public Button[] prev = new Button[maxChars];
        public Button[] next = new Button[maxChars];
        public Label[] classLabel = new Label[maxChars];
        public PictureBox[] pictures = new PictureBox[maxChars];
        public CharacterInfo[] currentSelection = new CharacterInfo[maxChars];

        public UserControl1 control;

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
            }
        }

        private void updateByCurrent(int id)
        {
            if (currentSelection[id] == null)
            {
                addRemove[id].Text = addString;
                classLabel[id].Text = undefinedString;
                pictures[id].Image = TextureManager.getOtherTexture(TextureManager.noChar);
            }
            else
            {
                addRemove[id].Text = removeString;
                classLabel[id].Text = currentSelection[id].name;
                pictures[id].Image = currentSelection[id].image;
            }
            control.Refresh();
        }

        private void addRemoveHandler(object Sender, EventArgs args)
        {
            int id = (int) ((Control)Sender).Tag;
            if (currentSelection[id] == null)
            {
                currentSelection[id] = CharacterInfo.allInfos[0];
            }
            else
            {
                currentSelection[id] = null;
            }
            updateByCurrent(id);
        }

        private void prevHandler(object Sender, EventArgs args)
        {
            int id = (int)((Control)Sender).Tag;
            if (currentSelection[id] != null)
            {
                currentSelection[id] = CharacterInfo.next(currentSelection[id], -1);
                updateByCurrent(id);
            }
        }

        private void nextHandler(object Sender, EventArgs args)
        {
            int id = (int)((Control)Sender).Tag;
            if (currentSelection[id] != null)
            {
                currentSelection[id] = CharacterInfo.next(currentSelection[id], 1);
                updateByCurrent(id);
            }
        }
    }
}
