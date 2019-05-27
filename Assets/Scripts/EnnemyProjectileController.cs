using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyProjectileController : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed = 5.0f;
    public int projectileType; //1 ennemy1 / 2 ennemy4



    private bool zeroVelocity = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = -transform.right * speed;
    }

    void Update()
    {
        if (GameManager.Instance.GamePaused && !zeroVelocity)
        {
            rb.velocity = Vector2.zero;
            zeroVelocity = true;
        }
        else if ((!GameManager.Instance.GamePaused && zeroVelocity) || !PlayerManager.Instance.InGame)
        {
            rb.velocity = -transform.right * speed;
            zeroVelocity = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {

        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        if (playerManager != null)
        {
            if (projectileType == 1)
            {
                playerManager.Slow(3.0f, 0.5f);
            }
            playerManager.Damaged(1);
            Destroy(gameObject);
            return;
        }

    }
}
