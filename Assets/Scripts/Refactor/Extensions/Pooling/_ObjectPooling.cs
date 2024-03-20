using System.Collections.Generic;
using UnityEngine;

namespace ObjectPooling {
    public class _ObjectPooling{
        private static _ObjectPooling _instance;
        private Dictionary<_TypeGameObjectEnum, Queue<GameObject>> _poolDictionary;
        private Dictionary<_TypeGameObjectEnum, GameObject> _prefabDictionary;
        private Transform _poolParent;
        private Transform _disabledPoolParent;

        public static _ObjectPooling Instance{
            get{
                if (_instance == null)
                {
                    _instance = new _ObjectPooling();
                }
                return _instance;
            }
        }

        private _ObjectPooling(){
            _poolDictionary = new Dictionary<_TypeGameObjectEnum, Queue<GameObject>>();
            _prefabDictionary = new Dictionary<_TypeGameObjectEnum, GameObject>();
            _poolParent = new GameObject("PoolParent").transform;
            _disabledPoolParent = new GameObject("DisabledPoolParent").transform;
        }

        public void CreatePool(_TypeGameObjectEnum key, GameObject prefab, int size){
            if (!_poolDictionary.ContainsKey(key))
            {
                _poolDictionary.Add(key, new Queue<GameObject>());
                _prefabDictionary.Add(key, prefab);
                for (int i = 0; i < size; i++)
                {
                    GameObject tmp = GameObject.Instantiate(prefab, _disabledPoolParent);
                    tmp.SetActive(false);
                    _poolDictionary[key].Enqueue(tmp);
                }
            }
        }

        public GameObject SpawnFromPool(_TypeGameObjectEnum key, Vector3 position, Quaternion rotation){
            if (_poolDictionary.ContainsKey(key))
            {
                if (_poolDictionary[key].Count > 0)
                {
                    GameObject tmp = _poolDictionary[key].Dequeue();
                    tmp.SetActive(true);
                    tmp.transform.position = position;
                    tmp.transform.rotation = rotation;
                    return tmp;
                }
                else
                {
                    GameObject tmp = GameObject.Instantiate(_prefabDictionary[key], position, rotation, _poolParent);
                    return tmp;
                }
            }
            else
            {
                Debug.LogError("Pool with key: " + key + " doesn't exist");
                return null;
            }
        }

        public void ReturnToPool(_TypeGameObjectEnum key, GameObject gameObject){
            if (_poolDictionary.ContainsKey(key))
            {
                gameObject.transform.SetParent(_disabledPoolParent);
                gameObject.SetActive(false);
                _poolDictionary[key].Enqueue(gameObject);
            }
            else
            {
                Debug.LogError("Pool with key: " + key + " doesn't exist");
            }
        }
    }
}