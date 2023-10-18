using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bank : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI goldText;

    [SerializeField] int startingBalance;

    [SerializeField] int currentBalance;
    public int CurrentBalance { get { return currentBalance; } }


    void Awake()
    {
        currentBalance = startingBalance;
        UpdateGoldText();
    }

    public void Deposit(int amount)
    {
        currentBalance += Mathf.Abs(amount);
        UpdateGoldText();
    }

    public void Withdraw(int amount)
    {
        currentBalance -= Mathf.Abs(amount);
        UpdateGoldText();
        HandleGameOver();

    }

    void UpdateGoldText()
    {
        goldText.text = $"Gold: {currentBalance}";
    }

    void HandleGameOver()
    {
        if (currentBalance < 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }


}
