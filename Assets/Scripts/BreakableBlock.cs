using DigitalRuby.AdvancedPolygonCollider;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BreakableBlock : Hp
{
    public Texture2D baseTexture;
    Texture2D cloneTexture;
    SpriteRenderer sr;

    public AudioSource audioSource;
    public AudioClip audioClip;

    float widthWorld, heightWorld;
    int widthPixel, heightPixel;

    public delegate void OnDamage(int value);
    public event OnDamage onDamage;

    public float WidthWorld
    {
        get
        {
            if (widthWorld == 0)
                widthWorld = sr.bounds.size.x;
            return widthWorld;
        }
    }

    public float HeightWorld
    {
        get
        {
            if (heightWorld == 0)
                heightWorld = sr.bounds.size.y;
            return heightWorld;
        }
    }

    public int WidthPixel
    {
        get
        {
            if (widthPixel == 0)
                widthPixel = sr.sprite.texture.width;

            return widthPixel;
        }
    }
    public int HeightPixel
    {
        get
        {
            if (heightPixel == 0)
                heightPixel = sr.sprite.texture.height;

            return heightPixel;
        }
    }
    Vector2Int World2Pixel(Vector2 pos)
    {
        Vector2Int v = Vector2Int.zero;

        var dx = (pos.x - transform.position.x);
        var dy = (pos.y - transform.position.y);

        v.x = Mathf.RoundToInt(0.5f * WidthPixel + dx * (WidthPixel / WidthWorld));
        v.y = Mathf.RoundToInt(0.5f * HeightPixel + dy * (HeightPixel / HeightWorld));

        return v;
    }

    void Start()
    {
        onDamage += GameManager.instance.AddPoints;
        this.transform.localScale = Vector3.one * (0.5f);
        sr = GetComponent<SpriteRenderer>();
        cloneTexture = Instantiate(baseTexture);

        if (cloneTexture.format != TextureFormat.ARGB32)
            Debug.LogWarning("Texture must be ARGB32");
        if (cloneTexture.wrapMode != TextureWrapMode.Clamp)
            Debug.LogWarning("wrapMode must be Clamp");

        UpdateTexture();


        for (int i = 0; i <= cloneTexture.width; i++)
        {
            for (int j = 0; j <= cloneTexture.height; j++)
            {
                Color pixelColor = cloneTexture.GetPixel(i, j);
                //if(pixelColor != Color.clear)
                {
                    maxHp += 1;
                }
            }
        }

        base.Start();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        MakeAHole(collision.GetComponent<CircleCollider2D>());
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!collision.CompareTag("Bullet"))
            return;

        MakeAHole(collision.GetComponent<CircleCollider2D>());
    }

    public float random = 0;

    void MakeAHole(CircleCollider2D col)
    {
        Vector2Int c = World2Pixel(col.bounds.center);
        int width = Mathf.RoundToInt(col.bounds.size.x * WidthPixel / widthWorld);
        //int height = Mathf.RoundToInt(col.bounds.size.y * heightPixel / heightWorld);

        int px, nx, py, ny, d;

        for (int i = 0; i <= width; i++)
        {
            d = Mathf.RoundToInt(Mathf.Sqrt(width * width - i * i));

            for (int j = 0; j <= d; j++)
            {
                if (Random.Range(0, 100) > random)
                {
                    px = c.x + i;
                    nx = c.x - i;
                    py = c.y + j;
                    ny = c.y - j;

                    cloneTexture.SetPixel(px, py, Color.clear);
                    cloneTexture.SetPixel(nx, py, Color.clear);
                    cloneTexture.SetPixel(px, ny, Color.clear);
                    cloneTexture.SetPixel(nx, ny, Color.clear);

                    currentHp -= 1;
                }
            }
        }
        audioSource.PlayOneShot(audioClip, Random.Range(0.1f, 0.3f));
        cloneTexture.Apply();
        UpdateTexture();

        onDamage?.Invoke(1);
        if(currentHp <= maxHp * 0.25f)
        {
            gameObject.SetActive(false);
        }
    }

    void UpdateTexture()
    {
        sr.sprite = Sprite.Create(cloneTexture, new Rect(0, 0, cloneTexture.width, cloneTexture.height), new Vector2(0.5f, 0.5f), 50f);
    }
}
