using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace Core.ResourceGamePlay
{
    public class _OnlineDataManager : MonoBehaviour
    {
        private static string _sheetId = "1BJlGwjsbHRF8DHj_qzvdgxQmzb_luWkQ-czgtOn-gG4";
        private static int _gID = 1419078634;

        public static List<TempLevelClass> ReadGoogleData()
        {
            var data = _CSVOnlineReader.ReadGSheet(_sheetId, _gID);
            //StartCoroutine(LoadDataFromGoogleSheet());
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.Log(json);
            var tmp = JsonConvert.DeserializeObject<List<TempLevelClass>>(json);
            Debug.Log(tmp.Count);
            return tmp;
            //Debug.Log(json);
        }

        

        private IEnumerator LoadDataFromGoogleSheet()
        {
            string url = $"https://docs.google.com/spreadsheets/d/{_sheetId}/gviz/tq?tqx=out:csv&sheet={_gID}";
            Debug.Log($"Load:\n{url}"); 

            UnityWebRequest www = UnityWebRequest.Get(url);
            yield return www.SendWebRequest();

            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.Log(www.error);
            }
            else
            {
                ProcessCSVData(www.downloadHandler.text);
            }
        }

        private void ProcessCSVData(string csvData)
        {
            string[] rows = csvData.Split('\n');

            for (int i = 1; i < rows.Length; i++)
            {
                string[] cells = rows[i].Split(',');

                string lv = cells[0];
                string size = cells[1];
                string data = cells[2];

                // Process data here
            }
        }
    }
#if UNITY_EDITOR
    [CustomEditor(typeof(_OnlineDataManager))]
    public class _OnlineDataManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = (_OnlineDataManager)target;
            if (GUILayout.Button("Read Google Data"))
            {
                //manager.ReadGoogleData();
            }
        }
    }
#endif
}