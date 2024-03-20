using Core.Extensions;
using UnityEditor;
using UnityEngine;

namespace Core.Data{
    public class _ReadLevelData : MonoBehaviour{
        [SerializeField] private LevelDatas _levelSO;
        [SerializeField] private TextAsset _levelJson;

        public void ReadData(){
            Debug.Log(_levelJson.text);
            _levelSO.datasControllers.Clear();
            //LevelDatas LevelDatas = JsonUtility.FromJson<LevelDatas>((_levelJson.text));
            //_levelSO = LevelDatas;
            string[] res = _levelJson.text.Split("\n" + "-----------------------------------" + "\n", System.StringSplitOptions.RemoveEmptyEntries);
            foreach (var data in res){
                Debug.Log(data);
                LevelData levelData = JsonUtility.FromJson<LevelData>(data);
                _levelSO.datasControllers.Add(levelData);
            } 
            _levelSO.numberOfLevels = _levelSO.datasControllers.Count;

            #if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(_levelSO);
            AssetDatabase.SaveAssets();
            #endif
        }
    }

    #if UNITY_EDITOR
    [CustomEditor(typeof(_ReadLevelData))]
    public class _ReadLevelDataEditor : Editor{
        public override void OnInspectorGUI(){
            DrawDefaultInspector();
            var script = (Core.Data._ReadLevelData)target;
            if(GUILayout.Button("Read Data")){
                script.ReadData();
            }
        }
    }
    #endif
}