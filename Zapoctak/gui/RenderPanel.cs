using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using Zapoctak.resources;
using Zapoctak.game;
using Zapoctak.game.monsters;
using Zapoctak.game.events;

namespace Zapoctak.gui
{
    public class RenderPanel : Panel
    {
        public static int[][] spliting = new int[][]{
            new int[0], //indexing from 1 lol
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

        private static Font floatFont = new Font(FontFamily.GenericMonospace, 20, FontStyle.Bold);

        private Image background = TextureManager.getOtherTexture(bgName);
        private Game game;

        private Matrix projection; //final frojection from virtual to screen coordinates
        private Matrix entityMatrix; //scaling matrix for all entities
        private Matrix weaponPos, armorPos, weaponRot;

        private Matrix dummy;

        private float[,] positions;

        private InfoPanel infoPanel;
        private Matrix infoMatrix, menuMatrix;

        public RenderPanel(Game game)
        {
            this.game = game;
            game.panel = this;

            float ratio = .75f;
            Width = (int)(width * ratio);
            Height = 10 + 150 + (int)(height * ratio);

            var margin = Margin;
            margin.All = 0;
            Margin = margin;

            initMatrices(ratio);
            infoPanel = new InfoPanel(game.characters);

            this.DoubleBuffered = true;
            //this.OnKeyDown += new KeyEventHandler(game.selector.OnKeyPress);
            this.Focus();
        }

        public void EntityDied()
        {
            //entity positions
            positions = new float[game.entities.Length, 2];
            initPlayerPos();
            initMonsterPos();
        }

        private void initMatrices(float ratio)
        {
            //final frojection from virtual to acctuall coordinates
            projection = new Matrix();
            projection.Scale(ratio, ratio, MatrixOrder.Append);

            entityMatrix = new Matrix();
            entityMatrix.Scale(.2f, .2f);

            //entity positions
            positions = new float[game.entities.Length, 2];
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

            //info
            infoMatrix = new Matrix();
            infoMatrix.Translate(0, 5 + ratio * height);

            //menu
            menuMatrix = new Matrix();
            menuMatrix.Translate(4*(130+10), 5 + ratio * height);
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
            float monsterDX = 120;
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

        public void update(double time)
        {
            infoPanel.update(time);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics gr = e.Graphics;

            gr.Transform = projection;
            gr.DrawImage(background, 0, 0, width, height);

            //attacking
            Entity attacking = null;
            Event curEvent = game.processer.getCurEvent();
            if (curEvent != null && curEvent.data is AttackEvent)
            {
                attacking = curEvent.source;
            }

            //characters
            for (int i = 0; i < game.characters.Length; i++)
            {
                if (game.characters[i] != attacking)
                    drawCharacter(gr, game.characters[i], defPos(game.characters[i].entityId));
            }

            //monsters
            for (int i = 0; i < game.monsters.Length; i++)
            {
                if (game.monsters[i] != attacking)
                    drawMonster(gr, game.monsters[i], defPos(game.monsters[i].entityId));
            }

            //floating text
            for (int i = 0; i < game.entities.Length; i++)
            {
                if (game.entities[i] != attacking)
                    drawFloatingTexts(gr, game.entities[i], defPos(game.entities[i].entityId));
            }

            //attacking
            if (attacking != null)
            {
                int sId = curEvent.source.entityId;
                int tId = curEvent.target.entityId;
                Matrix pos = (curEvent.data as AttackEvent).positionTrans(
                    positions[sId, 0], positions[sId, 1], positions[tId, 0], positions[tId, 1]);
                if (attacking is Character)
                    drawCharacter(gr, attacking as Character, pos);
                else
                    drawMonster(gr, attacking as Monster, pos);
                drawFloatingTexts(gr, attacking, pos);
            }

            //info
            gr.Transform = infoMatrix;
            infoPanel.paint(gr);

            //menu
            gr.Transform = menuMatrix;
            game.selector.paint(gr);
        }

        //with projection
        private void setDummyOn(Matrix pos)
        {
            dummy.Reset();
            dummy.Multiply(entityMatrix);
            dummy.Multiply(pos, MatrixOrder.Append);
            dummy.Multiply(projection, MatrixOrder.Append);
        }

        private Matrix defPos(int entId)
        {
            Matrix pos = new Matrix();
            pos.Translate(positions[entId, 0], positions[entId, 1]);
            return pos;
        }

        private void drawCharacter(Graphics gr, Character charac, Matrix pos)
        {
            setDummyOn(pos);
            gr.Transform = dummy;
            gr.DrawImage(charac.info.image,
                -entWidth / 2, -entHeight / 2, entWidth, entHeight);

            if (charac.weapon != null)
            {
                setDummyOn(pos);
                dummy.Multiply(weaponPos, MatrixOrder.Prepend);
                dummy.Multiply(weaponRot, MatrixOrder.Prepend);
                gr.Transform = dummy;
                gr.DrawImage(charac.weapon.image,
                    -entWidth / 2, -entHeight * 3 / 4, entWidth, entHeight);
            }

            if (charac.armor != null)
            {
                setDummyOn(pos);
                dummy.Multiply(armorPos, MatrixOrder.Prepend);
                gr.Transform = dummy;
                gr.DrawImage(charac.armor.image,
                    -entWidth / 2, -entHeight / 2, entWidth, entHeight);
            }

            drawFloatingTexts(gr, charac, pos);
        }

        private void drawMonster(Graphics gr, Monster m, Matrix pos)
        {
            setDummyOn(pos);
            gr.Transform = dummy;
            gr.DrawImage(m.info.texture, -entWidth / 2f, -entHeight / 2f, entWidth, entHeight);
            drawFloatingTexts(gr, m, pos);
        }

        private void drawFloatingTexts(Graphics gr, Entity ent, Matrix pos)
        {
            //also selector
            if (ent == game.selector.targetetEntity)
            {
                setDummyOn(pos);
                dummy.Translate(0, -300);
                gr.Transform = dummy;
                gr.DrawImage(MenuSelector.cursor, -128, -128, 256, 256);
            }

            //flaoting texts
            float offset = -300;
            foreach (var text in ent.getTexts())
            {
                setDummyOn(pos);
                dummy.Translate(-200, -300+offset * (float)text.progress, MatrixOrder.Prepend);
                dummy.Scale(5, 5, MatrixOrder.Prepend);
                gr.Transform = dummy;

                gr.DrawString(text.text, floatFont, new SolidBrush(text.color), 0, 0);
            }
        }
    }
}
