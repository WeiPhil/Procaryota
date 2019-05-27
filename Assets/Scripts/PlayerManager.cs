using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

[System.Serializable]
public class Boundary
{
    public float xMin, xMax, yMin, yMax;
}

public class PlayerManager : MonoBehaviour
{
    private static PlayerManager _instance;
    public static PlayerManager Instance { get { return _instance; } }

    // Components
    public Rigidbody2D rigidbody2d;
    AudioSource audioSource;
    Animator animator;

    // Public Script Specific
    public Boundary boundary;
    public GameObject projectilePrefab;
    public GameObject evolutionAnimation;
    public GameObject protectorPrefab;
    public AudioClip[] projectileShotClips;
    public AudioClip evolveClip;
    public Transform shotSpawn;
    public Vector2 initialPosition;
    public GameObject explosion;

    public int maxHealth = 4;
    public float speed;
    public float timeInvincible = 1.0f;
    public float fireRate;

    // Private
    private int currentHealth;
    private int level;
    private bool isInvincible;
    private float invincibleTimer;
    private int experience;
    private int experienceForNextLevel;
    private bool inGame;
    private float nextFire = 0.0f;

    // Animation specific
    private float animationTime;
    private bool autoAnimating;
    private bool evolving;
    private float animationTimeLeft;
    private Vector2 animationStartPosition;
    private Vector2 animationGoalPosition;

    // Upgrades specific
    private int maxUpgrades = 4;
    private int timeFreezeLevel;
    private int lifePointsLevel;
    private int shieldTimeLevel;
    private int protectorsLevel;
    private GameObject[] protectors;
    private int evolutionPointsLeft;
    private Action onAnimationEnd;

    // bonus specific
    private bool shieldActive;
    private float nextFreeze;
    private float freezeTimeout;
    private float freezeDuration;
    private int shotLevel;

    // malus specific
    private float slowedUntil;
    private float slowedFactor;
    private bool slowed;

    // Properties
    public bool InGame { get { return inGame; } set { inGame = value; } }
    public bool Animating { get { return autoAnimating; } }
    public int EvolutionPointsLeft { get { return evolutionPointsLeft; } set { evolutionPointsLeft = value; } }
    public int TimeFreezeLevel { get { return timeFreezeLevel; } }
    public int LifePointsLevel { get { return lifePointsLevel; } }
    public int ShieldTimeLevel { get { return shieldTimeLevel; } }
    public int ProtectorsLevel { get { return protectorsLevel; } }
    public int MaxUpgrades { get { return maxUpgrades; } }


    private void Awake()
    {
        if (_instance != null && _instance != this)
            Destroy(this.gameObject);
        else
            _instance = this;

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);
        inGame = false;
        rigidbody2d = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        Vector2 edgeVector = Camera.main.ViewportToWorldPoint(Vector2.one);
        float height = edgeVector.y * 2;
        float width = edgeVector.x * 2;
        boundary.xMin = -width * 0.5f;
        boundary.xMax = width * 2.0f / 3.0f - width * 0.5f;
        boundary.yMin = -height * 0.5f;
        boundary.yMax = height * 0.5f;
        initialPosition = new Vector3(boundary.xMin - 0.5f, 1.0f, 1.0f);
    }

    void Start()
    {
        currentHealth = maxHealth;
        isInvincible = false;
        level = 1;
        experience = 0;
        experienceForNextLevel = 200;
        GUI.Instance.ResetGUI();
        autoAnimating = false;
        evolving = false;

        // Upgrades
        timeFreezeLevel = 0;
        lifePointsLevel = 0;
        shieldTimeLevel = 0;
        protectorsLevel = 0;
        evolutionPointsLeft = 0;

        // Bonus specific
        shieldActive = false;
        nextFreeze = 0.0f;
        freezeTimeout = 8.0f;
        freezeDuration = 1.5f;
        shotLevel = 0;

        // Malus specific
        slowedUntil = 0.0f;
        slowed = false;
        slowedFactor = 1.0f;
    }

    void FixedUpdate()
    {
        if (!autoAnimating && !evolving && !GameManager.Instance.GamePaused && inGame)
        {
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            Vector2 movement = new Vector2(horizontal, vertical);
            rigidbody2d.velocity = movement * speed * 1.0f / Time.timeScale;

            rigidbody2d.position = new Vector2(
                Mathf.Clamp(rigidbody2d.position.x, boundary.xMin, boundary.xMax),
                Mathf.Clamp(rigidbody2d.position.y, boundary.yMin, boundary.yMax)
            );
        }
        else
        {
            rigidbody2d.velocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (timeFreezeLevel > 0 && Time.unscaledTime < nextFreeze)
        {
            float deltaTime = nextFreeze - Time.unscaledTime;
            float timeout = (freezeTimeout + timeFreezeLevel);
            GUI.Instance.SetFreezeWait((timeout - deltaTime) / timeout);
        }

        if (slowed && Time.time > slowedUntil)
        {
            slowed = false;
            speed /= slowedFactor;
            slowedUntil = 0.0f;
        }

        if (!autoAnimating && !evolving && !GameManager.Instance.GamePaused && inGame)
        {
            if (Input.GetButton("Fire1") && Time.time > nextFire)
            {
                nextFire = Time.time + fireRate;
                Shoot();
            }

            if (timeFreezeLevel > 0 && (Input.GetButton("Fire2")) && Time.unscaledTime > nextFreeze)
            {
                nextFreeze = Time.unscaledTime + freezeTimeout + timeFreezeLevel;
                GUI.Instance.SetFreezeWait(0.0f);
                StartCoroutine(FreezeTime());
            }



            if (isInvincible)
            {
                invincibleTimer -= Time.deltaTime;
                if (invincibleTimer < 0)
                {
                    isInvincible = false;
                    if (shieldActive)
                    {
                        shieldActive = false;
                        animator.SetTrigger("Unshield");
                    }

                }
            }
            if (GameObject.FindWithTag("Ennemy") == null)
            {
                inGame = false;
                GameManager.Instance.GoToNextLevel();
            }
        }
        else if (autoAnimating)
        {
            animationTimeLeft -= Time.deltaTime;
            Vector2 position = Vector2.Lerp(animationStartPosition, animationGoalPosition, (1.0f - (animationTimeLeft / animationTime)));
            rigidbody2d.MovePosition(position);

            if (animationTimeLeft < 0.0f) // End of animation
            {
                autoAnimating = false;
                if (evolving)
                {
                    Instantiate(evolutionAnimation, rigidbody2d.position, Quaternion.identity);
                    StartCoroutine(Evolve());
                }
                else
                {
                    GameManager.Instance.GamePaused = false;
                }
                if (onAnimationEnd != null) { onAnimationEnd(); onAnimationEnd = null; }
            }
        }
    }

    public void EnterLevelAndStart()
    {
        GameManager.Instance.GamePaused = true;
        AnimateTo(new Vector2(-5.0f, 0.0f), 3.5f, OnEnterLevelComplete);
    }

    public void OnEnterLevelComplete()
    {
        GameManager.Instance.GamePaused = false;
        inGame = true;
    }

    public void AnimateTo(Vector2 position, float time = 4.0f, Action afterAnimating = null)
    {
        animationTime = time;
        animationStartPosition = rigidbody2d.position;
        rigidbody2d.velocity = Vector2.zero;
        animationGoalPosition = position;
        animationTimeLeft = time;
        autoAnimating = true;
        onAnimationEnd = afterAnimating;
    }

    public int ExperienceForNextLevel()
    {
        return experienceForNextLevel;
    }

    public void AddExperience(int ammount)
    {
        experience += ammount;
        GUI.Instance.SetExperience(experience / (float)experienceForNextLevel);
        if (experience == experienceForNextLevel)
        {
            level++;
            experienceForNextLevel *= 3;
            experience = 0;
            evolutionPointsLeft += 3;
            evolving = true;
            GameManager.Instance.GamePaused = true;

            AnimateTo(Vector2.zero, 2.5f);
            SoundManager.Instance.ToggleSound(true);
            PlaySound(evolveClip);
        }
    }

    IEnumerator Evolve()
    {
        animator.SetTrigger("Evolve");
        yield return new WaitForSeconds(1.5f);
        EvolutionUI.Instance.Activate();
        yield return new WaitForSeconds(5.0f);
        SoundManager.Instance.ToggleSound(false);
    }

    public void EndEvolve()
    {
        // Destroy old childs
        for (int i = 0; i < transform.childCount; i++)
        {
            if (transform.GetChild(i).transform.name == "Protector(Clone)")
                Destroy(transform.GetChild(i).gameObject);
        }
        float thetaStep = Mathf.PI * 2 / protectorsLevel;
        float r = 1.0f;
        for (int i = 0; i < protectorsLevel; i++)
        {
            Vector2 position = new Vector2(r * Mathf.Sin(i * thetaStep), r * Mathf.Cos(i * thetaStep));
            Instantiate(protectorPrefab, rigidbody2d.position + position, Quaternion.identity, transform);
        }

        // Set all upgrades correctly
        evolving = false;
        AnimateTo(animationStartPosition);
    }

    public void UpgradeTimeFreeze()
    {
        if (timeFreezeLevel == 0)
        {
            EvolutionUI.Instance.TimeFreezeInfo.SetActive(true);
            GUI.Instance.freezeBar.SetActive(true);
            GUI.Instance.SetFreezeWait(1.0f);
        }
        timeFreezeLevel++;
        if (timeFreezeLevel == maxUpgrades)
            EvolutionUI.Instance.DisableBuyTimeFreeze();
    }

    public void UpgradeLifePoints()
    {
        lifePointsLevel++;
        maxHealth++;
        currentHealth++;
        GUI.Instance.ChangeMaxLife(1);
        if (lifePointsLevel == maxUpgrades)
            EvolutionUI.Instance.DisableBuyLifePoints();
    }

    public void UpgradeProtectors()
    {
        protectorsLevel++;
        if (protectorsLevel == maxUpgrades)
            EvolutionUI.Instance.DisableBuyProtectors();
    }

    public void UpgradeShieldTime()
    {
        shieldTimeLevel++;
        if (shieldTimeLevel == maxUpgrades)
            EvolutionUI.Instance.DisableBuyShieldTime();
    }

    public void TriggerShieldBonus()
    {
        /*  1.5 / 3 / 4.5 / 6 seconds shield */
        isInvincible = true;
        invincibleTimer = (shieldTimeLevel + 1) * 1.5f;
        if (!shieldActive)
        {
            shieldActive = true;
            animator.SetTrigger("Shield");
        }
    }

    public void TriggerShotBonus()
    {
        shotLevel++;
    }

    public void TriggerLifeBonus()
    {
        if (currentHealth < maxHealth)
        {
            currentHealth++;
            GUI.Instance.ChangeLife(1);
        }
    }

    public void Slow(float time, float factor)
    {
        if (!isInvincible && !slowed)
        {
            speed *= factor;
            slowedUntil = Time.time + time;
            slowedFactor = factor;
            slowed = true;
        }
    }

    public void Damaged(int damage)
    {
        if (!isInvincible && !autoAnimating && !evolving)
        {
            currentHealth -= damage;
            shotLevel = Math.Max(0, shotLevel - 1);
            GUI.Instance.ChangeLife(-damage);
            if (currentHealth == 0)
            {
                animator.SetTrigger("Damaged");
                for (int i = 0; i < 5; i++)
                {
                    Vector3 random = new Vector3((UnityEngine.Random.value - 0.5f) * 3.0f, (UnityEngine.Random.value - 0.5f) * 4.0f, 0.0f);
                    Instantiate(explosion, transform.position + random, Quaternion.identity);
                }
                Animations.Instance.animator.SetTrigger("GameOver");
                Destroy();
            }
            else
            {
                isInvincible = true;
                invincibleTimer = timeInvincible;
                animator.SetTrigger("Damaged");
            }

        }
    }

    public void ResetPosition()
    {
        transform.position = initialPosition;
        rigidbody2d.position = initialPosition;
    }

    IEnumerator FreezeTime()
    {
        Time.timeScale = 0.6f;
        Animations.Instance.animator.ResetTrigger("UnfreezeScreen");
        Animations.Instance.animator.SetTrigger("FreezeScreen");
        yield return new WaitForSeconds(timeFreezeLevel * freezeDuration);
        Animations.Instance.animator.SetTrigger("UnfreezeScreen");
        Time.timeScale = 1.0f;
    }

    void Shoot()
    {
        switch (shotLevel)
        {
            case 0:
                Instantiate(projectilePrefab, shotSpawn.position, Quaternion.identity);
                break;
            case 1:
                Instantiate(projectilePrefab, shotSpawn.position, Quaternion.identity);
                Instantiate(projectilePrefab, shotSpawn.position + new Vector3(0, -0.1f, 0.2f), Quaternion.Euler(new Vector3(0, 0, 20)));
                Instantiate(projectilePrefab, shotSpawn.position + new Vector3(0, -0.1f, -0.2f), Quaternion.Euler(new Vector3(0, 0, -20)));
                break;
            default:
                Instantiate(projectilePrefab, shotSpawn.position + Vector3.up * 0.2f, Quaternion.identity);
                Instantiate(projectilePrefab, shotSpawn.position + Vector3.down * 0.2f, Quaternion.identity);
                Instantiate(projectilePrefab, shotSpawn.position + new Vector3(0, -0.1f, 0.2f), Quaternion.Euler(new Vector3(0, 0, 20)));
                Instantiate(projectilePrefab, shotSpawn.position + new Vector3(0, -0.1f, -0.2f), Quaternion.Euler(new Vector3(0, 0, -20)));
                break;
        }
        int rnd = UnityEngine.Random.Range(0, projectileShotClips.Length);

        PlaySound(projectileShotClips[rnd]);
    }

    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void Destroy()
    {
        EvolutionUI.Instance.ResetUI();
        Destroy(gameObject);
    }

}
