using UnityEngine;
namespace MemDub
{
    public class SaveManager
    {
        #region Save functions
        public void SaveBoardState(string jsonState)
        {
            PlayerPrefs.SetString("BoardData", jsonState);
            PlayerPrefs.Save();
        }
        public string GetBoardState()
        {
            return PlayerPrefs.GetString("BoardData", "");
        }

        public void UpdateInGameState(bool isInGame)
        {
            PlayerPrefs.SetInt("InGame", isInGame ? 1 : 0);
            PlayerPrefs.Save();
        }

        public bool GetInGameState()
        {
            return PlayerPrefs.GetInt("InGame", 0) == 1;
        }
        #endregion
        #region Singleton
        private static SaveManager instance;
        public static SaveManager GetInstance
        {
            get
            {
                if (instance == null)
                {
                    instance = new();
                }
                return instance;
            }
        }

        private SaveManager() { }
        #endregion
    }
}