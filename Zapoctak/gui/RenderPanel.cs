using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Zapoctak.resources;
using Zapoctak.game;

namespace Zapoctak.gui
{
    class RenderPanel : Panel
    {
        private const string bgName = "background.png";
        private const float width = 1024, height = 512; // virtual
        private const float entWidth = 512, entHeight = 512; // entity dimensions

        private Image background = TextureManager.getOtherTexture(bgName);
        private Game game;

        private Matrix projection;
        private Matrix[] playerPositions; //pre-multiplied by projection

        public RenderPanel(Game game)
        {
            this.game = game;

            float ratio = .75f;
            Width = (int)(width * ratio);
            Height = (int)(height * ratio);

            var margin = Margin;
            margin.All = 0;
            Margin = margin;

            initMatrices(ratio);
        }

        private void initMatrices(float ratio)
        {
            //final frojection from virtual to acctuall coordinates
            projection = new Matrix();
            projection.Scale(ratio, ratio, MatrixOrder.Append);

            float playerX = .9f * width;
            float playerY = .5f * height;
            float playerDX = -10;
            float playerDY = 110;
            playerPositions = new Matrix[game.players];

            for (int i = 0; i < game.players; i++)
            {
                playerPositions[i] = new Matrix();
                playerPositions[i].Scale(.2f, .2f, MatrixOrder.Append);
                float coef = (i - ((game.players - 1) / 2f));
                Console.WriteLine("i: {0}, c: {1}", i, coef);
                playerPositions[i].Translate(
                    playerX + coef * playerDX, playerY + coef * playerDY, MatrixOrder.Append);
                playerPositions[i].Multiply(projection, MatrixOrder.Append);
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gr = e.Graphics;
            gr.Transform = projection;
            gr.DrawImage(background, 0, 0, width, height);

            for (int i = 0; i < game.players; i++)
            {
                gr.Transform = playerPositions[i];
                gr.DrawImage(game.characters[i].info.image,
                    -entWidth / 2, -entHeight / 2, entWidth, entHeight);
            }
        }
    }
}
