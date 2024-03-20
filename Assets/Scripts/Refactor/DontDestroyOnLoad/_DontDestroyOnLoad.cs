namespace Core.DontDestroyOnLoad{
    public class _DontDestroyOnLoad : UnityEngine.MonoBehaviour{
        public void Awake(){
            UnityEngine.Object.DontDestroyOnLoad(this);
        }
    }
}