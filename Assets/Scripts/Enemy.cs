using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
	public string enemyname;
	public int enemyscore;
	public float speed;
	public int hp;

	public float curshotdelay;
	public float maxshotdelay;
	public GameObject bulletS;
	public GameObject bulletL;
	public GameObject player;
	public Sprite[] sprites;
	public GameObject itemcoin;
	public GameObject itemboom;
	public GameObject itempower;

	public ObjectManager objmanager;
	public GameManager gamemanager;

	//boss
	Animator bossani;
	SpriteRenderer spriteRenderer;
	public int patternindex;
	public int curpatterncount;
	public int[] maxpatterncount;

	private void Awake()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		if(enemyname == "B")
			bossani = GetComponent<Animator>();
	}

	private void OnEnable()
	{
		switch (enemyname)
		{
			case "B":
				hp = 3000;
				Invoke("Stop", 1.75f);
				break;
			case "S":
				hp = 3;
				break;
			case "M":
				hp = 5;
				break;
			case "L":
				hp = 10;
				break;
		}
	}

	void Think()
	{
		if (hp <= 0)
			return;

		patternindex = patternindex == 3 ? 0 : patternindex + 1;

		curpatterncount = 0;

		switch (patternindex)
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

	void Stop()
	{
		if (!gameObject.activeSelf)
			return;

		Rigidbody2D rigid = GetComponent<Rigidbody2D>();
		rigid.velocity = Vector3.zero;

		Invoke("Think", 3);
	}

	void FireFoward()
	{
		//게임오브젝트 생성(총알)
		GameObject bulletR = objmanager.MakeObj("bossbulletA");
		bulletR.transform.position = transform.position + Vector3.right * 0.55f + Vector3.down * 1.0f;
		GameObject bulletL = objmanager.MakeObj("bossbulletA");
		bulletL.transform.position = transform.position + Vector3.left * 0.55f + Vector3.down * 1.0f;
		GameObject bulletRR = objmanager.MakeObj("bossbulletA");
		bulletRR.transform.position = transform.position + Vector3.right * 0.7f+ Vector3.down * 1.0f;
		GameObject bulletLL = objmanager.MakeObj("bossbulletA");
		bulletLL.transform.position = transform.position + Vector3.left * 0.7f+ Vector3.down * 1.0f;

		//컴포넌트 받아오기
		Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidRR = bulletRR.GetComponent<Rigidbody2D>();
		Rigidbody2D rigidLL = bulletLL.GetComponent<Rigidbody2D>();

		//물리설정
		rigidR.AddForce(Vector2.down* 8, ForceMode2D.Impulse);
		rigidL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		rigidRR.AddForce(Vector2.down * 8, ForceMode2D.Impulse);
		rigidLL.AddForce(Vector2.down * 8, ForceMode2D.Impulse);

		curpatterncount++;

		if(curpatterncount < maxpatterncount[patternindex])
			Invoke("FireFoward", 2);
		else
			Invoke("Think", 3);

	}

	void FireShot()
	{
		for(int i = 0;i < 5; i++)
		{
			GameObject bullet = objmanager.MakeObj("bossbulletB");
			bullet.transform.position = transform.position + Vector3.down * 0.2f;

			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector2 dirvec = player.transform.position - gameObject.transform.position;
			Vector2 ranvec = new Vector2(Random.Range(-0.5f, 0.5f), Random.Range(0f, 2f));
			dirvec += ranvec;
			
			rigid.AddForce(dirvec.normalized * 5, ForceMode2D.Impulse);
		}

		curpatterncount++;

		if (curpatterncount < maxpatterncount[patternindex])
			Invoke("FireShot", 2);
		else
			Invoke("Think", 3);


	}

	void FireArc()
	{
		GameObject bullet = objmanager.MakeObj("bossbulletA");
		bullet.transform.position = transform.position + Vector3.down * 0.2f;
		bullet.transform.rotation = Quaternion.identity;


		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		Vector2 dirvec = new Vector2(Mathf.Sin(Mathf.PI*10*curpatterncount/maxpatterncount[patternindex]), -1);

		rigid.AddForce(dirvec.normalized * 4, ForceMode2D.Impulse);


		curpatterncount++;

		if (curpatterncount < maxpatterncount[patternindex])
			Invoke("FireArc", 0.15f);
		else
			Invoke("Think", 3);

	}

	void FireAround()
	{
		int roundnumA = 50;
		int roundnumB = 40;
		int roundnum = curpatterncount % 2 == 0 ? roundnumA : roundnumB;



		for (int i =0;i< roundnum; i++)
		{
			GameObject bullet = objmanager.MakeObj("enemybulletA");
			bullet.transform.position = transform.position + Vector3.down * 0.2f;
			bullet.transform.rotation = Quaternion.identity;


			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector2 dirvec = new Vector2(Mathf.Cos(Mathf.PI * 2 * i / roundnum)
				, Mathf.Sin(Mathf.PI * 2 * i / roundnum));

			rigid.AddForce(dirvec.normalized * 2.5f, ForceMode2D.Impulse);

			Vector3 rotatevec = Vector3.forward * 360 * i / roundnum + Vector3.forward * 90; 
			bullet.transform.Rotate(rotatevec);
		}

		curpatterncount++;

		if (curpatterncount < maxpatterncount[patternindex])
			Invoke("FireAround", 1.5f);
		else
			Invoke("Think", 3);

	}






	void Update()
	{
		if (enemyname == "B")
			return;

		Fire();
		Reload();
	}

	void Reload()
	{
		curshotdelay += Time.deltaTime;
	}

	void Fire()
	{
		//재장전
		if (curshotdelay < maxshotdelay)
			return;

		if (gameObject.transform.position.y < 0)
			return;

		//적의 이름이 S(small)이면
		if(enemyname == "S")
		{
			GameObject bullet = objmanager.MakeObj("enemybulletA");
			bullet.transform.position = transform.position + Vector3.down * 0.2f;

			Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
			Vector3 dirvec = player.transform.position - gameObject.transform.position;
			rigid.AddForce(dirvec.normalized * 5, ForceMode2D.Impulse);
		}
		//적의 이름이 L이면
		else if(enemyname == "L")
		{
			//게임오브젝트 생성(총알)
			GameObject bulletR = objmanager.MakeObj("enemybulletB");
			bulletR.transform.position = transform.position + Vector3.right * 0.2f;
			GameObject bulletL = objmanager.MakeObj("enemybulletB");
			bulletL.transform.position = transform.position + Vector3.left * 0.2f;

			//컴포넌트 받아오기
			Rigidbody2D rigidR = bulletR.GetComponent<Rigidbody2D>();
			Rigidbody2D rigidL = bulletL.GetComponent<Rigidbody2D>();

			//방향 설정
			Vector3 dirvecR = player.transform.position - gameObject.transform.position + Vector3.right * 0.2f;
			Vector3 dirvecL = player.transform.position - gameObject.transform.position + Vector3.left * 0.2f;

			//물리설정
			rigidR.AddForce(dirvecR.normalized * 5, ForceMode2D.Impulse);
			rigidL.AddForce(dirvecL.normalized * 5, ForceMode2D.Impulse);
		}

		curshotdelay = 0;
	}

	//충돌시 함수
	public void OnHit(int dmg)
	{
		if (hp <= 0)
			return;

		hp -= dmg;
		if(enemyname =="B")
		{
			bossani.SetTrigger("OnHit");
		}
		else
		{
			spriteRenderer.sprite = sprites[1];
			Invoke("ReturnSprite", 0.5f);
		}

		//enemy의 hp가 0이하일시
		if (hp <= 0)
		{
			Player playerlogic = player.GetComponent<Player>();
			playerlogic.score += enemyscore;

			//랜덤으로 아이템 드랍
			// 2 ~ 6 코인
			// 7 ~ 8 파워
			// 9 ~   필살기
			int rand =enemyname == "B" ? 0 : Random.Range(0, 10);
			if(rand < 2) //꽝 30%
			{
				Debug.Log("Not Item");
			}
			else if(rand <7)	//코인 50%
			{
				GameObject itemcoin = objmanager.MakeObj("itemcoin");
				itemcoin.transform.position = transform.position;
			}
			else if(rand < 9)	//파워 20%
			{
				GameObject itempower = objmanager.MakeObj("itempower");
				itempower.transform.position = transform.position;
			}
			else if(rand == 9)	//필살기 10%
			{
				GameObject itemboom = objmanager.MakeObj("itemboom");
				itemboom.transform.position = transform.position;
			}

			gameObject.SetActive(false);
			CancelInvoke();
			transform.rotation = Quaternion.identity;
			gamemanager.CallExplosion(transform.position, enemyname);

			//Kill Boss
			if (enemyname == "B")
				gamemanager.StageEnd();
		}
	}

	void ReturnSprite()
	{
		spriteRenderer.sprite = sprites[0];
	}

	//충돌
	void OnTriggerEnter2D(Collider2D collision)
	{
		//맵에서 벗어남
		if(collision.gameObject.tag == "BulletBorder" && enemyname != "B")
		{
			gameObject.SetActive(false);
			transform.rotation = Quaternion.identity;
		}
		//플레이어의 총알과 충돌
		else if(collision.gameObject.tag == "PlayerBullet")
		{
			Bullet bullet = collision.gameObject.GetComponent<Bullet>();
			OnHit(bullet.dmg);
			collision.gameObject.SetActive(false);
		}

	}
	

}
