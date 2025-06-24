using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

	public Transform target;
	public float smoothing = 1f;
	float offset;
	public bool follow = true;
	[HideInInspector]
	public bool switchOff;
	public Vector3 initialPos;
	Vector3 targetPos;

	
	void Start()
	{		
		target = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
		offset = transform.position.z - target.position.z;
		initialPos = transform.position;
	}
	void FixedUpdate()
	{
		// moving in z axis
		// Debug.Log(diff);

		if(follow)
		{
			// if(Mathf.Abs(diff) > 35)
			// {
			// 	targetPos = target.position;
			// }
			// else
			// {
			// 	targetPos = transform.position;
			// }

			Vector3 desiredPos = new Vector3(target.position.x, transform.position.y, target.position.z + offset);
			Vector3 lerpedPos = Vector3.Lerp(transform.position, desiredPos, Time.smoothDeltaTime*smoothing);
			transform.position = lerpedPos;
		}
		if(switchOff)
		{
			FollowOff();
		}
	
	}
	public Vector4 offsets;
	void Update()
	{

		// Shader.SetGlobalVector("Offset",offsets);
		/* if(follow)
		{
			float diff =  target.position.x - transform.position.x;

			if(Mathf.Abs(diff) > 55)
			{
				targetPos = target.position;
			}
			else
			{
				targetPos = transform.position;
			}
		} */
	}

	public void FollowOff()
	{
		
	}

	/* Vector2 Offset = Vector2.zero;
    float camPos = -20;
    public Material[] Mats;
    public Transform cam;
    
    void OnGUI ()
    {
        GUILayout.BeginArea(new Rect(5,5,Screen.width-10,Screen.height-10));

        GUILayout.BeginHorizontal();
        GUILayout.Label("xOffset",GUILayout.Width(100));
        Offset.x = GUILayout.HorizontalSlider(Offset.x,-20,20);
        if (GUILayout.Button("0",GUILayout.Width(30)))
            Offset.x = 0;
        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("yOffset",GUILayout.Width(100));
        Offset.y = GUILayout.HorizontalSlider(Offset.y,-20,20);
        if (GUILayout.Button("0",GUILayout.Width(30)))
            Offset.y = 0;

        GUILayout.EndHorizontal();

        GUILayout.BeginHorizontal();
        GUILayout.Label("Cam pos",GUILayout.Width(100));
        camPos = GUILayout.HorizontalSlider(camPos,-40,30);
        GUILayout.EndHorizontal();
        GUILayout.EndArea();
        foreach(Material M in Mats)
        {
            M.SetVector("_QOffset",Offset);
        }
        Vector3 P = cam.position;
        P.z = camPos;
        cam.position = P;

    } */
	
}
