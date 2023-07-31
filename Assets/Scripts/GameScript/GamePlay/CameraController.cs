using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Chỉnh lại size camera bằng cách đặt kích thước ban đầu tỉ lệ thuận với khoảng cách xa nhất giữa 2 trong số các block
// Độ zoom Camera cũng sẽ tỉ lệ thuận với khoảng cách đó.
public class CameraController : MonoBehaviour
{
    //public float speed = 2.0f;

    //void Update()
    //{
    //    float horizontal = speed * Input.GetAxis("Mouse X");
    //    float vertical = speed * Input.GetAxis("Mouse Y");

    //    transform.Rotate(-vertical, horizontal, 0);
    //}

    [SerializeField] private static Camera _camera;

    private static float startSize;
    private static float increaseSize;
    private static float decreaseSize;
    private float olddis = 1;

    private void Awake()
    {
        _camera = GetComponent<Camera>();
    }

    private void Start()
    {
        //SetCameraSize();

    }

    private void Update()
    {
        //_camera.orthographicSize = 15;
        //Debug.Log(Input.GetAxis("Mouse ScrollWheel"));
        if (GameManager.Instance.blockPool.canRotate)
            ZoomCamera();
    }

    void ZoomCamera()
    {

        if (InputController.instance.CheckSpread() == 2)
        {
            if (_camera.orthographicSize >= startSize - decreaseSize && _camera.orthographicSize <= startSize + increaseSize)
            {
                _camera.orthographicSize -= Input.GetAxis("Mouse ScrollWheel") * 5;
            }

            if (_camera.orthographicSize < startSize - decreaseSize)
                _camera.orthographicSize = startSize - decreaseSize;
            if (_camera.orthographicSize > startSize + increaseSize)
                _camera.orthographicSize = startSize + increaseSize;
            GameManager.Instance.blockPool.rb.angularVelocity = Vector3.zero;
        }
        if (InputController.instance.CheckSpread() == 1)
        {
            Touch touch1 = Input.GetTouch(0);
            Touch touch2 = Input.GetTouch(1);
            float dis = Vector3.Distance(touch1.position, touch2.position);
            float size = dis / olddis;
            if (size == 1)
            {
                return;
            }
            if (_camera.orthographicSize >= startSize - decreaseSize && _camera.orthographicSize <= startSize + increaseSize)
            {
                _camera.orthographicSize -= 0.5f * (size > 1 ? 1 : -1);
            }

            if (_camera.orthographicSize < startSize - decreaseSize)
                _camera.orthographicSize = startSize - decreaseSize;
            if (_camera.orthographicSize > startSize + increaseSize)
                _camera.orthographicSize = startSize + increaseSize;
            GameManager.Instance.blockPool.rb.angularVelocity = Vector3.zero;
            olddis = dis;
        }
    }

    public static void SetCameraSize(int i)
    {
        if (i >= 15)
            _camera.orthographicSize = 4 * i;
        else if (i >= 5)
            _camera.orthographicSize = 7 * i;
        else
            _camera.orthographicSize = 10 * i;
        startSize = _camera.orthographicSize;
        Vector3 pos = new Vector3(0, 0, -100);
        _camera.transform.position = pos;
        increaseSize = (int)startSize / 10 * 3.5f;
        decreaseSize = (int)startSize / 10 * 2.5f;
    }
}
