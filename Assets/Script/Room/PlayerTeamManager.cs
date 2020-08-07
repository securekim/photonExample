using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTeamManager : MonoBehaviour
{
    public static PlayerTeamManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    void Start()
    {
        InitTeam();
    }


    public List<Player>[] _playerTeam;

    void InitTeam()
    {
        _playerTeam = new List<Player>[2];
        _playerTeam[(int)Team.Red]  = new List<Player>(); // Red
        _playerTeam[(int)Team.Blue] = new List<Player>(); // Blue
    }

    public void UpdateTeams()
    {
        _playerTeam[(int)Team.Red] .Clear();
        _playerTeam[(int)Team.Blue].Clear();

        for (int i = 0; i < PhotonNetwork.PlayerList.Length; ++i)
        {
            Player player = PhotonNetwork.PlayerList[i];

            Team team = player.GetTeam();
            if( team != Team.None)
                _playerTeam[(int)team].Add(player);
        }
    }

    public void SetTeam(Player newPlayer, Team newTeam)
    {
        int maxTeamCount = PhotonNetwork.CurrentRoom.MaxPlayers / 2;
        int teamCount = _playerTeam[(int)newTeam].Count;

        if (teamCount >= maxTeamCount)
        {
            Debug.Log("상대팀의 인원이 충족되어 팀을 바꿀수 없습니다.");
            return;
        }

        newPlayer.SetTeam(newTeam);
    }

    public void SetAutoTeam(Player newPlayer)
    {
        int redCount  = _playerTeam[0].Count;
        int blueCount = _playerTeam[1].Count;

        if ((redCount + blueCount) >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            Debug.LogWarning("방에 인원이 가득차 변경할 수 없습니다.");
            return;
        }

        if (redCount > blueCount)
        {
            _playerTeam[1].Add(newPlayer);
            newPlayer.SetTeam(Team.Blue);
        }
        else if (redCount <= blueCount)
        {
            _playerTeam[0].Add(newPlayer);
            newPlayer.SetTeam(Team.Red);
        }
    }

    public List<Player> GetTeamList(Team team)
    {
        return _playerTeam[(int)team];
    }
}
