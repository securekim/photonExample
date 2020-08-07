using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[System.Serializable]
public struct CustomGameRoomData
{
    public MultiPlayMode _multiPlayMode;
    public int _maxPlayerCount;

    public bool _privateRoom;
    public string _password;

    public string _roomCreator;
    public string _sceneName;

    public RoomState _roomState;

    public static CustomGameRoomData GetCustomGameRoomData(RoomInfo roomInfo)
    {
        var data = new CustomGameRoomData();
        if (roomInfo == null)
            return data;

        data._multiPlayMode = (MultiPlayMode)((int)roomInfo.CustomProperties[PropertyKey.GameMode]);
        data._maxPlayerCount = (int)((byte)roomInfo.CustomProperties[PropertyKey.MaxExpectedPlayer]);

        data._privateRoom = ((bool)roomInfo.CustomProperties[PropertyKey.IsPrivateRoom]);
        data._password = ((string)roomInfo.CustomProperties[PropertyKey.Password]);

        data._roomCreator = ((string)roomInfo.CustomProperties[PropertyKey.RoomCreator]);

        data._sceneName = ((string)roomInfo.CustomProperties[PropertyKey.SceneName]);

        data._roomState = (RoomState)((int)roomInfo.CustomProperties[PropertyKey.RoomState]);

        return data;
    }
}

