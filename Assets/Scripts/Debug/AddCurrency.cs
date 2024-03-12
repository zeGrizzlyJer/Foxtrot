using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AddCurrency : MonoBehaviour
{
    public int moneyToAdd;
    [SerializeField] TextMeshProUGUI moneyTxt;

    void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(AddMoney);
    }

    private void AddMoney()
    {
        int currency = GameData.Money;
        currency += moneyToAdd;
        moneyTxt.text = currency.ToString();
        GameData.Money = currency;
        PlayerPrefs.Save();
    }
}
