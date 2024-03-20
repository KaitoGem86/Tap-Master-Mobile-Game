using UnityEngine;

namespace Core.SystemGame{
    public class _InputSystem{
        private static _InputSystem _instance;
        
        public static _InputSystem Instance { 
            get { 
                if (_instance == null)
                {
                    _instance = new _InputSystem();
                }
                return _instance;
            }
        }

        public float Timer { get; set; }

        public bool CheckSelect(){
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended){
                return true;
            }
            return Input.GetMouseButtonUp(0);
        }

        public bool CheckSelectDown(){
            if(Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began){
                return true;
            }
            return Input.GetMouseButtonDown(0);
        }

        public bool CheckHold(){
            if(Input.GetMouseButton(0) || Input.touchCount == 1 && Input.GetTouch(0).phase != TouchPhase.Began){
                return true;
            }
            else{
                Timer = 0;
                return false;
            }
        }

        public Vector3 GetInputPositionInWorld(){
            if(Input.GetMouseButtonUp(0)){
                Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                //inputPos.z = -30;
                return inputPos;
            }

            if(Input.touchCount == 1){
                Vector3 inputPos = Camera.main.ScreenToWorldPoint(Input.GetTouch(0).position);
                inputPos.z = -30;
                return inputPos;
            }

            return Vector3.positiveInfinity;
        }
    }
}