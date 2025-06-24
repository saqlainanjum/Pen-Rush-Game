using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pen : MonoBehaviour {	

	Rigidbody rb;
	float offset;
    public float forwardSpeed = 50f;
	public float rotSpeed = 10f;
	public float rotSensitivity = 8f;
	public float smoothing = 2f;
	public Vector2 xClamp;
	public Vector2 xRotClamp = new Vector2 (-15f,15f);

	// ButtonCanvas buttonCanvas; 
	public float apparentSwipeSpeed = 500f;
	public bool hit = false;
	public float health = 100;
	public float inkIncreaseValue = 20f;
	public float decreaseFactor = 5f;
	public GameObject inkBar;
	bool empty, dead, right, left, win;
	public Color[] colors;
	public Color currentColor;
	public Material colorMaterial;
	public Material trailMaterial, collectibleMaterial;
	Vector3 store, storeRot;
	float timer = 0;
	int randomColor;
	ButtonCanvas bc;
	public GameObject collectibleParticles;
	public AudioClip collectSound, drawSound, winSound, hitSound;
	AudioSource aS;
	void Start () 
	{
		bc = GameObject.FindGameObjectWithTag("ButtonCanvas").GetComponent<ButtonCanvas>();
		rb = this.GetComponent<Rigidbody>();
		aS = this.GetComponent<AudioSource>();
		
		randomColor = PlayerPrefs.GetInt("PenColor");
			
		colorMaterial.color = colors[randomColor];
		trailMaterial.color = colors[randomColor];
		currentColor =  colors[randomColor];
		CollectibleColor();
		bc.SliderColor(currentColor);
        
	}	
	float finalPos ;
	Vector3 lerpedPos;
	bool move;
	void Update()
	{		
		if(!dead && !win && bc.start)
		{
			health -= Time.deltaTime*decreaseFactor;
			if(health > 0.02)
			{
				Vector3 inkScale = new Vector3(inkBar.transform.localScale.x, health/100, inkBar.transform.localScale.z);
				inkBar.transform.localScale = inkScale;
			}
			else
			{
				OnDeath();
			}

			if(Input.GetMouseButtonDown(0))
			{
				store = Input.mousePosition;
				Vector3 initialPos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, apparentSwipeSpeed));
				offset = transform.position.x - initialPos.x;
				aS.clip = drawSound;				
				aS.Play();
			}
			if (Input.GetMouseButton(0))
			{
				timer = 0;
				Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, apparentSwipeSpeed));
				Vector3 desiredPos = new Vector3(currentPos.x+offset, transform.position.y, transform.position.z);
				/* Vector3 */ lerpedPos = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothing);
				/* float */ finalPos = Clamp(lerpedPos.x, xClamp.x, xClamp.y);
				// transform.position = new Vector3(finalPos, lerpedPos.y, lerpedPos.z);
				move = true;

				float checkDir = Input.mousePosition.x - store.x ;				
				
				float rot = Mathf.Clamp(checkDir/rotSensitivity, xRotClamp.x, xRotClamp.y);
				Quaternion lerpedRot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,rot), Time.deltaTime*rotSpeed);
				transform.rotation = lerpedRot;
				storeRot = transform.rotation.eulerAngles;					
			}
			else
			{			
				move = false;	
				timer += Time.deltaTime;
				if(timer >= 0.2f)
				{
					transform.rotation = Quaternion.Lerp(transform.rotation, 
										Quaternion.identity, Time.deltaTime *rotSpeed/2);
					
				}
				else
				transform.rotation = Quaternion.Lerp(transform.rotation, 
										Quaternion.Euler(0,0,-(storeRot.z)), Time.deltaTime *rotSpeed/2);
				
			}
		}
		if(Input.GetMouseButtonUp(0))
		{
			aS.Stop();
		}

	}
	
	public float initialLerpSpeed, finalLerpSpeed, timeToChange;
	float timerForLerp;
	bool changeLerp;
	void FixedUpdate () 
	{
		if(!dead && !win && bc.start)
		{
			if(move)
			{
				Vector3 newLerp = new Vector3(finalPos, lerpedPos.y, lerpedPos.z);
				if(!changeLerp)
				{
					transform.position = Vector3.Lerp(transform.position, newLerp, Time.fixedDeltaTime*initialLerpSpeed);
				}
				else
				{					
					transform.position = Vector3.Lerp(transform.position, newLerp, Time.fixedDeltaTime*finalLerpSpeed);
				}
				timerForLerp+= Time.fixedDeltaTime;
				if(timerForLerp >= timeToChange)
				{
					changeLerp = true;
				}				
			}
			else
			{
				timerForLerp=0;
				changeLerp = false;
			}
			#region old
			/* if(Input.GetMouseButtonDown(0))
			{
				store = Input.mousePosition;
				Vector3 initialPos =  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, apparentSwipeSpeed));
				offset = transform.position.x - initialPos.x;
			}
			if (Input.GetMouseButton(0))
			{
				timer = 0;
				Vector3 currentPos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, 0, apparentSwipeSpeed));
				Vector3 desiredPos = new Vector3(currentPos.x+offset, transform.position.y, transform.position.z);
				Vector3 lerpedPos = Vector3.Lerp(transform.position, desiredPos, Time.deltaTime * smoothing);
				// float finalPos = Mathf.Clamp(lerpedPos.x, xClamp.x, xClamp.y);
				float finalPos = Clamp(lerpedPos.x, xClamp.x, xClamp.y);
				transform.position = new Vector3(finalPos, lerpedPos.y, lerpedPos.z);
				
				


				float checkDir = Input.mousePosition.x - store.x ;				
				
				float rot = Mathf.Clamp(checkDir/rotSensitivity, xRotClamp.x, xRotClamp.y);
				Quaternion lerpedRot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,rot), Time.deltaTime*rotSpeed);
				transform.rotation = lerpedRot;
				storeRot = transform.rotation.eulerAngles;					
			}
			else
			{				
				timer += Time.deltaTime;
				if(timer >= 0.2f)
				{
					transform.rotation = Quaternion.Lerp(transform.rotation, 
										Quaternion.identity, Time.deltaTime *rotSpeed/2);
					
				}
				else
				transform.rotation = Quaternion.Lerp(transform.rotation, 
										Quaternion.Euler(0,0,-(storeRot.z)), Time.deltaTime *rotSpeed/2);
				
			} */	
			#endregion		
			if(!win)
			{
				rb.velocity = Vector3.forward* forwardSpeed;
			}			
		}
		if(win)
		{
			Quaternion lerpedRot = Quaternion.Lerp(transform.rotation, Quaternion.Euler(0,0,0), Time.deltaTime*rotSpeed);
			transform.rotation = lerpedRot;
		}
		// Debug.Log(rb.velocity);
	}	

	public void AddInk(float inkValue)
	{
		health += inkValue;
		if(health > 100)
		{
			health = 100;
		}
	}
	public void AddCoin()
	{
		PlayerPrefs.SetInt("TotalCoins", PlayerPrefs.GetInt("TotalCoins") + 1);
	}

	void OnDeath()
	{
		dead = true;
		bc.lose = true;
		bc.start = false;
		rb.constraints = RigidbodyConstraints.None;
		rb.velocity = Vector3.forward* forwardSpeed;
		rb.AddTorque(Vector3.up*10);		
		this.gameObject.tag = "Untagged";
		Transform child = this.transform.GetChild(0).transform;
		child.parent = null;		
		Debug.Log("Death");
	}

	void OnWin()
	{
		win = true;
		bc.win = true;
		bc.start = false;
		rb.velocity = Vector3.zero;
		
	}

	
	void OnTriggerEnter(Collider other)
	{
		if(other.gameObject.tag == "Fill" && !dead)
		{
			other.GetComponent<Renderer>().material.color = colors[randomColor];
			other.gameObject.tag = "Untagged";
			
			other.GetComponentInParent<Animator>().Play("PathAnim");
			bc.AddProgress();
			
		}
		if(other.gameObject.tag == "Collectible" && !dead)
		{
			other.gameObject.tag = "Untagged";
			other.GetComponent<Animator>().Play("Collected");
			Instantiate(collectibleParticles, other.transform.position, Quaternion.identity);
			collectibleMaterial.color = currentColor;
			// other.GetComponent<BoxCollider>().enabled = false;
			Destroy(other.gameObject,2f);
			AddInk(inkIncreaseValue);
			// sound
			AudioSource soundInstance = new GameObject("CollectSound").AddComponent<AudioSource>().GetComponent<AudioSource>();
			soundInstance.clip = collectSound;
			soundInstance.Play();
			Destroy(soundInstance.gameObject, soundInstance.clip.length);
		}
		if(other.gameObject.tag == "Coins" && !dead)
		{
			other.GetComponent<Animator>().Play("Collected");
			// Instantiate(collectibleParticles, other.transform.position, Quaternion.identity);
			// collectibleMaterial.color = currentColor;
			// other.GetComponent<BoxCollider>().enabled = false;
			Destroy(other.gameObject,2f);
			AddCoin();
		}

		if(other.gameObject.tag == "Finish" && !dead)
		{
			OnWin();
			AudioSource soundInstance = new GameObject("WinSound").AddComponent<AudioSource>().GetComponent<AudioSource>();
			soundInstance.clip = winSound;
			soundInstance.Play();
			Destroy(soundInstance.gameObject, soundInstance.clip.length);
		}
	}

	void OnCollisionEnter(Collision other)
	{
		if(other.collider.tag != "Ground" && !dead)
		{
			// hit = true;	
			OnDeath();
			AudioSource soundInstance = new GameObject("HitSound").AddComponent<AudioSource>().GetComponent<AudioSource>();
			soundInstance.clip = hitSound;			
			soundInstance.Play();
			Destroy(soundInstance.gameObject, soundInstance.clip.length);
		}
	}

	void CollectibleColor()
	{
		GameObject[] collectibles = GameObject.FindGameObjectsWithTag("Collectible");
		for (int i = 0; i < collectibles.Length; i++)
		{
			collectibles[i].GetComponent<MeshRenderer>().material.color = currentColor;
		}
	}	

	public static float Clamp(float value, float min, float max)
	{
		if (value < min)
		{
			value = min;
		}
		else
		{
			if (value > max)
			{
				value = max;
			}
		}
		return value;
	}
	
}
