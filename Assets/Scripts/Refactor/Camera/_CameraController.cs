using Core.SystemGame;
using UnityEngine;

namespace Core.GamePlay
{
    public class _CameraController : MonoBehaviour
    {
        [SerializeField] private float _sensitivity = 1.0f;
        [SerializeField] private float _damping = -1.0f;
        [SerializeField][Range(0.0f, 1.0f)] private float _inertia;
        [SerializeField] private Transform _cameraRotation;

        private Vector3 _remainingDelta;
        private Vector3 _lastMousePosition;

        public void SetUp()
        {
        }


        private void LateUpdate()
        {
            if (_InputSystem.Instance.CheckSelectDown())
            {
                _lastMousePosition = Input.mousePosition;
            }
            else if (_InputSystem.Instance.CheckHold())
            {
                Vector3 mouseDelta = Input.mousePosition - _lastMousePosition;
                _lastMousePosition = Input.mousePosition;

                _remainingDelta = mouseDelta * _sensitivity * Time.deltaTime;
            }

            Vector3 remainTmp = _remainingDelta;
            _cameraRotation.Rotate(Vector3.left, remainTmp.y, Space.Self);
            _cameraRotation.Rotate(Vector3.up, remainTmp.x, Space.Self);
                                                                                                                                                                                                                                                                    
            if (_damping > 0.0f)
            {
                _remainingDelta = Vector3.Lerp(_remainingDelta, Vector3.zero, _inertia);
            }     
        }

        //Set Camera size by modify position.z and position.y of camera
        // private void SetCameraSize(){
        //     var cameraSize = _GameManager.Instance.CameraCanvas.aspect;
        //     if(cameraSize < 0.5 && cameraSize > 0.4)
        //         _GameManager.Instance.CameraCanvas.transform.position = _ConstantCameraSetting._9x21PositionSetting;
        //     if(cameraSize < 0.6 && cameraSize > 0.5)
        //         _GameManager.Instance.CameraCanvas.transform.position = _ConstantCameraSetting._9x16PositionSetting;
        //     Debug.Log(cameraSize);
        // }
    }
}
