using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    [Header("Bomb Features")]
    [SerializeField] ParticleSystem bombExplosion;
    [SerializeField] LineRenderer touchToSetDirectionLine;
    [SerializeField] DrawDirectionLine directionLine;

    Vector3 pos;
    Vector3 oldPos;
    Vector3 direction;
    Vector3 checkBlockDirection;
    float timer = 0;
    float coeff;
    bool isUsing = false;
    RaycastHit hitInfo;

    List<Vector3> leftTop = new List<Vector3>();
    List<Vector3> centerTop = new List<Vector3>();
    List<Vector3> rightTop = new List<Vector3>();
    List<Vector3> leftBottom = new List<Vector3>();
    List<Vector3> centerBottom = new List<Vector3>();
    List<Vector3> rightBottom = new List<Vector3>();
    List<TestMoveBlock> destroyedBlock = new List<TestMoveBlock>();

    // Start is called before the first frame update
    private void OnEnable()
    {
        //GameManager.Instance.blockPool.canRotate = false;
        coeff = GameManager.Instance.mainCam.GetComponent<Camera>().orthographicSize / 20;
        pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y * coeff, pos.z);
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(new Vector3(0.05f * coeff, 0.05f * coeff, 0.05f * coeff), duration: 0.5f);
        this.direction = Vector3.zero;
        timer = 0;

        touchToSetDirectionLine.SetPosition(0, this.transform.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButton(0) && Camera.main.ScreenToWorldPoint(Input.mousePosition).y <= pos.y + this.GetComponent<MeshCollider>().bounds.size.y / 2)
        {
            GameManager.Instance.blockPool.canRotate = false;
            isUsing = true;
            touchToSetDirectionLine.gameObject.SetActive(true);
            directionLine.gameObject.SetActive(true);
            touchToSetDirectionLine.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
            direction = touchToSetDirectionLine.GetPosition(0) - touchToSetDirectionLine.GetPosition(1);
            SetDirection(direction);
        }
        else if (Input.GetMouseButtonUp(0) && isUsing)
        {
            Vector3 t = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            t.z = this.transform.position.z;
            float f = Vector3.Distance(t, this.transform.position);
            if (f < 1f)
                DontThrowBomb();
            else
                ThrowBomb();
            isUsing = false;
        }
        else
        {
            GameManager.Instance.blockPool.canRotate = true;
        }
    }

    void SetDirection(Vector3 dir)
    {
        Vector3 bombDir = dir;
        bombDir.z = 0;
        if (Physics.Raycast(this.transform.position + bombDir * 3, Vector3.forward, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("BlockChild"))
            {
                directionLine.PreDraw(hitInfo.point);
                directionLine.DrawLine(hitInfo.point, hitInfo.normal);
                checkBlockDirection = hitInfo.normal;
            }
        }
    }

    void ThrowBomb()
    {
        directionLine.gameObject.SetActive(false);
        touchToSetDirectionLine.gameObject.SetActive(false);

        SetDestroyedList();
        DG.Tweening.Sequence seq = DOTween.Sequence();
        this.transform.DOScale(this.transform.localScale * 0.2f, duration: 1f);
        Vector3 rotate = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);

        this.transform.DORotate(rotate + new Vector3(Random.Range(0, 360), Random.Range(0, 360), Random.Range(0, 360)), duration: 1f).OnComplete(() => this.transform.rotation = Quaternion.Euler(0, 180, -90));
        int i = 0;
        for (i = 0; i < directionLine.Line.positionCount; i++)
        {
            seq.Append(this.transform.DOMove(directionLine.Line.GetPosition(i), duration: 0.02f));
        }

        seq.OnComplete(() =>
        {
            this.transform.position = pos;
            GameManager.Instance.blockPool.canRotate = true;
            DestroyBlocks(directionLine.Line.GetPosition(i - 1));
            this.gameObject.SetActive(false);
        });
    }

    void SetDestroyedList()
    {
        destroyedBlock.Clear();
        destroyedBlock = directionLine.ExploreArea.SetTestMoveBlocks(checkBlockDirection);
    }

    void DontThrowBomb()
    {
        directionLine.gameObject.SetActive(false);
        touchToSetDirectionLine.gameObject.SetActive(false);
    }

    private void DestroyBlocks(Vector3 targetPos)
    {
        Vector3 p = targetPos;
        p.z = -30;
        bombExplosion.transform.position = p;
        //Debug.Log(this.transform.position);
        bombExplosion.Play();
        //Debug.Log(bombExplosion.transform.position);
        foreach (var block in destroyedBlock)
        {
            GameManager.Instance.countTouchs += 1;
            StartCoroutine(block.UpdateData());
            block.gameObject.SetActive(false);
        }
    }
}
