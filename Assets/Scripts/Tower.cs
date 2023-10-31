using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] int cost;
    [SerializeField] List<GameObject> children;
    [SerializeField] float buildTime;


    private void Start()
    {
        DisableChildren();

        StartCoroutine(Build());
    }

    public bool PlaceTower(Tower tower, Vector3 position)
    {
        Bank bank = FindObjectOfType<Bank>();
        if (bank == null)
        {
            return false;
        }
        if (bank.CurrentBalance >= cost)
        {
            Instantiate(tower, position, Quaternion.identity);
            bank.Withdraw(cost);
            return true;
        }
        return false;

    }

    IEnumerator Build()
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetActive(true);
            yield return new WaitForSeconds(buildTime);
        }
    }

    void DisableChildren()
    {
        for (int i = 0; i < children.Count; i++)
        {
            children[i].SetActive(false);
        }
    }
}
