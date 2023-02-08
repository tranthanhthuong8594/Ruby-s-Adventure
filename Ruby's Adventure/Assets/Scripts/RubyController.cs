using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RubyController : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;
    const int MINHEALTH = 0;
    public int maxHealth = 5;
    
    int currentHealth;
    public int health { get { return currentHealth; } }

    Rigidbody2D rigidbody2d;
    float horizontal;
    float vertical;

    public float timeInvincible = 2.0f;
    bool isInvincible;
    float invincibleTimer;

    Animator animator;

    Vector2 lookDirection = new Vector2(1,0);
    public GameObject projectilePrefab;
    float timeLaunch = 2.0f;
    float launchTimer;
    bool isLaunch = true;

    public ParticleSystem hitEffect;

    AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip throwClip;

    bool isMove = false;

    // Start is called before the first frame update
    void Start()
    {
        rigidbody2d = GetComponent<Rigidbody2D>();
        currentHealth = maxHealth;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        //Vector2 position = transform.position;
        //position.x = position.x + 0.1f;
        //transform.position = position;

        //transform.position = new Vector2(transform.position.x + speed * Time.deltaTime, transform.position.y);

        horizontal = Input.GetAxis("Horizontal");
        vertical = Input.GetAxis("Vertical");

        //Vector2 position = transform.position;
        //position.x = position.x + 3f * horizontal * Time.deltaTime;
        //position.y = position.y + 3f * vertical * Time.deltaTime;
        //transform.position = position;

        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        Vector2 move = new Vector2(horizontal, vertical);
        if(!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
            if(!audioSource.isPlaying)
            {
                audioSource.Play();
            }
        }
        else
        {
            audioSource.Stop();
        }

        animator.SetFloat("Look X", lookDirection.x);
        animator.SetFloat("Look Y", lookDirection.y);
        animator.SetFloat("Speed", move.magnitude);

        UpdateLaunch();
    }

    private void UpdateLaunch()
    {
        if(isLaunch)
        {
            if(Input.GetKeyDown(KeyCode.C))
            {
                Launch();
                isLaunch = false;
                launchTimer = timeLaunch;
            }
        }
        else
        {
            launchTimer -= Time.deltaTime;
            if(launchTimer < 0)
            {
                isLaunch = true;
            }
        }

        if(Input.GetKeyDown(KeyCode.X))
        {
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2d.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
            if(hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if(character != null)
                {
                    character.DisplayDialog();
                }
            }
        }
    }

    private void FixedUpdate()
    {
        Vector2 position = rigidbody2d.position;
        position.x = position.x + speed * horizontal * Time.deltaTime;
        position.y = position.y + speed * vertical * Time.deltaTime;
        rigidbody2d.MovePosition(position);
    }

    public void changeCurrentHealth(int amount)
    {
        if(amount < 0)
        {
            if (isInvincible)
                return;

            isInvincible = true;
            invincibleTimer = timeInvincible;
            animator.SetTrigger("Hit");
            Instantiate(hitEffect.gameObject, rigidbody2d.position + Vector2.up * 1.0f, Quaternion.identity);
            AudioManager.Instance.PlaySound(AudioManager.Instance.hitClip);
        }
        
        currentHealth = Mathf.Clamp(currentHealth + amount, MINHEALTH, maxHealth);
        //Debug.Log("Ruby's Current Health: " + currentHealth + "/" + maxHealth);
        UIHealthBar.instance.SetValue(currentHealth/(float)maxHealth);
    }

    void Launch()
    {
        GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2d.position + Vector2.up * 0.5f, Quaternion.identity);

        Projectile projectile = projectileObject.GetComponent<Projectile>();
        projectile.Launch(lookDirection, 300);

        animator.SetTrigger("Launch");
        AudioManager.Instance.PlaySound(AudioManager.Instance.throwClip);
    }

    
}
