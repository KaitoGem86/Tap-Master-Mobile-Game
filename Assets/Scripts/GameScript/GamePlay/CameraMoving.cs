using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMoving : MonoBehaviour
{
    [SerializeField] Rigidbody camRotate;

    Vector3 newPos;
    Vector3 oldPos;
    bool canRotate = true;

    public bool CanRotate
    {
        get { return canRotate; }
        set { canRotate = value; }
    }


    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && canRotate)
        {
            newPos = Input.mousePosition;
            Vector3 direction = newPos - oldPos;
            direction.Normalize();
            //Debug.Log(direction);
            //rbOfCameraParent.AddRelativeTorque(new Vector3(direction.y, -direction.y, direction.z));
            camRotate.AddRelativeTorque(new Vector3(-direction.y, direction.x, direction.z) * 10);
            oldPos = newPos;
        }
    }
}
