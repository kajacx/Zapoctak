using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;

namespace Zapoctak.resources
{
    static class TextureManager
    {
        //ALL pathsin format: data/img/...
        private const string NOT_FOUND = "assets/img/other/texture_not_found.png";
        private static Dictionary<String, Image> textures = new Dictionary<string, Image>();

        public const string noChar = "no_character.png";
        public const string defWeapon = "";

        private static Image loadFromFile(string path)
        {
            FileInfo info = ResourceManager.loadFile(path);
            if (info.Exists)
            {
                return Image.FromFile(info.FullName);
            }
            else
            {
                Image ret;
                Log.W("Texture not found: " + path);
                textures.TryGetValue(NOT_FOUND, out ret);
                return ret;
            }
        }

        private static Image addTexture(string path)
        {
            return textures[path] = loadFromFile(path);
        }

        private static Image getTexture(string path)
        {
            Image ret;
            if (!textures.TryGetValue(path, out ret))
                return addTexture(path);
            return ret;
        }

        public static void initAll()
        {
            addTexture(NOT_FOUND);
        }

        //only name with extension
        public static Image getEquipTexture(string name)
        {
            return getTexture("assets/img/equip/" + name);
        }

        public static Image getCharacterTexture(string name)
        {
            return getTexture("assets/img/characters/" + name);
        }

        public static Image getOtherTexture(string name)
        {
            return getTexture("assets/img/other/" + name);
        }

        public static Image getMonsterTexture(string name)
        {
            return getTexture("assets/img/monsters/" + name);
        }

        public static Image getMagicTexture(string name)
        {
            return getTexture("assets/img/magic/" + name);
        }
    }
}
