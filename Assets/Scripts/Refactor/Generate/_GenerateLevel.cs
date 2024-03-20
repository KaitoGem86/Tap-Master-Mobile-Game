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

            var t2 = _blockMesh.triangles;
            for(int i = 0; i < t2.Length; i += 3){
                // Debug.Log(v);

                var pos1 = t[t2[i]];
                var pos2 = t[t2[i + 1]];
                var pos3 = t[t2[i + 2]];
                Debug.DrawRay(pos2, pos1 - pos2, Color.blue, 100f);
                Debug.DrawRay(pos3, pos2 - pos3, Color.blue, 100f);
                Debug.DrawRay(pos1, pos3 - pos1, Color.blue, 100f);
            }
        }
    }
}