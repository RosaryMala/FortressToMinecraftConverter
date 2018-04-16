using RemoteFortressReader;
using System.Windows.Media;
using Substrate;

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
        public bool BottomSolid
        {
            get
            {
                switch (TileType.shape)
                {
                    case TiletypeShape.FLOOR:
                    case TiletypeShape.BOULDER:
                    case TiletypeShape.PEBBLES:
                    case TiletypeShape.WALL:
                    case TiletypeShape.FORTIFICATION:
                    case TiletypeShape.STAIR_UP:
                    case TiletypeShape.RAMP:
                    case TiletypeShape.BROOK_BED:
                    case TiletypeShape.TREE_SHAPE:
                    case TiletypeShape.SAPLING:
                    case TiletypeShape.SHRUB:
                        return true;
                    default:
                        return false;
                }
            }
        }
        public bool TopSolid
        {
            get
            {
                switch (TileType.shape)
                {
                    case TiletypeShape.WALL:
                        return true;
                    default:
                        return false;
                }
            }
        }
        static Color MultiplyColor(Color color, float number)
        {
            var alpha = color.A;
            color *= number;
            color.A = alpha;
            return color;
        }

        public AlphaBlock GetSolidBlock(int x, int y, int z)
        {
            switch (TileType.material)
            {
                case TiletypeMaterial.NO_MATERIAL:
                    break;
                case TiletypeMaterial.AIR:
                    break;
                case TiletypeMaterial.SOIL:
                    return new AlphaBlock(3);
                case TiletypeMaterial.STONE:
                    break;
                case TiletypeMaterial.FEATURE:
                    break;
                case TiletypeMaterial.LAVA_STONE:
                    break;
                case TiletypeMaterial.MINERAL:
                    break;
                case TiletypeMaterial.FROZEN_LIQUID:
                    break;
                case TiletypeMaterial.CONSTRUCTION:
                    break;
                case TiletypeMaterial.GRASS_LIGHT:
                case TiletypeMaterial.GRASS_DARK:
                    return new AlphaBlock(2);
                case TiletypeMaterial.GRASS_DRY:
                    break;
                case TiletypeMaterial.GRASS_DEAD:
                    break;
                case TiletypeMaterial.PLANT:
                    break;
                case TiletypeMaterial.HFS:
                    break;
                case TiletypeMaterial.CAMPFIRE:
                    break;
                case TiletypeMaterial.FIRE:
                    break;
                case TiletypeMaterial.ASHES:
                    break;
                case TiletypeMaterial.MAGMA:
                    break;
                case TiletypeMaterial.DRIFTWOOD:
                    break;
                case TiletypeMaterial.POOL:
                    break;
                case TiletypeMaterial.BROOK:
                    break;
                case TiletypeMaterial.RIVER:
                    break;
                case TiletypeMaterial.ROOT:
                    break;
                case TiletypeMaterial.TREE_MATERIAL:
                    return new AlphaBlock(17);
                case TiletypeMaterial.MUSHROOM:
                    return new AlphaBlock(100);
                case TiletypeMaterial.UNDERWORLD_GATE:
                    break;
                default:
                    break;
            }
            return new AlphaBlock(1);
        }

        public AlphaBlock GetAirBlock(int x, int y, int z)
        {
            if (Water > 0 && (Water / 7.0f * MapReader.tileHeight >= y % MapReader.tileHeight))
                return new AlphaBlock(9);
            if (Magma > 0 && (Magma / 7.0f * MapReader.tileHeight >= y % MapReader.tileHeight))
                return new AlphaBlock(11);
            return new AlphaBlock(0);
        }
    }
}
