using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Follower : MonoBehaviour
{
	public float curshotdelay;
	public float maxshotdelay;

	public ObjectManager objmanager;

	public Vector3 followerpos;
	public int followdelay;
	public Transform parent;
	public Queue<Vector3> parentpos;

	void Awake()
	{
		parentpos = new Queue<Vector3>();
	}

	void Update()
	{
		Watch();
		Follow();
		Fire();
		Reload();
	}

	void Watch()
	{
		//input pos
		if (!parentpos.Contains(parent.position)) 
			parentpos.Enqueue(parent.position);
		
		//out pos
		if(parentpos.Count > followdelay)	//큐가 followdalay이상만큼 채워져있으면
		{
			followerpos = parentpos.Dequeue();
		}
		else if(parentpos.Count < followdelay)
		{ 
			followerpos = parent.position;
		}

	}

	//팔로워 이동
	void Follow()
	{
		transform.position = followerpos;
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

		GameObject bullet = objmanager.MakeObj("followerbullet");
		bullet.transform.position = transform.position + Vector3.up * 0.2f;
		Rigidbody2D rigid = bullet.GetComponent<Rigidbody2D>();
		rigid.AddForce(Vector2.up * 10, ForceMode2D.Impulse);

		curshotdelay = 0;


	}
}
