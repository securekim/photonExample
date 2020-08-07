using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseControl : MonoBehaviour
{
    float rotX = 0.0f;    

    public float speed = 900.0f;

    void Update ()
    {
        float x = Input.GetAxis("Mouse Y");        
        rotX += x * speed * Time.deltaTime;

        rotX = Mathf.Clamp(rotX, -90.0f, 90.0f);

        transform.localEulerAngles = new Vector3(-rotX, 0.0f, 0.0f);       
    }
}
