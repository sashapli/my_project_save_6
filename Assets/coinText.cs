using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class coinText : MonoBehaviour
{
    public TMP_Text CoinText;
    private void Start()
    {
        CoinText = GetComponent<TMP_Text>();
    }
    void Update()
    {
        CoinText.text = $"{SaveMenager.Instance.GetCoins()}";
    }
}
