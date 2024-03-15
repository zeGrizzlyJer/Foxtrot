using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AdPopup : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI rewardsText;

    private void OnEnable()
    {
        UpdateRewardsAmount();
    }

    public void WatchAd()
    {
        Debug.Log("Decision made! Player will watch ad");
        UIScreen.playerDecisionMade = true;

        GameManager.Instance.Money += GameData.Rewards * GameManager.REWARDS_MULTIPLIER;

        GameData.Rewards = 0;
    }

    public void SkipAd()
    {
        Debug.Log("Decision made! Player will skip ad");
        UIScreen.playerDecisionMade = true;
    }

    private void UpdateRewardsAmount()
    {
        rewardsText.text = $"Watch Ad - {GameData.Rewards * GameManager.REWARDS_MULTIPLIER}";
    }
}
