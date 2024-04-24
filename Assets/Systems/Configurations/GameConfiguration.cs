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

        //Could have used any third party dictionary to value mapping script, skipping for
        //a more linear built in solutions.
        //Index of enum corresponds to index of color.

        [Header("Tile configuration")]
        public EShapeColor[] ColorMappingEnum;
        public Color[] GameTileColors;

        //Same treatment for shape
        public EShapeType[] ShapeMappingEnum;
        public Sprite[] Shapes;
        public int ScorePerMatch;
    }
}
