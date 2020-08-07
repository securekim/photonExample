using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomState : BaseState
{
    public SelectRoomUi _selectRoomUi;

    public GameObject _teamUi;

    [Header("Player List")]
    public NickNameUi[] deathMatchNickNames;
    public NickNameUi[] teamRed;
    public NickNameUi[] teamBlue;

    void HideNickNameUi()
    {
        for (int i = 0; i < deathMatchNickNames.Length; ++i)
        {
            deathMatchNickNames[i].Hide();
        }
    }

    bool IsTeamMode()
    {
        return _teamUi.activeSelf;
    }

    void UpdateUserName()
    {
        HideNickNameUi();

        if(IsTeamMode() == false) // death match
        {
            Player[] players = PhotonNetwork.PlayerList;
            int maxPlayerCount = players.Length;
            for (int i = 0; i < maxPlayerCount; ++i)
            {
                deathMatchNickNames[i].SetName(players[i].NickName);
                deathMatchNickNames[i].Show();

                if (players[i].GetTeam() != Team.None)
                {
                    players[i].SetTeam(Team.None);
                }
            }
        }
        else // Team Mode
        {
            PlayerTeamManager playerTeamManager = PlayerTeamManager.instance;
            for (int i = 0; i < 2; ++i) // red -> blue
            {
                List<Player> teamList = playerTeamManager.GetTeamList((Team)i);
                for (int j = 0; j < teamList.Count; ++j)
                {
                    Team team = teamList[j].GetTeam();

                    if (team == Team.Red)
                    {
                        teamRed[j].SetName(teamList[j].NickName);
                        teamRed[j].Show();
                    }
                    else if (team == Team.Blue)
                    {
                        teamBlue[j].SetName(teamList[j].NickName);
                        teamBlue[j].Show();
                    }
                }
            }
        }      
    }

    public override void InitState()
    {
        gameObject.SetActive(true);

        UpdateRoomInfo();
        UpdateButtons();

        SetAutoTeam(PhotonNetwork.LocalPlayer);

        UpdateTeamUi();

        PunNetworkManager.instance.onMasterClientSwitched += OnMasterClientSwitched;
        PunNetworkManager.instance.onPlayerPropertiesUpdate += OnPlayerPropertiesUpdate;

        PunNetworkManager.instance.onPlayerEnteredRoom += OnPlayerEnterOrLeftRoom;
        PunNetworkManager.instance.onPlayerLeftRoom += OnPlayerEnterOrLeftRoom;
    }

    public override void ReleaseState()
    {
        gameObject.SetActive(false);

        PunNetworkManager.instance.onMasterClientSwitched -= OnMasterClientSwitched;
        PunNetworkManager.instance.onPlayerPropertiesUpdate -= OnPlayerPropertiesUpdate;

        PunNetworkManager.instance.onPlayerEnteredRoom -= OnPlayerEnterOrLeftRoom;
        PunNetworkManager.instance.onPlayerLeftRoom -= OnPlayerEnterOrLeftRoom;
    }

    private void OnDestroy()
    {
        PunNetworkManager.instance.onMasterClientSwitched -= OnMasterClientSwitched;
        PunNetworkManager.instance.onPlayerPropertiesUpdate -= OnPlayerPropertiesUpdate;

        PunNetworkManager.instance.onPlayerEnteredRoom -= OnPlayerEnterOrLeftRoom;
        PunNetworkManager.instance.onPlayerLeftRoom -= OnPlayerEnterOrLeftRoom;
    }

    void SetAutoTeam(Player newPlayer)
    {
        PlayerTeamManager playerTeamManager = PlayerTeamManager.instance;
        if( IsTeamMode() )
        {
            playerTeamManager.UpdateTeams();
            playerTeamManager.SetAutoTeam(newPlayer);
        }
    }

    void UpdateTeamUi()
    {
        RoomInfo roomInfo = PhotonNetwork.CurrentRoom;
        CustomGameRoomData gameRoomData = CustomGameRoomData.GetCustomGameRoomData(roomInfo);

        if (gameRoomData._multiPlayMode == MultiPlayMode.Free)
            _teamUi.SetActive(false);
        else
            _teamUi.SetActive(true);
    }

    void OnMasterClientSwitched()
    {
        UpdateButtons();
    }

    void UpdateRoomInfo()
    {
        Debug.Log(PhotonNetwork.LocalPlayer.NickName);
        // Update Player Count(include me)
        RoomInfo roomInfo = PhotonNetwork.CurrentRoom;
        _selectRoomUi.UpdateRoomInfo(roomInfo, PhotonNetwork.CurrentRoom.PlayerCount);        
    }

    public void LeaveRoom()
    {
        PunNetworkManager.instance.LeaveRoom();
    }

    public GameObject _startButton;
    public GameObject _joinButton;
    void UpdateButtons()
    {
        _startButton.SetActive(PhotonNetwork.IsMasterClient);

        _joinButton.SetActive(!PhotonNetwork.IsMasterClient);
    }

    public void SetBlueTeam()
    {
        PlayerTeamManager playerTeamManager = PlayerTeamManager.instance;
        playerTeamManager.SetTeam(PhotonNetwork.LocalPlayer, Team.Blue);
    }

    public void SetRedTeam()
    {
        PlayerTeamManager playerTeamManager = PlayerTeamManager.instance;
        playerTeamManager.SetTeam(PhotonNetwork.LocalPlayer, Team.Red);
    }

    void OnPlayerPropertiesUpdate()
    {
        PlayerTeamManager.instance.UpdateTeams();
        UpdateUserName();
    }

    void OnPlayerEnterOrLeftRoom()
    {
        UpdateRoomInfo();

        PlayerTeamManager.instance.UpdateTeams();
        UpdateUserName();
    }
}


