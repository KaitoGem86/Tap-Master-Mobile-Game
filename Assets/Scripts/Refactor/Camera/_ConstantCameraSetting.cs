using UnityEngine;

namespace Core.GamePlay{
    public static class _ConstantCameraSetting{
        public static Vector3 _9x21PositionSetting = new Vector3(0, 0.3f, -13);
        public static Vector3 _9x16PositionSetting = new Vector3(0, 0, -10);
    
        private static Vector3 _maxSizeIs5Setting = new Vector3(0, 0, -10);
        private static Vector3 _maxSizeIs10Setting = new Vector3(0, 0, -16);
        private static Vector3 _maxSizeIs15Setting = new Vector3(0, 0, -25);
        private static Vector3 _maxSizeIs20Setting = new Vector3(0, 0, -32);

        public static Vector3 GetCameraPositionValue(int size){
            if(size <= 5){
                return _maxSizeIs5Setting;
            }else if(size <= 10){
                return _maxSizeIs10Setting;
            }else if(size <= 15){
                return _maxSizeIs15Setting;
            }else 
                return _maxSizeIs20Setting;
        }
    }
}