using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnnemyController : MonoBehaviour
{
    // Components
    Rigidbody2D rb;
    Animator animator;
    AudioSource audioSource;

    // Script Specific

    public float speed;
    public float frequency;
    public float magnitude;
    public int lives = 5;
    public GameObject explosion;
    public int ennemyType;
    public GameObject bonusPrefab;
    public float dropRate;
    public GameObject projectilePrefab;
    public float fireRate = 0.0f;

    // Private
    private float nextFire;
    private Bounds bounds;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        frequency *= Random.value < 0.5 ? -1 : 1;
        if (ennemyType == 2)
        {
            speed *= Random.value < 0.5 ? -1 : 1;
        }
        fireRate /= (int)GameManager.Instance.GameDifficulty + 1;
        nextFire = fireRate;
        speed += (int)GameManager.Instance.GameDifficulty * 0.5f;
        magnitude += (int)GameManager.Instance.GameDifficulty * 0.05f;
        dropRate /= (int)GameManager.Instance.GameDifficulty + 1;
    }

    void Start()
    {
        bounds = GameBoundary.Instance.GetBounds();

    }

    void Update()
    {
        if (!GameManager.Instance.GamePaused)
        {
            Vector3 newPosition = new Vector3();
            Vector3 circlePosition = new Vector3();
            switch (ennemyType)
            {
                case 0:
                    circlePosition = (Random.value + 0.5f) * magnitude * new Vector3(Mathf.Sin(frequency * Time.time), Mathf.Cos(frequency * Time.time), 0);
                    newPosition = transform.position + circlePosition + (Random.value + 0.5f) * speed * Vector3.left * Time.deltaTime;
                    break;
                case 1:
                case 3:
                    circlePosition = (Random.value + 0.5f) * magnitude * new Vector3(Mathf.Sin(frequency * Time.time), Mathf.Cos(frequency * Time.time), 0);
                    newPosition = transform.position.x > bounds.max.x - 0.5f ?
                        transform.position + circlePosition + (Random.value + 0.5f) * speed * Vector3.left * Time.deltaTime :
                        transform.position + circlePosition;
                    break;
                case 2:
                    if (transform.position.x > bounds.max.x - 0.5f)
                    { // Until inside screen
                        newPosition = transform.position + (Random.value + 0.5f) * Mathf.Abs(speed) * Vector3.left * Time.deltaTime;
                    }
                    else
                    { // Once inside screen
                        if (speed > 2)
                        { //charging player
                            newPosition = transform.position + speed * Vector3.left * Time.deltaTime;
                        }
                        else
                        { // crossing screen
                            if (Vector2.Angle(-transform.right, PlayerManager.Instance.transform.position - transform.position) < 1.0f)
                            { // player seen
                                speed *= 7 * Mathf.Sign(speed);
                                newPosition = transform.position + speed * Vector3.left * Time.deltaTime;
                            }
                            else
                            { //  marching along +-y
                                if (Mathf.Abs(transform.position.y) + 0.5f < bounds.max.y)
                                {
                                    newPosition = transform.position + speed * Vector3.up * Time.deltaTime;
                                }
                                else
                                {
                                    speed *= -1.0f;
                                    newPosition = transform.position + speed * Vector3.up * Time.deltaTime;
                                }
                            }
                        }
                    }
                    break;
                default:
                    newPosition = transform.position + speed * Vector3.left * Time.deltaTime;
                    break;
            }

            rb.MovePosition(newPosition);

            if (fireRate != 0.0f && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate + 2.0f * fireRate * Random.value;
                Shoot();
            }
        }

    }

    void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
    }

    public void Damaged(int damage)
    {
        lives -= damage;
        if (lives <= 0)
        {
            if (Random.value < dropRate)
            {
                Instantiate(bonusPrefab, rb.position, Quaternion.identity);
            }
            Death();
        }
        else
            animator.SetTrigger("Damaged");
    }

    private void Death()
    {
        GetComponent<Collider2D>().enabled = false;
        Instantiate(explosion, rb.position, Quaternion.identity);
        GameManager.Instance.Score += 10;
        PlayerManager.Instance.AddExperience(10);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        if (playerManager != null && !GameManager.Instance.GamePaused)
        {
            playerManager.Damaged(1);
            Death();
            return;
        }

    }

    public void RestartAttack(Vector2 position)
    {
        magnitude += 0.01f;
        transform.position = position;
    }

}
