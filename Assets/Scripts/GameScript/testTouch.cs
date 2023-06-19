using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class testTouch : MonoBehaviour
{
    [SerializeField] private Rigidbody rb;
    //Touch touch;
    //// Start is called before the first frame update
    //void Start()
    //{

    //}

    // Update is called once per frame
    void Update()
    {
        if (Input.touchCount == 1 && Input.GetTouch(0).phase == TouchPhase.Began)
            Debug.Log("Touch");
    }

    private void Start()
    {
        rb.angularVelocity = new Vector3(1, 0, 0);
    }
}
