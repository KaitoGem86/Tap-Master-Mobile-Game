using Core.GamePlay.BlockPool;
using Core.SystemGame;

namespace Core.GamePlay{
    public class _GameManager{
        private static _GameManager _instance;
        public static _GameManager Instance => _instance ?? (_instance = new _GameManager());

        private _LevelSystem _LevelSystem;
        private _GamePlayManager _gamePlayManager;

        public void InitGame(_BlockPool blockPool, LevelData levelData){
            BlockPool = blockPool;
            _LevelSystem = _LevelSystem.Instance;
            _LevelSystem.InitLevelSystem(levelData);
            _gamePlayManager = _GamePlayManager.Instance;
            _gamePlayManager.InitGamePlayManager(blockPool);
        }

        public void StartLevel(){
            Level = _LevelSystem.GetLevelData();
            _gamePlayManager.StartLevel(Level);
            _GameEvent.OnGamePlayReset?.Invoke();
        }

        public void WinGame(){

        }

        public void NextLevel(){
            StartLevel();
        }

        public _BlockPool BlockPool { get; set; }
        public LevelDatasController Level {get; set;}

    }
}