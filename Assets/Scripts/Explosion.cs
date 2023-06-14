using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponent<Animator>();
    }

	void OnEnable()
	{
		Invoke("Disable", 2f);
	}

	void Disable()
	{
		gameObject.SetActive(false);
	}

	public void StartExplosion(string target)
	{
        ani.SetTrigger("isExplosion");

        switch(target)
        {
			case "S":
				transform.localScale = Vector3.one * 0.5f;
				break;
			case "M":
			case "P":
				transform.localScale = Vector3.one * 1f;
				break;
			case "L":
				transform.localScale = Vector3.one * 2f;
				break;
			case "B":
				transform.localScale = Vector3.one * 3f;
				break;
		}
	}
}
