using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{

    Rigidbody2D rb;
    public float speed = 5.0f;

    private bool zeroVelocity = true;

    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
        rb.velocity = transform.right * speed;
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
            rb.velocity = transform.right * speed;
            zeroVelocity = false;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnnemyController ennemyController = other.GetComponent<EnnemyController>();
        if (ennemyController != null)
        {
            ennemyController.Damaged(1);
            Destroy(gameObject);
            return;
        }

    }
}
