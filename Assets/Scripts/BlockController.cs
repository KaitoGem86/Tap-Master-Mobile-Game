using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class BlockController : MonoBehaviour
{

    [SerializeField] private Vector3 direction;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private BoxCollider col;

    private GameObject cam;
    private float speed = 20;
    //private int count = 0;
    private Vector3 pos;
    private bool canMove = false;
    private bool canEscape = false;
    private Vector3 startPos;
    private Vector3 obstaclePos;
    private RaycastHit hit;
    private float distance;
    private float time = 5;
    private bool isUpdateBlock = true;

    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    public bool CanEscape
    {
        get { return canEscape; }
    }
    // Start is called before the first frame update
    void Start()
    {
        isUpdateBlock = true;
        this.cam = GameManager.Instance.mainCam;
        startPos = transform.position;
        pos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        canEscape = CheckCanEscape();
        if (canMove)
        {
            Move();
            if (canEscape)
            {
                if (isUpdateBlock)
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
                    isUpdateBlock = false;
                }
            }
            else
            {
                Debug.Log("Failed in Move");
            }
        }
        pos = transform.position;
        CheckActiveBlock();
    }

    private void Move()
    {
        rb.velocity = transform.up * speed;
    }


    private void OnTriggerEnter(Collider collision)
    {
        CanMove = false;
        this.ResetObstaclePos();
        if (collision.gameObject.GetComponent<BlockController>().rb.velocity == Vector3.zero)
            collision.gameObject.GetComponent<BlockController>().ResetObstaclePos();

    }


    private void ResetObstaclePos()
    {
        this.transform.position = startPos;
        rb.velocity = Vector3.zero;
    }

    //IEnumerator ThisResetPos()
    //{
    //    Debug.Log(distance);
    //    while (Vector3.Distance(obstaclePos, this.transform.position) < distance)
    //    {
    //        rb.velocity = -transform.up;
    //    }
    //    rb.velocity = Vector3.zero;
    //    if (Vector3.Distance(obstaclePos, this.transform.position) >= distance)
    //    {
    //        //Vector3 pos = new Vector3();
    //        //pos = (this.transform.position - obstaclePos).normalized * distance + obstaclePos;
    //        this.transform.position = pos;
    //    }
    //    yield break;
    //    //count = 0;
    //}

    private void CheckActiveBlock()
    {
        if (rb.velocity.magnitude > 0)
        {
            if (time > 0)
            {
                time -= Time.deltaTime;
            }
            else
            {
                this.gameObject.SetActive(false);
            }
            if (hit.collider != null)
                startPos = hit.transform.position + (this.transform.position - hit.transform.position).normalized * distance;
        }
        else
        {
            time = 2;
            startPos = this.transform.position;
        }
    }

    bool CheckCanEscape()
    {
        if (Physics.Raycast(this.transform.position, transform.up * 30, out hit))
        {
            if (rb.velocity.magnitude == 0)
            {
                distance = hit.distance + 2f;
            }
            return hit.transform.GetComponent<BlockController>().CanEscape && hit.rigidbody.velocity != Vector3.zero;
        }
        return true;
    }

}
