using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayeVisibleManager : MonoBehaviour
{
    public static PlayeVisibleManager instance { get; private set; }

    void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public bool IsSameScene()
    {
        var gameRoomInfo = CustomGameRoomData.GetCustomGameRoomData(PhotonNetwork.CurrentRoom);        

        return SceneManager.GetActiveScene().name == gameRoomInfo._sceneName;
    }

    // PhotonView에 접근할 수 있는 컴퍼넌트 사용
    public List<PlayerNetwork> playerList = new List<PlayerNetwork>();
    void ActiveOrNot()
    {
        bool sameScene = IsSameScene();
        for (int i = 0; i < playerList.Count; ++i)
        {
            playerList[i].gameObject.SetActive(sameScene);

            if (sameScene == false)
                playerList[i].transform.parent = transform;
            else
                playerList[i].transform.parent = null;
        }
    }

    public void AddPlayer(PlayerNetwork newPlayer)
    {
        if (playerList.Contains(newPlayer) == false)
        {
            playerList.Add(newPlayer);

            ActiveOrNot();
        }
    }

    public void RemovePlayer(PlayerNetwork removePlayer)
    {
        if (playerList.Contains(removePlayer) == true)
        {
            playerList.Remove(removePlayer);

            ActiveOrNot();
        }
    }

    void OnLevelWasLoaded(int level)
    {
        if (level != 0)
        {
            ActiveOrNot();
            UpdatePlayersInfo();
        }
    }

    void UpdatePlayersInfo()
    {
        for (int i = 0; i < playerList.Count; ++i)
        {
            playerList[i].GetComponent<PlayerHP>().UpdateHp();
        }

        var gameRoomInfo = CustomGameRoomData.GetCustomGameRoomData(PhotonNetwork.CurrentRoom);
        for (int i = 0; i < playerList.Count; ++i)
        {
            if (playerList[i]._photonView.Owner == PhotonNetwork.LocalPlayer)
            {
                playerList[i].gameObject.layer = LayerMask.NameToLayer("TeamPlayer");
            }
            else
            {
                if (gameRoomInfo._multiPlayMode == MultiPlayMode.TeamDeath)
                {
                    if (playerList[i]._photonView.Owner.GetTeam() == 
                        PhotonNetwork.LocalPlayer.GetTeam())
                    {
                        playerList[i].gameObject.layer = LayerMask.NameToLayer("TeamPlayer");
                    }
                    else
                    {
                        playerList[i].gameObject.layer = LayerMask.NameToLayer("EnemyPlayer");
                    }
                }
                else
                {
                    playerList[i].gameObject.layer = LayerMask.NameToLayer("EnemyPlayer");
                }
            }
        }
    }
}
