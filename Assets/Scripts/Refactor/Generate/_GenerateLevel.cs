using System.Collections.Generic;
using System.IO;
using Core.Extensions;
using Core.ResourceGamePlay;
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
        [SerializeField] private TextAsset _testAsset;
        [SerializeField] private TextAsset _levelPosAsset;
        [SerializeField] private string _determineChar = "\n";

        private const string _levelPath = "Assets/Data/JSonData/TestGenLevelData.txt";

        private bool[,,] _isChecked;
        private Vector3Int[] _positionArray;
        private (Vector3, int)[,,] _stateMatrix = new (Vector3, int)[30, 30, 30];
        private bool _isGenerateCompleted = false;
        private List<int> _currentPath = new List<int>();
        private int _maxDis = 0;
        private List<Vector3Int> _direction = new List<Vector3Int>(){
            Vector3Int.up,
            Vector3Int.down,
            Vector3Int.left,
            Vector3Int.right,
            Vector3Int.forward,
            Vector3Int.back
        };

        public void ConvertData()
        {
            ConvertData(_testAsset);
        }

        private void ConvertData(TextAsset text)
        {
            var data = text.text.Split('\n', System.StringSplitOptions.RemoveEmptyEntries);
            int minX = 999, minY = 999, minZ = 999;
            int count = 0;
            foreach (var d in data)
            {
                var tmp = d.Split(' ');
                if (System.Int32.TryParse(tmp[0], out int res)) break;
                count += 1;
            }
            Vector3Int[] posArray = new Vector3Int[data.Length - count];
            for (int i = count; i < data.Length; i++)
            {
                var tmp = data[i].Split(' ');
                var pos = new Vector3Int(System.Int32.Parse(tmp[0]), System.Int32.Parse(tmp[1]), System.Int32.Parse(tmp[2]));
                posArray[i - count] = pos;
                minX = Mathf.Min(minX, pos.x);
                minY = Mathf.Min(minY, pos.y);
                minZ = Mathf.Min(minZ, pos.z);
            }
            _positionArray = posArray;
            foreach (var pos in posArray)
            {
                Debug.Log("pos: " + pos);
            }
        }

        public List<state> GenerateLevel(Vector3Int[] positionArray, bool isColored = false, Vector3Int[] colorArray = null)
        {
            //File.WriteAllText(_levelPath, "");
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
                _stateMatrix[x, y, z] = (_direction[GetRandomDirectionIndex()], i);
                _isChecked[x, y, z] = false;
            }

            for (int i = 0; i < positionArray.Length; i++)
            {
                _isGenerateCompleted = false;
                _currentPath.Clear();
                Try(i);
            }
            // foreach (var pos in positionArray)
            // {
            //     File.AppendAllText(_levelPath, "pos: " + pos + " dir: " + _stateMatrix[pos.x, pos.y, pos.z].Item1 + "\n");
            // }
            List<state> states = new List<state>();
            if (isColored)
            {
                for(int i = 0; i < positionArray.Length; i++){
                    var tmpDirect = Vector3.zero;
                    if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.up)
                    {
                        tmpDirect = new Vector3(0, 0, -90);
                    }
                    else if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.down)
                    {
                        tmpDirect = new Vector3(0, 0, 90);
                    }
                    else if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.left)
                    {
                        tmpDirect = new Vector3(-180, 0, 0);
                    }
                    else if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.right)
                    {
                        tmpDirect = new Vector3(0, 180, 0);
                    }
                    else if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.forward)
                    {
                        tmpDirect = new Vector3(0, 90, 0);
                    }
                    else if (_stateMatrix[positionArray[i].x, positionArray[i].y, positionArray[i].z].Item1 == Vector3.back)
                    {
                        tmpDirect = new Vector3(0, -90, 0);
                    }
                    states.Add(new state(positionArray[i], tmpDirect, colorArray[i]));
                }
            }
            else
            {
                foreach (var pos in positionArray)
                {
                    var tmpDirect = Vector3.zero;
                    if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.up)
                    {
                        tmpDirect = new Vector3(0, 0, -90);
                    }
                    else if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.down)
                    {
                        tmpDirect = new Vector3(0, 0, 90);
                    }
                    else if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.left)
                    {
                        tmpDirect = new Vector3(-180, 0, 0);
                    }
                    else if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.right)
                    {
                        tmpDirect = new Vector3(0, 180, 0);
                    }
                    else if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.forward)
                    {
                        tmpDirect = new Vector3(0, 90, 0);
                    }
                    else if (_stateMatrix[pos.x, pos.y, pos.z].Item1 == Vector3.back)
                    {
                        tmpDirect = new Vector3(0, -90, 0);
                    }
                    states.Add(new state(pos, tmpDirect));
                }
            }
            return states;
        }
#if UNITY_EDITOR
        public void GenerateLevelFromText()
        {
            File.WriteAllText(_levelPath, "");
            var data = _OnlineDataManager.ReadGoogleData();
            //Debug.Log("Data Length: " + data.Length);
            foreach (var d in data)
            {
                //var (levelIndex, size, posArray) = FromStringToArray(d);
                var levelIndex = d.GetLevel();
                var size = d.GetSize();
                var posArray = d.GetPosData();
                var isColored = d.IsColored();
                var colorArray = d.GetColorData();
                var states = GenerateLevel(posArray, isColored, colorArray);
                var levelData = new LevelData(levelIndex, posArray.Length, size, states, isColored);
                File.AppendAllText(_levelPath, _ConvertFromSOToJSon.FormatJson(JsonUtility.ToJson(levelData)) + "\n" + "-----------------------------------" + "\n");
            }
        }
#endif

        private string[] ReadTextData()
        {
            return _levelPosAsset.text.Split(_determineChar, System.StringSplitOptions.RemoveEmptyEntries);
        }

        private (int, Vector3, Vector3Int[]) FromStringToArray(string posLevelText)
        {
            var data = posLevelText.Split(new string[] { "\n", "\r" }, System.StringSplitOptions.RemoveEmptyEntries);
            int levelIndex = System.Int32.Parse(data[0]);
            var sizeText = data[1].Split(" ");
            Vector3Int size = new Vector3Int(System.Int32.Parse(sizeText[0]), System.Int32.Parse(sizeText[1]), System.Int32.Parse(sizeText[2]));
            List<Vector3Int> posArray = new List<Vector3Int>();
            var posData = data[2].Split(",");
            foreach (var pos in posData)
            {
                var tmp = pos.Split(" ", System.StringSplitOptions.RemoveEmptyEntries);
                if (!System.Int32.TryParse(tmp[0], out int posX))
                {
                    Debug.LogError("Can't parse int from string: " + tmp[0]);
                }
                else if (!System.Int32.TryParse(tmp[1], out int posY))
                {
                    Debug.LogError("Can't parse int from string: " + tmp[1]);
                }
                else if (!System.Int32.TryParse(tmp[2], out int posZ))
                {
                    Debug.LogError("Can't parse int from string: " + tmp[2]);
                }

                posArray.Add(new Vector3Int(System.Int32.Parse(tmp[0]), System.Int32.Parse(tmp[1]), System.Int32.Parse(tmp[2])));
            }
            return (levelIndex, size, posArray.ToArray());
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
                    if (_isNext) continue;
                    Try(t[0]);
                }
            }
            _currentPath.Remove(k);
            //_isChecked[currentPos.x, currentPos.y, currentPos.z] = false;
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
#if UNITY_EDITOR
    [CustomEditor(typeof(_GenerateLevel))]
    public class _GenerateLevelEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _GenerateLevel _generateLevel = (_GenerateLevel)target;
            if (GUILayout.Button("Gen Level"))
            {
                _generateLevel.GenerateLevelFromText();
            }
            // else if (GUILayout.Button("Convert Data"))
            // {
            //     // var text = Resources.Load<TextAsset>("TestGenLevelData");
            //     _generateLevel.ConvertData();
            // }
        }
    }
#endif
}