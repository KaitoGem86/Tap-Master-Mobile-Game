using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay.BlockPool{
    public class _BlockPool{
        public _BlockPool(){
        }

        public async void InitPool(LevelDatasController levelData){
            var blockContainer = new GameObject("BlockContainer");
            var gameObject = await AddressablesManager.LoadAssetAsync<GameObject>("Block");
            ObjectPooling._ObjectPooling.Instance.CreatePool(ObjectPooling._TypeGameObjectEnum.Block, gameObject, 100);            
            for(int i = 0; i < levelData.states.Count; i++){
                var block = ObjectPooling._ObjectPooling.Instance.SpawnFromPool(ObjectPooling._TypeGameObjectEnum.Block, Vector3.zero, Quaternion.identity);
                block.name = "Block" + i;
                block.transform.SetPositionAndRotation(levelData.states[i].pos, Quaternion.Euler(levelData.states[i].rotation));
            }
        }
    }
}