using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Enemy))]
public class EnemyHealth : MonoBehaviour
{
    [SerializeField] int maxHealth;
    [SerializeField] int difficultyRamp;

    Enemy enemy;

    int currentHealth;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
    }

    void OnEnable()
    {
        currentHealth = maxHealth;
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("hit");
        currentHealth--;
        if (currentHealth <= 0)
        {
            enemy.RewardGold();
            maxHealth += difficultyRamp;
            gameObject.SetActive(false);

        }
    }

   
   
}
