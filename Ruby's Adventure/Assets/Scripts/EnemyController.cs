using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    [SerializeField]
    float speed = 4.0f;

    Rigidbody2D _body;

    [SerializeField]
    private bool vertical;

    float timer;
    int direction = 1;

    Animator animator;

    bool isBroken = true;

    public ParticleSystem smokeEffect;

    AudioSource audioSource;
    public AudioClip fixedClip;

    // Start is called before the first frame update
    void Start()
    {
        _body = gameObject.GetComponent<Rigidbody2D>();
        timer = Random.Range(2, 5);
        vertical = (Random.Range(0, 2) == 0) ? true : false;
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        if(!isBroken)
        {
            return;
        }

        timer -= Time.deltaTime;

        if(timer < 0)
        {
            direction = -direction;
            timer = Random.Range(2, 5);
            vertical = (Random.Range(0, 2) == 0) ? true : false; //Random 0 or 1
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(!isBroken)
        {
            return;
        }
        
        Vector2 position = _body.position;

        if (vertical)
        {
            position.y += speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            position.x += speed * Time.deltaTime * direction;
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }
        _body.MovePosition(position);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        RubyController controller = collision.collider.GetComponent<RubyController>();
        if(controller != null)
        {
            controller.changeCurrentHealth(-1);
        }
    }

    public void Fix()
    {
        isBroken = false;
        _body.simulated = false;
        animator.SetTrigger("Fixed");
        smokeEffect.Stop();
        audioSource.loop = false;
        audioSource.clip = fixedClip;
        audioSource.Play();
    }
}
