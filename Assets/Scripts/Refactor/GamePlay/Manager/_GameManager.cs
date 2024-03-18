using Core.GamePlay.BlockPool;
using Core.SystemGame;

namespace Core.GamePlay{
    public class _GameManager{
        private static _GameManager _instance;
        public static _GameManager Instance => _instance ?? (_instance = new _GameManager());

        public void StartLevel(){
            _LevelSystem.Instance.InitBlockPool();
        }

        public void WinGame(){

        }

        public _BlockPool BlockPool { get; set; }


    }
}