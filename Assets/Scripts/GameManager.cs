using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public List<ObjectPooling> pooling = new List<ObjectPooling>();

    private int playerPoints;

    public int PlayerPoints { get => playerPoints; }

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;

        foreach (Transform child in transform)
        {
            if (child.GetComponent<ObjectPooling>())
            {
                pooling.Add(child.GetComponent<ObjectPooling>());
            }
        }
    }

    public void AddPoints(int value)
    {
        playerPoints += value;
    }
}
