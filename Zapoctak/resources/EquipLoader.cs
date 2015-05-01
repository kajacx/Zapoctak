using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Zapoctak.game;

namespace Zapoctak.resources
{
    class EquipLoader
    {
        public static Equip[] readWeapons()
        {
            return readFromDir(ResourceManager.loadFile("data/entities/weapons"), EquipType.WEAPON);
        }

        public static Equip[] readArmors()
        {
            return readFromDir(ResourceManager.loadFile("data/entities/armors"), EquipType.ARMOR);
        }

        private static Equip[] readFromDir(FileInfo dir, EquipType type)
        {
            var equips = new List<Equip>();

            foreach (var name in Directory.GetFiles(dir.FullName))
            {
                addEquips(name, equips);
            }

            foreach (var eq in equips) eq.type = type;

            return equips.ToArray();
        }

        private static void addEquips(string filename, List<Equip> list)
        {
            var stream = new Reader(filename);

            while (!stream.EOF())
            {
                string line = stream.Line().Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                Equip equip = fromLine(line);
                if (equip != null)
                {
                    list.Add(equip);
                }
            }

            stream.Close();
        }

        private static Equip fromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 9)
            {
                Log.e("not enought arguments when creating equip: " + line);
                return null;
            }
            Equip equip = new Equip();
            equip.name = words[0];

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
                Log.e("Parse error in character: " + line, ex);
            }

            equip.image = TextureManager.getEquipTexture(words[8]);

            return equip;
        }
    }
}
