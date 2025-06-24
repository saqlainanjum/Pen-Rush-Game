using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEditor;
using GoogleMobileAds.Api;

/* public class MenuItems
{
	[MenuItem("Tools/Clear PlayerPrefs")]
	private static void NewMenuOption()
	{
		PlayerPrefs.DeleteAll();
	}
} */

public class ButtonCanvas : MonoBehaviour {

	public Slider progressBar;
	[SerializeField]
	public float totalNumberPath;
	public int[] startThreshold = {20, 50, 80};
	public Animator[] stars;
	bool star1,star2,star3;
	Pen pc;
	public bool win,lose, start;
	public Text giftTimerText, coinText;
	public Text[] levelNumbers;
	public float giftTimer;
	public GameObject winPanel1, winPanel2, losePanel, mainMenuPanel, gamePanel, levelSelectionPanel,
						giftPanel, tutorial	;
	public Sprite unlockedSprite, lockedSprite, oneStar, twoStar, threeStar;

	public GameObject[] allLevels;
	private AdsManager adsManager;

    /* void Awake()
	{
		for (int i = 0; i < allLevels.Length; i++)
		{
			if(PlayerPrefs.GetInt(i.ToString()) == 1)
			{
				continue;
			}
			else
			{
				SceneManager.LoadScene((i+1).ToString());
				break;
			}
		}
	} */
    void Start () 
	{
	

        PlayerPrefs.SetInt("CurrentStars", 0);
		PlayerPrefs.SetString("LevelToLoad", SceneManager.GetActiveScene().name);
		pc = GameObject.FindGameObjectWithTag("Player").GetComponent<Pen>();
		progressBar.value = 0;
		GiftTimerRestart();
		if(PlayerPrefs.GetInt("Restart") == 1 || PlayerPrefs.GetInt("MenuOn") == 1)
		{
			start = true;
			PlayerPrefs.SetInt("Restart", 0);
			mainMenuPanel.SetActive(false);
			gamePanel.SetActive(true);
			PlayerPrefs.SetInt("MenuOn", 0);
		}
		else
		{
			start = false;
			mainMenuPanel.SetActive(true);
			gamePanel.SetActive(false);
		}
        adsManager = FindObjectOfType<AdsManager>();
        if (adsManager != null)
        {
            adsManager.OnInitializationComplete();
        }
        else
        {
            Debug.LogError("AdsManager not found in the scene.");
        }
    }
    public void SliderColor(Color color)
	{
		ColorBlock cb = progressBar.colors;
        cb.normalColor = color;
        progressBar.colors = cb;
		// Debug.Log("SliderColor");
	}
	

	void Update () 
	{
		if(win && !lose)
		{
			win = false;
			StartCoroutine(PostWin());
		}
		else if(!win && lose)
		{
			lose = false;
			StartCoroutine(PostLose());
		}
		GiftTimer();
		LevelSelectionInitialization();	
		for (int i = 0; i < levelNumbers.Length; i++)
		{
			levelNumbers[i].text = "LEVEL " + SceneManager.GetActiveScene().name;
		}
		
		Debug.Log(PlayerPrefs.GetInt("CurrentStars") + " - CurrentStars");
		Debug.Log(PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"stars") + " - EarnedStars");
		// coinText.text = PlayerPrefs.GetInt("TotalCoins").ToString();
	}

	IEnumerator PostWin()
	{
        yield return new WaitForSeconds(1f);

        adsManager.ShowInterstitialAd();

        yield return new WaitForSeconds(0.5f);
		// winPanel.SetActive(true);
		winPanel1.SetActive(true);
		winPanel1.GetComponent<Animator>().Play("WinPanel1Open");
        // SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        PlayerPrefs.SetInt("MenuOn", 1);
		SaveStars();
	}

	void SaveStars()
	{
		PlayerPrefs.SetInt(SceneManager.GetActiveScene().name, 1);
		if(PlayerPrefs.GetInt("CurrentStars")> PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"stars"))
		{
			PlayerPrefs.SetInt(SceneManager.GetActiveScene().name+"stars", PlayerPrefs.GetInt("CurrentStars"));
			Debug.Log("StarsStored" + PlayerPrefs.GetInt(SceneManager.GetActiveScene().name+"stars"));
		}
	}
	IEnumerator PostLose()
	{
		yield return new WaitForSeconds(1.5f);	
		// losePanel.SetActive(true);
		losePanel.GetComponent<Animator>().Play("PanelOpen");
	}
	
	
	public void AddProgress()
	{
		progressBar.value += 100/(totalNumberPath-3);
		StarAnimation();		
	}

	void StarAnimation()
	{
		if(progressBar.value >= startThreshold[0] && !star1)
		{
			stars[0].Play("Star");
			star1 = true;
			PlayerPrefs.SetInt("CurrentStars", 1);
		}
		else if(progressBar.value >= startThreshold[1] && !star2)
		{
			stars[1].Play("Star");
			star2 = true;
			PlayerPrefs.SetInt("CurrentStars", 2);
		}
		else if(progressBar.value >= startThreshold[2] && !star3)
		{
			stars[2].Play("Star");
			star3 = true;
			PlayerPrefs.SetInt("CurrentStars", 3);
		}
	}

	#region Gift
	public void GiftTimerRestart()
	{
		if(PlayerPrefs.GetFloat("GiftTimer") <=0)
		{
			giftTimer = 1000;
			PlayerPrefs.SetFloat("GiftTimer", giftTimer);
		}
		else
		{
			giftTimer = PlayerPrefs.GetFloat("GiftTimer");
		}
		// Debug.Log("GiftTimer " + giftTimer);
		// Debug.Log("PlayerPrefs " + PlayerPrefs.GetFloat("GiftTimer"));
	}
	void GiftTimer()
	{		
		if(PlayerPrefs.GetFloat("GiftTimer")<=0)
		{
			giftTimerText.enabled = false;
			
		} 
		else
		{
			giftTimerText.enabled = true;
			giftTimer -= Time.deltaTime;
			PlayerPrefs.SetFloat("GiftTimer", giftTimer);
			giftTimerText.text = Mathf.RoundToInt(giftTimer).ToString();
		}
		
	}
	#endregion

	#region CommonButtons
	
	public void Done()
	{
		winPanel1.GetComponent<Animator>().Play("WinPanel1Close");
		winPanel1.SetActive(false);
		winPanel2.GetComponent<Animator>().Play("PanelOpen");
		if(FindObjectOfType<AdMobManager>())
		{
			AdMobManager._AdMobInstance.showInterstitial();
		}
	}
	public void Next()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex +1);
	}
	public void Restart()
	{
		PlayerPrefs.SetInt("Restart", 1);
		SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
		if(FindObjectOfType<AdMobManager>())
		{
			AdMobManager._AdMobInstance.showInterstitial();
		}
	}
	
	public void LevelSelection()
	{
		levelSelectionPanel.GetComponent<Animator>().Play("PanelOpen");
	}
	public void Rate()
	{
		Application.OpenURL("https://play.google.com/store/apps/dev?id=8414334405368948845&pli=1");
	}
	public void WatchVideo()
	{

	}

	#endregion

	#region MainMenuButtons

	public void Play()
	{
		// start = true;
		mainMenuPanel.SetActive(false);
		if(PlayerPrefs.GetInt("Tutorial") == 0)
		{
			tutorial.SetActive(true);
			PlayerPrefs.SetInt("Tutorial", 1);
		}
		else
		{
			start = true;
			gamePanel.SetActive(true);			
		}		
	}

	public void TutorialScreen()
	{
		start = true;
		tutorial.SetActive(false);
		gamePanel.SetActive(true);
		PlayerPrefs.SetInt("Tutorial", 0);
	}
	/* public void Shop()
	{
		
	} */
	
	/* public void Settings()
	{

	} */
	/* public void Gift()
	{
		if(giftTimer<=0)
		{
			giftPanel.GetComponent<Animator>().Play("PanelOpen");
			giftPanel.GetComponent<GiftScript>().DelayGift();
		}
	}	 */
	#endregion

	#region LevelSelectionButtons
	void LevelSelectionInitialization()
	{
		PlayerPrefs.SetInt("0", 1);
		for (int i = 0; i < allLevels.Length; i++)
		{
			allLevels[i].GetComponentInChildren<Text>().text = (i+1).ToString();
			if(PlayerPrefs.GetInt((i).ToString()) == 1)
			{
				RespectiveStarSprite(i);
				allLevels[i].GetComponentInChildren<Text>().enabled  = true;
				// allLevels[i+1].GetComponent<Text>().text = i.ToString();
				allLevels[i].GetComponent<Button>().interactable = true;
			}
			else
			{
				allLevels[i].GetComponent<Image>().sprite = lockedSprite;
				allLevels[i].GetComponentInChildren<Text>().enabled  = false;
				allLevels[i].GetComponent<Button>().interactable = false;
			}
		}
	}

	void RespectiveStarSprite(int i)
	{
		if(PlayerPrefs.GetInt((i+1).ToString()+"stars") == 0)
		{
			allLevels[i].GetComponent<Image>().sprite = unlockedSprite;
		}
		else if((PlayerPrefs.GetInt((i+1).ToString()+"stars") == 1))
		{
			allLevels[i].GetComponent<Image>().sprite = oneStar;
		}
		else if((PlayerPrefs.GetInt((i+1).ToString()+"stars") == 2))
		{
			allLevels[i].GetComponent<Image>().sprite = twoStar;
		}
		else if((PlayerPrefs.GetInt((i+1).ToString()+"stars") == 3))
		{
			allLevels[i].GetComponent<Image>().sprite = threeStar;
		}
	}
    public void UnlockNextLevel()
    {
        for (int i = 0; i < allLevels.Length - 1; i++)
        {
            // Check if the current level is completed
            if (PlayerPrefs.GetInt(i.ToString()) == 1)
            {
                // Unlock the next level only if it's not already unlocked
                if (PlayerPrefs.GetInt((i + 1).ToString()) != 1)
                {
                    PlayerPrefs.SetInt((i + 1).ToString(), 1);
                    Debug.Log("Next level unlocked: " + (i + 2));
                    break;
                }
            }
        }
    }

    public void LevelLoad(string levelNo)
	{
		PlayerPrefs.SetInt("MenuOn", 1);
		SceneManager.LoadScene(levelNo);
	}
	public void LevelSelectionBack()
	{
		levelSelectionPanel.GetComponent<Animator>().Play("PanelClose");
	}

	#endregion

	

}
