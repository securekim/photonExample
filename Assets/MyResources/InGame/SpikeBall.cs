using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Multiplay;
using Photon.Pun;

public class SpikeBall : MonoBehaviour
{
    public int actorNumber = -1;
    public GameObject particle;
    public float lifeTime;
    public float damage = 1.0f;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(lifeTime);

        DestroyBall(transform.position);
    }

    private void OnTriggerEnter(Collider other)
    {
        DestroyBall(transform.position);

        // 발사한 대상과 내가 동일하고
        if (actorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
        {
            PlayerControl pc = other.gameObject.GetComponent<PlayerControl>();
            if (pc != null)
            {
                // 나랑 다른 팀이면
                if ((PhotonNetwork.LocalPlayer.GetTeam() == Team.None) ||
                    (PhotonNetwork.LocalPlayer.GetTeam() != pc._photonView.Owner.GetTeam()))
                {
                    // 대미지를 처리한다.
                    pc._photonView.RPC("DamageHp", RpcTarget.All, damage, actorNumber);
                }                
            }
        }
    }

    public void DestroyBall(Vector3 destroPos)
    {
        GameObject obj = Instantiate(particle) as GameObject;
        obj.transform.position = destroPos;

        Destroy(gameObject);
    }
}
