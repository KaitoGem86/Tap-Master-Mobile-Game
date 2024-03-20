#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace Core.Extensions
{
    public class _ConvertFromSOToJSon : MonoBehaviour
    {
        [SerializeField] private LevelDatas _scriptableObject;
        private static string path = "Assets/Data/JSonData/TestJson.txt";

        public void ConvertFromSOToJSon()
        {
            //var so = _scriptableObject;
            // foreach (var data in so.datasControllers)
            // {
            //     Debug.Log(FormatJson(JsonUtility.ToJson(data)));
            // }
            //string testJson = (JsonUtility.ToJson(_scriptableObject));
            //System.IO.File.WriteAllText(path, testJson);
            //return JsonUtility.ToJson(so);
            System.IO.File.WriteAllText(path, "");
            foreach (var data in _scriptableObject.datasControllers)
            {
                System.IO.File.AppendAllText(path, FormatJson(JsonUtility.ToJson(data)));
                System.IO.File.AppendAllText(path, "\n" + "-----------------------------------" + "\n");
            }
        }

        public static string FormatJson(string json)
        {
            var str = "";
            var indentLevel = 0;
            var quoteCount = 0;
            var len = json.Length;
            for (var i = 0; i < len; i++)
            {
                var ch = json[i];
                switch (ch)
                {
                    case '{':
                    case '[':
                        str += ch + "\n" + new string(' ', ++indentLevel * 2);
                        break;
                    case '}':
                    case ']':
                        str += "\n" + new string(' ', --indentLevel * 2) + ch;
                        break;
                    case ',':
                        str += ",\n" + new string(' ', indentLevel * 2);
                        break;
                    case ':':
                        str += quoteCount % 2 == 0 ? ": " : ":";
                        break;
                    default:
                        if (ch == '\"') quoteCount++;
                        str += ch;
                        break;
                }
            }
            return str;
        }

        public static string UnFormatJson(string json)
        {
            return json.Replace("\n", "").Replace(" ", "");
        }
    }

    [CustomEditor(typeof(_ConvertFromSOToJSon))]
    public class _ConvertFromSOToJSonEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _ConvertFromSOToJSon controller = (_ConvertFromSOToJSon)target;
            if (GUILayout.Button("Convert to JSon"))
            {
                controller.ConvertFromSOToJSon();
                //                Debug.Log(json);
            }
        }
    }
}
#endif