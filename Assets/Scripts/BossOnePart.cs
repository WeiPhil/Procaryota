using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossOnePart : MonoBehaviour
{
    // Components
    Animator animator;
    ImageSwitcher imageSwitcher;

    // Script Specific
    public int partNumber;
    public int lives;
    public GameObject explosion;
    public GameObject bonusPrefab;
    public float dropRate;
    public GameObject generatedPrefab;
    public float generateRate = 0.0f;

    private float nextGenerate;

    void Awake()
    {
        animator = transform.parent.GetComponent<Animator>();
        nextGenerate = 0.0f;
        imageSwitcher = GetComponent<ImageSwitcher>();
    }

    void Update()
    {
        if (!GameManager.Instance.GamePaused)
        {
            if (generateRate != 0.0f && Time.time > nextGenerate)
            {
                nextGenerate = Time.time + generateRate + 2.0f * Random.value;
                Generate();
            }
        }
    }

    void Generate()
    {
        Instantiate(generatedPrefab, transform.position, Quaternion.identity);
    }

    public void Damaged(int damage)
    {
        lives -= damage;
        transform.parent.GetComponent<Animation>().Play();
        GetComponent<Animation>().Play();
        if (lives <= 0)
            Death();
        else if (lives % 4 == 0)
        {
            GetComponent<Animator>().SetTrigger("ChangeLook");
            imageSwitcher.NextImage();
        }

    }

    void Death()
    {
        if (Random.value < dropRate)
        {
            Instantiate(bonusPrefab, transform.position, Quaternion.identity);
        }
        GameManager.Instance.Score += 20;
        PlayerManager.Instance.AddExperience(20);
        Instantiate(explosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        ProjectileController projectileController = other.GetComponent<ProjectileController>();
        if (projectileController != null && !GameManager.Instance.GamePaused)
        {
            Damaged(1);
            Destroy(projectileController.gameObject);
        }
        PlayerManager playerManager = other.GetComponent<PlayerManager>();
        if (playerManager != null && !GameManager.Instance.GamePaused)
        {
            playerManager.Damaged(1);
            return;
        }

    }

}
