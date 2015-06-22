using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using Zapoctak.game.monsters;
using Zapoctak.gui;

namespace Zapoctak.game
{
    public class Game
    {
        public const double timeLoadSpeed = .1; //5 seconds to load

        public Character[] characters;
        public Monster[] monsters;
        public RenderPanel panel;

        private double maxFps = 60, minFps = 30;
        private Stopwatch watch;
        private long lastMilis;

        delegate void Runnable();

        public Game(Character[] characters)
        {
            this.characters = characters;
            for (int i = 0; i < characters.Length; i++) {
                characters[i].entityId = i;
                characters[i].replenish();
            }

            monsters = MonsterSpawner.spawn(characters.Length);
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

                    update(time);

                    //panel.Refresh();
                    Runnable r = panel.Refresh;
                    panel.Invoke(r, new object[0]);

                    lastMilis = curMilis;
                    Thread.Sleep((int)(1000 / maxFps));
                }
                catch (Exception ex)
                {
                    Log.d("Exception in main program loop", ex);
                    run = false;
                }
            }
        }

        private void update(double dt)
        {
            foreach(var charac in characters) charac.update(dt);
            panel.update(dt);
        }
    }
}
