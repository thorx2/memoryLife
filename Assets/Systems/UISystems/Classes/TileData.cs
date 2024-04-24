using System;

namespace MemDub
{
    [Serializable]
    public struct TileData
    {
        public int X;
        public int Y;
        public int Color;
        public int Type;
        public bool IsConsumed;
    }
}