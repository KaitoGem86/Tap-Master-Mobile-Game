using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;

public class BombImageController : MonoBehaviour
{
    [Header("Bomb UI")]
    [SerializeField] Image bombImage;
    [SerializeField] LineRenderer touchToSetDirectionLine;
    [SerializeField] DrawDirectionLine directionLine;
    [SerializeField] BombController bomb;

    RaycastHit hitInfo;
    bool isUsing = false;
    bool isReadyToThrow = false;
    Vector3 checkBlockDirection;
    List<TestMoveBlock> destroyedBlock = new List<TestMoveBlock>();

    private void OnEnable()
    {
        bombImage.transform.DOScale(Vector3.one, duration: 0.3f);
        touchToSetDirectionLine.SetPosition(0, Camera.main.ScreenToWorldPoint(this.transform.position));
    }

    // Update is called once per frame
    void Update()
    {
        //if (Input.GetMouseButtonDown(0) && CheckMousePosition(Input.mousePosition))
        //{
        //    isUsing = true;
        //}
        //else if (Input.GetMouseButtonUp(0) && Vector3.Distance(Input.mousePosition, UIManager.instance.cancelArea.GetComponent<RectTransform>().position) > UIManager.instance.cancelArea.GetComponent<RectTransform>().rect.width / 2)
        //{
        //    isUsing = false;
        //}

        //if (isUsing)
        //{
        //    GameManager.Instance.blockPool.canRotate = false;
        //    touchToSetDirectionLine.gameObject.SetActive(true);
        //    directionLine.gameObject.SetActive(true);
        //    touchToSetDirectionLine.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //    var direction = touchToSetDirectionLine.GetPosition(0) - touchToSetDirectionLine.GetPosition(1);
        //    SetDirection(direction);
        //    isReadyToThrow = true;
        //}
        //else if (isReadyToThrow)
        //{
        //    StartCoroutine(GameManager.Instance.bombButton.GetComponent<BombButtonController>().CountDown());
        //    Debug.Log("throw");
        //    this.gameObject.SetActive(false);
        //    SetDestroyedList();
        //    this.bomb.destroyedBlock = this.destroyedBlock;
        //    this.bomb.transform.position = this.directionLine.Line.GetPosition(0);
        //    this.bomb.gameObject.SetActive(true);
        //    this.bomb.ThrowBomb(this.directionLine);
        //    UIManager.instance.cancelArea.gameObject.SetActive(false);
        //    GameManager.Instance.blockPool.canRotate = true;
        //    isReadyToThrow = false;
        //}
        if (Input.GetMouseButton(0))
        {
            if (CheckMousePosition(Input.mousePosition) || isUsing)
            {
                isUsing = true;
                isReadyToThrow = true;
                GenerateBombDirection();
            }
        }
        else if (Input.GetMouseButtonUp(0) && isReadyToThrow)
        {
            isUsing = false;
            isReadyToThrow = false;
            if (Vector3.Distance(Input.mousePosition, UIManager.instance.cancelArea.GetComponent<RectTransform>().position) > UIManager.instance.cancelArea.GetComponent<RectTransform>().rect.width / 2)
            {
                ThrowBomb();
            }
            else
                DontThrow();
        }

    }

    void GenerateBombDirection()
    {
        GameManager.Instance.camMoving.CanRotate = false;
        touchToSetDirectionLine.gameObject.SetActive(true);
        directionLine.gameObject.SetActive(true);
        touchToSetDirectionLine.SetPosition(0, Camera.main.ScreenToWorldPoint(this.transform.position));
        touchToSetDirectionLine.SetPosition(1, Camera.main.ScreenToWorldPoint(Input.mousePosition));
        var direction = touchToSetDirectionLine.GetPosition(0) - touchToSetDirectionLine.GetPosition(1);
        SetDirection(direction);
    }

    void ThrowBomb()
    {
        GameManager.Instance.bombButton.GetComponent<BombButtonController>().StartCoroutine(GameManager.Instance.bombButton.GetComponent<BombButtonController>().CountDown(15));
        GameManager.Instance.bombButton.GetComponent<BombButtonController>().RechangeSprite();
        Debug.Log("throw");
        this.gameObject.SetActive(false);
        SetDestroyedList();
        if (destroyedBlock.Count == 0)
            return;
        this.bomb.destroyedBlock = this.destroyedBlock;
        this.bomb.transform.position = this.directionLine.Line.GetPosition(0);
        this.bomb.gameObject.SetActive(true);
        this.bomb.ThrowBomb(this.directionLine);
        UIManager.instance.cancelArea.gameObject.SetActive(false);
        GameManager.Instance.camMoving.CanRotate = true;
    }

    void DontThrow()
    {
        Debug.Log("Dont Throw");
    }


    public void SetActiveElement(bool t)
    {
        this.touchToSetDirectionLine.gameObject.SetActive(t);
        this.directionLine.gameObject.SetActive(t);
    }

    bool CheckMousePosition(Vector3 direction)
    {
        Vector2 rect = bombImage.GetComponent<RectTransform>().position;
        Vector2 size = bombImage.GetComponent<RectTransform>().rect.size;
        return direction.x < rect.x + size.x / 2 && direction.x > rect.x - size.x / 2 && direction.y < rect.y + size.y / 2 && direction.y > rect.y - size.y / 2;
    }

    void SetDirection(Vector3 dir)
    {
        Vector3 bombDir = dir;
        //bombDir.z = 0;
        if (Physics.Raycast(Camera.main.ScreenToWorldPoint(this.transform.position) + bombDir * 3, Camera.main.transform.forward, out hitInfo))
        {
            if (hitInfo.collider.CompareTag("BlockChild"))
            {
                directionLine.PreDraw(Camera.main.ScreenToWorldPoint(this.transform.position), hitInfo.point);
                directionLine.DrawLine(Camera.main.ScreenToWorldPoint(this.transform.position), hitInfo.point, hitInfo.normal);
                checkBlockDirection = hitInfo.normal;
            }
        }
    }

    void SetDestroyedList()
    {
        destroyedBlock.Clear();
        destroyedBlock = directionLine.ExploreArea.SetTestMoveBlocks(checkBlockDirection);
    }
}
