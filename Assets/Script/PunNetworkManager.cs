using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PunNetworkManager : MonoBehaviourPunCallbacks
{
    public static PunNetworkManager instance { get; private set; }

    private void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;

        DontDestroyOnLoad(gameObject);
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }    

    public override void OnConnected()
    {
        Debug.Log("포톤 서버 접속 되었습니다.");
        base.OnConnected();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("마스터 서버 접속 되었습니다.");

        base.OnConnectedToMaster();

        PhotonNetwork.JoinLobby();
    }




    public void Disconnect()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("접속이 종료되었습니다. : " + cause.ToString());
        base.OnDisconnected(cause);
    }


    public void JoinLobby()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        Debug.Log("로비에 접속 되었습니다.");
        
        //Debug.Log(PhotonNetwork.CountOfPlayers);
        //Debug.Log(PhotonNetwork.CountOfPlayersOnMaster);
        //Debug.Log(PhotonNetwork.CountOfPlayersInRooms);

        base.OnJoinedLobby();
    }


    public void CreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true; // 리스트에 보이지는 않고, 방 이름이 정확하면 접속 가능
        roomOptions.IsOpen = true; // 방이 오픈되어 있어야 사용자 접속 가능. false일 경우 접속 불가.
        // roomOptions.CustomRoomProperties  // 방에 필요한 다른 정보 필요시 사용 (방 생성자, 스테이지 인덱스.. 등등)

        PhotonNetwork.CreateRoom("my room name", roomOptions);
    }

    public void JoinOrCreateRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
            return;

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true; // 리스트에 보이지는 않고, 방 이름이 정확하면 접속 가능
        roomOptions.IsOpen = true; // 방이 오픈되어 있어야 사용자 접속 가능. false일 경우 접속 불가.
        // roomOptions.CustomRoomProperties  // 방에 필요한 다른 정보 필요시 사용 (방 생성자, 스테이지 인덱스.. 등등)

        PhotonNetwork.JoinOrCreateRoom("my room name", roomOptions, null);
    }

    public override void OnCreatedRoom()
    {
        Debug.Log("방이 생성되었습니다.");
        base.OnCreatedRoom();
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("방이 생성을 실패하였습니다.");

        base.OnCreateRoomFailed(returnCode, message);
    }

    public void LeaveRoom()
    {
        if (PhotonNetwork.NetworkClientState != ClientState.Joined)
            return;

        PhotonNetwork.LeaveRoom();        
    }

    public override void OnLeftRoom()
    {
        Debug.Log("방을 나왔습니다.");
        base.OnLeftRoom();
    }




    public void JoinRoom()
    {
        if (PhotonNetwork.NetworkClientState == ClientState.Joined)
            return;

        PhotonNetwork.JoinRandomRoom();

        //PhotonNetwork.JoinRoom("roomName"); // 방을 지정해서 참여하는 경우
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("방을 참여하였습니다.");
        base.OnJoinedRoom();

        PhotonNetwork.LocalPlayer.InitHp();

        //if(PhotonNetwork.InRoom)
        //{
        //    Debug.Log(PhotonNetwork.CurrentRoom.Name);        
        //    Debug.Log(PhotonNetwork.CurrentRoom.PlayerCount);
        //    Debug.Log(PhotonNetwork.CurrentRoom.MaxPlayers);
        //    Debug.Log(PhotonNetwork.CurrentRoom.MasterClientId);
        //    foreach(Player player in PhotonNetwork.PlayerList)
        //    {
        //        Debug.LogWarning("User : " + player.NickName);
        //    }
        //}
    }

    //public override void OnRoomListUpdate(List<RoomInfo> roomList)
    //{
    //    int roomCount = roomList.Count;
    //    for (int i = 0; i < roomCount; i++)
    //    {
    //        if (!roomList[i].RemovedFromList)
    //        {
    //            if (!myList.Contains(roomList[i])) myList.Add(roomList[i]);
    //            else myList[myList.IndexOf(roomList[i])] = roomList[i];
    //        }
    //        else if (myList.IndexOf(roomList[i]) != -1) myList.RemoveAt(myList.IndexOf(roomList[i]));
    //    }
    //    MyListRenewal();
    //}

    public List<RoomInfo> _roomList = new List<RoomInfo>();
    public System.Action<List<RoomInfo>> updateRoomList;

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        // 새로 갱신된 방 리스트 순회
        int roomCount = roomList.Count;
        for (int i = 0; i < roomCount; ++i)
        {
            var curRoomInfo = roomList[i];

            // 기존 목록에 있는지 확인
            int index = _roomList.IndexOf(curRoomInfo);
            if (index == -1) // 목록에 없음. 
            {
                // 지우는 방이 아니라면 추가
                if (curRoomInfo.RemovedFromList == false)
                    _roomList.Add(curRoomInfo);
            }
            else // 목록에 있음 -> 제거 혹은 갱신
            {
                if (curRoomInfo.RemovedFromList == true)
                    _roomList.RemoveAt(index);
                else
                    _roomList[index] = curRoomInfo;
            }
        }

        if (updateRoomList != null)
            updateRoomList(_roomList);
    

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
    }

    public System.Action onMasterClientSwitched;
    public override void OnMasterClientSwitched(Player newMasterClient)
    {
        base.OnMasterClientSwitched(newMasterClient);

        Debug.Log("마스터 클라이언트가 변경되었습니다.");

        if (onMasterClientSwitched != null)
            onMasterClientSwitched();
    }

    public System.Action onPlayerEnteredRoom;

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);

        Debug.Log("새로운 플레이어가 들어왔습니다.");

        newPlayer.InitHp();

        if (onPlayerEnteredRoom != null)
            onPlayerEnteredRoom();
    }

    public System.Action onPlayerLeftRoom;
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);

        Debug.Log("다른 플레이어가 나갔습니다.");

        if (onPlayerLeftRoom != null)
            onPlayerLeftRoom();
    }



    public System.Action onPlayerPropertiesUpdate;
    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        base.OnPlayerPropertiesUpdate(targetPlayer, changedProps);

        Debug.Log("플레이어 속성정보가 갱신되었습니다.");

        if(onPlayerPropertiesUpdate != null)
            onPlayerPropertiesUpdate();
    }
}
