using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawDirectionLine : MonoBehaviour
{
    [Header("Elements of line")]
    [SerializeField] private LineRenderer line;
    [SerializeField] private Transform startPos;
    [SerializeField] private DetermineBombArea exploreArea;

    private readonly int COUNT_OF_VERTEX = 50;
    private Vector3 intermediatePos = Vector3.zero;



    public LineRenderer Line
    {
        get { return line; }
    }

    public DetermineBombArea ExploreArea
    {
        get { return exploreArea; }
    }

    public void PreDraw(Vector3 startPos, Vector3 endPos)
    {
        line.positionCount = COUNT_OF_VERTEX;
        intermediatePos = (startPos + endPos) / 2;
        intermediatePos.y = startPos.y > endPos.y ? startPos.y + 30 : endPos.y + 30;
    }

    public void DrawLine(Vector3 startPos, Vector3 endPos, Vector3 normalVector)
    {
        for (int i = 0; i < COUNT_OF_VERTEX; i++)
        {
            float t = i / (float)COUNT_OF_VERTEX;
            line.SetPosition(i, CalculatePoint(t, startPos, endPos, intermediatePos));
        }
        exploreArea.gameObject.transform.position = endPos;
        Quaternion rotation = Quaternion.LookRotation(normalVector);
        exploreArea.transform.rotation = rotation;
        exploreArea.gameObject.SetActive(true);
    }

    Vector3 CalculatePoint(float t, Vector3 startPos, Vector3 endPos, Vector3 interPos)
    {
        Vector3 returnPos = new Vector3();
        returnPos = (1 - t) * ((1 - t) * startPos + t * interPos) + t * ((1 - t) * interPos + t * endPos);
        return returnPos;
    }

}
