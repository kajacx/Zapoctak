using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Zapoctak.resources;
using Zapoctak.game;
using Zapoctak.game.monsters;

namespace Zapoctak.gui
{
    class RenderPanel : Panel
    {
        private int[][] spliting = new int[][]{
            null, //indexing from 1 lol
            new int[]{ 1 },
            new int[]{ 2 },
            new int[]{ 3 },
            new int[]{ 2,2 },
            new int[]{ 3,2 },
            new int[]{ 3,3 },
            new int[]{ 3,2,2 },
            new int[]{ 3,3,2 },
            new int[]{ 3,3,3 }
        };

        private const string bgName = "background.png";
        private const float width = 1024, height = 512; // virtual
        private const float entWidth = 512, entHeight = 512; // entity dimensions

        private Image background = TextureManager.getOtherTexture(bgName);
        private Game game;

        private Matrix projection; //final frojection from virtual to screen coordinates
        private Matrix entityMatrix; //scaling matrix for all entities
        private Matrix weaponPos, armorPos, weaponRot;

        private Matrix dummy;

        private float[,] positions;

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

            entityMatrix = new Matrix();
            entityMatrix.Scale(.2f, .2f);

            //entity positions
            positions = new float[game.characters.Length+game.monsters.Length,2];
            initPlayerPos();
            initMonsterPos();

            //equip
            weaponPos = new Matrix();
            weaponPos.Scale(.8f, .8f);
            weaponPos.Translate(-160, -64, MatrixOrder.Append);

            armorPos = new Matrix();
            armorPos.Scale(.4f, .4f);
            armorPos.Translate(0, -32, MatrixOrder.Append);

            weaponRot = new Matrix();
            weaponRot.Rotate(-45);

            //dummy
            dummy = new Matrix();
        }

        private void initPlayerPos()
        {
            float playerX = .9f * width;
            float playerY = .5f * height;
            float playerDX = -10;
            float playerDY = 110;

            for (int i = 0; i < game.characters.Length; i++)
            {
                float coef = i - (game.characters.Length - 1) / 2f;
                positions[i, 0] = playerX + coef * playerDX;
                positions[i, 1] = playerY + coef * playerDY;
            }
        }

        private void initMonsterPos()
        {
            float monsterX = .2f * width;
            float monsterY = .5f * height;
            float monsterDX = -120;
            float monsterDY = -120;

            int[] split = spliting[game.monsters.Length];
            for (int i = 0, k = 0; i < split.Length; i++)
            {
                float x = monsterX + monsterDX * (i - (split.Length - 1) / 2f);
                for (int j = 0; j < split[i]; j++, k++)
                {
                    float y = monsterY + monsterDY * (j - (split[i] - 1) / 2f);
                    positions[game.characters.Length + k, 0] = x;
                    positions[game.characters.Length + k, 1] = y;
                }
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gr = e.Graphics;
            gr.Transform = projection;
            gr.DrawImage(background, 0, 0, width, height);

            for (int i = 0; i < game.characters.Length; i++)
            {
                drawCharacter(gr, i);
            }

            for (int i = 0; i < game.monsters.Length; i++)
            {
                drawMonster(gr, game.monsters[i]);
            }
        }

        //with projection
        private void setDummyOn(int entId)
        {
            dummy.Reset();
            dummy.Multiply(entityMatrix);
            dummy.Translate(positions[entId, 0], positions[entId, 1], MatrixOrder.Append);
            dummy.Multiply(projection, MatrixOrder.Append);
        }

        private void drawCharacter(Graphics gr, int i)
        {
            setDummyOn(i);
            gr.Transform = dummy;
            gr.DrawImage(game.characters[i].info.image,
                -entWidth / 2, -entHeight / 2, entWidth, entHeight);

            if (game.characters[i].weapon != null)
            {
                setDummyOn(i);
                dummy.Multiply(weaponPos, MatrixOrder.Prepend);
                dummy.Multiply(weaponRot, MatrixOrder.Prepend);
                gr.Transform = dummy;
                gr.DrawImage(game.characters[i].weapon.image,
                    -entWidth / 2, -entHeight * 3 / 4, entWidth, entHeight);
            }

            if (game.characters[i].armor != null)
            {
                setDummyOn(i);
                dummy.Multiply(armorPos, MatrixOrder.Prepend);
                gr.Transform = dummy;
                gr.DrawImage(game.characters[i].armor.image,
                    -entWidth / 2, -entHeight / 2, entWidth, entHeight);
            }
        }

        private void drawMonster(Graphics gr, Monster m)
        {
            setDummyOn(m.entityId);
            gr.Transform = dummy;
            gr.DrawImage(m.info.texture, -entWidth / 2f, -entHeight / 2f, entWidth, entHeight);
        }
    }
}
