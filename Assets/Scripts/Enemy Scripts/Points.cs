using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Points : MonoBehaviour
{
    [SerializeField] private int objectPoints;

    public int ObjectPoints { get => objectPoints; set => objectPoints = value; }
}
