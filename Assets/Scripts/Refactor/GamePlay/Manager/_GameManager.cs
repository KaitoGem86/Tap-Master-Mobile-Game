using Core.GamePlay.BlockPool;

namespace Core.GamePlay{
    public class _GameManager{
        private static _GameManager _instance;
        public static _GameManager Instance => _instance ?? (_instance = new _GameManager());

        public _BlockPool BlockPool { get; set; }
    }
}