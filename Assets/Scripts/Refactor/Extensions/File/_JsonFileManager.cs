using Core.Extensions.File;
using UnityEditor;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;



#if UNITY_EDITOR
using UnityEditor.AddressableAssets;
#endif

namespace Core.File
{
    public static class _JsonFileManager
    {

        /// <summary>
        /// load json from addressables group by addressableName, return object
        /// only public fields will be serialized
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addressablesName"></param>
        /// <returns></returns>
        public static async Task<T> LoadJsonFileFromAddressables<T>(string addressablesName)
        {
            var textAsset = await AddressablesManager.LoadAssetAsync<TextAsset>(addressablesName);
            string json = textAsset.Value.text;
            T item = UnityEngine.JsonUtility.FromJson<T>(json);
            return item;
        }

#if UNITY_EDITOR
        /// <summary>
        /// save object to json file
        /// only public fields will be serialized
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="addressName"></param>
        /// <param name="data"></param>
        public static void SaveLevelJsonFile<T>(string addressName, T data, bool isSavedAtAddressable = true)
        {
            string json = UnityEngine.JsonUtility.ToJson(data);
            Debug.Log(json);
            global::System.IO.File.WriteAllText(_JsonPath.GetJsonLevelDataPath(addressName), json);
            if (isSavedAtAddressable)
                AddressableAssetUtility.AddLevelDataAssetToAddressables(addressName, "TextData");
        }

        public static void SaveLayerJsonFile<T>(string addressName, T data, bool isSavedAtAddressable = true)
        {
            string json = UnityEngine.JsonUtility.ToJson(data);
            Debug.Log(json);
            global::System.IO.File.WriteAllText(_JsonPath.GetJsonLayerDataPath(addressName), json);
            if (isSavedAtAddressable)
                AddressableAssetUtility.AddLevelDataAssetToAddressables(addressName, "TextData");
        }
#endif
    }

#if UNITY_EDITOR
    public class AddressableAssetUtility
    {
        public static async void AddLevelDataAssetToAddressables(string addressName, string groupName)
        {
            var settings = AddressableAssetSettingsDefaultObject.Settings;

            var group = settings.FindGroup(groupName);
            if (group == null)
            {
                group = settings.CreateGroup(groupName, false, false, true, null);
            }

            var entry = settings.CreateOrMoveEntry(AssetDatabase.AssetPathToGUID(_JsonPath.GetJsonLevelDataPath(addressName)), group);
            await UniTask.WaitUntil(() => entry != null);
            entry.address = addressName;
            entry.SetLabel("TextData", true, true, true);
        }
    }
#endif
}