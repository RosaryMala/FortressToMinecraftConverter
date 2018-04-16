using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using Substrate;

namespace FortressToMinecraftConverter
{
    class ZLevel : INotifyPropertyChanged
    {
        Tile[,] tiles;

        public int Level { get; }

        private bool dirty = true;

        public ZLevel(int width, int height, int level)
        {
            tiles = new Tile[width, height];
            Level = level;
        }

        public Tile this[int x, int y]
        {
            get
            {
                return tiles[x, y];
            }
            set
            {
                tiles[x, y] = value;
                dirty = true;
            }
        }

        /// <summary>
        /// Gets a minecraft block from this level, in minecraft coordinates.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="z"></param>
        /// <returns></returns>
        public AlphaBlock GetBlock(int x, int y, int z)
        {
            int dfX = x / 3;
            int dfY = z / 3;
            int localX = x % MapReader.tileWidth;
            int localY = y % MapReader.tileHeight;
            int localZ = z % MapReader.tileWidth;
            var tile = tiles[dfX, dfY];

            Tile xPlus, xMinus, zPlus, zMinus;

            if (dfX > 0)
                xMinus = tiles[dfX - 1, dfY];
            else
                xMinus = tile;

            if (dfX < Width - 1)
                xPlus = tiles[dfX + 1, dfY];
            else
                xPlus = tile;

            if (dfY > 0)
                zMinus = tiles[dfX, dfY - 1];
            else
                zMinus = tile;

            if (dfY < Height - 1)
                zPlus = tiles[dfX, dfY + 1];
            else
                zPlus = tile;

            if (tile == null)
                return new AlphaBlock(0);

            switch (tile.TileType.shape)
            {
                case RemoteFortressReader.TiletypeShape.NO_SHAPE:
                    break;
                case RemoteFortressReader.TiletypeShape.EMPTY:
                    break;
                case RemoteFortressReader.TiletypeShape.FLOOR:
                    break;
                case RemoteFortressReader.TiletypeShape.BOULDER:
                    break;
                case RemoteFortressReader.TiletypeShape.PEBBLES:
                    break;
                case RemoteFortressReader.TiletypeShape.WALL:
                    break;
                case RemoteFortressReader.TiletypeShape.FORTIFICATION:
                    if (localY < 2)
                        return tile.GetSolidBlock(x, y, z);
                    if (localX == 1)
                        return tile.GetAirBlock(x, y, z);
                    if (localZ == 1)
                        return tile.GetAirBlock(x, y, z);
                    return tile.GetSolidBlock(x, y, z);
                case RemoteFortressReader.TiletypeShape.STAIR_UP:
                    break;
                case RemoteFortressReader.TiletypeShape.STAIR_DOWN:
                    break;
                case RemoteFortressReader.TiletypeShape.STAIR_UPDOWN:
                    break;
                case RemoteFortressReader.TiletypeShape.RAMP:
                    if (localY == 0)
                        return tile.GetSolidBlock(x, y, z);
                    switch (localX)
                    {
                        case 0:
                            switch (localZ)
                            {
                                case 0:
                                    if (xMinus.TopSolid || zMinus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    else
                                        return tile.GetAirBlock(x, y, z);
                                case 1:
                                    if(xMinus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    if((zMinus.TopSolid || zPlus.TopSolid) && localY == 1)
                                        return tile.GetSolidBlock(x, y, z);
                                    return tile.GetAirBlock(x, y, z);
                                case 2:
                                    if (xMinus.TopSolid || zPlus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    else
                                        return tile.GetAirBlock(x, y, z);
                            }
                            break;
                        case 1:
                            switch (localZ)
                            {
                                case 0:
                                    if (zMinus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    if ((xMinus.TopSolid || xPlus.TopSolid) && localY == 1)
                                        return tile.GetSolidBlock(x, y, z);
                                    return tile.GetAirBlock(x, y, z);
                                case 1:
                                    if (localY == 1)
                                        return tile.GetSolidBlock(x, y, z);
                                    return tile.GetAirBlock(x, y, z);
                                case 2:
                                    if (zPlus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    if ((xMinus.TopSolid || xPlus.TopSolid) && localY == 1)
                                        return tile.GetSolidBlock(x, y, z);
                                    return tile.GetAirBlock(x, y, z);
                            }
                            break;
                        case 2:
                            switch (localZ)
                            {
                                case 0:
                                    if (xPlus.TopSolid || zMinus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    else
                                        return tile.GetAirBlock(x, y, z);
                                case 1:
                                    if (xPlus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    if ((zMinus.TopSolid || zPlus.TopSolid) && localY == 1)
                                        return tile.GetSolidBlock(x, y, z);
                                    return tile.GetAirBlock(x, y, z);
                                case 2:
                                    if (xPlus.TopSolid || zPlus.TopSolid)
                                        return tile.GetSolidBlock(x, y, z);
                                    else
                                        return tile.GetAirBlock(x, y, z);
                            }
                            break;
                    }
                    break;
                case RemoteFortressReader.TiletypeShape.RAMP_TOP:
                    break;
                case RemoteFortressReader.TiletypeShape.BROOK_BED:
                    break;
                case RemoteFortressReader.TiletypeShape.BROOK_TOP:
                    break;
                case RemoteFortressReader.TiletypeShape.TREE_SHAPE:
                    break;
                case RemoteFortressReader.TiletypeShape.SAPLING:
                    break;
                case RemoteFortressReader.TiletypeShape.SHRUB:
                    break;
                case RemoteFortressReader.TiletypeShape.ENDLESS_PIT:
                    break;
                case RemoteFortressReader.TiletypeShape.BRANCH:
                    break;
                case RemoteFortressReader.TiletypeShape.TRUNK_BRANCH:
                    break;
                case RemoteFortressReader.TiletypeShape.TWIG:
                    break;
                default:
                    break;
            }

            if (localY == 0 && tile.BottomSolid)
                return tile.GetSolidBlock(x, y, z);
            if (localY > 0 && tile.TopSolid)
                return tile.GetSolidBlock(x, y, z);
            return tile.GetAirBlock(x, y, z);
        }

        private bool _enabled;
        public bool Enabled {
            get
            {
                return _enabled;
            }
            set
            {
                if(_enabled != value)
                {
                    _enabled = value;
                    NotifyPropertyChanged();
                }
            }
        }

        BitmapSource bitmapSource;
        const double DPI = 96;

        public BitmapSource BitmapSource
        {
            get
            {
                if (dirty)
                    RegenerateBitmap();
                return bitmapSource;
            }
        }

        public int Width
        {
            get
            {
                return tiles.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return tiles.GetLength(1);
            }
        }

        internal void RegenerateBitmap()
        {
            byte[] pixels = new byte[Width * Height * 4];
            for (int y = 0; y < Height; y++)
            {
                int yIndex = y * Width * 4;
                for (int x = 0; x < Width; x++)
                {
                    int xIndex = x * 4;
                    var color = Color.FromArgb(0, 0, 0, 0);
                    if (tiles[x, y] != null)
                        color = tiles[x, y].ColorValue;
                    pixels[xIndex + yIndex + 0] = color.B;
                    pixels[xIndex + yIndex + 1] = color.G;
                    pixels[xIndex + yIndex + 2] = color.R;
                    pixels[xIndex + yIndex + 3] = color.A;
                }
            }
            bitmapSource = BitmapSource.Create(Width, Height, DPI, DPI, PixelFormats.Bgra32, null, pixels, Width * 4);
            bitmapSource.Freeze();
            dirty = false;
        }

        public void EnableIfImportant()
        {
            foreach (var tile in tiles)
            {
                if (tile == null)
                    continue;
                switch (tile.TileType.shape)
                {
                    case RemoteFortressReader.TiletypeShape.FLOOR:
                    case RemoteFortressReader.TiletypeShape.BOULDER:
                    case RemoteFortressReader.TiletypeShape.PEBBLES:
                    case RemoteFortressReader.TiletypeShape.FORTIFICATION:
                    case RemoteFortressReader.TiletypeShape.STAIR_UP:
                    case RemoteFortressReader.TiletypeShape.STAIR_DOWN:
                    case RemoteFortressReader.TiletypeShape.RAMP:
                    case RemoteFortressReader.TiletypeShape.BROOK_BED:
                    case RemoteFortressReader.TiletypeShape.TREE_SHAPE:
                    case RemoteFortressReader.TiletypeShape.SAPLING:
                    case RemoteFortressReader.TiletypeShape.SHRUB:
                    case RemoteFortressReader.TiletypeShape.ENDLESS_PIT:
                    case RemoteFortressReader.TiletypeShape.BRANCH:
                    case RemoteFortressReader.TiletypeShape.TRUNK_BRANCH:
                    case RemoteFortressReader.TiletypeShape.TWIG:
                        Enabled = true;
                        return;
                    default:
                        break;
                }
            }
        }

        public event PropertyChangedEventHandler PropertyChanged;

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
