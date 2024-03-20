using Core.ResourceGamePlay;
using UnityEngine;
using System.IO;

namespace Core.Data
{
    public class _PlayerData
    {
#if UNITY_EDITOR
        private static string path = "Assets/Data/JSonData/UserData.txt";
#else
     private static string path = Path.Combine(Application.persistentDataPath, "UserData.txt");
#endif

        private static _UserData _userData;
        public static void StartGame(){
            InitData();
            LoadUserData();
        }

        private static void InitData()
        {
            //UserData
            Debug.Log("InitData");
            if (!PlayerPrefs.HasKey(_Const.KEY_USER_DATA))
            {
                UserData = new _UserData();
                UserData.InitUserData();
                SaveUserData();
                if (!System.IO.File.Exists(path))
                {
                    // Create a file to write to.
                    System.IO.File.Create(path);
                }
            }
            else
            {
                LoadUserData();
            }
        }

        private static void LoadUserData()
        {
            var saveData = PlayerPrefs.GetString(_Const.KEY_USER_DATA);
            var data = JsonUtility.FromJson<_UserData>(saveData);
            UserData = data;
        }

        public static void SaveUserData()
        {
            string saveData = JsonUtility.ToJson(UserData);
            Debug.Log("SaveUserData: " + saveData);
            PlayerPrefs.SetString(_Const.KEY_USER_DATA, saveData);
            {
                System.IO.File.WriteAllText(path, saveData);
            }
        }

        public static _UserData UserData{
            get {
                if (_userData == null)
                {
                    LoadUserData();
                }
                return _userData;
            }
            set => _userData = value;
        }
    }
}