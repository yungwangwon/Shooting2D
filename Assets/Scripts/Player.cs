using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;


public class Player : MonoBehaviour
{

	public bool istouchT;
	public bool istouchB;
	public bool istouchL;
	public bool istouchR;
	public bool ishit;
	public bool isboomactive;
	public bool isspawntime;

	public int hp;
	public int score;
	public float speed;
	public int power;
	public int boom;

	//조이패드 컨트롤
	public bool[] joycontrol;
	public bool isjoycontrol;

	//버튼
	public bool isbuttonattack;
	public bool isbuttonboom;


	public float curshotdelay;
	public float maxshotdelay;
	public int maxpower;
	public int maxboom;

	public SpriteRenderer sprite;
	public GameObject bulletA;
	public GameObject bulletB;
	public GameObject boomeffect;
	public GameManager manager;
	public GameObject[] followers;
	public ObjectManager objmanager;

	Animator ani;

	void Awake()
	{
		ani = GetComponent<Animator>();
	}

	void Update()
	{
		Move();
		Fire();
		Boom();
		Reload();
	}

	//오브젝트 활성화
	void OnEnable()
	{
		SpawnTime();
		Invoke("SpawnTime", 1.5f);
	}

	//리스폰 무적시간
	void SpawnTime()
	{
		isspawntime = !isspawntime;

		//무적시간이 활성화 되어있는경우
		if (isspawntime)
		{
			//플레이어와 보조무기의 스프라이트 적용
			sprite.color = new Color(1, 1, 1, 0.5f);

			for(int i =0; i < followers.Length; i++)
			{
				followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
			}
		}
		else
		{
			sprite.color = new Color(1, 1, 1, 1f);

			for (int i = 0; i < followers.Length; i++)
			{
				followers[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.5f);
			}
		}

	}

	void Reload()
	{
		curshotdelay += Time.deltaTime;
	}

	//필살기
	void Boom()
	{
		//if (!Input.GetButton("Fire2"))
		//	return;

		if (!isbuttonboom)
			return;

		if (isboomactive)
			return;

		if (boom == 0)
			return;

		boom--;
		isboomactive = true;
		manager.UpdateBoomIcons(boom);
		//폭탄 이펙트 활성화
		boomeffect.SetActive(true);
		Invoke("BoomEffectOff", 3f);

		//적에게 데미지
		GameObject[] enemiesS = objmanager.GetPool("enemyS");
		GameObject[] enemiesM = objmanager.GetPool("enemyM");
		GameObject[] enemiesL = objmanager.GetPool("enemyL");
		for (int i = 0; i < enemiesS.Length; i++)
		{
			if(enemiesS[i].activeSelf)
			{
				Enemy enemylogic = enemiesS[i].GetComponent<Enemy>();
				enemylogic.OnHit(100);
			}
		}
		for (int i = 0; i < enemiesM.Length; i++)
		{
			if (enemiesM[i].activeSelf)
			{
				Enemy enemylogic = enemiesM[i].GetComponent<Enemy>();
				enemylogic.OnHit(100);
			}
		}
		for (int i = 0; i < enemiesL.Length; i++)
		{
			if (enemiesL[i].activeSelf)
			{
				Enemy enemylogic = enemiesL[i].GetComponent<Enemy>();
				enemylogic.OnHit(100);
			}
		}

		//총알 삭제
		GameObject[] enemybulletA = objmanager.GetPool("enemybulletA");
		GameObject[] enemybulletB = objmanager.GetPool("enemybulletB");
		for (int i = 0; i < enemybulletA.Length; i++)
		{
			if(enemybulletA[i].activeSelf)
			{
				enemybulletA[i].SetActive(false);
			}
		}
		for (int i = 0; i < enemybulletB.Length; i++)
		{
			if (enemybulletB[i].activeSelf)
			{
				enemybulletB[i].SetActive(false);
			}
		}

		isbuttonboom = false;
	}

	public void AttackDown()
	{
		isbuttonattack = true;
	}

	public void AttackUp()
	{
		isbuttonattack = false;
	}

	public void BoomDown()
	{
		isbuttonboom = true;
	}

	void Fire()
	{
		//입력값이 Fire1이 아닐시
		//if (!Input.GetButton("Fire1"))
		//	return;
		if (!isbuttonattack)
			return;

		//재장전
		if (curshotdelay < maxshotdelay)
			return;

		//power에 따른 총알생성
		switch (power)
		{
			case 1:
				GameObject bullet = objmanager.MakeObj("playerbulletA");
				bullet.transform.position = transform.position + Vector3.up * 0.2f;
				Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
				rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
				break;
			case 2:
				GameObject bulletR = objmanager.MakeObj("playerbulletA");
				bulletR.transform.position = transform.position + Vector3.up * 0.2f + Vector3.right * 0.2f;

				GameObject bulletL = objmanager.MakeObj("playerbulletA");
				bulletL.transform.position = transform.position + Vector3.up * 0.2f + Vector3.left * 0.2f;

				Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
				Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
				rigidR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
				rigidL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
				break;
			case 3:
			case 4:
			case 5:
			case 6:
				GameObject bulletRR = objmanager.MakeObj("playerbulletA");
				bulletRR.transform.position = transform.position + Vector3.up * 0.2f + Vector3.right * 0.3f;
				GameObject bulletCC = objmanager.MakeObj("playerbulletB");
				bulletCC.transform.position = transform.position + Vector3.up * 0.2f;
				GameObject bulletLL = objmanager.MakeObj("playerbulletA");
				bulletLL.transform.position = transform.position + Vector3.up * 0.2f + Vector3.left * 0.3f;

				Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
				Rigidbody2D rigidCC = bulletCC.GetComponent<Rigidbody2D>();
				Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

				rigidRR.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
				rigidCC.AddForce(Vector2.up * 10, ForceMode2D.Impulse);
				rigidLL.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

				break;
		}

		curshotdelay = 0;
	}

	public void JoyPad(int type)
	{
		for(int i = 0;i< 9;i++)
		{
			joycontrol[i] = (i == type);
		}
	}

	public void JoyDown()
	{
		isjoycontrol = true;
	}

	public void JoyUp()
	{
		isjoycontrol = false;
	}

	//플레이어 이동
	void Move()
	{
		//이동 키보드
		float h = Input.GetAxisRaw("Horizontal"); //좌우
		float v = Input.GetAxisRaw("Vertical");	//상하

		//이동 조이패드
		if (joycontrol[0]) { h = -1; v = 1; }
		if (joycontrol[1]) { h = 0; v = 1; }
		if (joycontrol[2]) { h = 1; v = 1; }
		if (joycontrol[3]) { h = -1; v = 0; }
		if (joycontrol[4]) { h = 0; v = 0; }
		if (joycontrol[5]) { h = 1; v = 0; }
		if (joycontrol[6]) { h = -1; v = -1; }
		if (joycontrol[7]) { h = 0; v = -1; }
		if (joycontrol[8]) { h = 1; v = -1; }

		if ((h == -1 && istouchL) || (h == 1 && istouchR) || !isjoycontrol)
			h = 0;

		if ((v == -1 && istouchB) || (v == 1 && istouchT) || !isjoycontrol)
			v = 0;

		if (Input.GetButtonDown("Horizontal") || Input.GetButtonUp("Horizontal"))
			ani.SetInteger("Input", (int)h);


		Vector3 curpos = transform.position; //현재위치 저장

		Vector3 movepos = new Vector3(h, v, 0) * speed * Time.deltaTime; //다음위치

		transform.position = curpos + movepos;
	}

	//충돌
	void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "Border")
		{
			switch(collision.gameObject.name)
			{
				case "Top":
					istouchT = true;
					break;
				case "Bottom":
					istouchB = true;
					break;
				case "Left":
					istouchL = true;
					break;
				case "Right":
					istouchR = true;
					break;
			}
		}
		//적 비행기의 총알, 비행체에 부딪혔을때
		else if(collision.gameObject.tag == "EnemyBullet" || 
			collision.gameObject.tag == "Enemy")
		{
			//리스폰 무적이 적용중일때
			if (isspawntime)
				return;

			//동시에 맞았을경우 2번실행되는걸 막기위함
			if (ishit)
				return;

			ishit = true;
			hp--;
			manager.UpdateHpIcons(hp);
			manager.CallExplosion(transform.position, "P");

			if (hp <= 0)
			{
				manager.GameOver();

			}
			else
			{
				manager.ReSpawnPlayer();

			}
			gameObject.SetActive(false);
		}
		//아이템 충돌
		else if(collision.gameObject.tag == "Item")
		{
			Item item = collision.gameObject.GetComponent<Item>();
			switch(item.type)
			{
				case "Coin":
					score += 1000;
					break;
				case "Boom":
					if (boom == maxboom)
					{
						score += 500;
					}
					else
					{
						boom++;
						manager.UpdateBoomIcons(boom);
					}
					break;
				case "Power":
					if (power == maxpower)
					{
						score += 500;
					}
					else
					{
						power++;
						AddFollower();
					}
					break;
			}
			collision.gameObject.SetActive(false);
		}
	}

	void AddFollower()
	{
		if (power == 4)
		{
			followers[0].SetActive(true);
		}
		else if (power == 5)
		{
			followers[1].SetActive(true);

		}
		else if(power == 6)
			followers[2].SetActive(true);


	}

	void BoomEffectOff()
	{
		boomeffect.SetActive(false);
		isboomactive= false;
	}

	void OnTriggerExit2D(Collider2D collision)
	{
		if (collision.gameObject.tag == "Border")
		{
			switch (collision.gameObject.name)
			{
				case "Top":
					istouchT = false;
					break;
				case "Bottom":
					istouchB = false;
					break;
				case "Left":
					istouchL = false;
					break;
				case "Right":
					istouchR = false;
					break;
			}
		}
	}

}
