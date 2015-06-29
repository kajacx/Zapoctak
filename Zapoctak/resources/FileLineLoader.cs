using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

using Zapoctak.game;
using Zapoctak.game.monsters;

namespace Zapoctak.resources
{
    class FileLineLoader
    {
        private delegate T lineParser<T>(String line);

        public static CharacterInfo[] LoadCharInfos()
        {
            var ret = loadFromDir("assets/entities/characters", charFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].id = i;
            return ret;
        }

        public static Equip[] LoadWeapons()
        {
            var ret = loadFromDir("assets/entities/weapons", equipFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].type = EquipType.WEAPON;
            return ret;
        }

        public static Equip[] LoadArmors()
        {
            var ret = loadFromDir("assets/entities/armors", equipFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].type = EquipType.ARMOR;
            return ret;
        }

        public static MonsterInfo[] LoadMonstersInfos()
        {
            var data = loadFromDir("assets/entities/monsters", monsterOrActionFromLine);
            var ret = new List<MonsterInfo>();
            for (int i = 0; i < data.Length; i++)
            {
                if (data[i] is MonsterInfo) ret.Add((MonsterInfo) data[i]);
                else ret[ret.Count - 1].plans.Add((Plan)data[i]);
            }
            ret.ForEach(m => m.countProbSum());
            return ret.ToArray();
        }

        public static Magic[] LoadMagic()
        {
            var magic = loadFromDir("assets/entities/magic", magicFromLine);
            return magic;
        }

        private static T[] loadFromDir<T>(String dirName, lineParser<T> parser)
        {
            var dir = ResourceManager.loadFile(dirName);
            var infos = new List<T>();

            foreach (var name in Directory.GetFiles(dir.FullName))
            {
                loadFromFile(name, infos, parser);
            }

            return infos.ToArray();
        }

        private static void loadFromFile<T>(string filename, List<T> list, lineParser<T> parser)
        {
            var stream = new Reader(filename);

            while (!stream.EOF())
            {
                string line = stream.Line().Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                try
                {
                    T info = parser(line);
                    if (info != null)
                    {
                        list.Add(info);
                    }
                }
                catch (Exception ex)
                {
                    Log.E("Error in loading line: "+line, ex);
                }
            }

            stream.Close();
        }

        private static CharacterInfo charFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 8)
            {
                Log.E("not enought arguments when creating character: " + line);
                return null;
            }
            CharacterInfo info = new CharacterInfo();
            info.name = words[0].Replace("_", " ");

            try
            {
                for (int i = 0; i < 6; i++)
                {
                    info.stats.setStat(i, Double.Parse(words[i + 1], CultureInfo.InvariantCulture));
                }
            }
            catch (Exception ex)
            {
                Log.E("Parse error in character: " + line, ex);
            }

            info.image = TextureManager.getCharacterTexture(words[7]);

            return info;
        }

        private static Equip equipFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 9)
            {
                Log.E("not enought arguments when creating equip: " + line);
                return null;
            }
            Equip equip = new Equip();
            equip.name = words[0].Replace("_", " ");

            try
            {
                for (int i = 0; i < 6; i++)
                {
                    equip.stats.setStat(i, Double.Parse(words[i + 1], CultureInfo.InvariantCulture));
                }
                equip.cost = Convert.ToInt32(words[7]);
            }
            catch (Exception ex)
            {
                Log.E("Parse error in equip: " + line, ex);
            }

            equip.image = TextureManager.getEquipTexture(words[8]);

            return equip;
        }

        private static Object monsterOrActionFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Equals("monster")) return (Object) monsterFromWords(words);
            else if (words[0].Equals("plan")) return (Object) planFromWords(words);
            Log.W("Input error, not monter nor plan: "+words[0]);
            return null;
        }

        private static MonsterInfo monsterFromWords(string[] words)
        {
            MonsterInfo ret = new MonsterInfo();
            ret.name = words[1].Replace("_", " ");

            for (int i = 0; i < 6; i++)
            {
                ret.stats.setStat(i, Double.Parse(words[i + 2], CultureInfo.InvariantCulture));
            }
            ret.spawnProb = Double.Parse(words[8], CultureInfo.InvariantCulture);
            ret.difficulty = Double.Parse(words[9], CultureInfo.InvariantCulture);
            ret.texture = TextureManager.getMonsterTexture(words[10]);
            return ret;
        }

        private static Plan planFromWords(string[] words)
        {
            return null;
        }

        private static Magic magicFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);

            //#name, mana, gold, amont, damage, target, frameDuration, frames
            Magic magic = new Magic();
            magic.name = words[0].Replace("_", " ");
            magic.goldCost = Convert.ToInt32(words[2]);
            magic.frameDuration = Double.Parse(words[6], CultureInfo.InvariantCulture);

            magic.effect = new Effect();
            magic.effect.amount = Double.Parse(words[3], CultureInfo.InvariantCulture);
            magic.effect.damageHeal = words[4].Equals("DAMAGE") ? DamageHeal.DAMAGE : DamageHeal.HEAL;
            magic.effect.target = words[5].Equals("HP") ? Target.HP : Target.MP;
            magic.effect.type = DamageType.AP;

            for(int i = 7; i<words.Length; i++)
                magic.frames.Add(TextureManager.getMagicTexture(words[i]));

            return magic;
        }
    }
}
