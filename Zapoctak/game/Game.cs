using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Zapoctak.game.monsters;

namespace Zapoctak.game
{
    class Game
    {
        public Character[] characters;
        public Monster[] monsters;

        public Game(Character[] characters)
        {
            this.characters = characters;
            for (int i = 0; i < characters.Length; i++) characters[i].entityId = i;
            monsters = MonsterSpawner.spawn(characters.Length);
        }

        private void init()
        {

        }
    }
}
