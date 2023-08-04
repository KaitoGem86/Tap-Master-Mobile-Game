using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleController : MonoBehaviour
{
    [Header("Elements of puzzle reward")]
    [SerializeField] GameObject[] pieces;
    [SerializeField] Button collectRewardButton;
    [SerializeField] Image image;

    [Space]
    [Header("Data to save or load")]
    [SerializeField] PuzzleStatus status;

    List<int> notUnlokcPieces = new List<int>();
    bool isCollected;
    int index;
    public int p_index;

    public int Index

    {
        get => index;
        set => index = value;
    }

    public List<int> NotUnlokcPieces
    {
        get => notUnlokcPieces;
    }

    public PuzzleStatus Status
    {
        get { return status; }
        set { status = value; }
    }


    private void OnApplicationQuit()
    {
        SavePiecesStatus();
    }

    public void OnCollectPieces()
    {
        int j = Random.Range(0, notUnlokcPieces.Count);
        int i = notUnlokcPieces[j];
        p_index = i;
        pieces[i].SetActive(false);
        notUnlokcPieces.Remove(i);
        int n = notUnlokcPieces.Count;
        collectRewardButton.interactable = n <= 0;
        this.isCollected = n <= 0;
        SavePiecesStatus();
    }

    public void OnCollectReward()
    {
        UIManager.instance.BlockListPanel.GetComponent<BlockListPanelController>().SpecialSkinList.blockItems[index].SetInteractable(true);
        this.collectRewardButton.interactable = false;
        this.isCollected = true;
    }

    public void SavePiecesStatus()
    {
        for (int i = 0; i < pieces.Length; i++)
        {
            this.status.status[i] = this.pieces[i].activeSelf;
        }
        this.status.isCollected = this.isCollected;
    }


    //load đang chưa ổn
    public void LoadPiecesStatus()
    {
        this.image.sprite = this.status._sprite;
        this.isCollected = this.status.isCollected;
        if (isCollected)
        {
            UIManager.instance.BlockListPanel.GetComponent<BlockListPanelController>().SpecialSkinList.blockItems[index].SetInteractable(true);
            foreach (var p in pieces)
            {
                p.SetActive(false);
            }
            this.collectRewardButton.interactable = false;
            return;
        }
        else if (this.status.status.Count == 0 || this.status.status == null)
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                this.pieces[i].SetActive(true);
                notUnlokcPieces.Add(i);
            }
            this.collectRewardButton.interactable = true;
            //this.rewardBlock.SetInteractable(this.notUnlokcPieces.Count <= 0 || this.notUnlokcPieces == null);
        }
        else
        {
            for (int i = 0; i < pieces.Length; i++)
            {
                this.pieces[i].SetActive(this.status.status[i]);
                if (this.status.status[i])
                    notUnlokcPieces.Add(i);
            }
            this.collectRewardButton.interactable = this.notUnlokcPieces.Count <= 0 || this.notUnlokcPieces == null;
            //this.rewardBlock.SetInteractable(this.notUnlokcPieces.Count <= 0 || this.notUnlokcPieces == null);
        }
    }
}
