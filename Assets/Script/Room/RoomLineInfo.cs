using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RoomLineInfo : MonoBehaviour
{
    public TextMeshProUGUI _index;
    public TextMeshProUGUI _mode;
    public TextMeshProUGUI _roomName;
    public TextMeshProUGUI _creator;
    public TextMeshProUGUI _playersNumber;
    public TextMeshProUGUI _gameState;

    public GameObject _lockUI;

    RoomInfo _curRoomInfo;

    public void UpdateContents(int index, RoomInfo roomInfo)
    {
        CustomGameRoomData gameRoomData = CustomGameRoomData.GetCustomGameRoomData(roomInfo);
        _index.text = (index + 1).ToString();
        _mode.text = gameRoomData._multiPlayMode.ToString();
        _roomName.text = roomInfo.Name;
        _creator.text = gameRoomData._roomCreator;
        _playersNumber.text = roomInfo.PlayerCount + "/" + roomInfo.MaxPlayers;
        _gameState.text = gameRoomData._roomState.ToString();

        _lockUI.SetActive(gameRoomData._privateRoom);

        _curRoomInfo = roomInfo;
    }

    public InLobbyState _inLobbyState;
    public GameObject _activeLine;

    public void SelectRoomInfo()
    {
        _inLobbyState.SelectionUpdate(_curRoomInfo);
    }

    public void UpdateSelection(RoomInfo selectedRoomInfo)
    {
        _activeLine.SetActive(selectedRoomInfo == _curRoomInfo);
    }

    public void InitSelection()
    {
        _activeLine.SetActive(false);
    }
}


