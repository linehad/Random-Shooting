using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public string enemyName;
    public int enemyScroe;
    public float speed;
    public int health;
    public int bossDeathCount;
    public Sprite[] sprites;

    public float maxShotDealay;
    public float curShotDealay;

    public GameObject player;
    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject itemCoin;
    public GameObject itemPower;
    public GameObject itemBoom;
    public ObjectManager objectManager;
    public GameManager gameManager;

    SpriteRenderer spriteRenderer;
    Animator anim;

    public int patternIndex;
    public int curPatternCount;
    public int[] maxPatternCount;

    int LDeathCount = 0;
    int MDeathCount = 0;
    int SDeathCount = 0;
    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (enemyName == "B")
        {
            anim = GetComponent<Animator>();
        }
    }

    void OnEnable()
    {
        switch (enemyName)
        {
            case "B":
                health = 1000 + bossDeathCount;
                bossDeathCount = bossDeathCount + 1000;
                Invoke("Stop", 2);
                break;
            case "L":               
                health = 20 + LDeathCount;
                LDeathCount = LDeathCount + 10;
                break;
            case "M":
                health = 7 + MDeathCount;
                MDeathCount = MDeathCount + 3;
                break;
            case "S":
                health = 2 + SDeathCount;
                SDeathCount = SDeathCount + 1;
                break;
        }
    }

    void Stop()
    {
        if (!gameObject.activeSelf)
        {
            return;
        }

        Rigidbody2D rigid = GetComponent<Rigidbody2D>();
        rigid.velocity = Vector2.zero;

        Invoke("Think", 2);
    }

    void Think()
    {
        curPatternCount = 0;
        patternIndex = patternIndex == 3 ? 0 : patternIndex + 1;

        switch (patternIndex)
        {
            case 0:
                FireFoward();
                break;
            case 1:
                FireShot();
                break;
            case 2:
                FireArc();
                break;
            case 3:
                FireAround();
                break;
        }
    }

    void FireFoward()
    {
        //앞으로 4발 발사

        if (health <= 0) return;
        
        GameObject bulletR = objectManager.MakeObj("Bullet Boss A");
        bulletR.transform.position = transform.position + Vector3.right * 0.3f;
        GameObject bulletRR = objectManager.MakeObj("Bullet Boss A");
        bulletRR.transform.position = transform.position + Vector3.right * 0.45f;
        GameObject bulletL = objectManager.MakeObj("Bullet Boss A");
        bulletL.transform.position = transform.position + Vector3.left * 0.3f;
        GameObject bulletLL = objectManager.MakeObj("Bullet Boss A");
        bulletLL.transform.position = transform.position + Vector3.left * 0.45f;

        Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
        Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

        rigidR.AddForce(Vector2.down * 9, ForceMode2D.Impulse);
        rigidRR.AddForce(Vector2.down * 9, ForceMode2D.Impulse);
        rigidL.AddForce(Vector2.down * 9, ForceMode2D.Impulse);
        rigidLL.AddForce(Vector2.down * 9, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireFoward", 2);
        }
        else
        {
            Invoke("Think", 3);
        }       
    }

    void FireShot()
    {
        //랜덤하게 퍼지는 총알 발사

        if (health <= 0) return;

        for (int index=0; index < 10; index++)
        {
            GameObject bullet = objectManager.MakeObj("Bullet Enemy B");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = player.transform.position - transform.position;
            Vector2 ranVec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
            dirVec += ranVec;

            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireShot", 3.5f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireArc()
    {
        //물줄기로 뿌리는 총알 발사

        if (health <= 0) return;

        GameObject bullet = objectManager.MakeObj("Bullet Enemy A");
        bullet.transform.position = transform.position;
        bullet.transform.rotation = Quaternion.identity;

        Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

        Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 10 * curPatternCount / maxPatternCount[patternIndex]), -1);

        rigid.AddForce(dirVec.normalized * 10, ForceMode2D.Impulse);

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireArc", 0.15f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void FireAround()
    {
        //원 형태의 전체 공격

        if (health <= 0) return;

        int roundNumA = 50;
        int roundNumB = 40;
        int roundNum = curPatternCount % 2 == 0 ? roundNumA : roundNumB;

        for (int index=0; index< roundNum; index++)
        {
            GameObject bullet = objectManager.MakeObj("Bullet Boss B");
            bullet.transform.position = transform.position;
            bullet.transform.rotation = Quaternion.identity;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector2 dirVec = new Vector2(Mathf.Cos(Mathf.PI * 2 * index / roundNum), Mathf.Sin(Mathf.PI * 2 * index / roundNum));

            rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse);

            Vector3 rotVec = Vector3.forward * 360 * index / roundNum + Vector3.forward * 90;
            bullet.transform.Rotate(rotVec);
        }

        curPatternCount++;

        if (curPatternCount < maxPatternCount[patternIndex])
        {
            Invoke("FireAround", 0.7f);
        }
        else
        {
            Invoke("Think", 3);
        }
    }

    void Update()
    {
        if (enemyName == "B")
        {
            return;
        }
        Fire();
        Reload();
    }

    public void OnHit(int dmg)
    {
        if (health <= 0)
        {
            return;
        }
        health -= dmg;

        if (enemyName == "B")
        {
            anim.SetTrigger("OnHit");
        }
        else
        {
            spriteRenderer.sprite = sprites[1];
            Invoke("ReturnSprite", 0.1f);
        }
        

        if (health <= 0)
        {
            Player playerLogic = player.GetComponent<Player>();
            playerLogic.score += enemyScroe;

            //Random Item Drop
            int ran = enemyName == "B" ? 0 : Random.Range(0, 100);
            if (ran < 30)
            {               
                //Coin Drop 30%
                GameObject itemCoin = objectManager.MakeObj("Item Coin");
                itemCoin.transform.position = transform.position;
            }
            else if(ran < 85)
            {
                Debug.Log("No Item");
            }
            else if (ran < 90)
            {
                //Power Drop 3%
                GameObject itemPower = objectManager.MakeObj("Item Power");
                itemPower.transform.position = transform.position;
            }
            else if (ran < 100)
            {
                //Boom Drop 3%
                GameObject itemBoom = objectManager.MakeObj("Item Boom");
                itemBoom.transform.position = transform.position;
            }

            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
            gameManager.CallExplosion(transform.position, enemyName);

            //Boss Kill
            if (enemyName == "B")
            {
                gameManager.StageEnd();
            }
        }
    }

    void ReturnSprite()
    {
        spriteRenderer.sprite = sprites[0];
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "BorderBullet" && enemyName != "B")
        {
            gameObject.SetActive(false);
            transform.rotation = Quaternion.identity;
        }

        else if (collision.gameObject.tag == "PlayerBullet")
        {
            Bullet bullet = collision.gameObject.GetComponent<Bullet>();
            OnHit(bullet.dmg);

            collision.gameObject.SetActive(false);
        }
    }
    
    void Fire()
    {
        if (curShotDealay < maxShotDealay)
        {
            return;
        }

        if (enemyName == "S")
        {
            GameObject bullet = objectManager.MakeObj("Bullet Enemy A");
            bullet.transform.position = transform.position;

            Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();

            Vector3 dirVec = player.transform.position - transform.position;
            rigid.AddForce(dirVec.normalized * 5, ForceMode2D.Impulse);
        }

        else if(enemyName == "L")
        {
            GameObject bulletR = objectManager.MakeObj("Bullet Enemy B");
            bulletR.transform.position = transform.position + Vector3.right * 0.3f;

            GameObject bulletL = objectManager.MakeObj("Bullet Enemy B");
            bulletL.transform.position = transform.position + Vector3.left * 0.3f;

            Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
            Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

            Vector3 dirVecR = player.transform.position - (transform.position + Vector3.right * 0.3f);
            Vector3 dirVecL = player.transform.position - (transform.position + Vector3.left * 0.3f);

            rigidR.AddForce(dirVecR.normalized * 4, ForceMode2D.Impulse);
            rigidL.AddForce(dirVecL.normalized * 4, ForceMode2D.Impulse);
        }

        curShotDealay = 0;
    }

    void Reload()
    {
        curShotDealay += Time.deltaTime;
    }
}
