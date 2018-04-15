using DFHack;
using dfproto;
using RemoteFortressReader;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Substrate;

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
        private RemoteFunction<EmptyMessage, MaterialList> materialListCall;

        public ZLevel[] Tiles { get; set; }

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
            materialListCall = RemoteFunction<EmptyMessage, MaterialList>.CreateAndBind(client, "GetMaterialList", "RemoteFortressReader");
            return true;
            
        }

        public void ReadMap(object sender, DoWorkEventArgs e)
        {
            BackgroundWorker worker = sender as BackgroundWorker;

            worker.ReportProgress(0, "Connecting to DF");

            if (!isConnected)
            {
                if (!ConnectToDF())
                {
                    isConnected = false;
                    worker.ReportProgress(0, "Could not connect to DF.");
                    return;
                }
                isConnected = true;
            }
            worker.ReportProgress(0, "Connected!");

            mapResetCall.Execute();

            var info = mapInfoCall.Execute();

            var tileTypes = tileTypeListCall.Execute();

            var materials = materialListCall.Execute();

            Dictionary<MatPairStruct, MaterialDefinition> matLookup = new Dictionary<MatPairStruct, MaterialDefinition>();


            foreach (var item in materials.material_list)
            {
                matLookup[item.mat_pair] = item;
            }

            Tiles = new ZLevel[info.block_size_z];
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i] = new ZLevel(info.block_size_x * BlockSize, info.block_size_y * BlockSize, i);
                Tiles[i].PropertyChanged += LevelToggleEvent;
            }

            int totalColumns = info.block_size_x * info.block_size_y;
            int done = 0;
            for (int column_y = 0; column_y < info.block_size_y; column_y++)
                for (int column_x = 0; column_x < info.block_size_x; column_x++)
                {
                    BlockRequest request = new BlockRequest
                    {
                        min_x = column_x,
                        min_y = column_y,
                        min_z = 0,
                        max_x = column_x + 1,
                        max_y = column_y + 1,
                        max_z = info.block_size_z,
                        blocks_needed = info.block_size_z
                    };
                    var blocks = mapReadCall.Execute(request);
                    foreach (var block in blocks.map_blocks)
                    {
                        for (int local_y = 0; local_y < BlockSize; local_y++)
                            for (int local_x = 0; local_x < BlockSize; local_x++)
                            {
                                int index = local_y * BlockSize + local_x;
                                int x = block.map_x + local_x;
                                int y = block.map_y + local_y;
                                int z = block.map_z;
                                var tile = new Tile();
                                Tiles[z][x, y] = tile;
                                if (block.tiles.Count > 0)
                                    tile.TileType = tileTypes.tiletype_list[block.tiles[index]];
                                if (block.water.Count > 0)
                                    tile.Water = block.water[index];
                                if (block.magma.Count > 0)
                                    tile.Magma = block.magma[index];
                                if (block.materials.Count > 0)
                                {
                                    tile.MaterialIndex = block.materials[index];
                                    if (matLookup.ContainsKey(tile.MaterialIndex))
                                    {
                                        tile.MaterialDefinition = matLookup[tile.MaterialIndex];
                                    }
                                }

                            }
                    }
                    done++;
                    worker.ReportProgress(done * 100 / totalColumns, "Reading map from DF.");
                }

            for(int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i].EnableIfImportant();
                worker.ReportProgress(i * 100 / Tiles.Length, "Finding useful levels.");
            }
            for (int i = 0; i < Tiles.Length; i++)
            {
                Tiles[i].RegenerateBitmap();
                worker.ReportProgress(i * 100 / Tiles.Length, "Building Preview Images");
            }
            worker.ReportProgress(100, "Done! Ready to export.");

            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Tiles"));
            e.Result = this;
        }

        private void LevelToggleEvent(object sender, PropertyChangedEventArgs e)
        {
            NotifyPropertyChanged("NumSelectedLevels");
            NotifyPropertyChanged("SelectedLevelsString");
        }

        // This method is called by the Set accessor of each property.
        // The CallerMemberName attribute that is applied to the optional propertyName
        // parameter causes the property name of the caller to be substituted as an argument.
        private void NotifyPropertyChanged([CallerMemberName] String propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public int NumSelectedLevels
        {
            get
            {
                int num = 0;
                if (Tiles != null)
                    foreach (var level in Tiles)
                    {
                        if (level.Enabled)
                            num++;
                    }
                return num;
            }
        }

        public string SelectedLevelsString
        {
            get
            {
                return string.Format("{0}/{1}", NumSelectedLevels, 256 / 3);
            }
        }

        public void ExportMap(string path)
        {
            AnvilWorld world = AnvilWorld.Create(path);
        }
    }
}
