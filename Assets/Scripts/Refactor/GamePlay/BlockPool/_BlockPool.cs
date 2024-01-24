using System.Collections.Generic;
using Core.GamePlay.Block;
using Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay.BlockPool{
    public class _BlockPool{
        
        private bool _isLogicInit;
        private bool[][][] _blockLogicPool;
        private List<_BlockController> _blockObjectPool;

        public _BlockPool(){
            InitLogicPool(30, 30, 30);
        }

        public async void InitPool(LevelDatasController levelData){
            _blockObjectPool ??= new List<_BlockController>();
            var blockContainer = new GameObject("BlockContainer");
            var gameObject = await AddressablesManager.LoadAssetAsync<GameObject>("Block");
            ObjectPooling._ObjectPooling.Instance.CreatePool(ObjectPooling._TypeGameObjectEnum.Block, gameObject, 100);            
            int minX = 0;
            int minY = 0;
            int minZ = 0;
            for(int i = 0; i < levelData.states.Count; i++){
                var block = ObjectPooling._ObjectPooling.Instance.SpawnFromPool(ObjectPooling._TypeGameObjectEnum.Block, Vector3.zero, Quaternion.identity);
                block.name = "Block" + i;
                block.GetComponent<_BlockController>().InitBlock();
                _blockObjectPool.Add(block.GetComponent<_BlockController>());
                block.transform.SetPositionAndRotation(levelData.states[i].pos, Quaternion.Euler(levelData.states[i].rotation));
                Vector3Int logicPos = _ConvertPositionToLogic.LogicPos(block.transform.position);
                minX = Mathf.Min(minX, logicPos.x);
                minY = Mathf.Min(minY, logicPos.y);
                minZ = Mathf.Min(minZ, logicPos.z);
            }

            Debug.Log(minX + " " + minY + " " + minZ);

            for(int i = 0; i < levelData.states.Count; i++){
                Vector3Int logicPos = _ConvertPositionToLogic.LogicPos(_blockObjectPool[i].transform.position);
                SetBlockPool(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ, true);
                _blockObjectPool[i].LogicPos = new Vector3Int(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ);
            }
        }

        private void InitLogicPool(int sizeX, int sizeY, int sizeZ){
            if (_isLogicInit) return;
            _blockLogicPool = new bool[sizeX][][];
            for(int i = 0; i < sizeX; i++){
                _blockLogicPool[i] = new bool[sizeY][];
                for(int j = 0; j < sizeY; j++){
                    _blockLogicPool[i][j] = new bool[sizeZ];
                }
            }
            _isLogicInit = true;
        }

        public void SetBlockPool(int x, int y, int z, bool value){
            _blockLogicPool[x][y][z] = value;
        }
    }
}