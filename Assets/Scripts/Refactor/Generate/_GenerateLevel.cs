using UnityEngine;

namespace Generate{
    public class _GenerateLevel : MonoBehaviour{
        [SerializeField] private Mesh _blockMesh;

        private void Start(){
            var t = _blockMesh.vertices;
            foreach (var v in t){
                Debug.Log(v);
                Debug.DrawRay(v, Vector3.up*0.2f, Color.red, 100f);
            }
        }
    }
}