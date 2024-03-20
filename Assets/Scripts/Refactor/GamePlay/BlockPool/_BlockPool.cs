using System.Collections.Generic;
using Core.GamePlay.Block;
using Core.ResourceGamePlay;
using Extensions;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay.BlockPool
{
    public class _BlockPool
    {

        private const int sizeX = 30;
        private const int sizeY = 30;
        private const int sizeZ = 30;

        private bool _isLogicInit;
        private bool[][][] _blockLogicPool;
        private List<_BlockController> _blockObjectPool;

        public _BlockPool()
        {
            InitLogicPool(sizeX, sizeY, sizeZ);
        }

        public async void InitPool(LevelData levelData)
        {
            _blockObjectPool ??= new List<_BlockController>();
            _blockObjectPool.Clear();
            ClearLogicPool();
            var blockContainer = new GameObject("BlockContainer");
            var gameObject = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyBlock);
            var movingMaterial = await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyMovingMaterial);
            var blockedMaterial = await AddressablesManager.LoadAssetAsync<Material>(_KeyMaterialResources.KeyBlockedMaterial);
            ObjectPooling._ObjectPooling.Instance.CreatePool(ObjectPooling._TypeGameObjectEnum.Block, gameObject, 100);
            int minX = 0;
            int minY = 0;
            int minZ = 0;
            for (int i = 0; i < levelData.blockStates.Count; i++)
            {
                var block = ObjectPooling._ObjectPooling.Instance.SpawnFromPool(ObjectPooling._TypeGameObjectEnum.Block, Vector3.zero, Quaternion.identity);
                block.name = "Block" + i;
                block.GetComponent<_BlockController>().InitBlock(movingMaterial, blockedMaterial, levelData.blockStates[i].rotation);
                _blockObjectPool.Add(block.GetComponent<_BlockController>());
                block.transform.SetPositionAndRotation(levelData.blockStates[i].pos, Quaternion.Euler(levelData.blockStates[i].rotation));
                Vector3Int logicPos = _NormalizingVector3.LogicPos(block.transform.position);
                minX = Mathf.Min(minX, logicPos.x);
                minY = Mathf.Min(minY, logicPos.y);
                minZ = Mathf.Min(minZ, logicPos.z);
            }

            for (int i = 0; i < levelData.blockStates.Count; i++)
            {
                Vector3Int logicPos = _NormalizingVector3.LogicPos(_blockObjectPool[i].transform.position);
                SetStateElementBlockInPool(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ, true);
                _blockObjectPool[i].LogicPos = new Vector3Int(logicPos.x - minX, logicPos.y - minY, logicPos.z - minZ);
            }
        }

        private void InitLogicPool(int sizeX, int sizeY, int sizeZ)
        {
            if (_isLogicInit) return;
            _blockLogicPool = new bool[sizeX][][];
            for (int i = 0; i < sizeX; i++)
            {
                _blockLogicPool[i] = new bool[sizeY][];
                for (int j = 0; j < sizeY; j++)
                {
                    _blockLogicPool[i][j] = new bool[sizeZ];
                }
            }
            _isLogicInit = true;
        }

        private void ClearLogicPool(){
            for(int i = 0; i < sizeX; i++){
                for(int j = 0; j < sizeY; j++){
                    for(int k = 0; k < sizeZ; k++){
                        _blockLogicPool[i][j][k] = false;
                    }
                }
            }
        }

        public void SetStateElementBlockInPool(int x, int y, int z, bool value)
        {
            _blockLogicPool[x][y][z] = value;
        }


        public bool CheckCanEscape(_BlockController block)
        {
            Vector3 direction = _NormalizingVector3.ConvertToVector3Int(-block.transform.right);
            Vector3 tempLogicPos = block.LogicPos + direction;
            for (int i = 0; i < sizeX; i++)
            {
                //neu logicPos nam ngoai kich thuoc cua pool
                if (tempLogicPos.x < 0 || tempLogicPos.x >= sizeX) return true;
                if (tempLogicPos.y < 0 || tempLogicPos.y >= sizeY) return true;
                if (tempLogicPos.z < 0 || tempLogicPos.z >= sizeZ) return true;

                //neu tai vi tri logicPos co block
                if (_blockLogicPool[(int)tempLogicPos.x][(int)tempLogicPos.y][(int)tempLogicPos.z])
                {
                    block.ObstacleLogicPos = _NormalizingVector3.IgnoreDecimalPart(tempLogicPos);
                    return false;
                }
                tempLogicPos += direction;
            }
            //neu logicPos nam trong kich thuoc cua pool va khong co block
            return true;
        }

        public void DeSpawnBlock()
        {
            foreach (var block in _blockObjectPool)
            {
                ObjectPooling._ObjectPooling.Instance.ReturnToPool(ObjectPooling._TypeGameObjectEnum.Block, block.gameObject);
            }
        }

        public _BlockController GetBlock(Vector3Int logicPos)
        {
            return _blockObjectPool.Find(block => block.LogicPos.Equals(logicPos));
        }
    }
}