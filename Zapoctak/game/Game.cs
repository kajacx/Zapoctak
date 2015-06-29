using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Zapoctak.game.monsters;
using Zapoctak.gui;
using Zapoctak.game.events;

namespace Zapoctak.game
{
    public class Game
    {
        public const double timeLoadSpeed = .1; //5 seconds to load

        public Character[] characters;
        public Monster[] monsters;
        public Entity[] entities;
        public RenderPanel panel;
        public Form1 form;

        private double maxFps = 60, minFps = 30;
        private Stopwatch watch;
        private long lastMilis;

        delegate void Runnable();

        public EventProcesser processer = new EventProcesser();
        public MenuSelector selector = new MenuSelector();

        public Queue<Character> readyChars = new Queue<Character>();

        public Game(Character[] characters)
        {
            this.characters = characters;
            for (int i = 0; i < characters.Length; i++)
            {
                characters[i].entityId = i;
                characters[i].replenish();
                characters[i].game = this;
                characters[i].time = U.ran.NextDouble() / 3 + 0.4;
            }

            monsters = MonsterSpawner.spawn(characters.Length);
            for (int i = 0; i < monsters.Length; i++)
            {
                monsters[i].game = this;
                monsters[i].time = U.ran.NextDouble() / 3;
            }

            entities = new Entity[characters.Length + monsters.Length];
            Array.Copy(characters, 0, entities, 0, characters.Length);
            Array.Copy(monsters, 0, entities, characters.Length, monsters.Length);
            RelinkEntities(characters, monsters, RenderPanel.spliting[monsters.Length]);
        }

        public void monsterReady(Monster m)
        {
            processer.Enqueue(m.DrawRandomEvent());
        }

        public void charReady(Character c)
        {
            readyChars.Enqueue(c);
            if (readyChars.Count == 1)
            {
                selector.charReady(c);
            }
        }

        public void removeActiveChar()
        {
            readyChars.Dequeue();
            if (readyChars.Count >= 1)
            {
                selector.charReady(readyChars.Peek());
            }
        }

        public void charWait(Character c)
        {
            readyChars.Dequeue();
            readyChars.Enqueue(c);
            selector.charReady(readyChars.Peek());
        }

        public void charAttack(Character c, Entity target)
        {
            processer.addAttackEvent(c, target);
            c.msg = "Waiting";
            readyChars.Dequeue();
            if (readyChars.Count >= 1)
            {
                selector.charReady(readyChars.Peek());
            }
        }

        public void charMagic(Character c, Entity target, Magic m)
        {
            processer.addMagicEvent(c, target, m);
            c.msg = "Waiting";
            readyChars.Dequeue();
            if (readyChars.Count >= 1)
            {
                selector.charReady(readyChars.Peek());
            }
        }

        public void RelinkEntities(Character[] chars, Monster[] monsters, int[] splitting)
        {
            for (int col = 0, count = 0; col < splitting.Length; col++)
            {
                for (int row = 0; row < splitting[col]; row++, count++)
                {
                    //up
                    monsters[count].up = monsters[count + (row == splitting[col] - 1 ? 0 : 1)];

                    //down
                    monsters[count].down = monsters[count + (row == 0 ? 0 : -1)];

                    //left
                    if (col == 0) monsters[count].left = monsters[count];
                    else monsters[count].left = monsters[count - Math.Max(splitting[col - 1], row + 1)];

                    //right
                    if (col == splitting.Length - 1) monsters[count].right = monsters[count];
                    else monsters[count].right = monsters[count + splitting[col] +
                        Math.Min(0, splitting[col + 1] - row - 1)];
                }
            }
        }

        public void EntityDies(Entity ent)
        {
            Character[] newCharacters = characters;
            Monster[] newMonsters = monsters;

            if (ent is Character)
            {
                newCharacters = new Character[characters.Length - 1];
                for (int i = 0, j = 0; i < newCharacters.Length; i++, j++)
                    if (characters[j] == ent) i--;
                    else newCharacters[i] = characters[j];
            }
            else
            {
                newMonsters = new Monster[monsters.Length - 1];
                for (int i = 0, j = 0; i < newMonsters.Length; i++, j++)
                    if (monsters[j] == ent) i--;
                    else newMonsters[i] = monsters[j];
            }

            characters = newCharacters;
            monsters = newMonsters;

            if (checkGameOver()) return;

            entities = new Entity[newCharacters.Length + newMonsters.Length];
            Array.Copy(newCharacters, 0, entities, 0, newCharacters.Length);
            Array.Copy(newMonsters, 0, entities, newCharacters.Length, newMonsters.Length);

            for (int i = 0; i < entities.Length; i++)
                entities[i].entityId = i;

            RelinkEntities(newCharacters, newMonsters, RenderPanel.spliting[monsters.Length]);
            panel.EntityDied();
            selector.EntityDied();
        }

        private bool checkGameOver() //true if game over
        {
            if (characters.Length == 0)
            {
                Log.I("TODO: display defeat");
                MessageBox.Show("Defeat!");

                //form.Close();
                Runnable r = form.Close;
                form.Invoke(r);
                return true;
            }
            if (monsters.Length == 0)
            {
                Log.I("TODO: display wictory");
                MessageBox.Show("Wictory!");

                //form.Close();
                Runnable r = form.Close;
                form.Invoke(r);
                return true;
            }
            return false;
        }

        public void Start()
        {
            watch = new Stopwatch();
            watch.Start();
            lastMilis = watch.ElapsedMilliseconds;

            Thread t = new Thread(run);
            t.Start();
            //run();
        }

        private void run()
        {
            bool run = true;
            while (run)
            {
                try
                {
                    long curMilis = watch.ElapsedMilliseconds;
                    double time = (curMilis - lastMilis) / 1000d;

                    time = Math.Max(time, 1d / minFps);
                    update(time);

                    //panel.Refresh();
                    Runnable r = panel.Refresh;
                    panel.Invoke(r, new object[0]);

                    lastMilis = curMilis;
                    Thread.Sleep((int)(1000 / maxFps));
                }
                catch (Exception ex)
                {
                    Log.D("Exception in main program loop", ex);
                    run = false;
                }
            }
        }

        private void update(double dt)
        {
            foreach (var ent in entities) ent.update(dt);
            processer.update(dt);
            panel.update(dt);
        }
    }
}
