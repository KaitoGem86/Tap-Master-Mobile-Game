using UnityEngine;

namespace Extensions{
    public static class _ConvertPositionToLogic{
        public static Vector3Int LogicPos(Vector3 vector3){
            int x = Mathf.FloorToInt(vector3.x);
            int y = Mathf.FloorToInt(vector3.y);
            int z = Mathf.FloorToInt(vector3.z);
            return new Vector3Int(x, y, z);
        }
    }
}