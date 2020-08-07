using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UiStateManager : MonoBehaviourPunCallbacks
{
    public enum NetworkState
    {
        ReadyForConnect = 0,
        InLobby,
        InRoom,
    }

    NetworkState _networkState = NetworkState.ReadyForConnect;

    public BaseState[] _photonStates;
    public NetworkState networkState
    {
        get { return _networkState; }
        set
        {
            if (_networkState != value)
            {
                _photonStates[(int)_networkState].ReleaseState();

                _networkState = value;

                _photonStates[(int)_networkState].InitState();
            }
        }
    }    

    void Awake()
    {
        for (int i = 0; i < _photonStates.Length; ++i)
        {
            _photonStates[i].gameObject.SetActive(false);
        }

        _photonStates[0].InitState();
    }

    void Update()
    {
        _photonStates[(int)_networkState].UpdateState();
    }

    // CALL BACK ====================== 
    public override void OnConnectedToMaster()
    {
        networkState = NetworkState.InLobby;
        base.OnConnectedToMaster();
    }

    public override void OnJoinedRoom()
    {
        networkState = NetworkState.InRoom;
        base.OnJoinedRoom();
    }

    public override void OnLeftRoom()
    {
        networkState = NetworkState.InLobby;
        base.OnLeftRoom();
    }
}

