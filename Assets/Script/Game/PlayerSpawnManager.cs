using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerSpawnManager : MonoBehaviour
{
    public GameObject _playerPrefabObj;
    
    public Transform[] _spawnPosRed;
    public Transform[] _spawnPosBlue;
    public Transform[] _spawnPosNone;

    private void Start()
    {
        CreatePlayer();
    }

    void CreatePlayer()
    {
        GameObject newPlayer = PhotonNetwork.Instantiate(_playerPrefabObj.name,
                                                         Vector3.zero, Quaternion.identity, 0);
        
        PhotonScriptEnabler pse = newPlayer.GetComponent<PhotonScriptEnabler>();

        // PhotonView는 Owner라는 변수를 제공하고 이를 통해 Player 정보를 가져올 수 있다.
        Team team = pse.photonView.Owner.GetTeam();
        if (team == Team.Red)
        {
            int spawnIndex = PhotonNetwork.CurrentRoom.GetSpawnIndex(PropertyKey.SpawnIndexRed);
            newPlayer.transform.position = _spawnPosRed[spawnIndex].position + Vector3.up * 2.0f;

            ++spawnIndex;
            spawnIndex %= _spawnPosRed.Length;

            PhotonNetwork.CurrentRoom.SetSpawnIndex(PropertyKey.SpawnIndexRed, spawnIndex);
        }
        else if (team == Team.Blue)
        {
            int spawnIndex = PhotonNetwork.CurrentRoom.GetSpawnIndex(PropertyKey.SpawnIndexBlue);
            newPlayer.transform.position = _spawnPosBlue[spawnIndex].position + Vector3.up * 2.0f;

            ++spawnIndex;
            spawnIndex %= _spawnPosBlue.Length;

            PhotonNetwork.CurrentRoom.SetSpawnIndex(PropertyKey.SpawnIndexBlue, spawnIndex);
        }
        else
        {
            int spawnIndex = PhotonNetwork.CurrentRoom.GetSpawnIndex(PropertyKey.SpawnIndexRed);
            newPlayer.transform.position = _spawnPosNone[spawnIndex].position + Vector3.up * 2.0f;

            ++spawnIndex;
            spawnIndex %= _spawnPosRed.Length;

            PhotonNetwork.CurrentRoom.SetSpawnIndex(PropertyKey.SpawnIndexRed, spawnIndex);
        };
    }
}

public static class RoomExtensions
{
    public static void SetSpawnIndex(this Room room, string key, int index)
    {
        Hashtable team = new Hashtable();
        team[key] = index;
        room.SetCustomProperties(team);
    }

    public static int GetSpawnIndex(this Room room, string key)
    {
        object getIndex;
        if (room.CustomProperties.TryGetValue(key, out getIndex))
        {
            return (int)getIndex;
        }

        return 0;
    }
}
