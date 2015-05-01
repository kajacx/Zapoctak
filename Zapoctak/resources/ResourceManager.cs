using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Zapoctak.game;

namespace Zapoctak.resources
{
    class ResourceManager
    {
        public static FileInfo loadFile(string path)
        {
            FileInfo fi = new FileInfo("../../" + path);
            if (!fi.Exists) Log.w("Loading non-existing file: " + fi);
            return fi;
        }
        
    }
}
