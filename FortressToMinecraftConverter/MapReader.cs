using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFHack;

namespace FortressToMinecraftConverter
{
    class MapReader
    {
        public bool ConnectToDF()
        {
            ColorConsoleStream stream = new ColorConsoleStream();
            RemoteClient client = new RemoteClient(stream);
            if(!client.Connect())
            {
                return false;
            }



            return true;
            
        }
    }
}
