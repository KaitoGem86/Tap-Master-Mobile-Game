using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;

namespace Core.ResourceGamePlay
{
    public class _OnlineDataManager : MonoBehaviour
    {
        [SerializeField] private string _sheetId;
        [SerializeField] private int _gID;

        public void ReadGoogleData()
        {
            var data = _CSVOnlineReader.ReadGSheet(_sheetId, _gID);
            //StartCoroutine(LoadDataFromGoogleSheet());
            var json = JsonConvert.SerializeObject(data, Formatting.Indented);
            Debug.Log(json);

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

    [CustomEditor(typeof(_OnlineDataManager))]
    public class _OnlineDataManagerEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            var manager = (_OnlineDataManager)target;
            if (GUILayout.Button("Read Google Data"))
            {
                manager.ReadGoogleData();
            }
        }
    }
}