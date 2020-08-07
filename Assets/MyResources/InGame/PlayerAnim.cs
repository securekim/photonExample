using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnim : MonoBehaviour
{
    public Animator anim;

    public float sideMoveValue = 0.0f;
    public float fbMoveValue = 0.0f;
    public float speed = 1.0f;


	void Update ()
    {
        float newValue = Input.GetAxis("Horizontal");
        sideMoveValue = newValue;
        sideMoveValue = Mathf.Clamp(sideMoveValue, -1.0f, 1.0f);

        newValue = Input.GetAxis("Vertical");
        fbMoveValue = newValue;
        fbMoveValue = Mathf.Clamp(fbMoveValue, -1.0f, 1.0f);

        UpdateAnimation();
    }

    void PlaySideMove()
    {
        anim.SetFloat("sideMove", sideMoveValue);
    }    

    void PlayFBMove()
    {
        anim.SetFloat("fbMove", fbMoveValue);
    }

    public void UpdateAnimation()
    {
        PlaySideMove();
        PlayFBMove();
    }
}
