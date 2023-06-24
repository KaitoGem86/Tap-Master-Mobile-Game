using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseAchievement : MonoBehaviour
{
    public bool isComplete;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        CheckReachAchievement();
    }

    public abstract void InitializeAchievement();
    public virtual void CheckReachAchievement()
    {
        if (isComplete)
            Destroy(gameObject);
    }
}
