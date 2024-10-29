using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BasicEnemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public TextMeshPro textMeshPro;
    protected float nowHp;
    public float maxHp;
    public float Speed;
    protected float strength = 150;
    public float Damage = 30;
    protected Rigidbody2D rb2D;
    protected BasicPlayer Player;
    protected Material material = null;
    protected EnemyManager enemyManager;
    public virtual void Awake()
    {
        material = spriteRenderer.material;
        rb2D = GetComponent<Rigidbody2D>();
    }


    public virtual void FixedUpdate()
    {
        if(Player != null)
        {
            rb2D.velocity += (Vector2)(Player.transform.position - transform.position).normalized * (Speed * Time.fixedDeltaTime);
            if(rb2D.velocity.magnitude > Speed )
            {
                rb2D.velocity = rb2D.velocity.normalized * Speed;
            }
        }
    }
    public void Init(Sprite sprite , string text, float Hp, float speed, Vector3 position, BasicPlayer player, EnemyManager manager) 
    {
        AppData data = new AppData(text);
        data.AppSprite = sprite;
        Init(data ,  Hp, speed, position, player, manager);
    }
    public void Init( AppData data , float Hp, float speed,Vector3 position , BasicPlayer player , EnemyManager manager) 
    {
        spriteRenderer.sprite = data.AppSprite;
        textMeshPro.text = data.AppName;
        maxHp = Hp;
        nowHp = maxHp;
        Speed = speed;
        transform.position = position; 
        transform.rotation = Quaternion.identity;
        Player = player;
        enemyManager = manager;
        if(material != null)
        {
            material.SetInt("_Boolean", 0);
        }
        gameObject.SetActive(true);
    }

    public virtual void GetDamage(float damage , Vector2 force)
    {
        nowHp -= damage;
        rb2D.AddForce(force , ForceMode2D.Impulse);
        AudioManager.Instance.PlayHurtSfx();
        if (nowHp < 0)
        {
            gameObject.SetActive(false);
            enemyManager.AddKillAmount(1);
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    public IEnumerator Flash()
    {
        material.SetInt("_Boolean", 1);
        yield return new WaitForSeconds(0.1f);
        material.SetInt("_Boolean", 0);
    }


    public void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player") == true)
        {
            if (collision.gameObject.TryGetComponent<BasicPlayer>(out var player))
            {
                if(player.isInvincible == false)
                {
                    Vector2 force = (collision.transform.position - transform.position).normalized * strength;
                    player.GetDamage(Damage, force);
                }
            }
        }
    }


}
