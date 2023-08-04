using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class BombController : MonoBehaviour
{
    [Header("Bomb Features")]
    [SerializeField] ParticleSystem bombExplosion;
    [SerializeField] LineRenderer touchToSetDirectionLine;
    [SerializeField] DrawDirectionLine directionLine;
    [SerializeField] Button bombButton;

    Vector3 pos;
    Vector3 oldPos;
    Vector3 direction;
    Vector3 checkBlockDirection;
    float coeff;
    bool isUsing = false;
    RaycastHit hitInfo;

    //List<Vector3> leftTop = new List<Vector3>();
    //List<Vector3> centerTop = new List<Vector3>();
    //List<Vector3> rightTop = new List<Vector3>();
    //List<Vector3> leftBottom = new List<Vector3>();
    //List<Vector3> centerBottom = new List<Vector3>();
    //List<Vector3> rightBottom = new List<Vector3>();
    public List<TestMoveBlock> destroyedBlock = new List<TestMoveBlock>();

    // Start is called before the first frame update
    private void OnEnable()
    {
        //GameManager.Instance.blockPool.canRotate = false;
        coeff = GameManager.Instance.mainCam.GetComponent<Camera>().orthographicSize / 20;
        pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y * coeff, pos.z);
        this.transform.localScale = new Vector3(0.05f * coeff, 0.05f * coeff, 0.05f * coeff);
        this.direction = Vector3.zero;
        touchToSetDirectionLine.SetPosition(0, this.transform.position);
    }

    // Update is called once per frame

    void SetDirection(Vector3 dir)
    {
        Vector3 bombDir = dir;
        bombDir.z = 0;
        if (Physics.Raycast(this.transform.position + bombDir * 3, Vector3.forward, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("BlockChild"))
            {
                //directionLine.PreDraw(hitInfo.point);
                //directionLine.DrawLine(hitInfo.point, hitInfo.normal);
                checkBlockDirection = hitInfo.normal;
            }
        }
    }

    public void ThrowBomb(DrawDirectionLine directionLine)
    {
        directionLine.gameObject.SetActive(false);
        touchToSetDirectionLine.gameObject.SetActive(false);

        ////SetDestroyedList(directionLine);
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

    void SetDestroyedList(DrawDirectionLine directionLine)
    {
        destroyedBlock.Clear();
        destroyedBlock = directionLine.ExploreArea.SetTestMoveBlocks(checkBlockDirection);
    }

    public void DontThrowBomb()
    {
        directionLine.gameObject.SetActive(false);
        touchToSetDirectionLine.gameObject.SetActive(false);
    }

    private void DestroyBlocks(Vector3 targetPos)
    {
        Vector3 p = targetPos;
        p.z = -30;
        bombExplosion.transform.position = p;
        bombExplosion.Play();
        foreach (var block in destroyedBlock)
        {
            GameManager.Instance.countTouchs += 1;
            StartCoroutine(block.UpdateData());
            block.gameObject.SetActive(false);
        }
    }
}
