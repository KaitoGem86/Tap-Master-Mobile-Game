using Core.GamePlay.BlockPool;

namespace Core.System{
    public class _LevelSystem{
        private static _LevelSystem _instance;
        public static _LevelSystem Instance => _instance ?? (_instance = new _LevelSystem());

        public void InitBlockPool(LevelDatasController levelDatasController){
            BlockPool?.InitPool(levelDatasController);
        }

        public _BlockPool BlockPool { get; set; }
    }
}