using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementBase : MonoBehaviour
{
    public float speed;
    
    protected void Move(float direction)
    {
        transform.position += direction * speed * Time.deltaTime * transform.right;
    }
}
