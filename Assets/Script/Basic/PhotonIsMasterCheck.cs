using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PhotonIsMasterCheck : MonoBehaviour
{
    public Image _isMaster;
    private void Update()
    {
        _isMaster.gameObject.SetActive(PhotonNetwork.IsMasterClient);
    }
}


