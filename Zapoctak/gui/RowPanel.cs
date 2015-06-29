using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Zapoctak.game;

namespace Zapoctak.gui
{
    public class RowPanel
    {
        private Character character;
        private DisplayPanel hpPanel, mpPanel, timePanel;

        private static Matrix hpMatrix = new Matrix(), mpMatrix = new Matrix(), timeMatrix = new Matrix();
        private static Brush redBrush = new SolidBrush(Color.Red);
        private static Brush grayBrush = new SolidBrush(Color.Gray);

        public RowPanel(Character character)
        {
            this.character = character;
            hpPanel = DisplayPanel.HpPanel(character.stats.maxhp);
            mpPanel = DisplayPanel.MpPanel(character.stats.maxmp);
            timePanel = DisplayPanel.TimePanel();
        }

        public void update(double time)
        {
            hpPanel.setValue(character.isDead ? 0 : character.hp);
            hpPanel.update(time);

            mpPanel.setValue(character.isDead ? 0 : character.mp);
            mpPanel.update(time);

            timePanel.setValue(character.isDead ? 0 : character.time * 100);
            if (!character.isDead && character.time >= .999)
                timePanel.setMsg(character.msg);
            timePanel.update(time);
        }

        public void paint(Graphics gr)
        {
            Brush brush;
            if (character.game.selector.activeCharacter == character) brush = redBrush;
            else if (character.isDead) brush = grayBrush;
            else brush = DisplayPanel.fontBrush;
            gr.DrawString(character.info.name, DisplayPanel.font, brush, 15, 5);

            Matrix orig = gr.Transform;
            Matrix tmp;

            tmp = orig.Clone();
            tmp.Multiply(hpMatrix, MatrixOrder.Prepend);
            gr.Transform = tmp;
            hpPanel.paint(gr);

            tmp = orig.Clone();
            tmp.Multiply(mpMatrix, MatrixOrder.Prepend);
            gr.Transform = tmp;
            mpPanel.paint(gr);

            tmp = orig.Clone();
            tmp.Multiply(timeMatrix, MatrixOrder.Prepend);
            gr.Transform = tmp;
            timePanel.paint(gr);

            gr.Transform = orig;
        }

        public static void Init()
        {
            hpMatrix.Translate(130 + 10, 0);
            mpMatrix.Translate(2*(130 + 10), 0);
            timeMatrix.Translate(3*(130 + 10), 0);
        }
    }
}
