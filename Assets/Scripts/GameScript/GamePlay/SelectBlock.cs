using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class SelectBlock : MonoBehaviour
{
    RaycastHit hit;

    private void Update()
    {
        Select();
    }

    void Select()
    {
        //Debug.Log(InputController.instance.CheckSelect());
        //Debug.Log(InputController.instance.Timer);
        //Debug.DrawRay(InputController.instance.GetInputPosition(), Vector3.forward * 60, Color.red, 3);
        if (GameManager.Instance.isOnMenu)
        {
            return;
        }

        if (InputController.instance.CheckSelect() && InputController.instance.Timer < 0.2f)
        {
            GameManager.Instance.blockPool.rb.angularVelocity = Vector3.zero;
            Vector3 selectPos = InputController.instance.GetInputPosition();
            Debug.DrawRay(selectPos, Vector3.forward * 60, Color.red, 3);
            if (Physics.Raycast(selectPos, Vector3.forward * 60, out hit))
            {
                //BlockController selectedBlock = hit.collider.gameObject.GetComponent<BlockController>();
                //selectedBlock.IsMoving = true;
                SoundManager.instance.PlayClickSound();
                TestMoveBlock testMoveBlock = hit.collider.gameObject.GetComponentInParent<TestMoveBlock>();
                testMoveBlock.IsSelected = true;
                RewardBlock rewardBlock = hit.collider.gameObject.GetComponentInParent<RewardBlock>();
                if (rewardBlock.enabled)
                    rewardBlock.OnCollectReward();
                GameManager.Instance.countTouchs -= 1;
                UIManager.instance.UpdateTouchsNum();
            }
        }
    }

    private void LateUpdate()
    {
        if (GameManager.Instance.countTouchs == 0)
        {
            if (GameManager.Instance.countBlocks > 0)
                GameManager.Instance.GameOver();
        }
    }
}