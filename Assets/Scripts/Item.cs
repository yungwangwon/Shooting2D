using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public string type; //������ �̸�
	Rigidbody2D rigid;

	private void Awake()
	{
		rigid = GetComponent<Rigidbody2D>();
	}

	private void OnEnable()
	{
		rigid.velocity = Vector2.down * 1.0f;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		//�ʿ��� ���
		if (collision.gameObject.tag == "BulletBorder")
			gameObject.SetActive(false);
	}
}
