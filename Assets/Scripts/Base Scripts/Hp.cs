using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hp : MonoBehaviour
{
    [SerializeField] protected int currentHp;
    [SerializeField] protected int maxHp;
    protected void Start()
    {
        currentHp = maxHp;
    }

    public virtual void RemoveHp(int damage)
    {
        currentHp-= damage;

        if(currentHp<=0)
        {
            currentHp = maxHp;
            gameObject.SetActive(false);
        }
    }
}
