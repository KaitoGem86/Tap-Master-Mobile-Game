using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
// đọc thêm 1 dòng chứa khoảng cách xa nhất của 2 block, lưu vào 1 biến quản lý bởi GameManager.

public class BlockPool : MonoBehaviour
{
    [SerializeField] GameObject blockPrefab;
    [SerializeField] GameObject puzzleRewardPrefab;
    [SerializeField] public Rigidbody rb;
    [SerializeField] TestMoveBlock block;
    [SerializeField] MB3_MeshBaker meshBaker;

    private Vector3 oldPos = Vector3.zero;
    private Vector3 newPos = Vector3.zero;
    private List<Vector3> listPos = new List<Vector3>();
    private GameObject[] combinedGo;
    //private Vector3 aRotate = new Vector3();
    private int level;
    private int count;
    private LevelDatasController levelData;
    private int size;
    private int i;
    //private bool isARotating = false;

    public bool isRotating = false;
    public List<GameObject> pool;
    public bool canRotate = true;



    public int Size
    {
        get { return size; }
    }

    public Rigidbody Rb { get { return rb; } set { rb = value; } }

    public int Level
    {
        get { return level; }
        set { level = value; }
    }

    public GameObject BlockPrefab
    {
        get => blockPrefab;
        set => blockPrefab = value;
    }

    // Start is called before the first frame update
    public void StartInit(int l)
    {
        MeshRenderer mesh = blockPrefab.GetComponent<TestMoveBlock>().Mesh;
        UnityEngine.Color a = mesh.sharedMaterial.color;
        a.a = 0;
        mesh.sharedMaterial.SetColor("_Color", a);
        this.Level = l;
        //InitPool();
        Initial();
        Readdata();
        this.transform.parent.transform.DORotate(new Vector3(UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360), UnityEngine.Random.Range(0, 360)), 3);
        a.a = 1;
        mesh.sharedMaterial.SetColor("_Color", a);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //if (canRotate)
        //    Rotate();
        //isRotating = CheckRotating();
        //if (isRotating)
        //    this.transform.rotation = rb.rotation;
        if (Input.GetKeyDown(KeyCode.C))
        {
            Debug.Log("C" + combinedGo.Length);

            StaticBatchingUtility.Combine(combinedGo, this.gameObject);
        }
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

        rb.AddTorque(new Vector3(vertical, -horizontal, 0).normalized * 20);
        oldPos = newPos;
    }

    bool CheckRotating()
    {
        return rb.angularVelocity != Vector3.zero;
    }

    void Initial()
    {
        //levelData = GameManager.Instance.data.datasControllers[this.level - 1];
    }

    void Readdata()
    {
        float max = 0;
        Vector3 maxPos = new Vector3();
        DG.Tweening.Sequence seq = DOTween.Sequence();
        this.size = levelData.states.Count;
        float timer = 0;
        if (this.size < 50)
            timer = 0.5f / this.size;
        else if (this.size < 100)
            timer = 1f / this.size;
        else if (this.size >= 300)
            timer = 3f / this.size;
        else
            timer = 2f / this.size;
        GameManager.Instance.camSize = levelData.maxDis;
        CameraController.SetCameraSize(GameManager.Instance.camSize > 2 ? GameManager.Instance.camSize : 2);
        combinedGo = new GameObject[levelData.states.Count];
        for (int i = 0; i < levelData.states.Count; i++)
        {
            var s = levelData.states[i];
            if (s.pos.magnitude > max)
            {
                maxPos = s.pos;
                max = s.pos.magnitude;
            }
            listPos.Add(s.pos);
            var go = Instantiate(blockPrefab, s.pos * 30, Quaternion.Euler(s.rotation + new Vector3(180, 180, 0)), this.transform);
            TestMoveBlock goBlock = go.GetComponent<TestMoveBlock>();
            goBlock.SetColorArrow();
            goBlock.SetLocalScale();
            seq.Append(goBlock.Mesh.material.DOFade(1, timer).OnComplete(() => goBlock.PreMove(s.pos * 30, s.rotation + new Vector3(180, 180, 0), s.pos * 4.05f, s.rotation, i)));
            pool.Add(go);
            combinedGo[i] = goBlock.Mesh.gameObject;
        }
        //seq.OnComplete(() => StartCoroutine(CombineMesh()));
        if (this.levelData.levelIndex >= 4 && UnityEngine.Random.Range(0, 100) < 40)
        {
            Vector3 puzzlePos = new Vector3();
            if (!listPos.Contains(maxPos + Vector3.up))
            {
                puzzlePos = maxPos + Vector3.up;
            }
            else if (!listPos.Contains(maxPos + Vector3.right))
            {
                puzzlePos = maxPos + Vector3.right;
            }
            else if (!listPos.Contains(maxPos + Vector3.down))
            {
                puzzlePos = maxPos + Vector3.down;
            }
            else if (!listPos.Contains(maxPos + Vector3.left))
            {
                puzzlePos = maxPos + Vector3.left;
            }
            else if (!listPos.Contains(maxPos + Vector3.forward))
            {
                puzzlePos = maxPos + Vector3.forward;
            }
            else if (!listPos.Contains(maxPos + Vector3.back))
            {
                puzzlePos = maxPos + Vector3.back;
            }
            var p_go = Instantiate(puzzleRewardPrefab, puzzlePos * 30, Quaternion.identity, this.transform);
            int i = UnityEngine.Random.Range(0, 2);
            p_go.GetComponent<RewardBlock>().Type = i != 0 ? RewardType.Puzzle : RewardType.ImediatedCoin;
            seq.Append(p_go.transform.DOLocalMove(puzzlePos * 4, 0.3f));
            size += 1;
            pool.Add(p_go);
        }
    }



    IEnumerator CombineMesh()
    {
        int i = 3;
        while (i > 0)
        {
            i -= 1;
            yield return new WaitForSeconds(1);
        }
        Debug.Log("OK");

        //StaticBatchingUtility.Combine(combinedGo, this.gameObject);
        meshBaker.AddDeleteGameObjects(combinedGo, null, true);
        meshBaker.Apply();
    }

    public void RandomChangeBlockToReward()
    {
        int n = pool.Count;
        for (int i = UnityEngine.Random.Range(0, n); i < n; i++)
        {
            if (!pool[i].activeSelf)
            {
                continue;
            }
            else
            {
                TestMoveBlock p = pool[i].GetComponent<TestMoveBlock>();
                if (p.IsSelected)
                    continue;
                else if (p != null)
                {
                    p.ChangeTypeOfBlock();
                    return;
                }
            }
        }
    }
}
