using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Zapoctak.game;

namespace Zapoctak.gui
{
    class InfoPanel
    {
        private RowPanel[] panels;
        private static Matrix rowMatrix = new Matrix();

        public InfoPanel(Character[] chars)
        {
            panels = new RowPanel[chars.Length];
            for (int i = 0; i < panels.Length; i++)
                panels[i] = new RowPanel(chars[i]);
            rowMatrix.Translate(0, 30+10);
        }

        public void update(double time)
        {
            for (int i = 0; i < panels.Length; i++)
                panels[i].update(time);
        }

        public void paint(Graphics gr)
        {
            for (int i = 0; i < panels.Length; i++)
            {
                panels[i].paint(gr);
                gr.MultiplyTransform(rowMatrix, MatrixOrder.Prepend);
            }
        }
    }
}
