using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBullet : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public float Damage = 20;
    private float time = 0;
    private float Strength = 150;
    private Vector2 Speed = Vector2.zero;
    public void Init(Sprite sprite, float damage,float strength, Vector3 position, Vector2 speed)
    {
        time = 0f;
        spriteRenderer.sprite = sprite;
        Damage = damage;
        Speed = speed;
        Strength = strength;
        transform.position = position;  
        gameObject.SetActive(true);
    }

    public void Update()
    {
        transform.position += (Vector3)Speed * Time.deltaTime;
        time += Time.deltaTime;
        if(time > 5f) 
        { 
            gameObject.SetActive(false);
        }
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy") == true && gameObject.activeSelf == true)
        {
            if(collision.TryGetComponent<BasicEnemy>(out var enemy))
            {
                Vector2 force = Speed.normalized * Strength;
                enemy.GetDamage(Damage,force);
                gameObject.SetActive(false);
            }
        }
    }
}
