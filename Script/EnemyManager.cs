using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyManager : MonoBehaviour
{
    public Level_9 Level;
    public List<BasicEnemy> enemyPool;
    public BasicPlayer player;
    public GameObject bossPrefab;
    public GameObject EnemyPrefab;
    public ApplicationSo AppSo;
    public Sprite virusSprite;
    private float time = 3f;
    public int enemyWave = 0;
    private int killAmount = 0;
    private int EnemyAmount = 0;
    public void Update()
    {
        time += Time.deltaTime;
        if(enemyWave == 0)
        {
            if( time > 1.5f)
            {
                time -= 2f;
                BasicEnemy enemy = GetEnemy();

                enemy.Init(GetRandomAppData(), 200, Random.Range(100, 200), GetRandomPosition(), player, this);
            }
        }else if(enemyWave == 1)
        {
            if (time > 1f)
            {
                time -= 1f;
                BasicEnemy enemy = GetEnemy();

                enemy.Init(GetRandomAppData(), 300, Random.Range(200, 300), GetRandomPosition(), player,this);
            }
        }
        else if (enemyWave == 2)
        {
            if (time > 0.75f)
            {
                time -= 0.75f;
                BasicEnemy enemy = GetEnemy();

                enemy.Init(GetRandomAppData(), 400, Random.Range(300, 500), GetRandomPosition(), player,this);
            }
        }
        else if (enemyWave == 3)
        {
            if (time > 1f)
            {
                time -= 1f;
                BasicEnemy enemy = GetEnemy();

                enemy.Init(virusSprite , GetRandomAppData().AppName, 600, Random.Range(400, 600), GetRandomPosition(), player, this);
            }
        }
        else if (enemyWave == 4)
        {
            if (time > 1.5f)
            {
                time -= 1.5f;
                BasicEnemy enemy = GetEnemy();

                enemy.Init(virusSprite, "–‹√®…’øæ", 800, Random.Range(300, 500), GetRandomPosition(), player, this);
            }
        }
    }
    public void Init(Level_9 level9)
    {
        Level = level9;
    }
    public BasicEnemy GetClosetEnemy(Vector3 position)
    {
        int index = -1;
        float Distance = Mathf.Infinity;

        for(int i = 0; i < enemyPool.Count; i++)
        {
            if (enemyPool[i].gameObject.activeSelf == true) 
            {
                float thisDist = GetDistance(enemyPool[i].transform.position, position);
                if(thisDist < Distance)
                {
                    index = i;
                    Distance = thisDist;
                }
            }
        }

        if(index == -1)
        {
            return null;
        }
        else
        {
            return enemyPool[index];
        }
    }

    public static float GetDistance(Vector3 v1, Vector3 v2)
    {
        return (v1 - v2).magnitude;
    }

    public BasicEnemy GetEnemy()
    {
        UpEnemyWave();
        for (int i = 0; i < enemyPool.Count; i ++)
        {
            if (enemyPool[i].gameObject.activeSelf == false)
            {
                return enemyPool[i];
            }
        }
        return CreateNewEnemy();
    }

    public BasicEnemy CreateNewEnemy()
    {
        GameObject obj = Instantiate(EnemyPrefab , this.transform);
        BasicEnemy enemy = obj.GetComponent<BasicEnemy>();
        enemy.gameObject.SetActive(false);
        enemyPool.Add(enemy);
        obj.name = obj.name.Replace("(Clone)", enemyPool.Count.ToString());
        return enemy;
    }
    public BossEnemy CreateBossEnemy()
    {
        GameObject obj = Instantiate(bossPrefab, Level.canvas.transform);
        BossEnemy enemy = obj.GetComponent<BossEnemy>();
        enemy.gameObject.SetActive(false);
        enemyPool.Add(enemy);
        obj.name = obj.name.Replace("(Clone)", enemyPool.Count.ToString());
        return enemy;
    }

    public AppData GetRandomAppData()
    {
        int count = AppSo.apps.Count;
        return AppSo.apps[Random.Range(0, count)];
    }
    public void CreateBoss()
    {
        BossEnemy enemy = CreateBossEnemy();
        Image icon = Level.GetSystemIcon();
        RectTransform recttran = icon.gameObject.GetComponent<RectTransform>();
        icon.gameObject.SetActive(false);
        enemy.Init(DesktopManager.GetCentrePosition(recttran), player, this);
    }
    public Vector3 GetRandomPosition()
    {
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 topLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0));

        Vector3 pos = new Vector3(Random.Range(topLeft.x, topRight.x), topRight.y + Random.Range(100,150), 0);
        return pos;
    }


    public void AddKillAmount(int amount)
    {
        killAmount += amount;
        if(killAmount > 15)
        {
            killAmount -= 15;
            player.UpGrade();
        }
    }

    public void UpEnemyWave()
    {
        EnemyAmount++;
        if(enemyWave == 0 && EnemyAmount > 10)
        {
            Level.IntoWave_1();
            SetEnemyWave(1);
            EnemyAmount = 0;
        }
        else if (enemyWave == 1 && EnemyAmount > 30)
        {
            Level.IntoWave_2();
            SetEnemyWave(2);
            EnemyAmount = 0;
        }
        else if (enemyWave == 2 && EnemyAmount > 40)
        {
            Level.IntoWave_3();
            SetEnemyWave(3);
            EnemyAmount = 0;
        }
        else if (enemyWave == 3 && EnemyAmount > 50)
        {
            Level.IntoWave_4();
            SetEnemyWave(4);
            EnemyAmount = 0;
        }
    }

    public void SetEnemyWave(int wave)
    {
        StartCoroutine(WhenSetEnemyWave(wave));
    }

    IEnumerator WhenSetEnemyWave( int wave)
    {
        enemyWave = -1;
        yield return new WaitForSeconds(5f);
        enemyWave = wave;
    }
}
