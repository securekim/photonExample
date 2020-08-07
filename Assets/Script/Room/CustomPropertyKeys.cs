using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class PropertyKey
{
    public const string Hp = "Hp";

    //  PLAYERS
    public const string Team = "Team";

    // room property - master client는 write enable, 그 외는 readonly
    public const string RoomCreator = "RoomCreator";

    public const string SceneName = "SceneName";
    public const string GameMode = "GameMode";
    public const string MaxExpectedPlayer = "MaxExpectedPlayer";

    public const string IsPrivateRoom = "IsPrivateRoom";
    public const string Password = "Password";

    public const string RoomState = "RoomState";

    public const string SpawnIndexRed = "SpawnIndexRed";
    public const string SpawnIndexBlue = "SpawnIndexBlue";

    
}


