using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectibleRotate : MonoBehaviour {

	public Vector3 rotValues;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update ()
	{
		transform.Rotate(rotValues* Time.deltaTime);
	}
}
