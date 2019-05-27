using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonus : MonoBehaviour
{
    Rigidbody2D rb;
    public Sprite[] bonusSprites;
    public SpriteRenderer spriteRenderer;
    public float speed;

    private int bonusType;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        bonusType = (int)Random.Range(0, 2.99f);
        spriteRenderer.sprite = bonusSprites[bonusType];
    }

    void Update()
    {
        if (!GameManager.Instance.GamePaused)
        {
            Vector2 newPosition = transform.position + speed * Vector3.left * Time.deltaTime;
            rb.MovePosition(newPosition);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        if (playerManager != null && !GameManager.Instance.GamePaused)
        {
            switch (bonusType)
            {
                case 0:
                    PlayerManager.Instance.TriggerLifeBonus();
                    break;
                case 1:
                    PlayerManager.Instance.TriggerShieldBonus();
                    break;
                case 2:
                    PlayerManager.Instance.TriggerShotBonus();
                    break;
            }
            Destroy(gameObject);
        }

    }

}
