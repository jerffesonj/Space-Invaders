using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetLaserSprite : MonoBehaviour
{
    [SerializeField] private SpriteRenderer sprite;
    [SerializeField] private List<Sprite> sprites;

    private void OnEnable()
    {
        sprite.sprite = sprites[Random.Range(0, sprites.Count)];
    }
    public void TurnOff()
    {
        gameObject.SetActive(false);
    }
}
