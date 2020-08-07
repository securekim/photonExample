using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public enum Team
{
    Red = 0,
    Blue,

    None
}

public static class TeamExtensions
{
    public static void SetTeam(this Player player, Team newTeam)
    {
        Hashtable team = new Hashtable();
        team[PropertyKey.Team] = newTeam;

        player.SetCustomProperties(team);
    }

    public static Team GetTeam(this Player player)
    {
        object team;
        if (player.CustomProperties.TryGetValue(PropertyKey.Team, out team))
        {
            return (Team)team;
        }

        return Team.None;
    }
}