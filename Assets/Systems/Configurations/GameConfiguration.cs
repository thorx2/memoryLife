using UnityEngine;

namespace MemDub
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "MemoryDub/Config")]
    public class GameConfiguration : ScriptableObject
    {
        [Header("Gameplay configuration")]
        [Tooltip("The multiple of Row and Column count should be an even number or game will not start")]
        public int RowCount;
        [Tooltip("The multiple of Row and Column count should be an even number or game will not start")]
        public int ColCount;

        [Space(5)]

        [Header("Tile configuration")]
        public Color[] GameTileColors;
        public Sprite[] Shapes;
        public int ScorePerMatch;
    }
}
