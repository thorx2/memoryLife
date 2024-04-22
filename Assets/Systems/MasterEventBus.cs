using System;

namespace MemDub
{
    public class MasterEventBus
    {
        #region Events
        public Action<EGameState> OnGameStateChanged;
        public Action<GridTile> OnTileSelected;
        public Action<bool, int> OnPlayerActionDone;
        public Action<GameConfiguration, int> StartGameWithConfiguration;
        #endregion

        #region Singleton
        public static MasterEventBus GetMasterEventBus
        {
            get
            {
                if (instance != null)
                {
                    return instance;
                }
                instance = new();
                return instance;
            }
        }

        private static MasterEventBus instance;
        private MasterEventBus() { }
        #endregion
    }
}