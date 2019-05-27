using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOne : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    private Bounds bounds;

    // Script Specific
    public float speed;
    public float frequency;
    public float magnitude;
    public GameObject[] BossParts;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Start()
    {
        bounds = GameBoundary.Instance.GetBounds();
    }

    void Update()
    {
        if (!GameManager.Instance.GamePaused)
        {
            Vector3 circlePosition = (Random.value + 0.5f) * magnitude * new Vector3(Mathf.Sin(frequency * Time.time), Mathf.Cos(frequency * Time.time), 0);
            Vector3 newPosition = transform.position.x > bounds.max.x - 4.0f ?
                       transform.position + circlePosition + (Random.value + 0.5f) * speed * Vector3.left * Time.deltaTime :
                       transform.position + circlePosition;

            rb.MovePosition(newPosition);
        }
    }

}
