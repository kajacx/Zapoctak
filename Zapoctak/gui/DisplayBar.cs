using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;

namespace Zapoctak.gui
{
    public class DisplayBar
    {
        private const double speed = 0.5f; //full bars per second

        private Color background = Color.LightGray, border = Color.Black;
        private Color left, right, added, removed;
        private bool smooth;
        private double max, cur, des;

        private DisplayBar(Color left, Color right, double max)
        {
            this.left = left;
            this.right = right;
            this.max = max;
        }

        private DisplayBar(Color left, Color right, Color added, Color removed, double max)
            : this(left, right, max)
        {
            this.added = added;
            this.removed = removed;
            smooth = true;
            cur = des = max;
        }

        public static DisplayBar HpBar(double maxHp)
        {
            return new DisplayBar(Color.Green, Color.YellowGreen, Color.LightGreen, Color.Red, maxHp);
        }

        public static DisplayBar MpBar(double maxMp)
        {
            return new DisplayBar(Color.Blue, Color.Cyan, Color.LightBlue, Color.MediumPurple, maxMp);
        }

        public static DisplayBar TimeBar()
        {
            return new DisplayBar(Color.LightCoral, Color.NavajoWhite, 100);
        }

        public void setDesiredVal(double des)
        {
            if (smooth)
                this.des = des;
            else
                this.des = cur = des;
        }

        public double getCurVal()
        {
            return cur;
        }

        public void update(double time)
        {
            double mov = time * speed * max;
            if (Math.Abs(des-cur) < mov)
            {
                cur = des;
                return;
            }
            cur += Math.Sign(des - cur) * mov;
        }

        public void paint(Graphics gr) //100x10
        {
            Brush bgBrush = new SolidBrush(background);
            gr.FillRectangle(bgBrush, 0, 0, 100, 10);

            float mainEnd = (float)(100 * cur / max);
            using (Brush mainBrush =
                new LinearGradientBrush(new Point(0, 0), new Point((int)mainEnd, 10), left, right))
            {
                gr.FillRectangle(mainBrush, 0, 0, mainEnd, 10);
            }

            if (smooth)
            {
                float dis = (float)(100*(des-cur)/max);
                if (dis > 0)
                    gr.FillRectangle(new SolidBrush(added), mainEnd, 0, dis, 10);
                else
                    gr.FillRectangle(new SolidBrush(removed), mainEnd-dis, 0, -dis, 10);
            }

            Pen borderPen = new Pen(border, 2);
            gr.DrawRectangle(borderPen, 0, 0, 100, 10);
        }
    }
}
