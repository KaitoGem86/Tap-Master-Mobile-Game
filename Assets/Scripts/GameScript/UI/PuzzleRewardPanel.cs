using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleRewardPanel : MonoBehaviour
{
    [Header("Puzzles")]
    [SerializeField] List<PuzzleController> puzzles = new List<PuzzleController>();
    [SerializeField] PuzzleController puzzlePrefab;
    [SerializeField] PuzzleStatus[] statuses;
    [SerializeField] GameObject content;


    List<int> notCollectReward = new List<int>();
    float width;

    public List<PuzzleController> Puzzles { get { return puzzles; } }
    public List<int> NotCollectReward { get { return notCollectReward; } }
    public PuzzleStatus[] Statuses { get => statuses; }

    private void OnEnable()
    {
        this.transform.DOMove(UIManager.instance.canvas.transform.position, duration: 0.3f).SetEase(Ease.InOutSine);
    }

    // Start is called before the first frame update
    void Start()
    {
        width = UIManager.instance.canvas.pixelRect.width;

    }


    public void ExitPanel()
    {
        Vector3 r = this.transform.position + Vector3.right * width;
        this.transform.DOMove(r, duration: 0.3f).SetEase(Ease.InSine).OnComplete(Exit);
        GameManager.Instance.blockPool.gameObject.SetActive(true);
    }

    void Exit()
    {
        GameManager.Instance.isOnMenu = false;
        GameManager.Instance.camMoving.CanRotate = true;
        this.gameObject.SetActive(false);
    }

    public void SaveStatus()
    {
        foreach (var p in puzzles)
        {
            p.SavePiecesStatus();
        }
    }

    public void InitPuzzles()
    {
        for (int i = 0; i < statuses.Length; i++)
        {
            var go = Instantiate(puzzlePrefab.gameObject, this.content.transform);
            PuzzleController pu = go.GetComponent<PuzzleController>();
            pu.Status = statuses[i];
            pu.Index = i;
            pu.LoadPiecesStatus();
            puzzles.Add(pu);
            if (pu.NotUnlokcPieces.Count != 0)
                notCollectReward.Add(i);
        }
    }


    public void LoadStatus()
    {
        for (int p = 0; p < puzzles.Count; p++)
        {
            puzzles[p].LoadPiecesStatus();
            if (puzzles[p].NotUnlokcPieces.Count != 0)
                notCollectReward.Add(p);
        }
    }
}
