using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InLobbyState : BaseState
{
    public CreateRoomUi _createRoomUi;
    public RoomLineInfo[] _roomLineInfo;
    public SelectRoomUi _selectRoomUi;

    public override void InitState()
    {
        gameObject.SetActive(true);

        _createRoomUi.InitCreateRoomUi();

        PunNetworkManager.instance.updateRoomList += UpdateRoomList;

        UpdateRoomList(PunNetworkManager.instance._roomList);
    }
    

    public override void ReleaseState()
    {
        gameObject.SetActive(false);

        InitSelectUi();

        PunNetworkManager.instance.updateRoomList -= UpdateRoomList;
    }

    void UpdateRoomList(List<RoomInfo> roomList)
    {
        //for (int i = 0; i < roomList.Count; ++i)
        //{
        //    string t = "";
        //    t += roomList[i].CustomProperties[PropertyKey.RoomCreator] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.SceneName] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.GameMode] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.MaxExpectedPlayer] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.IsPrivateRoom] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.Password] + "\n";
        //    t += roomList[i].CustomProperties[PropertyKey.RoomState] + "\n";
        //
        //    Debug.Log(i + " == " + t);
        //}

        int max = Mathf.Min(4, roomList.Count);
        for(int i=0; i< _roomLineInfo.Length; ++i)
        {
            _roomLineInfo[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < max; ++i)
        {
            _roomLineInfo[i].gameObject.SetActive(true);

            _roomLineInfo[i].UpdateContents(i, roomList[i]);
        }
    }


    public RoomInfo _selectRoomInfo;

    public void SelectionUpdate(RoomInfo selectedRoomInfo)
    {
        _selectRoomInfo = selectedRoomInfo;

        for (int i = 0; i < _roomLineInfo.Length; ++i)
        {
            _roomLineInfo[i].UpdateSelection(_selectRoomInfo);
        }

        _selectRoomUi.UpdateRoomInfo(_selectRoomInfo, _selectRoomInfo.PlayerCount);
    }

    void InitSelectUi()
    {
        _selectRoomInfo = null;

        for (int i = 0; i < _roomLineInfo.Length; ++i)
        {
            _roomLineInfo[i].InitSelection();
        }
    }
}


