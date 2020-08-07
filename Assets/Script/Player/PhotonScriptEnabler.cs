using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhotonScriptEnabler : MonoBehaviour
{
    public PhotonView photonView;

    public GameObject camObj;
    public MonoBehaviour[] monoClasses;

    private void OnEnable()
    {
        if (PhotonNetwork.IsConnected == false)
            return;

        if(photonView == null)
            photonView = GetComponent<PhotonView>();

        for(int i=0;i< monoClasses.Length;++i)
        {
            monoClasses[i].enabled = photonView.IsMine;
        }

        if (photonView.IsMine)
        {
            camObj.SetActive(true);
        }
        else
        {
            Destroy(camObj); // VR과 같은 환경에서는 다른 캐릭터의 카메라를 지워야 하기 때문
        }
    }
}
