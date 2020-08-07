using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class SelectRoomUi : MonoBehaviour
{
    public TextMeshProUGUI _roomName;
    public TextMeshProUGUI _gameMode;
    public TextMeshProUGUI _playerCount;

    public Toggle _togglePrivate;

    public GameObject _passwordWindow;
    public TMP_InputField _pwInputField;

    public TextMeshProUGUI _uiCreatorName;
    public TextMeshProUGUI _roomState;
    public TextMeshProUGUI _sceneName;

    string _targetPassword = "";

    public void InitRoomInfo()
    {
        _roomName.text = "";

        _gameMode.text = "";
        _playerCount.text = "";

        _togglePrivate.isOn = false;
        _passwordWindow.SetActive(false);
        _targetPassword = "";

        _uiCreatorName.text = "";

        _roomState.text = "";
        _sceneName.text = "";
    }

    public void UpdateRoomInfo(RoomInfo roomInfo, int playerCount)
    {
        _roomName.text = roomInfo.Name;

        CustomGameRoomData gameRoomData = CustomGameRoomData.GetCustomGameRoomData(roomInfo);
        
        _gameMode.text = gameRoomData._multiPlayMode.ToString();
        _playerCount.text = playerCount + "/" + roomInfo.MaxPlayers;

        _togglePrivate.isOn = gameRoomData._privateRoom;
        _passwordWindow.SetActive(_togglePrivate.isOn);
        _targetPassword = gameRoomData._password;

        _uiCreatorName.text = gameRoomData._roomCreator;
        
        _roomState.text = gameRoomData._roomState.ToString();
        _sceneName.text = gameRoomData._sceneName;
    }

    public void JoinThisRoom()
    {
        if (_togglePrivate.isOn)
        {
            if (_targetPassword != _pwInputField.text)
            {
                Debug.Log("비밀번호가 틀렸습니다. 다시 입력하세요.");
                return;
            }
        }

        PhotonNetwork.JoinRoom(_roomName.text);
    }

    public void JoinGame()
    {
        PhotonNetwork.CurrentRoom.SetCustomProperties(
                    new Hashtable() { { PropertyKey.RoomState, (int)RoomState.InGame } });
                
        PhotonNetwork.LoadLevel(_sceneName.text);
    }
}
