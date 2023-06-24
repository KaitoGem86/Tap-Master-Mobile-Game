using System;
using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Security.Cryptography;
using Unity.VisualScripting;
using UnityEngine;
// đọc thêm 1 dòng chứa khoảng cách xa nhất của 2 block, lưu vào 1 biến quản lý bởi GameManager.

public class BlockPool : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] public Rigidbody rb;

    private Vector3 oldPos = Vector3.zero;
    private Vector3 newPos = Vector3.zero;
    //private Vector3 aRotate = new Vector3();
    private int level;
    private int count;
    //private TextAsset[] path_list;
    //private TextAsset levelText;
    private LevelDatasController levelData;
    private int size;
    private int i;
    //private bool isARotating = false;

    public bool isRotating = false;
    public List<GameObject> pool;

    public int Size
    {
        get { return size; }
    }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }


    // Start is called before the first frame update
    public void StartInit(int l)
    {
        this.Level = l;
        //InitPool();
        Initial();
        Readdata();
    }

    // Update is called once per frame
    void Update()
    {
        Rotate();
        isRotating = CheckRotating();
        if (isRotating)
            this.transform.rotation = rb.rotation;
    }



    void Rotate()
    {
        //if (Input.GetMouseButton(0))
        //    RotateMouse();
        if (InputController.instance.CheckInputRotate())
        {
            RotateTouch();
        }
        else
        {
            if (Input.GetMouseButtonUp(0))
                InputController.instance.Timer = 0;
            count = 1;
        }
    }

    void RotateMouse()
    {
        float speed = 10f;
        float horizontal = speed * Input.GetAxis("Mouse X");
        float vertical = speed * Input.GetAxis("Mouse Y");

        rb.AddTorque(vertical, -horizontal, 0);
        //transform.Rotate(this.rb.rotation.x,this.rb.rotation.y, this.rb.rotation.z);
    }


    void RotateTouch()
    {
        float speed = 5f;
        newPos = InputController.instance.GetInputRotate();
        if (count == 1)
        {
            oldPos = newPos;
            count = 0;
        }
        float horizontal = speed * (newPos.x - oldPos.x);
        float vertical = speed * (newPos.y - oldPos.y);

        rb.AddTorque(new Vector3(vertical, -horizontal, 0).normalized * 40);
        oldPos = newPos;
    }

    bool CheckRotating()
    {
        return rb.angularVelocity != Vector3.zero;
    }

    void Initial()
    {
        levelData = GameManager.Instance.data.datasControllers[this.level - 1];
    }

    void Readdata()
    {
        this.size = levelData.states.Count;
        GameManager.Instance.camSize = levelData.maxDis;
        CameraController.SetCameraSize(GameManager.Instance.camSize > 2 ? GameManager.Instance.camSize : 2);
        foreach (var s in levelData.states)
        {
            var go = Instantiate(blockPrefab, s.pos * 4, Quaternion.Euler(s.rotation), this.transform);
            pool.Add(go);
        }
    }
}
