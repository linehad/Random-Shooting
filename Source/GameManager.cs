using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update
    public int stage;
    public Animator stageAnim;
    public Animator clearAnim;
    public Animator fadeAnim;
    public Transform playerPos;

    public string[] enemyObjs;
    public Transform[] spawnPoints;

    public float nextSpawnDelay;
    public float curSpawnDelay;

    public GameObject player;
    public Text scoreText;
    public Image[] lifeImage;
    public Image[] boomImage;
    public GameObject gameOverSet;
    public ObjectManager objectManager;

    public List<Spawn> spawnList;
    public int spawnIndex;
    public bool spawnEnd;

    public TextAsset[] stageList;
    public int stageIndex;
    int beforeStage = 0;
    int nextStage;

    void Awake()
    {
        spawnList = new List<Spawn>();
        enemyObjs = new string[] { "Enemy S", "Enemy M", "Enemy L", "Enemy B" };
        StageStart();
    }

    void ReadSpawnFile()
    {
        // 변수 초기화
        spawnList.Clear();
        spawnIndex = 0;
        spawnEnd = false;       

        // 리스폰 파일 읽기        
        TextAsset textFile = stageList[stageIndex];
        StringReader stringReader = new StringReader(textFile.text);        

        while (stringReader != null)
        {
            string line = stringReader.ReadLine();
            //Debug.Log(line);

            if (line == null)
            {
                break;
            }
            //리스폰 데이터 생성
            Spawn spawnData = new Spawn();
            spawnData.delay = float.Parse(line.Split(',')[0]);
            spawnData.type = line.Split(',')[1];
            spawnData.point = int.Parse(line.Split(',')[2]);
            spawnList.Add(spawnData);
        }

        //텍스트 파일 닫기
        stringReader.Close();

        //첫번째 딜레이 적용
        nextSpawnDelay = spawnList[0].delay;
    }

    public void StageStart()
    {
        stageIndex = Random.Range(1, stageList.Length);
        //스테이지 인덱스를 다음스테이지에 저장
        nextStage = stageIndex;

        //이전 스테이지와 같으면 다시 읽기
        if (beforeStage == nextStage)
        {
            stageIndex = Random.Range(1, stageList.Length);
        }
        else
        {
            beforeStage = stageIndex;
        }

        //스테이지 UI 로드
        stageAnim.SetTrigger("On");
        stageAnim.GetComponent<Text>().text = stage + " 스테이지";
        clearAnim.GetComponent<Text>().text = stage + " 스테이지" + "\n클리어";

        //적 생성 데이터 읽어오기
        ReadSpawnFile();

        //페이드 인
        fadeAnim.SetTrigger("In");
       
    }

    public void StageEnd()
    {
        //클리어 UI 로드
        clearAnim.SetTrigger("On");
        //스테이지 증가
        stageIndex = Random.Range(1, stageList.Length);       

        //페이드 아웃
        fadeAnim.SetTrigger("Out");

        //Player Repostion
        player.transform.position = playerPos.position;

        stage++;
        Invoke("StageStart", 5);
    }

    void Update()
    {
        curSpawnDelay += Time.deltaTime;

        if (curSpawnDelay > nextSpawnDelay && !spawnEnd) 
        {
            SpawnEnemy();
            curSpawnDelay = 0;
        }
        Player playerLogic = player.GetComponent<Player>();
        scoreText.text = string.Format("{0:n0}", playerLogic.score);

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(1);
        }
    }

    void SpawnEnemy()
    {
        int enemyIndex = 0;
        switch (spawnList[spawnIndex].type)
        {
            case "S":
                enemyIndex = 0;
                break;
            case "M":
                enemyIndex = 1;
                break;
            case "L":
                enemyIndex = 2;
                break;
            case "B":
                enemyIndex = 3;
                break;
        }

        int enemyPoint = spawnList[spawnIndex].point;

        GameObject enemy = objectManager.MakeObj(enemyObjs[enemyIndex]);
        enemy.transform.position = spawnPoints[enemyPoint].position;

        Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
        Enemy enemyLogic = enemy.GetComponent<Enemy>();

        enemyLogic.player = player;
        enemyLogic.gameManager = this;
        enemyLogic.objectManager = objectManager;

        if (enemyPoint == 5|| enemyPoint == 6)//Left Spawn
        {
            enemy.transform.Rotate(Vector3.forward * 90);
            rigid.velocity = new Vector2(enemyLogic.speed, -1);
        }

        else if (enemyPoint == 7 || enemyPoint == 8)//Right Spawn
        {
            enemy.transform.Rotate(Vector3.back * 90);
            rigid.velocity = new Vector2(enemyLogic.speed * (-1), -1);
        }

        else // Front Spawn
        {
            rigid.velocity = new Vector2(0, enemyLogic.speed * (-1));
        }

        //리스폰 인덱스 증가
        spawnIndex++;
        if (spawnIndex == spawnList.Count)
        {
            spawnEnd = true;
            return;
        }

        //다음 리스폰 딜레이 갱신
        nextSpawnDelay = spawnList[spawnIndex].delay;
    }
    public void UpdateLifeIcon(int life)
    {
        for (int index = 0; index < 3; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < life; index++)
        {
            lifeImage[index].color = new Color(1, 1, 1, 1);
        }
    }
    public void UpdateBoomIcon(int boom)
    {
        for (int index = 0; index < 4; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 0);
        }

        for (int index = 0; index < boom; index++)
        {
            boomImage[index].color = new Color(1, 1, 1, 1);
        }
    }

    public void ReSpawnPlayer()
    {
        Invoke("ReSpawnPlayerExe", 2);
    }

    void ReSpawnPlayerExe()
    {
        player.transform.position = Vector3.down * 3.5f;
        player.SetActive(true);

        Player playerLogic = player.GetComponent<Player>();
        playerLogic.isHit = false;
    }

    public void CallExplosion(Vector3 pos, string type)
    {
        GameObject explosion = objectManager.MakeObj("Explosion");
        Explosion exceptionLogic = explosion.GetComponent<Explosion>();

        explosion.transform.position = pos;
        exceptionLogic.StartExplosion(type);
    }

    public void GameOver()
    {
        gameOverSet.SetActive(true);
    }
    public void MeinMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void GameRetry()
    {
        SceneManager.LoadScene(1);
    }

    public void GameExit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit(); // 어플리케이션 종료
    #endif
    }
}
