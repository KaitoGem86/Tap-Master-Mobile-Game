using UnityEngine;

public class TestBlock : MonoBehaviour
{
    public bool isTested = false;
    private float time = 0;
    private float dis = 0;
    private Vector3 startPos;

    [SerializeField] Rigidbody rb;

    private void Start()
    {
        startPos = transform.position;
        Debug.Log(this.transform.position);
    }


    private void Update()
    {
        rb.velocity = Vector3.up;
        time += Time.deltaTime;
        dis = time * rb.velocity.magnitude;
        if (time > 5)
        {
            Debug.Log(dis);
            Debug.Log(this.transform.position);
            return;
        }

    }
}
