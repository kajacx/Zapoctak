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

        public RowPanel(Character character)
        {
            this.character = character;
            hpPanel = DisplayPanel.HpPanel(character.stats.maxhp);
            mpPanel = DisplayPanel.MpPanel(character.stats.maxmp);
            timePanel = DisplayPanel.TimePanel();
        }

        public void update(double time)
        {
            hpPanel.setValue(character.hp);
            hpPanel.update(time);

            mpPanel.setValue(character.mp);
            mpPanel.update(time);

            timePanel.setValue(character.time * 100);
            timePanel.update(time);
        }

        public void paint(Graphics gr)
        {
            gr.DrawString(character.info.name, DisplayPanel.font, DisplayPanel.fontBrush, 15, 5);

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
