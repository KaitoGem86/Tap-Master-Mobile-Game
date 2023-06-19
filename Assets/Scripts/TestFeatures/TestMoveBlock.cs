using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TestMoveBlock : MonoBehaviour
{
    [SerializeField] Rigidbody blockRb;
    [SerializeField] GameObject startPos;
    [SerializeField] LayerMask layerMask;
    [SerializeField] MeshRenderer mesh;

    private GameObject obstaclePos;
    private float time = 3;
    private int count = 0;
    [SerializeField] private bool isSelected = false;

    public GameObject StartPos { get { return obstaclePos; } }
    public bool IsSelected
    {
        get { return isSelected; }
        set { isSelected = value; }
    }

    // Start is called before the first frame update
    void Start()
    {
        time = 3;
    }

    // Update is called once per frame
    void Update()
    {
        SetObstacle();

        //if (obstaclePos != null)
        //    Debug.Log(obstaclePos.transform.position + " - " + this.gameObject.name + " - " + CheckCanEscape());
        //else
        //    Debug.Log("NULL - " + this.gameObject.name + " - True");

        Move();
    }

    bool CheckCanEscape()
    {
        if (obstaclePos == null)
            return true;
        else
        {
            return obstaclePos.GetComponent<TestObstacleBlock>().isEscaping && obstaclePos.GetComponent<TestObstacleBlock>().isMoving;
        }
    }


    void Move()
    {
        if (isSelected)
        {
            this.startPos.GetComponent<TestObstacleBlock>().isMoving = true;
            blockRb.velocity = blockRb.transform.up * 20;
            if (CheckCanEscape())
            {
                if (count == 0)
                {
                    this.startPos.GetComponent<TestObstacleBlock>().isEscaping = true;
                    StartCoroutine(Escaped());
                    StartCoroutine(UpdateData());
                    count = 1;
                }
            }
            else
            {
                if (Vector3.Distance(this.blockRb.transform.position, obstaclePos.transform.position) <= 4f)
                {
                    blockRb.velocity = Vector3.zero;
                    StartCoroutine(MoveBack());
                }
            }
        }
    }

    void SetObstacle()
    {
        //Debug.Log(this.gameObject.name + this.blockRb.transform.position);
        if (Physics.Raycast(blockRb.transform.position, this.transform.up, out RaycastHit hitInfo))
        {
            //Debug.DrawRay(blockRb.transform.position, this.transform.up * hitInfo.distance, Color.red);
            this.obstaclePos = hitInfo.collider.gameObject.GetComponent<TestMoveBlock>().startPos;
        }
        else
        {
            //Debug.DrawRay(blockRb.transform.position, this.transform.up * 20);
            this.obstaclePos = null;
        }
    }

    IEnumerator MoveBack()
    {
        while (Vector3.Distance(blockRb.transform.position, startPos.transform.position) > 0.05f)
        {
            blockRb.velocity = -blockRb.transform.up * 20;
            yield return new WaitForEndOfFrame();
        }
        blockRb.transform.position = startPos.transform.position;
        blockRb.velocity = Vector3.zero;
        isSelected = false;
        startPos.GetComponent<TestObstacleBlock>().isMoving = false;
    }

    IEnumerator Escaped()
    {
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0;
        this.gameObject.SetActive(false);
    }

    IEnumerator UpdateData()
    {
        GameManager.Instance.count -= 1;
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        GameManager.Instance.coin += 1;
        PlayerPrefs.SetInt("Coin", currenCoin + 1);
        UIManager.instance.SetCoinText();
        UIManager.instance.UpdateBlocksNum();
        if (GameManager.Instance.count == 0)
        {
            Debug.Log("WinGame");
            GameManager.Instance.WinGame();
        }
        yield return null;
    }

    public void SetMaterial(Material material)
    {
        this.mesh.material = material;
    }
}
