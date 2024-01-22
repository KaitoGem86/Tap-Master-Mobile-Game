using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public static InputController instance;

    private Vector3 inputPos = new Vector3();

    float timer = 0;


    public Vector3 InputPos { get { return inputPos; } }

    public float Timer
    {
        get { return timer; }
        set { timer = value; }
    }

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (Input.GetMouseButton(0) || Input.touchCount == 1)
        {
            timer += Time.deltaTime;
        }
        else
        {
            timer = 0;
        }
    }

    public bool CheckSelect()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Ended)
        {
            return true;
        }
        return Input.GetMouseButtonUp(0);
    }


    public Vector3 GetInputPosition()
    {
        if (Input.GetMouseButtonUp(0))
        {
            inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Debug.Log(Input.mousePosition);
            Debug.Log(inputPos);
            //inputPos.z = -30;
            return inputPos;
        }

        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = Camera.main.ScreenToWorldPoint(touch.position);
            inputPos.z = -30;
            return inputPos;
        }

        return Vector3.positiveInfinity;
    }

    public Vector3 GetInputMoveObject()
    {
        Vector3 inputPos = Vector3.negativeInfinity;
        if (Input.GetMouseButton(0))
        {
            inputPos = Input.mousePosition;
        }
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);
            inputPos = Camera.main.ScreenToWorldPoint(touch.position);
        }
        return inputPos;
    }

    public int CheckSpread()
    {
        if (Input.touchCount == 2)
        {
            return 1;
        }
        if (Input.GetAxis("Mouse ScrollWheel") != 0)
        {
            return 2;
        }
        return 0;
    }

    public bool CheckInputRotate()
    {
        if (CheckSpread() == 1 || CheckSpread() == 2)
            return false;
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Moved || Input.GetMouseButton(0))
        {
            return true;
        }
        return false;
    }

    public Vector3 GetInputRotate()
    {
        inputPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        inputPos.z = -30;
        return inputPos;
    }
}
