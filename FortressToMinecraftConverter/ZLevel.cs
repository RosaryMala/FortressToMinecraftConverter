using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media.Imaging;
using System.Windows.Media;

namespace FortressToMinecraftConverter
{
    class ZLevel
    {
        Tile[,] tiles;
        private bool dirty = true;

        public ZLevel(int width, int height)
        {
            tiles = new Tile[width, height];
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

        public bool Enabled { get; set; }

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

        private void RegenerateBitmap()
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
            dirty = false;
        }
    }
}
