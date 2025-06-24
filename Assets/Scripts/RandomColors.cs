using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomColors : MonoBehaviour {

	Pen pen;
	public bool textureChange;
	public Texture[] textures;

	void Start()
	{
		
		pen = GameObject.FindGameObjectWithTag("Player").GetComponent<Pen>();
		if(!textureChange)
		{
			int randomNo = Random.Range(0, pen.colors.Length);
			this.GetComponent<MeshRenderer>().material.color = pen.colors[randomNo];
		}
		else
		{
			int randomNo = Random.Range(0, textures.Length);
			this.GetComponent<MeshRenderer>().material.SetTexture("_MainTex", textures[randomNo]);		
		}
	}
}
