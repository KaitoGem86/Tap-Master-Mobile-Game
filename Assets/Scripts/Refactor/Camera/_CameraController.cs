using System.Security.Policy;
using Core.SystemGame;
using DG.Tweening;
using Unity.VisualScripting;
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
        private Vector3 _lastRemainingDelta;
        private Vector3 _lastMousePosition;
        private float _zoomCameraValue;
        private float _lastZoomCameraValue;

        private Vector3 _maxSizeZoomCamera;
        private Vector3 _minSizeZoomCamera;

        private void Awake()
        {
            _GameEvent.OnGamePlayReset += SetUp;
        }

        private void Start()
        {
            _GameManager.Instance.CameraController = this;
            SetUp();
        }

        private void OnDestroy()
        {
            _GameEvent.OnGamePlayReset -= SetUp;
        }

        public void SetUp()
        {
            _cameraRotation.DORotate(new Vector3(-45, 90, 90), 0.5f);
            SetCameraSize();
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

            if (_InputSystem.Instance.CheckSpread())
            {
                float zoomValue = _InputSystem.Instance.GetZoomValue();
                _zoomCameraValue = zoomValue * Time.deltaTime;
            }

            Vector3 remainTmp = Vector3.Lerp(_lastRemainingDelta, _remainingDelta, _inertia);
            float remainZoomValue = Mathf.Lerp(_zoomCameraValue, 0, _inertia);
            _cameraRotation.Rotate(Vector3.left, remainTmp.y, Space.Self);
            _cameraRotation.Rotate(Vector3.up, remainTmp.x, Space.Self);
            ZoomCamera(remainZoomValue);

            if (_damping > 0.0f)
            {
                _remainingDelta = Vector3.Lerp(_remainingDelta, Vector3.zero, _damping);
                _zoomCameraValue = Mathf.Lerp(_zoomCameraValue, 0, _damping);
            }
            else
            {
                _remainingDelta = Vector3.zero;
                _zoomCameraValue = 0;
            }
            _lastRemainingDelta = _remainingDelta;
            _lastZoomCameraValue = _zoomCameraValue;
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

        private void ZoomCamera(float zoomValue = 0)
        {
            if (zoomValue > 0)
            {
                if (this.transform.localPosition.z + Vector3.forward.z < _maxSizeZoomCamera.z)
                {
                    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, _maxSizeZoomCamera, zoomValue);
                }
            }
            else if (zoomValue < 0)
            {
                if (this.transform.localPosition.z - Vector3.forward.z > _minSizeZoomCamera.z)
                    this.transform.localPosition = Vector3.Lerp(this.transform.localPosition, _minSizeZoomCamera, -zoomValue);
            }
        }

        private void SetCameraSize()
        {
            //Debug.Log(_GameManager.Instance.Level.size.x);
            //Debug.Log(_ConstantCameraSetting.GetCameraPositionValue((int)_GameManager.Instance.Level.size.x));
            int max = Mathf.Max((int)_GameManager.Instance.Level.size.x, (int)_GameManager.Instance.Level.size.y, (int)_GameManager.Instance.Level.size.z);
            (this.transform.localPosition, _maxSizeZoomCamera, _minSizeZoomCamera) = _ConstantCameraSetting.GetCameraPositionValue(max);
        }

        public float Sensitivity
        {
            get => _sensitivity;
            set => _sensitivity = value;
        }

        public float Damping
        {
            get => _damping;
            set => _damping = value;
        }

        public float Inertia
        {
            get => _inertia;
            set => _inertia = value;
        }
    }
}
