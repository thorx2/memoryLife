using UnityEngine;

namespace MemDub
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "MemoryDub/Config")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Gameplay configuration")]
        public int EasyTileCount;
        public int MediumTileCount;
        public int HardTileCount;

        [Header("Tile configuration")]
        public Color[] GameTileColors;
        public Sprite[] Shapes;

        public int ScorePerMatch;
    }
}
