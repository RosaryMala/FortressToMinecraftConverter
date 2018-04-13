using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RemoteFortressReader;

namespace FortressToMinecraftConverter
{
    class Tile
    {
        public Tiletype TileType { get; internal set; }
        public int Water { get; internal set; }
        public int Magma { get; internal set; }
    }
}
