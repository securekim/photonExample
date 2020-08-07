using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PhotonStateUpdateText : MonoBehaviour
{
    public TextMeshProUGUI _punState;

    void Update()
    {
        _punState.text = PhotonNetwork.NetworkClientState.ToString();
    }
}



