using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusic : MonoBehaviour {

	
	
	void Start()
	{
		DontDestroyOnLoad(gameObject);
	}

	void Update()
	{
		GameObject[] instances = GameObject.FindGameObjectsWithTag("BGMusic");
		if(instances.Length >1)
		{
			Destroy(instances[1].gameObject);
		}
	}
}
