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
    [SerializeField] MeshFilter meshFilter;
    //[SerializeField] TrailRenderer trail;
    [SerializeField] SpriteRenderer[] arrows;
    [SerializeField] Material escapeMaterial;
    [SerializeField] Material blockedMaterial;
    [SerializeField] private bool isSelected = false;

    private GameObject obstaclePos;
    private float time = 3;
    private int count = 0;
    private bool isHitted = true;
    private Material currentMaterial;
    private List<GameObject> obstacles = new List<GameObject>();

    public bool isMoving = false;

    public GameObject StartPos { get { return obstaclePos; } }
    public MeshFilter MeshFilter { get { return meshFilter; } }
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

    //Hiệu ứng trail, tạm thời bỏ đi hiệu ứng này
    //public TrailRenderer Trail
    //{
    //    get => trail;
    //    set => trail = value;
    //}

    // Start is called before the first frame update
    void Start()
    {
        this.currentMaterial = this.mesh.material;
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
            isMoving = true;
            this.startPos.GetComponent<TestObstacleBlock>().isMoving = true;
            if (CheckCanEscape())
            {
                Debug.Log("OK");
                blockRb.velocity += blockRb.transform.up * 5;
                //Time.timeScale = 0f;
                if (count == 0)
                {
                    //trail.gameObject.SetActive(true);
                    this.startPos.GetComponent<TestObstacleBlock>().isEscaping = true;
                    StartCoroutine(Escaped());
                    StartCoroutine(UpdateData());
                    count = 1;
                }
            }
            else
            {
                isSelected = false;
                this.mesh.material = blockedMaterial;
                SetActiveArrow(false);
                float duration = Vector3.Distance(this.transform.position, this.obstaclePos.transform.position - this.transform.up * 4);
                //Debug.Log(Vector3.Distance(this.transform.position, this.obstaclePos.transform.position - this.transform.up * 4));
                duration = duration > 20 ? 2 : duration / 10;
                if (duration < 0.2f)
                    duration = 0.2f;
                var t = this.blockRb.transform.DOLocalMove(
                    this.blockRb.transform.InverseTransformPoint(this.obstaclePos.transform.position - this.transform.up * 3.9f), duration: duration
                    ).SetLoops(2, LoopType.Yoyo).OnComplete(() =>
                    {
                        isHitted = true;
                        isMoving = false;
                        SetActiveArrow(true);
                        this.blockRb.velocity = Vector3.zero;
                    });
                t.OnStart(() => StartCoroutine(FindObstacle(this.blockRb.transform.up)));
                t.OnUpdate(() =>
                {
                    if (t.ElapsedPercentage() >= 0.5f)
                    {
                        this.mesh.material = currentMaterial;
                        SoundManager.instance.PlayFailedSound();
                        if (isHitted)
                        {
                            if (GameManager.Instance.allowedVibrating)
                            {
                                //Debug.Log("VIbrate");
                                VibrationManager.Vibrate(30);
                            }
                            isHitted = false;
                            HitBlock(this.transform.up);
                        }
                    }
                });
            }
        }
    }

    void SetObstacle()
    {
        if (Physics.Raycast(blockRb.transform.position, this.transform.up, out RaycastHit hitInfo))
        {
            if (hitInfo.collider.gameObject.CompareTag("BlockChild"))
            {
                this.obstaclePos = hitInfo.collider.gameObject.GetComponentInParent<TestMoveBlock>().startPos;
                //Debug.DrawRay(blockRb.transform.position, this.transform.up * hitInfo.distance, Color.yellow);
            }
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

    IEnumerator Escaped()
    {
        this.mesh.material = escapeMaterial;
        this.GetComponent<BoxCollider>().enabled = false;
        SetActiveArrow(false);
        while (time > 0)
        {
            time -= Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        time = 0;
        this.gameObject.SetActive(false);
    }

    IEnumerator FindObstacle(Vector3 direction)
    {
        GameObject obstacle = this.obstaclePos.GetComponent<TestObstacleBlock>().obstacleBlock;
        obstacles.Clear();
        while (true)
        {
            obstacles.Add(obstacle);
            if (Physics.Raycast(obstacle.transform.position + 2 * direction, direction, out RaycastHit hit, 1))
            {
                obstacle = hit.collider.gameObject;
                Debug.DrawRay(obstacle.transform.position + 2 * direction, direction * 30, Color.red, 300);
            }
            else
                break;
        }
        yield return null;
    }

    void HitBlock(Vector3 direction)
    {
        DG.Tweening.Sequence seq = DOTween.Sequence();
        for (int i = 0; i < obstacles.Count; i++)
        {
            GameObject startOPos = obstacles[i];
            Debug.DrawRay(obstacles[i].transform.TransformPoint(obstacles[i].transform.localPosition), direction * 30, Color.white, 300);
            var t = obstacles[i].transform.DOLocalMove(obstacles[i].transform.localPosition + obstacles[i].transform.InverseTransformDirection(direction.normalized * 0.8f),
                                                       duration: 0.1f).SetLoops(2, LoopType.Yoyo).OnStart(() =>
                                                       {
                                                           //startOPos.GetComponent<MeshRenderer>().material = this.blockedMaterial;
                                                           //startOPos.GetComponentInParent<TestMoveBlock>().SetActiveArrow(false);
                                                       });
            t.OnComplete(() =>
            {
                //startOPos.GetComponent<MeshRenderer>().material = this.currentMaterial;
                //startOPos.GetComponentInParent<TestMoveBlock>().SetActiveArrow(true);
            });
            seq.Append(t);
        }
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
            if (i < 100 && GameManager.Instance.countBlocks > 1)
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
        this.arrows[0].DOFade(1, 1.5f);
        this.arrows[1].DOFade(1, 1.5f);
        this.arrows[2].DOFade(1, 1.5f);
        this.arrows[3].DOFade(1, 1.5f);
        this.transform.DOScale(Vector3.one, duration: 1.5f);
        this.transform.DOLocalMove(endPos, duration: 1.5f).SetEase(Ease.InSine);
        this.transform.DOLocalRotate(endAngle, duration: 1.5f).SetEase(Ease.InSine);
    }

    public void SetColorArrow()
    {
        Color a = Color.white;
        a.a = 0;
        this.arrows[0].color = a;
        this.arrows[1].color = a;
        this.arrows[2].color = a;
        this.arrows[3].color = a;
    }

    public void SetActiveArrow(bool value)
    {
        this.arrows[0].gameObject.SetActive(value);
        this.arrows[1].gameObject.SetActive(value);
        this.arrows[2].gameObject.SetActive(value);
        this.arrows[3].gameObject.SetActive(value);
    }

    public void SetLocalScale()
    {
        this.transform.localScale = Vector3.zero;
    }

    public void SetMaterial(Material material)
    {
        this.currentMaterial = material;
        this.mesh.material = material;
    }

    public void ChangeTypeOfBlock()
    {
        DG.Tweening.Sequence sequence = DOTween.Sequence();
        this.arrows[0].gameObject.SetActive(false);
        this.arrows[1].gameObject.SetActive(false);
        this.arrows[2].gameObject.SetActive(false);
        this.arrows[3].gameObject.SetActive(false);
        sequence.Append(this.mesh.material.DOFade(0, 0.5f));
        this.mesh.material = rewardBlock.Material;
        sequence.Append(this.mesh.material.DOFade(0.5f, 0.5f));
        this.enabled = false;
        rewardBlock.enabled = true;
    }
}
