using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DetermineBombArea : MonoBehaviour
{
    [SerializeField] private SpriteRenderer area;
    [SerializeField] List<Transform> checkPoints;

    private void OnEnable()
    {
        this.transform.localScale = GameManager.Instance.mainCam.GetComponent<Camera>().orthographicSize / 120 * Vector3.one;
    }

    public List<TestMoveBlock> SetTestMoveBlocks(Vector3 dir)
    {
        List<TestMoveBlock> testMoveBlocks = new List<TestMoveBlock>();
        foreach (var checkPoint in checkPoints)
        {
            Vector3 p = checkPoint.position;
            p += dir * 10;
            if (Physics.Raycast(p, -dir, out RaycastHit hit))
            {
                if (!testMoveBlocks.Contains(hit.collider.GetComponentInParent<TestMoveBlock>()))
                {
                    Debug.DrawRay(p, -dir * hit.distance, Color.yellow, 300);
                    testMoveBlocks.Add(hit.collider.GetComponentInParent<TestMoveBlock>());
                }
                else
                {
                    Debug.DrawRay(p, -dir * 10, Color.white, 300);
                }
            }
        }
        return testMoveBlocks;
    }
}
