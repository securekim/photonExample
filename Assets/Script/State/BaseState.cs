using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseState : MonoBehaviour
{
    public virtual void InitState()
    {
        gameObject.SetActive(true);
    }

    public virtual void UpdateState()
    {

    }

    public virtual void ReleaseState()
    {
        gameObject.SetActive(false);
    }
}


