using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum MultiPlayMode
{
    Free = 0,
    Team
}

public enum RoomState
{
    Wait = 0,
    InTest,
}

public class CreateRoomUi : MonoBehaviour
{
    [Header("룸 생성 옵션")]
    public TextMeshProUGUI _roomName;
    public Image[] _modeBg;
    public TextMeshProUGUI _maxPlayerCountText;
    public TextMeshProUGUI _uiCreatorName;    

    public Toggle _togglePrivate;

    public GameObject _passwordWindow;
    public TMP_InputField _pwInputField;

    // 멤버 변수
    MultiPlayMode _multiPlayMode;
    byte _maxPlayerCount;


    public void InitCreateRoomUi()
    {
        _roomName.text = "";
        Free4All();

        _togglePrivate.isOn = false;
        ClickPrivateCheck(false);

        _uiCreatorName.text = PhotonNetwork.NickName;
    }

    public void Free4All()
    {
        _modeBg[0].color = Color.white;
        _modeBg[1].color = new Color(0.5f, 0.5f, 0.5f);

        _multiPlayMode = MultiPlayMode.Free;

        _maxPlayerCount = MaxPlayerCount(_multiPlayMode);
        _maxPlayerCountText.text = _maxPlayerCount.ToString();
    }

    public void TeamDeath()
    {
        _modeBg[0].color = new Color(0.5f, 0.5f, 0.5f);
        _modeBg[1].color = Color.white;

        _multiPlayMode = MultiPlayMode.Team;

        _maxPlayerCount = MaxPlayerCount(_multiPlayMode);
        _maxPlayerCountText.text = _maxPlayerCount.ToString();
    }

    byte MaxPlayerCount(MultiPlayMode mode)
    {
        if (mode == MultiPlayMode.Free)
            return 8;

        return 6;
    }

    public void ClickPrivateCheck(bool state)
    {        
        _passwordWindow.SetActive(_togglePrivate.isOn);
    }

    public void CreateRoom()
    {        
        CreateCustomRoom(_togglePrivate.isOn, _pwInputField.text);
    }


    void CreateCustomRoom(bool isPrivateRoom, string password)
    {
        if (isPrivateRoom == false)
            password = "";

        RoomOptions roomOptions = CreateRoomOptions();
        roomOptions.MaxPlayers = _maxPlayerCount;

        PhotonNetwork.CreateRoom(_roomName.text, roomOptions);
    }

    public RoomOptions CreateRoomOptions()
    {
        RoomOptions roomOptions = new RoomOptions();

        roomOptions.CustomRoomProperties = CreateCustomrRoomProperties();
        roomOptions.CustomRoomPropertiesForLobby = CreateCustomrRoomPropertiesForLobby();

        return roomOptions;
    }

    public Hashtable CreateCustomrRoomProperties()
    {
        return new Hashtable()
            {
                { PropertyKey.GameMode,             (int)_multiPlayMode },
                { PropertyKey.MaxExpectedPlayer,    _maxPlayerCount },

                { PropertyKey.IsPrivateRoom,        _togglePrivate.isOn },
                { PropertyKey.Password,             _pwInputField.text },

                { PropertyKey.RoomCreator,          PhotonNetwork.NickName },

                { PropertyKey.SceneName,            _multiPlayMode.ToString() },

                { PropertyKey.RoomState,            (int)RoomState.Wait }
            };
    }

    public string[] CreateCustomrRoomPropertiesForLobby()
    {
        return new string[] {
                PropertyKey.GameMode,
                PropertyKey.MaxExpectedPlayer,

                PropertyKey.IsPrivateRoom,
                PropertyKey.Password,

                PropertyKey.RoomCreator,

                PropertyKey.SceneName,

                PropertyKey.RoomState,
            };
    }
}
