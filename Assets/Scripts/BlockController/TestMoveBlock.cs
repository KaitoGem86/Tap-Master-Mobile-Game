using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using Unity.VisualScripting;
using UnityEngine;

public class TestMoveBlock : MonoBehaviour
{
    [SerializeField] Rigidbody blockRb;
    [SerializeField] GameObject startPos;
    [SerializeField] LayerMask layerMask;
    [SerializeField] RewardBlock rewardBlock;
    [SerializeField] MeshRenderer mesh;
    [SerializeField] TrailRenderer trail;

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

    public MeshRenderer Mesh
    {
        get => mesh;
        set => mesh = value;
    }

    public TrailRenderer Trail
    {
        get => trail;
        set => trail = value;
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
                    trail.gameObject.SetActive(true);
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
                    SoundManager.instance.PlayFailedSound();
                    if (GameManager.Instance.allowedVibrating)
                    {
                        Debug.Log("Vibrate");
                        VibrationManager.Vibrate(30);
                    }
                    StartCoroutine(MoveBack());
                }
            }
        }
    }

    void SetObstacle()
    {
        if (Physics.Raycast(blockRb.transform.position, this.transform.up, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.CompareTag("BlockChild"))
                this.obstaclePos = hitInfo.collider.gameObject.GetComponentInParent<TestMoveBlock>().startPos;
            else if (hitInfo.collider.gameObject.CompareTag("Bomb"))
            {
                this.obstaclePos = null;
            }
            else
                this.obstaclePos = hitInfo.collider.gameObject.GetComponent<TestMoveBlock>().startPos;
        }
        else
        {
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

    public IEnumerator UpdateData()
    {
        GameManager.Instance.countBlocks -= 1;
        int currenCoin = PlayerPrefs.GetInt("Coin", 0);
        GameManager.Instance.coin += 1;
        PlayerPrefs.SetInt("Coin", currenCoin + 1);
        UIManager.instance.SetCoinText();
        UIManager.instance.UpdateBlocksNum();
        if (GameManager.Instance.countBlocks == GameManager.Instance.blockPool.pool.Count / 2 && GameManager.Instance.countBlocks != 0)
        {
            int i = Random.Range(0, 100);
            if (i < 30)
                GameManager.Instance.blockPool.RandomChangeBlockToReward();
        }
        if (GameManager.Instance.countBlocks == 0)
        {
            GameManager.Instance.WinGame();
        }
        yield return null;
    }

    public void PreMove(Vector3 startPos, Vector3 startAngle, Vector3 endPos, Vector3 endAngle, int i)
    {
        //this.transform.position = startPos;
        //this.transform.rotation = Quaternion.Euler(startAngle);
        //this.mesh.material.DOFade(0, 0.0001f).OnComplete(() =>
        //{
        //this.mesh.material.DOFade(1, 1).OnComplete(() =>
        //{
        this.transform.DOLocalMove(endPos, duration: 1.5f).SetEase(Ease.InSine);
        this.transform.DOLocalRotate(endAngle, duration: 1.5f).SetEase(Ease.InSine);
        //});

        //});
    }

    public void SetMaterial(Material material)
    {
        this.mesh.material = material;
    }

    public void ChangeTypeOfBlock()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        sequence.Append(this.mesh.material.DOFade(0, 0.5f));
        this.mesh.material = rewardBlock.Material;
        sequence.Append(this.mesh.material.DOFade(1, 0.5f));
        this.enabled = false;
        rewardBlock.enabled = true;
    }
}
