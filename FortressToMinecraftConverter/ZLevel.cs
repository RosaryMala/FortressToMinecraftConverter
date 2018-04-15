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
            int dfY = Width - 1 - (z / 3);
            var tile = this[dfX, dfY];
            if (tile == null)
                return new AlphaBlock(0);
            if (y == 0 && tile.BottomSolid)
                return new AlphaBlock(1);
            if (y > 0 && tile.TopSolid)
                return new AlphaBlock(1);
            return new AlphaBlock(0);
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
