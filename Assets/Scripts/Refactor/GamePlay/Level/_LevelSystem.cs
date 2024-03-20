using Core.Data;
using Core.GamePlay.BlockPool;
using UnityEngine;

namespace Core.SystemGame{
    public class _LevelSystem{
        
        private LevelDatas _levelData;

        private static _LevelSystem _instance;
        public static _LevelSystem Instance => _instance ?? (_instance = new _LevelSystem());

        public void InitLevelSystem(LevelDatas levelData){
            _levelData = levelData;
        }

//         public LevelDatasController InitBlockPool(){
//             var level = GetLevelData();
// //            BlockPool?.InitPool(level);
//             return level;
//         }

        public LevelData GetLevelData(){
            return _levelData.datasControllers[Mathf.Min(_PlayerData.UserData.HighestLevel, _levelData.numberOfLevels - 1)];
        }
    }
}