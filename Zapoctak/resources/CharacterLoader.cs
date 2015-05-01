using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Zapoctak.game;

namespace Zapoctak.resources
{
    class CharacterLoader
    {
        public static CharacterInfo[] readCharInfos()
        {
            var dir = ResourceManager.loadFile("data/entities/characters");

            var infos = new List<CharacterInfo>();

            foreach (var name in Directory.GetFiles(dir.FullName))
            {
                addCharacterInfos(name, infos);
            }

            return infos.ToArray();
        }

        private static void addCharacterInfos(string filename, List<CharacterInfo> list)
        {
            var stream = new Reader(filename);

            while (!stream.EOF())
            {
                string line = stream.Line().Trim();
                if (line.Length == 0 || line.StartsWith("#"))
                {
                    continue;
                }
                CharacterInfo info = fromLine(line);
                if (info != null)
                {
                    info.id = list.Count;
                    list.Add(info);
                }
            }

            stream.Close();
        }

        private static CharacterInfo fromLine(string line)
        {
            string[] words = line.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            if (words.Length < 8)
            {
                Log.e("not enought arguments when creating character: " + line);
                return null;
            }
            CharacterInfo info = new CharacterInfo();
            info.name = words[0];

            try
            {
                for (int i = 0; i < 6; i++)
                {
                    info.stats.setStat(i, Double.Parse(words[i + 1], CultureInfo.InvariantCulture));
                }
                /*info.stats.maxhp = Double.Parse(words[1], CultureInfo.InvariantCulture);
                stats.attack = Double.Parse(words[2], CultureInfo.InvariantCulture);
                stats.armor = Double.Parse(words[3], CultureInfo.InvariantCulture);
                stats.maxmp = Double.Parse(words[4], CultureInfo.InvariantCulture);
                stats.magic = Double.Parse(words[5], CultureInfo.InvariantCulture);
                stats.resist = Double.Parse(words[6], CultureInfo.InvariantCulture);*/
            }
            catch (Exception ex)
            {
                Log.e("Parse error in character: " + line, ex);
            }

            info.image = TextureManager.getCharacterTexture(words[7]);

            return info;
        }
    }
}
