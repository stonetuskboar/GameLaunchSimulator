using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBullet : MonoBehaviour
{
    public Rigidbody2D rb;
    public float Damage = 20;
    private float time = 0;
    private float Strength = 150;
    public void Init(float damage, float strength, Vector3 position, Vector2 speed)
    {
        rb.velocity = speed;
        time = 0f;
        Damage = damage;
        Strength = strength;
        transform.position = position;
        gameObject.SetActive(true);
    }

    public void Update()
    {
        time += Time.deltaTime;
        if (time > 5f)
        {
            Destroy(gameObject);
        }
    }

    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") == true)
        {
            if (collision.gameObject.TryGetComponent<BasicPlayer>(out var player))
            {
                if (player.isInvincible == false)
                {
                    Vector2 force = (collision.transform.position - transform.position).normalized * Strength;
                    player.GetDamage(Damage, force);
                }
            }
        }
    }
}
