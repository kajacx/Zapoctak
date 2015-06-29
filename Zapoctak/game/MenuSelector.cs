using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using Zapoctak.resources;

namespace Zapoctak.game
{
    public class MenuSelector // for menu and target selection
    {
        public static Font font = new Font(FontFamily.GenericSerif, 15);
        public static Brush fontBrush = new SolidBrush(Color.Black);

        public static Image cursor = TextureManager.getOtherTexture("cursor.png");

        public static Keys OK = Keys.C, CANCEL = Keys.V;

        public Character activeCharacter;
        public Entity targetetEntity;

        public MenuState state = MenuState.WAIT;

        private Magic selectedMagic;
        public int cursorPosition;
        private Game game;

        public void charReady(Character charac)
        {
            game = charac.game;
            if (charac.isDead)
            {
                state = MenuState.WAIT;
                activeCharacter = null;
                targetetEntity = null;
                game.removeActiveChar();
            }
            else
            {
                activeCharacter = charac;
                state = MenuState.DEFAULT;
                cursorPosition = 0;
            }
        }

        public void paint(Graphics gr) //with correct matrix
        {
            float yoff = 25;
            switch (state)
            {
                case MenuState.WAIT:
                    gr.DrawString("Wait for your turn", font, fontBrush, 30, 10);
                    break;
                case MenuState.DEFAULT:
                    gr.DrawString("Attack", font, fontBrush, 30, 10);
                    gr.DrawString("Magic", font, fontBrush, 30, 10 + yoff);
                    gr.DrawString("Wait", font, fontBrush, 30, 10 + yoff * 2);
                    gr.DrawImage(cursor, 5, 5 + yoff * cursorPosition, 30, 30);
                    break;
                case MenuState.ATTACK:
                    gr.DrawString("Select target", font, fontBrush, 30, 10);
                    break;
            }
        }

        public void EntityDied() //after recomputation
        {
            if (targetetEntity != null && targetetEntity.isDead)
                targetetEntity = game.monsters[0];
            if (activeCharacter != null && activeCharacter.isDead)
            {
                state = MenuState.WAIT;
                activeCharacter = null;
                targetetEntity = null;
                game.removeActiveChar();
            }
        }

        public void KeyPressed(Keys key)
        {
            switch (state)
            {
                case MenuState.WAIT: return;
                case MenuState.DEFAULT: keyPressedDef(key); return;
                case MenuState.ATTACK: keyPressedAttack(key); return;
            }
        }

        private void keyPressedDef(Keys key)
        {
            switch (key)
            {
                case Keys.Up: cursorPosition = U.Mod(cursorPosition - 1, 3); return;
                case Keys.Down: cursorPosition = U.Mod(cursorPosition + 1, 3); return;
            }
            if (key == OK)
            {
                switch (cursorPosition)
                {
                    case 0: state = MenuState.ATTACK; targetetEntity = game.monsters[0]; break;
                    case 1: Log.I("Todo: magic"); break;
                    case 2: game.charWait(activeCharacter); break;
                }
            }
        }

        private void keyPressedAttack(Keys key)
        {
            if (key == OK)
            {
                Entity ent = targetetEntity;
                Character charac = activeCharacter;
                state = MenuState.WAIT; targetetEntity = null; activeCharacter = null;
                game.charAttack(charac, ent);
                return;
            }
            if (key == CANCEL)
            {
                state = MenuState.DEFAULT; targetetEntity = null;
                return;
            }
            switch (key)
            {
                case Keys.Up: targetetEntity = targetetEntity.up; break;
                case Keys.Right: targetetEntity = targetetEntity.right; break;
                case Keys.Down: targetetEntity = targetetEntity.down; break;
                case Keys.Left: targetetEntity = targetetEntity.left; break;
            }
        }
    }

    public enum MenuState { WAIT, DEFAULT, ATTACK, MAGIC_CHOOSE, MAGIC };
}
