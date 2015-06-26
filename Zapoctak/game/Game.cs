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

        private double maxFps = 60, minFps = 30;
        private Stopwatch watch;
        private long lastMilis;

        delegate void Runnable();

        public EventProcesser processer = new EventProcesser();

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
        }

        public void monsterReady(Monster m)
        {
            processer.Enqueue(m.DrawRandomEvent());
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

                    time = Math.Max(time, 1d/minFps);
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
