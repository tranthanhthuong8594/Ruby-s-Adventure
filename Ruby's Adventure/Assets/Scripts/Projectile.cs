using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    Rigidbody2D _body;
    [SerializeField]
    float timeDestroy = 4.0f;
    // Start is called before the first frame update
    private void Awake()
    {
        _body = GetComponent<Rigidbody2D>();
        Destroy(gameObject, timeDestroy);
    }

    private void Update() {
        // if(transform.position.magnitude > 100.0f)
        // {
        //     Destroy(gameObject);
        // }
            
    }

    public void Launch(Vector2 direction, float force)
    {
        _body.AddForce(direction * force);
    }

    void OnCollisionEnter2D(Collision2D other) 
    {
            Debug.Log("Projectile Collision with " + other.gameObject);
            EnemyController e = other.gameObject.GetComponent<EnemyController>();
            if(e != null)
            {
                e.Fix();
            }

            Destroy(gameObject);
    }

}