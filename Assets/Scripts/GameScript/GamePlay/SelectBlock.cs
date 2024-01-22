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
        if (GameManager.Instance.isOnMenu)
        {
            return;
        }

        if (InputController.instance.CheckSelect() && InputController.instance.Timer < 0.2f)
        {
            GameManager.Instance.blockPool.rb.angularVelocity = Vector3.zero;
            Vector3 selectPos = InputController.instance.GetInputPosition();
            //selectPos.z = -100;
            //Debug.Log(selectPos);
            Debug.DrawRay(selectPos, GameManager.Instance.mainCam.transform.forward * 60, Color.red, 100);
            ParticleController.instance.OnClick();
            if (Physics.Raycast(selectPos, GameManager.Instance.mainCam.transform.forward * 60, out hit))
            {
                SoundManager.instance.PlayClickSound();
                TestMoveBlock testMoveBlock = hit.collider.gameObject.GetComponentInParent<TestMoveBlock>();
                if (testMoveBlock.isMoving)
                    return;
                if (testMoveBlock != null)
                    testMoveBlock.IsSelected = true;
                RewardBlock rewardBlock = hit.collider.gameObject.GetComponentInParent<RewardBlock>();
                if (rewardBlock != null && rewardBlock.enabled)
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