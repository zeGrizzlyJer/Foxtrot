using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResetProgress : MonoBehaviour
{
    private void Start()
    {
        gameObject.GetComponent<Button>().onClick.AddListener(Reset);
    }

    private void Reset()
    {
        DataManager.Instance.ResetPlayerData();
        SceneTransitionManager.Instance.LoadScene(0);
    }
}
