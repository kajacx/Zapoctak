using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Drawing.Drawing2D;
using Zapoctak.resources;

namespace Zapoctak.gui
{
    public class DisplayPanel
    {
        private static Matrix barMatrix = new Matrix();
        public static Font font = new Font(FontFamily.GenericSerif, 15);
        public static Brush fontBrush = new SolidBrush(Color.Black);

        private const string heartImg = "heart.png";
        private const string manaImg = "mana.png";
        private const string timeImg = "zhonya.png";

        private Image image;
        private double cur, max;
        private DisplayBar bar;
        private bool inPercent;
        private string msg;

        private DisplayPanel(Image image, double max, DisplayBar bar, bool inPercent)
        {
            this.image = image;
            this.max = max;
            if (inPercent) cur = max;
            this.bar = bar;
            this.inPercent = inPercent;
        }

        public static DisplayPanel HpPanel(double maxHp)
        {
            return new DisplayPanel(TextureManager.getOtherTexture(heartImg), maxHp, DisplayBar.HpBar(maxHp), false);
        }

        public static DisplayPanel MpPanel(double maxMp)
        {
            return new DisplayPanel(TextureManager.getOtherTexture(manaImg), maxMp, DisplayBar.MpBar(maxMp), false);
        }

        public static DisplayPanel TimePanel()
        {
            return new DisplayPanel(TextureManager.getOtherTexture(timeImg), 100, DisplayBar.TimeBar(), true);
        }

        public void update(double time)
        {
            bar.update(time);
        }

        public void setValue(double val)
        {
            cur = val;
            bar.setDesiredVal(val);
        }

        private string createMsg()
        {
            if (inPercent)
            {
                if (msg != null) return msg;
                return (int)bar.getCurVal() + "%";
            }
            return (int)bar.getCurVal() + " / " + (int)max;
        }

        public void paint(Graphics gr)
        {
            gr.DrawImage(image, 0, 0, 30, 30);
            gr.DrawString(createMsg(), font, fontBrush, 30, 0);

            gr.MultiplyTransform(barMatrix, MatrixOrder.Prepend);
            bar.paint(gr);
        }

        public static void Init()
        {
            barMatrix.Translate(30, 20);
        }

        public void setMsg(string msg)
        {
            this.msg = msg;
        }
    }
}
