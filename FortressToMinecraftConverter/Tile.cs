using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using RemoteFortressReader;

namespace FortressToMinecraftConverter
{
    class Tile
    {
        public Tiletype TileType { get; internal set; }
        public int Water { get; internal set; }
        public int Magma { get; internal set; }

        public Color ColorValue
        {
            get
            {
                Color color = Color.FromRgb(128, 128, 128);

                switch (TileType.shape)
                {
                    case TiletypeShape.NO_SHAPE:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.EMPTY:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.FLOOR:
                        color *= 0.5f;
                        break;
                    case TiletypeShape.BOULDER:
                        color *= 0.6f;
                        break;
                    case TiletypeShape.PEBBLES:
                        color *= 0.55f;
                        break;
                    case TiletypeShape.WALL:
                        color *= 1f;
                        break;
                    case TiletypeShape.FORTIFICATION:
                        color *= 0.9f;
                        break;
                    case TiletypeShape.STAIR_UP:
                        break;
                    case TiletypeShape.STAIR_DOWN:
                        break;
                    case TiletypeShape.STAIR_UPDOWN:
                        break;
                    case TiletypeShape.RAMP:
                        color *= 0.75f;
                        break;
                    case TiletypeShape.RAMP_TOP:
                        break;
                    case TiletypeShape.BROOK_BED:
                        break;
                    case TiletypeShape.BROOK_TOP:
                        break;
                    case TiletypeShape.TREE_SHAPE:
                        break;
                    case TiletypeShape.SAPLING:
                        break;
                    case TiletypeShape.SHRUB:
                        break;
                    case TiletypeShape.ENDLESS_PIT:
                        break;
                    case TiletypeShape.BRANCH:
                        break;
                    case TiletypeShape.TRUNK_BRANCH:
                        break;
                    case TiletypeShape.TWIG:
                        break;
                    default:
                        break;
                }
                return color;
            }
        }

        public Color Color { get; internal set; }
    }
}
