using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GiftScript : MonoBehaviour {

	
	public Animator freeGift, videoGift, watchVideo, noThanks;
	public float openVideoAdOptionAfter = 3f;
	Animator anim;
	ButtonCanvas bc;

	void Start () 
	{
		bc = GameObject.FindGameObjectWithTag("ButtonCanvas").GetComponent<ButtonCanvas>();
		anim = this.GetComponent<Animator>();		
	}
	
	public void DelayGift()
	{
		Invoke("FreeGift", 0.4f);		
	}
	void FreeGift()
	{
		freeGift.Play("FreeGift");
		freeGift.SetBool("Gift", true);
		// noThanks.gameObject.SetActive(false);
		// watchVideo.gameObject.SetActive(false);
		Invoke("VideoGiftOption", openVideoAdOptionAfter);
	}

	void VideoGiftOption()
	{		
		videoGift.Play("FreeGift");
		noThanks.Play("GiftButtonOpen");
		watchVideo.Play("GiftButtonOpen");

		// freeGift.gameObject.SetActive(false);
		// videoGift.gameObject.SetActive(true);
		// noThanks.gameObject.SetActive(true);
		// watchVideo.gameObject.SetActive(true);
	}


	public void NoThanks()
	{
		videoGift.SetBool("Gift", true);
		noThanks.SetBool("ButtonClose", true);
		watchVideo.SetBool("ButtonClose", true);
		Invoke("ClosePanel", videoGift.GetCurrentAnimatorClipInfo(0).Length);
		
	}
	public void WatchVideoAd()
	{
		videoGift.SetBool("Gift", true);
		noThanks.SetBool("ButtonClose", true);
		watchVideo.SetBool("ButtonClose", true);
		Invoke("ClosePanel", videoGift.GetCurrentAnimatorClipInfo(0).Length);
	}
	
	void ClosePanel()
	{
		anim.Play("PanelClose");
		bc.GiftTimerRestart();
	}
}
