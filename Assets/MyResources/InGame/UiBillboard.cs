using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiBillboard : MonoBehaviour
{
    Transform camTr;
    private void Start()
    {
        camTr = Camera.main.transform;
    }
    // Update is called once per frame
    void Update ()
    {
        if (camTr == null)
            return;

        transform.LookAt(transform.position + camTr.rotation * Vector3.forward,
                             camTr.rotation * Vector3.up);
    }
}
