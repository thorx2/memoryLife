using System;

namespace MemDub
{
    [Serializable]
    public struct TileData
    {
        public int X;
        public int Y;
        public EShapeColor Color;
        public EShapeType Type;
        public bool IsConsumed;
    }
}