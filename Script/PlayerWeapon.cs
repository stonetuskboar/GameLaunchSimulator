using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeapon : MonoBehaviour
{
    public List<PlayerBullet> bulletsPool;
    public GameObject BulletPrefab;
    public Transform BulletPrefabParentTrans;

    public List<Sprite> sprites;
    public float fireRate = 5;
    public float Strength = 100;
    public float Damage = 40;
    public EnemyManager enemyManager;
    private BasicEnemy TargetEnemy;
    private float time = 0f;

    public void Update()
    {
        time += Time.deltaTime; 
        while(time > 1/fireRate)
        {
            if(TargetEnemy != null && TargetEnemy.gameObject.activeSelf != false)
            {
                Vector3 speed = (TargetEnemy.transform.position - transform.position).normalized * 1000;
                PlayerBullet bullet = GetBullet();
                bullet.Init(GetRandomSprite(), Damage ,Strength, transform.position, speed);
                AudioManager.Instance.PlaySfxByName("Shoot");
            }
            else
            {
                SetTarget();
            }
            time -= 1 / fireRate;
        }
    }

    public Sprite GetRandomSprite()
    {
        return sprites[Random.Range(0, sprites.Count)];
    }

    public void SetTarget()
    {
        BasicEnemy target = enemyManager.GetClosetEnemy(transform.position);
        TargetEnemy = target;
    }

    public PlayerBullet GetBullet()
    {
        for (int i = 0; i < bulletsPool.Count; i++)
        {
            if (bulletsPool[i].gameObject.activeSelf == false)
            {
                return bulletsPool[i];
            }
        }
        return CreateBullet();
    }

    public PlayerBullet CreateBullet()
    {
        GameObject obj = Instantiate(BulletPrefab, BulletPrefabParentTrans);
        obj.SetActive(false);
        PlayerBullet bullet = obj.GetComponent<PlayerBullet>();
        bulletsPool.Add(bullet);
        obj.name = obj.name.Replace("(Clone)", bulletsPool.Count.ToString());
        return bullet;
    }

    public void UpGrade()
    {
        fireRate += 1;
        Strength += 20;
        Damage += 5;
    }
}
