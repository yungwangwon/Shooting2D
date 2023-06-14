using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	public int dmg;
	public bool isrotate;

	private void Update()
	{
		if(isrotate)
		{
			transform.Rotate(Vector3.forward * 10);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if(collision.gameObject.tag == "BulletBorder")
		{
			gameObject.SetActive(false);
		}
	}
}
