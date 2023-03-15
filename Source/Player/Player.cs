using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public bool isTouch_TopWall;
    public bool isTouch_BottomWall;
    public bool isTouch_LeftWall;
    public bool isTouch_RightWall;

    public int life;
    public int score;
    public int boomDmg = 1000;

    public float speed;
    public int bullet_Power;
    public int boom_Num;
    public int max_Boom;
    public int max_bullet_Power;
    public float maxShotDealay;
    public float curShotDealay;

    public GameObject bulletObjA;
    public GameObject bulletObjB;
    public GameObject boomEffect;

    public GameManager gameManager;
    public ObjectManager objectManager;

    public bool isHit;
    public bool isBoomTime;

    public bool isRespawnTime;

    Animator anim;
    SpriteRenderer spriteRenderer;

    void Awake()
    {
        anim = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        gameManager.UpdateLifeIcon(life);
        gameManager.UpdateBoomIcon(boom_Num);
    }
    void OnEnable()
    {
        Unbeatavle();
        Invoke("Unbeatavle", 3);
    }

    void Unbeatavle()
    {
        isRespawnTime = !isRespawnTime;

        if (isRespawnTime)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            StartCoroutine(ShowReady());
        }
        else
        {
            spriteRenderer.color = new Color(1, 1, 1, 1);
        }
    }
    IEnumerator ShowReady()
    {
        int count = 0;
        while (count < 12)
        {
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = new Color(1, 1, 1, 1);
            yield return new WaitForSeconds(0.1f);
            count++;
        }
    }
    void Update()
    {
        Move();
        Fire();
        Reload();
        Boom();
    }

    void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");

        if ((isTouch_RightWall && h == 1) || (isTouch_LeftWall && h == -1))
        {
            h = 0;
        }


        float v = Input.GetAxisRaw("Vertical");

        if ((isTouch_TopWall && v == 1) || (isTouch_BottomWall && v == -1))
        {
            v = 0;
        }

        Vector3 curPos = transform.position;
        Vector3 nextPos = new Vector3(h, v, 0) * speed * Time.deltaTime;

        transform.position = curPos + nextPos;

        if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
        {
            anim.SetInteger("Input", (int)h);
        }
    }

    void Fire()
    {
        if (!Input.GetButton("Fire1"))
        {
            return;
        }

        if (curShotDealay < maxShotDealay)
        {
            return;
        }

        switch (bullet_Power)
        {
            case 1:
                GameObject bullet = objectManager.MakeObj("Bullet Player A");
                bullet.transform.position = transform.position;

                Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
                rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 2:
                GameObject bulletR = objectManager.MakeObj("Bullet Player A");
                bulletR.transform.position = transform.position + Vector3.right * 0.1f;

                GameObject bulletL = objectManager.MakeObj("Bullet Player A");
                bulletL.transform.position = transform.position + Vector3.left * 0.1f;

                Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

                rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);                
                rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 3:
                GameObject bulletRR = objectManager.MakeObj("Bullet Player A");
                bulletRR.transform.position = transform.position + Vector3.right * 0.15f;

                GameObject bulletC = objectManager.MakeObj("Bullet Player A");
                bulletC.transform.position = transform.position;

                GameObject bulletLL = objectManager.MakeObj("Bullet Player A");
                bulletLL.transform.position = transform.position + Vector3.left * 0.15f;

                Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidC = bulletC.GetComponent<Rigidbody2D>();
                Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

                rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;
            case 4:
                GameObject bulletCC = objectManager.MakeObj("Bullet Player B");
                bulletCC.transform.position = transform.position;

                Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
                rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
                break;

        }

        curShotDealay = 0;
    }

    void Boom()
    {
        if (!Input.GetButton("Fire2"))
        {
            return;
        }

        if (isBoomTime)
        {            
            return;
        }

        if (boom_Num == 0)
        {
            return;
        }

        boom_Num--;
        isBoomTime = true;
        gameManager.UpdateBoomIcon(boom_Num);

        boomEffect.SetActive(true);
        Invoke("OffBoomEffect", 2f);

        //Remove Enemy

        GameObject[] enemiesL = objectManager.GetPool("Enemy L");
        GameObject[] enemiesM = objectManager.GetPool("Enemy M");
        GameObject[] enemiesS = objectManager.GetPool("Enemy S");
        GameObject[] enemiesB = objectManager.GetPool("Enemy B");

        for (int index = 0; index < enemiesL.Length; index++)
        {
            if (enemiesL[index].activeSelf)
            {
                Enemy enemyLogic = enemiesL[index].GetComponent<Enemy>();
                enemyLogic.OnHit(boomDmg);
            }          
        }

        for (int index = 0; index < enemiesM.Length; index++)
        {
            if (enemiesM[index].activeSelf)
            {
                Enemy enemyLogic = enemiesM[index].GetComponent<Enemy>();
                enemyLogic.OnHit(boomDmg);
            }
        }

        for (int index = 0; index < enemiesS.Length; index++)
        {
            if (enemiesS[index].activeSelf)
            {
                Enemy enemyLogic = enemiesS[index].GetComponent<Enemy>();
                enemyLogic.OnHit(boomDmg);
            }
        }

        for (int index = 0; index < enemiesB.Length; index++)
        {
            if (enemiesB[index].activeSelf)
            {
                Enemy enemyLogic = enemiesB[index].GetComponent<Enemy>();
                enemyLogic.OnHit(boomDmg);
            }
        }

        //Remove Enemy Bullet

        GameObject[] bulletsA = objectManager.GetPool("Bullet Enemy A");
        GameObject[] bulletsB = objectManager.GetPool("Bullet Enemy B");
        GameObject[] bulletsC = objectManager.GetPool("Bullet Boss A");
        GameObject[] bulletsD = objectManager.GetPool("Bullet Boss B");

        for (int index = 0; index < bulletsA.Length; index++)
        {
            if (bulletsA[index].activeSelf)
            {
                bulletsA[index].SetActive(false);
            }           
        }

        for (int index = 0; index < bulletsB.Length; index++)
        {
            if (bulletsB[index].activeSelf)
            {
                bulletsB[index].SetActive(false);
            }
        }

        for (int index = 0; index < bulletsC.Length; index++)
        {
            if (bulletsC[index].activeSelf)
            {
                bulletsC[index].SetActive(false);
            }
        }

        for (int index = 0; index < bulletsD.Length; index++)
        {
            if (bulletsD[index].activeSelf)
            {
                bulletsD[index].SetActive(false);
            }
        }
    }

    void Reload()
    {
        curShotDealay += Time.deltaTime;
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top Wall":
                    isTouch_TopWall = true;
                    break;
                case "Bottom Wall":
                    isTouch_BottomWall = true;
                    break;
                case "Left Wall":
                    isTouch_LeftWall = true;
                    break;
                case "Right Wall":
                    isTouch_RightWall = true;
                    break;
            }
        }
        
        else if(collision.gameObject.tag == "Enemy"|| collision.gameObject.tag == "EnemyBullet")
        {
            if (isRespawnTime)
            {
                return;
            }
            if (isHit == true) 
            {
                return;
            }

            isHit = true;
            life--;
            gameManager.UpdateLifeIcon(life);
            gameManager.CallExplosion(transform.position, "P");

            if (life == 0)
            {
                gameManager.GameOver();
            }
            else
            {
                gameManager.ReSpawnPlayer();
            }          

            gameObject.SetActive(false);
            collision.gameObject.SetActive(false);
        }
        else if(collision.gameObject.tag == "Item")
        {
            Item item = collision.gameObject.GetComponent<Item>();
            switch (item.type)
            {
                case "Coin":
                    score += 1000;
                    break;
                case "Power":
                    if (bullet_Power == max_bullet_Power)
                    {
                        score += 500;
                    }
                    else
                        bullet_Power++;
                    break;
                case "Boom":
                    if (boom_Num == max_Boom)
                    {
                        score += 500;
                    }
                    else
                    {
                        boom_Num++;
                        gameManager.UpdateBoomIcon(boom_Num);
                    }                       
                    break;
            }
                collision.gameObject.SetActive(false);        
        }
    }

    void OffBoomEffect()
    {
        boomEffect.SetActive(false);
        isBoomTime = false;
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Border")
        {
            switch (collision.gameObject.name)
            {
                case "Top Wall":
                    isTouch_TopWall = false;
                    break;
                case "Bottom Wall":
                    isTouch_BottomWall = false;
                    break;
                case "Left Wall":
                    isTouch_LeftWall = false;
                    break;
                case "Right Wall":
                    isTouch_RightWall = false;
                    break;
            }
        }
    }
}
