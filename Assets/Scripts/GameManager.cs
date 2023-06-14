using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System.IO;
using UnityEditor;

public class GameManager : MonoBehaviour
{
	//stage
	public int stage;
	public Animator stagestartani;
	public Animator stageendani;
	public Animator fade;



	public float nextspawndelay;
	public float curspawndelay;

	public string[] enemyobjs;
    public Transform[] spawnpoints;
	public TextMeshProUGUI scoretext;
	public Image[] hpimages;
	public Image[] boomimages;

	public GameObject player;
	public GameObject gameoverset;
	public ObjectManager objmanager;
	public Transform playersetpos;

	//���������� ���� �� ������ ���� ����
	public List<Spawn> spawnlist;
	public int spawnindex;
	public bool isspawnend;

	private void Awake()
	{
		isspawnend = false;
		spawnlist = new List<Spawn>();
		enemyobjs = new string[]{"enemyS", "enemyM","enemyL", "enemyB" };

		StageStart();
	}

	private void Update()
	{
		
		curspawndelay += Time.deltaTime;

		if (curspawndelay > nextspawndelay && !isspawnend)
		{
			SpawnEnemy();
			curspawndelay = 0;
		}

		// UI ���� ������Ʈ
		Player playerlogic = player.GetComponent<Player>();
		scoretext.text = string.Format("{0:n0}", playerlogic.score);

	}

	public void StageStart()
	{
		//�������� UI
		stagestartani.SetTrigger("TextOn");
		stagestartani.GetComponent<TextMeshProUGUI>().text = "Stage "+ stage + "\nStart";
		stageendani.GetComponent<TextMeshProUGUI>().text = "Stage " + stage + "\nEnd";
		
		//���� �ҷ�����
		RespawnFile();

		//���̵���
		fade.SetTrigger("In");
	}

	public void StageEnd()
	{
		//�������� ���� UI
		stageendani.SetTrigger("TextOn");

		//���̵� �ƿ�
		fade.SetTrigger("Out");

		//�÷��̾� �ʱ�ȭ
		player.transform.position = playersetpos.position;

		stage++;
		if (stage > 3)
			Invoke("GameOver", 5);
		else
		{
			Invoke("StageStart", 3f);
		}
	}

	//�ؽ�Ʈ ������ �б�
	void RespawnFile()
	{
		//���� �ʱ�ȭ
		spawnlist.Clear();
		spawnindex = 0;
		isspawnend= false;

		//������ ���� �б�
		TextAsset textfile = Resources.Load("Stage" + stage) as TextAsset; //���Ͽ���
		StringReader stringreader = new StringReader(textfile.text);

		while(stringreader != null)
		{
			string line = stringreader.ReadLine();
			Debug.Log(line);

			if (line == null)
				break;
			
			//������ ����
			Spawn spawndata = new Spawn();
			spawndata.delay = float.Parse(line.Split(',')[0]);
			spawndata.type = line.Split(',')[1];
			spawndata.point = int.Parse(line.Split(',')[2]);

			spawnlist.Add(spawndata);
		}

		//�ؽ� ���� �ݱ� (�ʼ�)
		stringreader.Close();

		//ù��° ���� ������ ����?
		nextspawndelay = spawnlist[0].delay;

	}


	//�� ����
	private void SpawnEnemy()
	{
		// �� Ÿ�� ����
		// 0 : S
		// 1 : M
		// 2 : L
		int enemytype = 0;
		switch (spawnlist[spawnindex].type)
		{
			case "S":
				enemytype = 0;
				break;
			case "M":
				enemytype = 1;
				break;
			case "L":
				enemytype = 2;
				break;
			case "B":
				enemytype = 3;
				break;
		}

		//�� ��ġ ���� enemypoint
		int enemypoint = spawnlist[spawnindex].point;

		//����
		GameObject enemy = objmanager.MakeObj(enemyobjs[enemytype]);
		enemy.transform.position = spawnpoints[enemypoint].position;

		Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
		Enemy enemyscript = enemy.GetComponent<Enemy>();
		enemyscript.player = player;
		enemyscript.gamemanager = this;
		enemyscript.objmanager = objmanager;

		if (enemypoint == 5)	//���� ����
		{
			enemy.transform.Rotate(Vector3.forward * 45);
			rigid.velocity = new Vector2(enemyscript.speed, -1);
		}
		else if (enemypoint == 6)	//������ ����
		{
			enemy.transform.Rotate(Vector3.back * 45);

			rigid.velocity = new Vector2(enemyscript.speed * -1, -1);
		}
		else
			rigid.velocity = Vector2.down * enemyscript.speed;

		spawnindex++;
		if(spawnindex == spawnlist.Count)
		{
			isspawnend = true;
			Debug.Log("spawn end");
			return;
		}

		//���� ���� ������ ����
		nextspawndelay = spawnlist[spawnindex].delay;
	}

	public void UpdateHpIcons(int hp)
	{
		//ü�� �̹��� ��Ȱ��ȭ
		for(int i = 0; i < 3; i++)
		{
			hpimages[i].color = new Color(1, 1, 1, 0);
		}
		//���� hp��ŭ ü�� �̹��� Ȱ��ȭ
		for (int i = 0; i < hp; i++)
		{
			hpimages[i].color = new Color(1, 1, 1, 1);
		}
	}

	public void UpdateBoomIcons(int boom)
	{
		//ü�� �̹��� ��Ȱ��ȭ
		for (int i = 0; i < 3; i++)
		{
			boomimages[i].color = new Color(1, 1, 1, 0);
		}
		//���� hp��ŭ ü�� �̹��� Ȱ��ȭ
		for (int i = 0; i < boom; i++)
		{
			boomimages[i].color = new Color(1, 1, 1, 1);
		}
	}

	public void CallExplosion(Vector3 pos, string type)
	{
		GameObject explosion = objmanager.MakeObj("explosion");
		Explosion explosionlogic = explosion.GetComponent<Explosion>();

		explosion.transform.position = pos;
		explosionlogic.StartExplosion(type);
	}

	//�÷��̾� ������
	public void ReSpawnPlayer()
	{
		//2�ʵ� ����
		Invoke("ReSpawnPlayerExe", 2);
	}

	void ReSpawnPlayerExe()
	{
		player.transform.position = new Vector3(0f, -4.0f, 0f);
		player.SetActive(true);
		Player playerlogic = player.GetComponent<Player>();
		playerlogic.ishit = false;
	}

	//���ӿ��� �÷��̾��� hp = 0 || ������ �������� Ŭ����
	public void GameOver()
	{
		gameoverset.SetActive(true);
	}

	public void GameRetry()
	{
		SceneManager.LoadScene("SampleScene");
	}

}
