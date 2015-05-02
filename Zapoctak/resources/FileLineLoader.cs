using System;
using System.Collections.Generic;
using System.IO;
using System.Globalization;

using Zapoctak.game;

namespace Zapoctak.resources
{
    class FileLineLoader
    {
        private delegate T lineParser<T>(String line);

        public static CharacterInfo[] LoadCharInfos()
        {
            var ret = loadFromDir("data/entities/characters", charFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].id = i;
            return ret;
        }

        public static Equip[] LoadWeapons()
        {
            var ret = loadFromDir("data/entities/weapons", equipFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].type = EquipType.WEAPON;
            return ret;
        }

        public static Equip[] LoadArmors()
        {
            var ret = loadFromDir("data/entities/armors", equipFromLine);
            for (int i = 0; i < ret.Length; i++) ret[i].type = EquipType.ARMOR;
            return ret;
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
                T info = parser(line);
                if (info != null)
                {
                    list.Add(info);
                }
            }

            stream.Close();
        }

        private static CharacterInfo charFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 8)
            {
                Log.e("not enought arguments when creating character: " + line);
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
                Log.e("Parse error in character: " + line, ex);
            }

            info.image = TextureManager.getCharacterTexture(words[7]);

            return info;
        }

        private static Equip equipFromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 9)
            {
                Log.e("not enought arguments when creating equip: " + line);
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
                Log.e("Parse error in equip: " + line, ex);
            }

            equip.image = TextureManager.getEquipTexture(words[8]);

            return equip;
        }
    }
}
