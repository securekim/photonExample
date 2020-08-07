using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ReadyForConnectState : BaseState
{
    public TMP_InputField _nickName;

    public override void InitState()
    {
        gameObject.SetActive(true);

        InitNickName();
    }

    public override void ReleaseState()
    {
        gameObject.SetActive(false);
    }

    void InitNickName()
    {
        _nickName.text = PlayerPrefs.GetString("NickName", "Unnamed");
    }

    public void ConnectPhoton()
    {
        SetNewNickName();

        PunNetworkManager.instance.Connect();
    }

    public void SetNewNickName()
    {
        PhotonNetwork.NickName = _nickName.text;
        
        PlayerPrefs.SetString("NickName", PhotonNetwork.NickName);

        Debug.Log("Nick Name ==== " + PhotonNetwork.NickName);
    }
}
