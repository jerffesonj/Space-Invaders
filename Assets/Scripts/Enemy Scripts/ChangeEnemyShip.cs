using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeEnemyShip : MonoBehaviour
{
    [SerializeField] private SpriteRenderer ship;
    [SerializeField] private List<Sprite> shipsImages;
    
    public int ShipCount()
    {
        return shipsImages.Count;
    }
    public void ChooseSprite(int value)
    {
        ship.sprite = shipsImages[value];
    }
}
