using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Generate
{
    public class _GenerateLevel : MonoBehaviour
    {
        [SerializeField] private Mesh _blockMesh;

        // private void Start()
        // {
        //     var t = _blockMesh.vertices;
        //     foreach (var v in t)
        //     {
        //         Debug.Log(v);
        //         Debug.DrawRay(v, Vector3.up * 0.2f, Color.red, 100f);
        //     }

        //     var t2 = _blockMesh.triangles;
        //     for (int i = 0; i < t2.Length; i += 3)
        //     {
        //         // Debug.Log(v);

        //         var pos1 = t[t2[i]];
        //         var pos2 = t[t2[i + 1]];
        //         var pos3 = t[t2[i + 2]];
        //         Debug.DrawRay(pos2, pos1 - pos2, Color.blue, 100f);
        //         Debug.DrawRay(pos3, pos2 - pos3, Color.blue, 100f);
        //         Debug.DrawRay(pos1, pos3 - pos1, Color.blue, 100f);
        //     }
        // }
        [SerializeField] private Vector3Int[] _posArrayTest;

        public void GenLevel(){
            GenerateLevel(_posArrayTest);
        }

        private const string _levelPath = "Assets/Data/JSonData/TestGenLevelData.txt";

        private bool[,,] _isChecked;
        private Vector3Int[] _positionArray;
        private (Vector3, int)[,,] _stateMatrix = new (Vector3, int)[30, 30, 30];
        private bool _isGenerateCompleted = false;
        private List<int> _currentPath = new List<int>();
        private List<Vector3Int> _direction = new List<Vector3Int>(){
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.forward,
            Vector3Int.back
        };

        public void GenerateLevel(Vector3Int[] positionArray)
        {
            File.WriteAllText(_levelPath, "");
            _stateMatrix = new (Vector3, int)[30, 30, 30];
            _isGenerateCompleted = false;
            _isChecked = new bool[30, 30, 30];
            _positionArray = positionArray;
            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 30; j++)
                {
                    for (int k = 0; k < 30; k++)
                    {
                        _stateMatrix[i, j, k] = (Vector3.zero, -1);
                        _isChecked[i, j, k] = false;
                    }
                }
            }

            for (int i = 0; i < positionArray.Length; i++)
            {
                var x = positionArray[i].x;
                var y = positionArray[i].y;
                var z = positionArray[i].z;
                _stateMatrix[x, y, z] = ( _direction[GetRandomDirectionIndex()], i);
                _isChecked[x, y, z] = false;
            }

            for (int i = 0; i < positionArray.Length; i++)
            {
                _isGenerateCompleted = false;
                _currentPath.Clear();
                Try(i);
            }
            foreach (var pos in positionArray)
            {
                File.AppendAllText(_levelPath, "pos: " + pos + " dir: " + _stateMatrix[pos.x, pos.y, pos.z].Item1 + "\n");
            }
        }

        private void Try(int k)
        { // add rotation for block at postitionArray[k]
            var currentPos = _positionArray[k];
            if (_isChecked[currentPos.x, currentPos.y, currentPos.z]) return;
            _isChecked[currentPos.x, currentPos.y, currentPos.z] = true;
            _currentPath.Add(k);
            int randomIndex = GetRandomDirectionIndex();
            for (int count = 0; count < 6; count++)
            { // need to improve => change to random direction
                var dir = _direction[randomIndex]; randomIndex = (randomIndex + 1) % 6;
                bool _isNext = false;
                if (_isGenerateCompleted) break;
                _stateMatrix[currentPos.x, currentPos.y, currentPos.z].Item1 = dir;
                var t = GetIndicesWithDirectFromPos(k, dir);
                if (t.Count == 0)
                {
                    Solution();
                    break;
                }
                else
                {
                    foreach (var index in t)
                    {
                        if (_isChecked[_positionArray[index].x, _positionArray[index].y, _positionArray[index].z]
                        && _stateMatrix[_positionArray[index].x, _positionArray[index].y, _positionArray[index].z].Item1 == -dir)
                        {
                            _isNext = true;
                            break;
                        }
                        if (_currentPath.Contains(index))
                        {
                            _isNext = true;
                            break;
                        }
                    }
                    if (_isNext) break;
                    Try(t[0]);
                }
            }
            _currentPath.Remove(k);
        }

        private void Solution()
        {
            // if all block is placed
            // if all block is connected
            Debug.Log("Current Path Count: " + _currentPath.Count);
            _isGenerateCompleted = true;
            // foreach (var i in _currentPath)
            // {
            //     Debug.Log(_positionArray[i]);
            //     File.AppendAllText(_levelPath, "pos: " + _positionArray[i] + " dir: " + _stateMatrix[_positionArray[i].x, _positionArray[i].y, _positionArray[i].z].Item1 + "\n");
            // }
        }

        private List<int> GetIndicesWithDirectFromPos(int k, Vector3Int direction)
        {
            var tmp = new List<int>();
            var currentPos = _positionArray[k];
            Vector3Int tmpPos = currentPos + direction;
            while (true)
            {
                if (tmpPos.x >= 30 || tmpPos.x < 0) break;
                if (tmpPos.y >= 30 || tmpPos.y < 0) break;
                if (tmpPos.z >= 30 || tmpPos.z < 0) break;
                if (_stateMatrix[tmpPos.x, tmpPos.y, tmpPos.z].Item2 != -1)
                {
                    tmp.Add(_stateMatrix[tmpPos.x, tmpPos.y, tmpPos.z].Item2);
                }
                tmpPos += direction;
            }
            return tmp;
        }

        private int GetRandomDirectionIndex()
        {
            return Random.Range(0, 6);
        }
    }

    [CustomEditor(typeof(_GenerateLevel))]
    public class _GenerateLevelEditor : Editor{
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _GenerateLevel _generateLevel = (_GenerateLevel)target;
            if(GUILayout.Button("Gen Level")){
                _generateLevel.GenLevel();
            }
        }
    }
}