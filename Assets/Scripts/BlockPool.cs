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
    [SerializeField] List<BlockController> listBlock = new List<BlockController>();
    [SerializeField] public Rigidbody rb;

    private Vector3 oldPos = Vector3.zero;
    private Vector3 newPos = Vector3.zero;
    //private Vector3 aRotate = new Vector3();
    private int level;
    private int count;
    private TextAsset[] path_list;
    private TextAsset levelText;
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
        Debug.Log("Assets\\Resources\\LevelData\\level " + Level);
        path_list = Resources.LoadAll<TextAsset>("LevelData\\level " + Level);
        i = UnityEngine.Random.Range(0, path_list.Length);
        Debug.Log(path_list.Length);
        levelText = path_list[i];
    }

    void Readdata()
    {
        string[] lines = levelText.text.Split("\n");
        size = Int16.Parse(lines[0]);
        GameManager.Instance.camSize = Int16.Parse(lines[1]);
        if (GameManager.Instance.camSize == 0)
            GameManager.Instance.camSize = 1;
        CameraController.SetCameraSize(GameManager.Instance.camSize > 2 ? GameManager.Instance.camSize : 2);
        for (int i = 2; i < lines.Length - 1; i++)
        {
            string line = lines[i];
            int j = line.IndexOf('(') + 1;
            float x = float.Parse(line.Substring(j, 4));
            int k = line.IndexOf(' ', (int)j) + 1;
            float y = float.Parse(line.Substring(k, 4));
            j = line.IndexOf(' ', (int)k) + 1;
            float z = float.Parse(line.Substring(j, 4));
            Vector3 pos = new Vector3(x, y, z);

            k = line.IndexOf('(', (int)j) + 1;
            x = float.Parse(line.Substring(k, 4));
            j = line.IndexOf(' ', (int)k) + 1;
            y = float.Parse(line.Substring(j, 4));
            k = line.IndexOf(' ', (int)j) + 1;
            z = float.Parse(line.Substring(k, 4));
            Vector3 angle = new Vector3(x, y, z);

            var go = Instantiate(listBlock[0].gameObject, pos * 4.05f, Quaternion.Euler(angle), this.transform);
            pool.Add(go);
        }
    }
}
