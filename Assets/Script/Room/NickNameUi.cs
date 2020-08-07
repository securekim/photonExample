using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NickNameUi : MonoBehaviour
{
    public TextMeshProUGUI _nameText;

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    public void SetName(string nick)
    {
        _nameText.text = nick;

        Show();
    }
}

