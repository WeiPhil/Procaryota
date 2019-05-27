using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossTwo : MonoBehaviour
{
    // Components
    Rigidbody2D rb;


    // Script Specific
    public float speed;
    public float frequency;
    public float magnitude;
    public GameObject[] BossParts;

    private float rotateTimeout;
    private bool rotating;

    private Bounds bounds;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        rotateTimeout = Time.time + 2.0f;
        rotating = false;
    }

    void Start()
    {
        bounds = GameBoundary.Instance.GetBounds();
    }

    void Update()
    {
        if (!GameManager.Instance.GamePaused)
        {
            if (Time.time < rotateTimeout && !rotating)
            {
                Vector3 circlePosition = (Random.value + 0.5f) * magnitude * new Vector3(Mathf.Sin(frequency * Time.time), Mathf.Cos(frequency * Time.time), 0);
                Vector3 newPosition = transform.position.x > bounds.max.x - 4.0f ?
                        transform.position + circlePosition + (Random.value + 0.5f) * speed * Vector3.left * Time.deltaTime :
                        transform.position + circlePosition;
                rb.MovePosition(newPosition);
            }
            else if (rotating)
            {
                if (Time.time < rotateTimeout)
                {
                    transform.Rotate(Vector3.forward, -90 * Time.deltaTime, Space.Self);
                }
                else
                {
                    rotating = false;
                    rotateTimeout = Time.time + 2.0f;
                }
            }
            else if (Time.time >= rotateTimeout && !rotating)
            {
                rotateTimeout = Time.time + 1.0f;
                rotating = true;
            }



        }
    }

}
