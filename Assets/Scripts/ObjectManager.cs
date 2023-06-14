using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManager : MonoBehaviour
{
	//prefeb

	public GameObject enemyBprefab;
	public GameObject enemySprefab;
    public GameObject enemyMprefab;
    public GameObject enemyLprefab;

    public GameObject itemcoinprefab;
    public GameObject itemboomprefab;
    public GameObject itempowerprefab;

    public GameObject playerbulletAprefab;
    public GameObject playerbulletBprefab;
	public GameObject followerbulletprefab;

    public GameObject explosionprefab;


	public GameObject enemybulletAprefab;
    public GameObject enemybulletBprefab;

	public GameObject bossbulletAprefab;
	public GameObject bossbulletBprefab;

    GameObject[] enemyB;
	GameObject[] enemyS;
    GameObject[] enemyM;
    GameObject[] enemyL;

    GameObject[] itemcoin;
    GameObject[] itemboom;
    GameObject[] itempower;

    GameObject[] playerbulletA;
    GameObject[] playerbulletB;
    GameObject[] enemybulletA;
    GameObject[] enemybulletB;
	GameObject[] followerbullet;
	GameObject[] bossbulletA;
	GameObject[] bossbulletB;

    GameObject[] explosion;



	GameObject[] targetpool;


    private void Awake()
	{
		enemyB = new GameObject[5];
		enemyS = new GameObject[10];
        enemyM = new GameObject[10];
        enemyL = new GameObject[10];

        itemcoin = new GameObject[20];
        itemboom = new GameObject[10];
        itempower = new GameObject[10];

        playerbulletA = new GameObject[100];
        playerbulletB = new GameObject[100];
		followerbullet = new GameObject[50];
		enemybulletA = new GameObject[200];
        enemybulletB = new GameObject[200];
		bossbulletA = new GameObject[200];
		bossbulletB = new GameObject[200];

        explosion = new GameObject[100];


		Generate();





    }

    void Generate()
	{
		//적
		for (int i = 0; i < enemyB.Length; i++)
		{
			enemyB[i] = Instantiate(enemyBprefab);
			enemyB[i].SetActive(false);
		}
		for (int i =0; i < enemyS.Length; i++)
		{
            enemyS[i] = Instantiate(enemySprefab);
            enemyS[i].SetActive(false);
        }
        for (int i = 0; i < enemyM.Length; i++)
        {
            enemyM[i] = Instantiate(enemyMprefab);
            enemyM[i].SetActive(false);
        }
        for (int i = 0; i < enemyL.Length; i++)
        {
            enemyL[i] = Instantiate(enemyLprefab);
            enemyL[i].SetActive(false);
        }
        //아이템
        for (int i = 0; i < itemcoin.Length; i++)
        {
            itemcoin[i] = Instantiate(itemcoinprefab);
            itemcoin[i].SetActive(false);
        }
        for (int i = 0; i < itemboom.Length; i++)
        {
            itemboom[i] = Instantiate(itemboomprefab);
            itemboom[i].SetActive(false);
        }
        for (int i = 0; i < itempower.Length; i++)
        {
            itempower[i] = Instantiate(itempowerprefab);
            itempower[i].SetActive(false);
        }
        //플레이어, 적 총알, 보조무기총알
        for (int i = 0; i < playerbulletA.Length; i++)
        {
            playerbulletA[i] = Instantiate(playerbulletAprefab);
            playerbulletA[i].SetActive(false);
        }
        for (int i = 0; i < playerbulletB.Length; i++)
        {
            playerbulletB[i] = Instantiate(playerbulletBprefab);
            playerbulletB[i].SetActive(false);
        }
        for (int i = 0; i < enemybulletA.Length; i++)
        {
            enemybulletA[i] = Instantiate(enemybulletAprefab);
            enemybulletA[i].SetActive(false);
        }
        for (int i = 0; i < enemybulletB.Length; i++)
        {
            enemybulletB[i] = Instantiate(enemybulletBprefab);
            enemybulletB[i].SetActive(false);
        }
		for (int i = 0; i < followerbullet.Length; i++)
		{
			followerbullet[i] = Instantiate(followerbulletprefab);
			followerbullet[i].SetActive(false);
		}
		for (int i = 0; i < bossbulletA.Length; i++)
		{
			bossbulletA[i] = Instantiate(bossbulletAprefab);
			bossbulletA[i].SetActive(false);
		}
		for (int i = 0; i < bossbulletB.Length; i++)
		{
			bossbulletB[i] = Instantiate(bossbulletBprefab);
			bossbulletB[i].SetActive(false);
		}
        //기타
		for (int i = 0; i < explosion.Length; i++)
		{
			explosion[i] = Instantiate(explosionprefab);
			explosion[i].SetActive(false);
		}
	}

    public GameObject MakeObj(string type)
	{
		switch (type)
		{
			case "enemyB":
				targetpool = enemyB;
				break;
			case "enemyS":
                targetpool = enemyS;
                break;
            case "enemyM":
                targetpool = enemyM;
                break;
            case "enemyL":
                targetpool = enemyL;
                break;
            case "itemcoin":
                targetpool = itemcoin;
                break;
            case "itemboom":
                targetpool = itemboom;
                break;
            case "itempower":
                targetpool = itempower;
                break;
            case "playerbulletA":
                targetpool = playerbulletA;
                break;
            case "playerbulletB":
                targetpool = playerbulletB;
                break;
            case "enemybulletA":
                targetpool = enemybulletA;
                break;
            case "enemybulletB":
                targetpool = enemybulletB;
                break;
			case "bossbulletA":
				targetpool = bossbulletA;
				break;
			case "bossbulletB":
				targetpool = bossbulletB;
				break;
			case "followerbullet":
				targetpool = followerbullet;
				break;
			case "explosion":
				targetpool = explosion;
				break;
		}

        for (int i = 0; i < targetpool.Length; i++)
        {
            if (!targetpool[i].activeSelf)
			{
                targetpool[i].SetActive(true);
                return targetpool[i];
            }
		}

        return null;
	}

    public GameObject[] GetPool(string type)
	{
        switch (type)
        {
			case "enemyB":
				targetpool = enemyB;
				break;
			case "enemyS":
                targetpool = enemyS;
                break;
            case "enemyM":
                targetpool = enemyM;
                break;
            case "enemyL":
                targetpool = enemyL;
                break;
            case "itemcoin":
                targetpool = itemcoin;
                break;
            case "itemboom":
                targetpool = itemboom;
                break;
            case "itempower":
                targetpool = itempower;
                break;
            case "playerbulletA":
                targetpool = playerbulletA;
                break;
            case "playerbulletB":
                targetpool = playerbulletB;
                break;
            case "enemybulletA":
                targetpool = enemybulletA;
                break;
            case "enemybulletB":
                targetpool = enemybulletB;
                break;
			case "bossbulletA":
				targetpool = enemybulletA;
				break;
			case "bossbulletB":
				targetpool = enemybulletB;
				break;
			case "followerbullet":
				targetpool = followerbullet;
				break;
			case "explotion":
				targetpool = explosion;
				break;
		}

        return targetpool;
	}
}
