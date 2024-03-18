using Core.Data;
using Core.GamePlay.BlockPool;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Core.GamePlay
{
    public class _GamePlayManager
    {
        private static _GamePlayManager _instance;
        public static _GamePlayManager Instance => _instance ?? (_instance = new _GamePlayManager());

        private _BlockPool _blockPool;

        private int _totalBlocks;


        public void InitGamePlayManager(_BlockPool pool)
        {
            _totalBlocks = 0;
            _blockPool = pool;
        }
        public void StartLevel(LevelDatasController level)
        {
            _blockPool?.InitPool(level);
            _totalBlocks = level.numOfBlocks;
        }

        private async void WinGame()
        {
            Debug.Log("WinGame");
            _PlayerData.UserData.UpdateWinGameUserDataValue();
            await UniTask.Delay(1500);
            _blockPool?.DeSpawnBlock();
            _GameManager.Instance.NextLevel();
        }

        public void OnBlockSelected(bool isBlockCanMove = true)
        {
            if (isBlockCanMove)
            {
                _totalBlocks -= 1;
                if (_totalBlocks == 0)
                {
                    WinGame();
                }
            }
        }
    }
}