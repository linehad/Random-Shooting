﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
    public GameObject enemyBPrefab;
    public GameObject enemyLPrefab;
    public GameObject enemyMPrefab;
    public GameObject enemySPrefab;
    public GameObject itemCoinPrefab;
    public GameObject itemPowerPrefab;
    public GameObject itemBoomPrefab;
    public GameObject bulletPlayerAPrefab;
    public GameObject bulletPlayerBPrefab;
    public GameObject bulletEnemyAPrefab;
    public GameObject bulletEnemyBPrefab;
    public GameObject bulletBossAPrefab;
    public GameObject bulletBossBPrefab;
    public GameObject explosionPrefab;

    GameObject[] enemyB;
    GameObject[] enemyL;
    GameObject[] enemyM;
    GameObject[] enemyS;

    GameObject[] itemCoin;
    GameObject[] itemPower;
    GameObject[] itemBoom;

    GameObject[] bulletPlayerA;
    GameObject[] bulletPlayerB;
    GameObject[] bulletEnemyA;
    GameObject[] bulletEnemyB;
    GameObject[] bulletBossA;
    GameObject[] bulletBossB;

    GameObject[] explosion;

    GameObject[] targetPool;
    void Awake()
    {
        enemyB = new GameObject[1];
        enemyL = new GameObject[10];
        enemyM = new GameObject[10];
        enemyS = new GameObject[20];

        itemCoin = new GameObject[20];
        itemPower = new GameObject[10];
        itemBoom = new GameObject[10];

        bulletPlayerA = new GameObject[100];
        bulletPlayerB = new GameObject[100];
        bulletEnemyA = new GameObject[100];
        bulletEnemyB = new GameObject[100];
        bulletBossA = new GameObject[100];
        bulletBossB = new GameObject[1000];
        explosion = new GameObject[20];

        Generate();
    }

    void Generate()
    {
        //enemy
        for (int index = 0; index < enemyB.Length; index++)
        {
            enemyB[index] = Instantiate(enemyBPrefab);
            enemyB[index].SetActive(false);
        }
        for (int index=0; index < enemyL.Length; index++)
        {
            enemyL[index] = Instantiate(enemyLPrefab);
            enemyL[index].SetActive(false);
        }
        for (int index = 0; index < enemyM.Length; index++)
        {
            enemyM[index] = Instantiate(enemyMPrefab);
            enemyM[index].SetActive(false);
        }
        for (int index = 0; index < enemyS.Length; index++)
        {
            enemyS[index] = Instantiate(enemySPrefab);
            enemyS[index].SetActive(false);
        }

        //item
        for (int index = 0; index < itemCoin.Length; index++)
        {
            itemCoin[index] = Instantiate(itemCoinPrefab);
            itemCoin[index].SetActive(false);
        }
        for (int index = 0; index < itemPower.Length; index++)
        {
            itemPower[index] = Instantiate(itemPowerPrefab);
            itemPower[index].SetActive(false);
        }
        for (int index = 0; index < itemBoom.Length; index++)
        {
            itemBoom[index] = Instantiate(itemBoomPrefab);
            itemBoom[index].SetActive(false);
        }

        //bullet
        for (int index = 0; index < bulletPlayerA.Length; index++)
        {
            bulletPlayerA[index] = Instantiate(bulletPlayerAPrefab);
            bulletPlayerA[index].SetActive(false);
        }
        for (int index = 0; index < bulletPlayerB.Length; index++)
        {
            bulletPlayerB[index] = Instantiate(bulletPlayerBPrefab);
            bulletPlayerB[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyA.Length; index++)
        {
            bulletEnemyA[index] = Instantiate(bulletEnemyAPrefab);
            bulletEnemyA[index].SetActive(false);
        }
        for (int index = 0; index < bulletEnemyB.Length; index++)
        {
            bulletEnemyB[index] = Instantiate(bulletEnemyBPrefab);
            bulletEnemyB[index].SetActive(false);
        }
        for (int index = 0; index < bulletBossA.Length; index++)
        {
            bulletBossA[index] = Instantiate(bulletBossAPrefab);
            bulletBossA[index].SetActive(false);
        }
        for (int index = 0; index < bulletBossB.Length; index++)
        {
            bulletBossB[index] = Instantiate(bulletBossBPrefab);
            bulletBossB[index].SetActive(false);
        }

        //Explosion
        for (int index = 0; index < explosion.Length; index++)
        {
            explosion[index] = Instantiate(explosionPrefab);
            explosion[index].SetActive(false);
        }
    }

    public GameObject MakeObj(string type)
    {
        switch (type)
        {
            case "Enemy B":
                targetPool = enemyB;
                break;
            case "Enemy L":
                targetPool = enemyL;
                break;
            case "Enemy M":
                targetPool = enemyM;
                break;
            case "Enemy S":
                targetPool = enemyS;
                break;

            case "Item Coin":
                targetPool = itemCoin;
                break;
            case "Item Power":
                targetPool = itemPower;
                break;
            case "Item Boom":
                targetPool = itemBoom;
                break;

            case "Bullet Player A":
                targetPool = bulletPlayerA;
                break;
            case "Bullet Player B":
                targetPool = bulletPlayerB;
                break;
            case "Bullet Enemy A":
                targetPool = bulletEnemyA;
                break;
            case "Bullet Enemy B":
                targetPool = bulletEnemyB;
                break;
            case "Bullet Boss A":
                targetPool = bulletBossA;
                break;
            case "Bullet Boss B":
                targetPool = bulletBossB;
                break;

            case "Explosion":
                targetPool = explosion;
                break;
        }
        for (int index = 0; index < targetPool.Length; index++)
        {
            if (!targetPool[index].activeSelf)
            {
                targetPool[index].SetActive(true);
                return targetPool[index];
            }
        }
        return null;
    }

    public GameObject[] GetPool(string type)
    {
        switch (type)
        {
            case "Enemy B":
                targetPool = enemyB;
                break;
            case "Enemy L":
                targetPool = enemyL;
                break;
            case "Enemy M":
                targetPool = enemyM;
                break;
            case "Enemy S":
                targetPool = enemyS;
                break;

            case "Item Coin":
                targetPool = itemCoin;
                break;
            case "Item Power":
                targetPool = itemPower;
                break;
            case "Item Boom":
                targetPool = itemBoom;
                break;

            case "Bullet Player A":
                targetPool = bulletPlayerA;
                break;
            case "Bullet Player B":
                targetPool = bulletPlayerB;
                break;
            case "Bullet Enemy A":
                targetPool = bulletEnemyA;
                break;
            case "Bullet Enemy B":
                targetPool = bulletEnemyB;
                break;
            case "Bullet Boss A":
                targetPool = bulletBossA;
                break;
            case "Bullet Boss B":
                targetPool = bulletBossB;
                break;

            case "Explosion":
                targetPool = explosion;
                break;
        }
                return targetPool;
    }
}