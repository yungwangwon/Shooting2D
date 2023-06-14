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

	//스테이지에 따른 적 리스폰 관련 변수
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

		// UI 점수 업데이트
		Player playerlogic = player.GetComponent<Player>();
		scoretext.text = string.Format("{0:n0}", playerlogic.score);

	}

	public void StageStart()
	{
		//스테이지 UI
		stagestartani.SetTrigger("TextOn");
		stagestartani.GetComponent<TextMeshProUGUI>().text = "Stage "+ stage + "\nStart";
		stageendani.GetComponent<TextMeshProUGUI>().text = "Stage " + stage + "\nEnd";
		
		//파일 불러오기
		RespawnFile();

		//페이드인
		fade.SetTrigger("In");
	}

	public void StageEnd()
	{
		//스테이지 종료 UI
		stageendani.SetTrigger("TextOn");

		//페이드 아웃
		fade.SetTrigger("Out");

		//플레이어 초기화
		player.transform.position = playersetpos.position;

		stage++;
		if (stage > 3)
			Invoke("GameOver", 5);
		else
		{
			Invoke("StageStart", 3f);
		}
	}

	//텍스트 데이터 읽기
	void RespawnFile()
	{
		//변수 초기화
		spawnlist.Clear();
		spawnindex = 0;
		isspawnend= false;

		//리스폰 파일 읽기
		TextAsset textfile = Resources.Load("Stage" + stage) as TextAsset; //파일열기
		StringReader stringreader = new StringReader(textfile.text);

		while(stringreader != null)
		{
			string line = stringreader.ReadLine();
			Debug.Log(line);

			if (line == null)
				break;
			
			//데이터 저장
			Spawn spawndata = new Spawn();
			spawndata.delay = float.Parse(line.Split(',')[0]);
			spawndata.type = line.Split(',')[1];
			spawndata.point = int.Parse(line.Split(',')[2]);

			spawnlist.Add(spawndata);
		}

		//텍스 파일 닫기 (필수)
		stringreader.Close();

		//첫번째 스폰 딜레이 적용?
		nextspawndelay = spawnlist[0].delay;

	}


	//적 생성
	private void SpawnEnemy()
	{
		// 적 타입 설정
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

		//적 위치 변수 enemypoint
		int enemypoint = spawnlist[spawnindex].point;

		//생성
		GameObject enemy = objmanager.MakeObj(enemyobjs[enemytype]);
		enemy.transform.position = spawnpoints[enemypoint].position;

		Rigidbody2D rigid = enemy.GetComponent<Rigidbody2D>();
		Enemy enemyscript = enemy.GetComponent<Enemy>();
		enemyscript.player = player;
		enemyscript.gamemanager = this;
		enemyscript.objmanager = objmanager;

		if (enemypoint == 5)	//왼쪽 스폰
		{
			enemy.transform.Rotate(Vector3.forward * 45);
			rigid.velocity = new Vector2(enemyscript.speed, -1);
		}
		else if (enemypoint == 6)	//오른쪽 스폰
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

		//다음 스폰 딜레이 설정
		nextspawndelay = spawnlist[spawnindex].delay;
	}

	public void UpdateHpIcons(int hp)
	{
		//체력 이미지 비활성화
		for(int i = 0; i < 3; i++)
		{
			hpimages[i].color = new Color(1, 1, 1, 0);
		}
		//남은 hp만큼 체력 이미지 활성화
		for (int i = 0; i < hp; i++)
		{
			hpimages[i].color = new Color(1, 1, 1, 1);
		}
	}

	public void UpdateBoomIcons(int boom)
	{
		//체력 이미지 비활성화
		for (int i = 0; i < 3; i++)
		{
			boomimages[i].color = new Color(1, 1, 1, 0);
		}
		//남은 hp만큼 체력 이미지 활성화
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

	//플레이어 리스폰
	public void ReSpawnPlayer()
	{
		//2초뒤 실행
		Invoke("ReSpawnPlayerExe", 2);
	}

	void ReSpawnPlayerExe()
	{
		player.transform.position = new Vector3(0f, -4.0f, 0f);
		player.SetActive(true);
		Player playerlogic = player.GetComponent<Player>();
		playerlogic.ishit = false;
	}

	//게임오버 플레이어의 hp = 0 || 마지막 스테이지 클리어
	public void GameOver()
	{
		gameoverset.SetActive(true);
	}

	public void GameRetry()
	{
		SceneManager.LoadScene("SampleScene");
	}

}
