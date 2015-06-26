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
            Log.I("Loading file: "+fi);
            if (!fi.Exists && !Directory.Exists(fi.FullName))
                Log.W("Loading non-existing file: " + fi);
            return fi;
        }
        
    }
}
