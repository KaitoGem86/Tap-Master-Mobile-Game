using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour
{
    Vector3 pos;
    Vector3 oldPos;
    Vector3 direction;
    int count = 1;
    float timer = 0;
    float coeff;
    bool isUsing = false;

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
        GameManager.Instance.blockPool.canRotate = false;
        coeff = GameManager.Instance.mainCam.GetComponent<Camera>().orthographicSize / 20;
        pos = this.transform.position;
        this.transform.position = new Vector3(pos.x, pos.y * coeff, pos.z);
        this.transform.localScale = Vector3.zero;
        this.transform.DOScale(new Vector3(0.05f * coeff, 0.05f * coeff, 0.05f * coeff), duration: 0.5f);
        this.direction = Vector3.zero;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && Input.mousePosition.y > -7)
            GameManager.Instance.blockPool.canRotate = true;
        if (Input.GetMouseButtonUp(0) && timer < 0.3f && isUsing)
        {
            //Debug.Log(direction);
            CreateListTargetPos();
            Vector3 targetPos = ChooseTargetPos(direction);
            Debug.Log(targetPos);
            UseBomb(targetPos);
            isUsing = false;
        }
        if (Input.GetMouseButton(0) && InputController.instance.Timer > 0.3f)
        {
            timer = InputController.instance.Timer;
            if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -7)
            {
                StartCoroutine(MoveWithTouch());
            }
            isUsing = false;
        }
        else if (Input.GetMouseButton(0) && InputController.instance.Timer < 0.3f)
        {
            GameManager.Instance.blockPool.canRotate = false;
            timer = InputController.instance.Timer;
            Vector3 newPos = Input.mousePosition;
            if (count == 1)
            {
                oldPos = newPos;
                count = 0;
            }
            direction = newPos - oldPos;
            oldPos = newPos;
            isUsing = true;
        }
    }

    IEnumerator MoveWithTouch()
    {
        while (Input.GetMouseButton(0))
        {
            Vector3 movePos = InputController.instance.GetInputMoveObject();
            GameManager.Instance.blockPool.canRotate = false;
            GameManager.Instance.blockPool.isRotating = false;
            movePos.z = this.transform.position.z;
            this.transform.position = movePos;
            yield return new WaitForEndOfFrame();
        }

        GameManager.Instance.blockPool.canRotate = true;
        this.transform.DOMove(new Vector3(pos.x, pos.y * coeff, pos.z), duration: 0.3f);
        yield return null;
    }

    void UseBomb(Vector3 targetPos)
    {
        destroyedBlock.Clear();
        for (int i = -2; i < 3; i++)
        {
            for (int j = -2; j < 2; j++)
            {
                Debug.DrawRay(new Vector3(targetPos.x + i * 2, targetPos.y + j * 2, -30), new Vector3(0, 0, 1) * 30, Color.blue, 300);
                if (Physics.Raycast(new Vector3(targetPos.x + i * 2, targetPos.y + i * 2, -30), new Vector3(0, 0, 1), out RaycastHit hit))
                {
                    var hitBlock = hit.transform.gameObject.GetComponentInParent<TestMoveBlock>();
                    if (!destroyedBlock.Contains(hitBlock))
                    {
                        destroyedBlock.Add(hitBlock);
                    }
                }
            }
        }
        Debug.Log(destroyedBlock.Count);
        Vector3 direction = targetPos - this.transform.position;
        Debug.DrawRay(this.transform.position, direction, Color.white, 300);
        this.transform.DOMove(targetPos, duration: 0.3f);
        this.transform.DOScale(this.transform.localScale / 3, duration: 0.6f).OnComplete(() =>
        {
            this.transform.position = pos;
            GameManager.Instance.blockPool.canRotate = true;
            DestroyBlocks();
            this.gameObject.SetActive(false);

        });
        this.transform.DORotate(new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z - 30), duration: 0.6f).SetLoops(2, LoopType.Yoyo);
    }

    private void DestroyBlocks()
    {
        foreach (var block in destroyedBlock)
        {
            GameManager.Instance.countTouchs += 1;
            StartCoroutine(block.UpdateData());
            block.gameObject.SetActive(false);
        }
    }

    void CreateListTargetPos()
    {
        leftBottom.Clear();
        rightBottom.Clear();
        centerTop.Clear();
        centerBottom.Clear();
        leftTop.Clear();
        rightTop.Clear();
        GameManager.Instance.blockPool.Rb.angularVelocity = Vector3.zero;
        float width = GameManager.Instance.mainCam.GetComponent<Camera>().orthographicSize;
        for (int i = -20; i < 20; i++)
        {
            for (int j = -20; j < 20; j++)
            {
                if (Physics.Raycast(new Vector3(i * 4, j * 4, -30), new Vector3(0, 0, 1), out RaycastHit hit))
                {
                    Vector3 hitP = hit.point;
                    if (hitP.y < 0)
                    {
                        if (hitP.x < -width / 6)
                        {
                            rightBottom.Add(hitP);
                        }
                        else if (hitP.x >= -width / 6 && hitP.x < width / 6)
                        {
                            centerBottom.Add(hitP);
                        }
                        else if (hitP.x >= width / 6)
                        {
                            leftBottom.Add(hitP);
                        }
                    }
                    else
                    {
                        if (hitP.x < -width / 6)
                        {
                            rightTop.Add(hitP);
                        }
                        else if (hitP.x >= -width / 6 && hitP.x < width / 6)
                        {
                            centerTop.Add(hitP);
                        }
                        else if (hitP.x >= width / 6)
                        {
                            leftTop.Add(hitP);
                        }
                    }
                }
            }
        }
    }

    Vector3 ChooseTargetPos(Vector3 direction)
    {
        float angle = Vector2.Angle(new Vector2(direction.x, direction.y), Vector2.up);
        Vector3 target = new Vector3();
        int targetArea = -1;
        if (angle >= 20 && angle < 90)
        {
            if (direction.x < 0)
            {
                //góc trên trái
                targetArea = 0;
            }
            else
            {
                //góc trên phải
                targetArea = 2;
            }
        }
        else if (angle >= 90 && angle < 150)
        {
            if (direction.x < 0)
            {
                //góc dưới trái
                targetArea = 5;
            }
            else
            {
                //góc dưới phải
                targetArea = 3;
            }
        }
        else if (angle < 20)
        {
            if (direction.x < 0)
            {
                //góc giữa trên
                targetArea = 1;
            }
            else
            {
                //góc giữa dưới
                targetArea = 4;
            }
        }
        else if (angle >= 150)
        {
            if (direction.x < 0)
            {
                //góc giữa dưới
                targetArea = 4;
            }
            else
            {
                //góc giữa trên
                targetArea = 1;
            }
        }
        switch (targetArea)
        {
            case 0:
                if (leftTop.Count != 0)
                {
                    Debug.Log("Left Top");
                    int i = Random.Range(0, leftTop.Count);
                    target = leftTop[i];
                    break;
                }
                goto case 1;
            case 1:
                if (centerTop.Count != 0)
                {
                    Debug.Log("Center Top");

                    int i = Random.Range(0, centerTop.Count);
                    target = centerTop[i];
                    break;
                }
                goto case 2;
            case 2:
                if (rightTop.Count != 0)
                {
                    Debug.Log("Right Top");

                    int i = Random.Range(0, rightTop.Count);
                    target = rightTop[i];
                    break;
                }
                goto case 3;
            case 3:
                if (rightBottom.Count != 0)
                {
                    Debug.Log("Right Bottom");

                    int i = Random.Range(0, rightBottom.Count);
                    target = rightBottom[i];
                    break;
                }
                goto case 4;
            case 4:
                if (centerBottom.Count != 0)
                {
                    Debug.Log("Center Bottom");

                    int i = Random.Range(0, centerBottom.Count);
                    target = centerBottom[i];
                    break;
                }
                goto case 5;
            case 5:
                if (leftBottom.Count != 0)
                {
                    Debug.Log("left Bottom");

                    int i = Random.Range(0, leftBottom.Count);
                    target = leftBottom[i];
                    break;
                }
                goto default;
            default: break;

        }
        return target;
    }
}
