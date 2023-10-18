using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{


    [SerializeField] GameObject enemy;
    [SerializeField][Range(0.1f, 20)] float enemyRespawnRate;

    [SerializeField][Range(0, 50)] int poolSize;

    GameObject[] pool;



    private void Awake()
    {
        PopulatePool();
    }

    void Start()
    {
        StartCoroutine(SpawnEnemies());

    }

    void PopulatePool()
    {
        pool = new GameObject[poolSize];
        for (int i = 0; i < pool.Length; i++)
        {
            pool[i] = Instantiate(enemy, gameObject.transform);
            pool[i].SetActive(false);
        }
    }

    void EnableObjectInPool()
    {
        foreach (GameObject item in pool)
        {
            if (item.activeInHierarchy == false)
            {
                item.SetActive(true);
                return;
            }

        }
    }

    IEnumerator SpawnEnemies()
    {
        while (true)
        {
            EnableObjectInPool();
            yield return new WaitForSeconds(enemyRespawnRate);
        }



    }
}
