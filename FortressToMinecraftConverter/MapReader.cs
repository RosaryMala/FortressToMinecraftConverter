using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DFHack;
using dfproto;
using RemoteFortressReader;

namespace FortressToMinecraftConverter
{
    class MapReader : INotifyPropertyChanged
    {
        const int BlockSize = 16;
        
        bool isConnected = false;
        private RemoteClient client;
        private RemoteFunction<EmptyMessage, MapInfo> mapInfoCall;
        private RemoteFunction<EmptyMessage, TiletypeList> tileTypeListCall;
        private RemoteFunction<BlockRequest, BlockList> mapReadCall;
        private RemoteFunction<EmptyMessage> mapResetCall;
        public Tile[][][] Tiles { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        public bool ConnectToDF()
        {
            ColorConsoleStream stream = new ColorConsoleStream();
            client = new RemoteClient(stream);
            if(!client.Connect())
            {
                return false;
            }

            mapInfoCall = RemoteFunction<EmptyMessage, MapInfo>.CreateAndBind(client, "GetMapInfo", "RemoteFortressReader");
            tileTypeListCall = RemoteFunction<EmptyMessage, TiletypeList>.CreateAndBind(client, "GetTiletypeList", "RemoteFortressReader");
            mapReadCall = RemoteFunction<BlockRequest, BlockList>.CreateAndBind(client, "GetBlockList", "RemoteFortressReader");
            mapResetCall = RemoteFunction<EmptyMessage>.CreateAndBind(client, "ResetMapHashes", "RemoteFortressReader");
            return true;
            
        }

        public void ReadMap()
        {
            if (!isConnected)
            {
                if (!ConnectToDF())
                {
                    isConnected = false;
                    return;
                }
                isConnected = true;
            }

            mapResetCall.Execute();

            var info = mapInfoCall.Execute();

            var tileTypes = tileTypeListCall.Execute();

            BlockRequest request = new BlockRequest();
            request.min_x = 0;
            request.min_y = 0;
            request.min_z = 0;
            request.max_x = info.block_size_x;
            request.max_y = info.block_size_y;
            request.max_z = info.block_size_z;
            request.blocks_needed = info.block_size_x * info.block_size_y * info.block_size_z;
            var blocks = mapReadCall.Execute(request);

            Tiles = new Tile[info.block_size_z][][];

            foreach (var block in blocks.map_blocks)
            {
                for(int local_y = 0; local_y < BlockSize; local_y++)
                    for(int local_x = 0; local_x < BlockSize; local_x++)
                    {
                        int index = local_y * BlockSize + local_x;
                        int x = block.map_x + local_x;
                        int y = block.map_y + local_y;
                        int z = block.map_z;
                        var tile = new Tile();
                        if (Tiles[z] == null)
                            Tiles[z] = new Tile[info.block_size_y * BlockSize][];
                        if(Tiles[z][y] == null)
                            Tiles[z][y] = new Tile[info.block_size_x * BlockSize];
                        Tiles[z][y][x] = tile;
                        if (block.tiles.Count > 0)
                            tile.TileType = tileTypes.tiletype_list[block.tiles[index]];
                        if (block.water.Count > 0)
                            tile.Water = block.water[index];
                        if (block.magma.Count > 0)
                            tile.Magma = block.magma[index];
                    }
            }
            PropertyChanged(this, new PropertyChangedEventArgs("Tiles"));
            Console.WriteLine("Done!");
        }
    }
}
