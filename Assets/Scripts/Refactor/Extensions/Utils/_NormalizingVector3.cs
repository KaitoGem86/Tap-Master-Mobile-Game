using System;
using UnityEngine;

namespace Extensions{
    public static class _NormalizingVector3{
        public static Vector3Int LogicPos(Vector3 vector3){
            int x = Mathf.FloorToInt(vector3.x);
            int y = Mathf.FloorToInt(vector3.y);
            int z = Mathf.FloorToInt(vector3.z);
            return new Vector3Int(x, y, z);
        }

        public static Vector3Int IgnoreDecimalPart(Vector3 vector3){
            int x = (int)Math.Truncate(vector3.x);
            int y = (int)Math.Truncate(vector3.y);
            int z = (int)Math.Truncate(vector3.z);
            return new Vector3Int(x, y, z);
        }
    }
}