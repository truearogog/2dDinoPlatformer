using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HittableManager : MonoBehaviour
{
    public float health;
    public GameObject particleSystemOnDeath;
    public float particleSystemTime;
    public bool particleSaveVelocity;
    private Rigidbody2D rb;
    public string soundOnDeath;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        
    }

    public void GetHit(float damage)
    {
        if (damage > 0)
        {
            health = Mathf.Max(0, health - damage);


            if (health == 0)
            {
                if (particleSystemOnDeath != null)
                {
                    GameObject parts = Instantiate(particleSystemOnDeath, transform.position, transform.rotation);
                    if (particleSaveVelocity)
                    {
                        Rigidbody2D rb = parts.AddComponent<Rigidbody2D>();
                        rb.velocity = this.rb.velocity;
                    }
                    Destroy(parts, particleSystemTime);
                }
                SoundManager.PlaySound(soundOnDeath);
                Destroy(gameObject);
            }
            else
            {
                SoundManager.PlaySound("hit");
            }
        }
    }
}
