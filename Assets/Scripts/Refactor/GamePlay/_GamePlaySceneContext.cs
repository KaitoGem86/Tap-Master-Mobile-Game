using Core.ResourceGamePlay;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Core.GamePlay
{
    public class _GamePlaySceneContext : MonoBehaviour
    {
        private void Awake()
        {
            Init();
            DontDestroyOnLoad();
        }

        // Start is called before the first frame update
        void Start()
        {
            SetUpCamera();
        }

        private async void DontDestroyOnLoad(){
            var dontDestroyOnLoad = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyDontDestroyOnLoad);
            var gameObject = GameObject.Instantiate(dontDestroyOnLoad);
        }
        
        private void Init(){
            DOTween.Init();
            Application.targetFrameRate = 60;
        }

        private async void SetUpCamera(){
            var cameraRotation = await AddressablesManager.LoadAssetAsync<GameObject>(_KeyPrefabResources.KeyCameraRotation);
            var gameObject = GameObject.Instantiate(cameraRotation);
#if UNITY_EDITOR
            gameObject.name = cameraRotation.Value.name;
#endif     
        }
    }
}