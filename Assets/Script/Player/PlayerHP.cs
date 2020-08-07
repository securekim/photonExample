using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Hashtable = ExitGames.Client.Photon.Hashtable;

public class PlayerHP : MonoBehaviour
{
    public Slider _hpSlider;
    public PhotonView photonView;

    float _curHp = 10.0f;
    public float curHp
    {
        get { return _curHp; }
        set
        {
            _curHp = value;
            _hpSlider.value = _curHp / maxHp;
        }
    }
    public float maxHp = 10.0f;

    [PunRPC]
    public void DamageHp(float damage, int hitActorNumber)
    {
        curHp -= damage;
        Debug.Log(curHp);

        if (curHp <= 0.0f)
        {
            Debug.Log("DEAD BY : " + hitActorNumber);
        }

        photonView.Owner.SetHp(curHp);
    }

    public void UpdateHp()
    {
        curHp = photonView.Owner.GetHp();
    }
}

public static class HpExtensions
{
    public static void SetHp(this Player player, float newHp)
    {
        Hashtable hp = new Hashtable();
        hp[PropertyKey.Hp] = newHp;

        player.SetCustomProperties(hp);
    }

    public static float GetHp(this Player player)
    {
        object hp;
        if (player.CustomProperties.TryGetValue(PropertyKey.Hp, out hp))
        {
            return (float)hp;
        }

        return 0.0f;
    }

    public static void InitHp(this Player player)
    {
        float maxHp = 10.0f;
        player.SetHp(maxHp);
    }
}

