using Core.Data;
using Core.GamePlay.BlockPool;

namespace Core.SystemGame{
    public class _LevelSystem{
        
        private LevelData _levelData;

        private static _LevelSystem _instance;
        public static _LevelSystem Instance => _instance ?? (_instance = new _LevelSystem());

        public void InitLevelSystem(LevelData levelData){
            _levelData = levelData;
        }

        public void InitBlockPool(){
            BlockPool?.InitPool(GetLevelData());
        }

        private LevelDatasController GetLevelData(){
            return _levelData.datasControllers[_PlayerData.UserData.HighestLevel];
        }

        public _BlockPool BlockPool { get; set; }
    }
}