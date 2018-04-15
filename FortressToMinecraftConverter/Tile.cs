using RemoteFortressReader;
using System.Windows.Media;

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
                if (MaterialDefinition != null && MaterialDefinition.state_color != null)
                    color = Color.FromRgb(
                        (byte)(MaterialDefinition.state_color.red),
                        (byte)(MaterialDefinition.state_color.green),
                        (byte)(MaterialDefinition.state_color.blue)
                        );

                switch (TileType.shape)
                {
                    case TiletypeShape.NO_SHAPE:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.EMPTY:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.FLOOR:
                        color = MultiplyColor(color, 1f);
                        break;
                    case TiletypeShape.BOULDER:
                        color = MultiplyColor(color, 0.9f);
                        break;
                    case TiletypeShape.PEBBLES:
                        color = MultiplyColor(color, 0.95f);
                        break;
                    case TiletypeShape.WALL:
                        color = MultiplyColor(color, 0.25f);
                        break;
                    case TiletypeShape.FORTIFICATION:
                        color = MultiplyColor(color, 0.3f);
                        break;
                    case TiletypeShape.STAIR_UP:
                        break;
                    case TiletypeShape.STAIR_DOWN:
                        break;
                    case TiletypeShape.STAIR_UPDOWN:
                        break;
                    case TiletypeShape.RAMP:
                        color = MultiplyColor(color, 0.5f);
                        break;
                    case TiletypeShape.RAMP_TOP:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.BROOK_BED:
                        color = MultiplyColor(color, 0.3f);
                        break;
                    case TiletypeShape.BROOK_TOP:
                        color = Color.FromArgb(0, 0, 0, 0);
                        break;
                    case TiletypeShape.TREE_SHAPE:
                        color = MultiplyColor(color, 0.5f);
                        break;
                    case TiletypeShape.SAPLING:
                        color = MultiplyColor(color, 0.75f);
                        break;
                    case TiletypeShape.SHRUB:
                        break;
                    case TiletypeShape.ENDLESS_PIT:
                        break;
                    case TiletypeShape.BRANCH:
                        color = MultiplyColor(color, 0.5f);
                        break;
                    case TiletypeShape.TRUNK_BRANCH:
                        color = MultiplyColor(color, 0.25f);
                        break;
                    case TiletypeShape.TWIG:
                        color = MultiplyColor(color, 1f);
                        break;
                    default:
                        break;
                }

                float waterLevel = Water / 7.0f;
                color = (color * (1.0f - waterLevel)) + (Color.FromRgb(0, 0, 255) * waterLevel);
                float magmaLevel = Magma / 7.0f;
                color = (color * (1.0f - magmaLevel)) + (Color.FromRgb(255, 69, 0) * magmaLevel);

                return color;
            }
        }

        public MatPair MaterialIndex { get; internal set; }
        public MaterialDefinition MaterialDefinition { get; internal set; }

        static Color MultiplyColor(Color color, float number)
        {
            var alpha = color.A;
            color *= number;
            color.A = alpha;
            return color;
        }
    }
}
