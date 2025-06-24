﻿using UnityEngine;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public int frequency;

	public bool lookForward;

	public Transform[] items;
	ButtonCanvas bc;


	private void Awake () {
		 bc = GameObject.FindGameObjectWithTag("ButtonCanvas").GetComponent<ButtonCanvas>();
		 bc.totalNumberPath = frequency;
		if (frequency <= 0 || items == null || items.Length == 0) {
			return;
		}
		float stepSize = frequency * items.Length;
		if (spline.Loop || stepSize == 1) {
			stepSize = 1f / stepSize;
		}
		else {
			stepSize = 1f / (stepSize-1); //	stepSize = 1f / (stepSize - 1);
		}
		// GameObject parent = new GameObject("Instantiated");
		for (int p = 0, f = 0; f < frequency; f++) {
			for (int i = 0; i < items.Length; i++, p++) {
				Transform item = Instantiate(items[i]) as Transform;
				// item.parent = parent.transform;
				Vector3 position = spline.GetPoint(p * stepSize);
				item.transform.localPosition = position;
				if (lookForward) {
					item.transform.LookAt(position + spline.GetDirection(p * stepSize));
					// item.eulerAngles = new Vector3(transform.eulerAngles.x + 90f, transform.eulerAngles.y+90, transform.eulerAngles.z);
				}
				item.transform.parent = transform;
			}
		}
	}
}