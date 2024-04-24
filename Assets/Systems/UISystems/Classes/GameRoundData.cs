using System;
using System.Collections.Generic;

namespace MemDub
{
    [Serializable]
    public struct GameRoundData
    {
        public int BoardSizeX;
        public int BoardSizeY;
        public List<TileData> TileInformation;
        public int Score;
    }
}