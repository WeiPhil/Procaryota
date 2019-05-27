using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Protector : MonoBehaviour
{
    public GameObject projectilePrefab;
    public AudioClip projectileShotClip;
    public AudioSource audioSource;
    public float rotationSpeed;


    public float fireRate;
    private float nextFire = 0.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        nextFire = Time.time + fireRate * (Random.value + 0.5f);
    }

    void Update()
    {
        transform.RotateAround(transform.parent.position, Vector3.forward, rotationSpeed * Time.deltaTime);
        transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime, Space.Self);

        if (Time.time > nextFire && !GameManager.Instance.GamePaused)
        {
            nextFire = Time.time + fireRate * (Random.value + 0.5f);
            Shoot();
        }
    }

    void Shoot()
    {
        Instantiate(projectilePrefab, transform.position, Quaternion.identity);
        PlaySound(projectileShotClip);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        EnnemyController ennemyController = other.GetComponent<EnnemyController>();
        BossOnePart bossOnePart = other.GetComponent<BossOnePart>();
        BossTwoPart bossTwoPart = other.GetComponent<BossTwoPart>();
        EnnemyProjectileController ennemyProjectileController = other.GetComponent<EnnemyProjectileController>();
        if (ennemyController != null && !GameManager.Instance.GamePaused)
        {
            ennemyController.Damaged(1);
        }
        else if (bossOnePart != null && !GameManager.Instance.GamePaused)
        {
            bossOnePart.Damaged(1);
        }
        else if (bossTwoPart != null && !GameManager.Instance.GamePaused)
        {
            bossTwoPart.Damaged(1);
        }
        else if (ennemyProjectileController != null)
        {
            Destroy(ennemyProjectileController.gameObject);
        }

    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }


}
