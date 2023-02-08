using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthCollectible : MonoBehaviour
{
    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //GameObject obj = collision.gameObject;
        //if(obj.tag == "Character")
        //{
        //    obj.GetComponent<RubyController>().changeCurrentHealth(1);
        //    Destroy(gameObject);
        //}

        RubyController controller = collision.GetComponent<RubyController>();
        if(controller != null)
        {
            if(controller.health < controller.maxHealth)
            {
                controller.changeCurrentHealth(1);
                Destroy(gameObject);
                //controller.PlaySound(collectedClip);
                AudioManager.Instance.PlaySound(AudioManager.Instance.collectedClip);
            }
        }
    }
}
