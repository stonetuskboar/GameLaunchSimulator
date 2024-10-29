using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BossEnemy : BasicEnemy
{
    public Image image;
    private float IdleTime = 0;
    public BossStatue statue;
    public GameObject WarnBulletPrefab;
    public GameObject ErrorBulletPrefab;
    public GameObject WaitBulletPrefab;
    private Transform canvasTransform;
    public override void Awake()
    {
        material = image.material;
        rb2D = GetComponent<Rigidbody2D>();
        strength = 250;
    }
    // Start is called before the first frame update
    public override void FixedUpdate()
    {
        if (Player != null )
        {
            if(statue != BossStatue.attack3)
            {
                if ((Player.transform.position - transform.position).magnitude > 1080)
                {
                    ChasePlayer();
                }
            }

            if(statue == BossStatue.idle)
            {
                IdleTime += Time.fixedDeltaTime;
                if(IdleTime > 0.5f)
                {
                    IdleTime = 0;
                    DecideAttack();
                }
            }
        }
    }
    public void Init(Vector3 position, BasicPlayer player, EnemyManager manager) //ÔÚDesktopManager´¦´¥·¢
    {
        transform.position = position;
        transform.rotation = Quaternion.identity;
        Player = player;
        enemyManager = manager;
        nowHp = maxHp;
        canvasTransform = enemyManager.Level.canvas.transform;
        if (material != null)
        {
            material.SetInt("_Boolean", 0);
        }
        gameObject.SetActive(true);
    }
    public void ChasePlayer()
    {
        rb2D.velocity += (Vector2)(Player.transform.position - transform.position).normalized * (Speed * Time.fixedDeltaTime);
        if (rb2D.velocity.magnitude > Speed)
        {
            rb2D.velocity = rb2D.velocity.normalized * Speed;
        }
    }

    public void DecideAttack()
    {
        int rand = Random.Range(1, 5);

        if (rand == 1)
        {
            StartCoroutine(Attack1());
        }else if (rand == 2)
        {
            StartCoroutine (Attack2());
        }
        else if (rand == 3)
        {
            StartCoroutine (Attack3());
        }else
        {

        }
    }
    public override void GetDamage(float damage, Vector2 force)
    {
        nowHp -= damage;
        image.fillAmount = 0.5f + 0.5f *(nowHp / nowHp);
        rb2D.AddForce(force/4, ForceMode2D.Impulse);
        AudioManager.Instance.PlayHurtSfx();
        if (nowHp < 0)
        {
            enemyManager.Level.Wining();
            gameObject.SetActive(false);
            enemyManager.AddKillAmount(1);
        }
        else
        {
            StartCoroutine(Flash());
        }
    }

    IEnumerator Attack1()
    {
        statue = BossStatue.attack1;
        Color ogcolor = image.color;
        image.color = ogcolor / 2 + new Color(0, 0, 0, 1f);
        float attackTime = 0f;
        for (float time = 0; time < 2f; time += Time.deltaTime )
        {
            attackTime += Time.deltaTime;
            if(attackTime > 0.5f)
            {
                attackTime -= 0.5f;
                Vector3 delta = Player.transform.position - transform.position;
                Vector3 normal = new Vector3(- delta.y, delta.x, delta.z).normalized;
                Vector3 StartPos = transform.position + normal * Random.Range(-400f, 400f);
                GameObject obj = Instantiate(WarnBulletPrefab, canvasTransform);
                BossBullet bullet = obj.GetComponent<BossBullet>();
                bullet.Init(Damage* 2, strength, StartPos, delta.normalized * 1000);
            }
            yield return null;
        }
        image.color = ogcolor;
        statue = BossStatue.idle;
    }

    IEnumerator Attack2()
    {
        statue = BossStatue.attack2;
        float attackTime = 0f;
        for (float time = 0; time < 2f; time += Time.deltaTime)
        {
            rb2D.angularVelocity += time  * Time.deltaTime *  360;
            yield return null;
        }
        for (float time = 0; time < 3f; time += Time.deltaTime)
        {
            if(rb2D.angularVelocity < 720)
            {
                rb2D.angularVelocity +=  Time.deltaTime * 360;
            }
            attackTime += Time.deltaTime;
            while(attackTime > 0.1f)
            {
                attackTime -= 0.1f;
                Vector3 delta = Player.transform.position - transform.position;
                Vector3 normal = new Vector3(-delta.y, delta.x, delta.z).normalized;
                Vector3 StartPos = transform.position + normal * Random.Range(-200f, 200f);
                Vector3 targetPos = delta + normal * Random.Range(-600f, 600f);
                GameObject obj = Instantiate(ErrorBulletPrefab, enemyManager.Level.canvas.transform);
                BossBullet bullet = obj.GetComponent<BossBullet>();
                bullet.Init(Damage, strength/ 2 , StartPos, targetPos.normalized * 1400);
            }
            yield return null;
        }
        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            if(rb2D.angularVelocity > 0.1)
            {
                rb2D.angularVelocity -= 2 * Time.deltaTime * 360;
            }
            yield return null;
        }
        rb2D.angularVelocity = 0;
        statue = BossStatue.idle;
    }

    IEnumerator Attack3()
    {
        statue = BossStatue.attack3;
        float attackTime = 0f;

        Vector3 ogEulerAngles = transform.localEulerAngles;
        rb2D.velocity = Vector3.zero;
        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            transform.localEulerAngles += new Vector3(0, time * Time.deltaTime * 360, 0);
            yield return null;
        }
        for (float time = 0; time < 5f; time += Time.deltaTime)
        {
            transform.localEulerAngles += new Vector3(0 , Time.deltaTime * 360, 0);
            attackTime += Time.deltaTime;
            if (attackTime > 0.05f)
            {
                attackTime -= 0.05f;
                Vector3 delta = Player.transform.position - transform.position;
                GameObject obj = Instantiate(WaitBulletPrefab, enemyManager.Level.canvas.transform);
                BossBullet bullet = obj.GetComponent<BossBullet>();
                bullet.Init(Damage, strength / 4, transform.position, delta.normalized * 1000);
                bullet.rb.angularVelocity = 720;
            }
            yield return null;
        }
        for (float time = 0; time < 1f; time += Time.deltaTime)
        {
            if (transform.localEulerAngles.x > 5)
            {
                transform.localEulerAngles -= new Vector3( 0, time * Time.deltaTime * 360, 0);
            }
            yield return null;
        }
        transform.localEulerAngles=  new Vector3( 0 , 0 , transform.localEulerAngles.z);
        statue = BossStatue.idle;
    }
}

public enum BossStatue
{
    idle = 0,
    attack1 = 1,
    attack2 = 2,
    attack3 = 3,
}