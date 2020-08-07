using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerNetwork : MonoBehaviour, IPunObservable
{
    public PlayerAnim playerAnim;

    public float sideMove;
    public float fbMove;
    public float animSpeed = 10.0f;


    public PhotonView _photonView;

    public Vector3 curPos;
    public Vector3 curScale;
    public Quaternion curRot;

    public float moveSpeed = 5.0f;
    public float rotSpeed = 10.0f;

    bool first = true;

    private void Awake()
    {
        PlayeVisibleManager.instance.AddPlayer(this);
    }

    private void OnDestroy()
    {
        PlayeVisibleManager.instance.RemovePlayer(this);
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(transform.position);
            stream.SendNext(transform.localScale);
            stream.SendNext(transform.rotation);

            stream.SendNext(playerAnim.sideMoveValue);
            stream.SendNext(playerAnim.fbMoveValue);
        }
        else
        {
            curPos = (Vector3)stream.ReceiveNext();
            curScale = (Vector3)stream.ReceiveNext();
            curRot = (Quaternion)stream.ReceiveNext();

            sideMove = (float)stream.ReceiveNext();
            fbMove = (float)stream.ReceiveNext();

            if (first)
            {
                transform.position = curPos;
                transform.localScale = curScale;
                transform.rotation = curRot;

                first = false;
            }
        }
    }

    void Update()
    {
        if (_photonView.IsMine == false)
        {
            transform.position = Vector3.Lerp(transform.position, curPos, Time.deltaTime * moveSpeed);
            transform.localScale = Vector3.Lerp(transform.localScale, curScale, Time.deltaTime * moveSpeed);
            transform.rotation = Quaternion.Lerp(transform.rotation, curRot, Time.deltaTime * rotSpeed);

            playerAnim.sideMoveValue = Mathf.Lerp(playerAnim.sideMoveValue, sideMove, Time.deltaTime * animSpeed);
            playerAnim.fbMoveValue = Mathf.Lerp(playerAnim.fbMoveValue, fbMove, Time.deltaTime * animSpeed);
            playerAnim.UpdateAnimation();
        }
    }
}

