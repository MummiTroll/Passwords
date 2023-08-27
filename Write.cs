using System;
using System.IO;

namespace Passwords
{
    public class Write
    {
        public string dir
        {
            get
            {
                string dir = Path.GetPathRoot(System.Reflection.Assembly.GetEntryAssembly().Location);
                GC.Collect();
                GC.WaitForPendingFinalizers();
                if (Directory.Exists(dir + "Tmp" + @"\"))
                {
                    Directory.CreateDirectory(dir + "Tmp" + @"\");
                }
                return dir;
            }
        }
    }
}
