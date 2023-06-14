using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGround : MonoBehaviour
{
    public float speed;

	public int startindex;
	public int endindex;
	public Transform[] sprites;

	float viewheight;

	private void Awake()
	{
		//메인카메라 사이즈 받아오기
		viewheight = Camera.main.orthographicSize *2;
	}

	private void Update()
	{

		//아래로 이동
		Vector3 curpos = transform.position;
		Vector3 nextpos = Vector3.down * speed * Time.deltaTime;
		transform.position = curpos + nextpos;

		//맵 스크롤링
		if(sprites[endindex].position.y < viewheight *(-1))
		{
			Vector3 backspritespos = sprites[startindex].localPosition;
			Vector3 frontspritespos = sprites[endindex].localPosition;


			sprites[endindex].transform.localPosition = backspritespos + Vector3.up * viewheight;

			int startindexsave = startindex;
			startindex = endindex;
			endindex = startindexsave - 1 == -1 ? sprites.Length - 1 : startindexsave - 1;
		}

	}
}
