using UnityEngine;

namespace Core.Extensions.File
{
    public static class _JsonPath
    {
        private const string _jsonExtension = ".json";

        private static string _jsonLevelDataPath = "Assets/TextData/LevelData/";
        private static string _jsonLayerDataPath = "Assets/TextData/LayerData/";

        /// <summary>
        /// Get Path to save json file, jsonName used as addressable name
        /// </summary>
        /// <param name="jsonName"></param>
        /// <returns></returns>
        public static string GetJsonLevelDataPath(string jsonName)
        {
            return _jsonLevelDataPath + jsonName + _jsonExtension;
        }

        /// <summary>
        /// Get Path to save json file, jsonName used as addressable name
        /// </summary>
        /// <param name="jsonName"></param>
        /// <returns></returns>
        public static string GetJsonLayerDataPath(string jsonName)
        {
            return _jsonLayerDataPath + jsonName + _jsonExtension;
        }
    }
}