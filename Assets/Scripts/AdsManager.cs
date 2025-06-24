using System;
using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsLoadListener, IUnityAdsShowListener
{
    private string gameId = "5729454"; // Replace with your Unity game ID
    private string bannerAdId = "Banner_Android"; // Replace with your banner ad ID
    private string interstitialAdId = "Interstitial_Android"; // Replace with your interstitial ad ID
    private string rewardedAdId = "Rewarded_Android"; // Replace with your rewarded ad ID
    private bool testMode = false; // Set to false for production
    private bool interstitialAdLoaded = false;
    private bool rewardedAdLoaded = false;

    void Start()
    {
        Advertisement.Initialize(gameId, testMode, this);
        ShowBannerAd();
    }

    // Load Banner ad
    public void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER); // Position the banner at the bottom
        Advertisement.Banner.Show(bannerAdId); // Show the banner ad
        Debug.Log("Banner ad is shown at the bottom.");
    }

    // Load Interstitial ad
    public void LoadInterstitialAd()
    {
        Advertisement.Load(interstitialAdId, this);  // Load the interstitial ad
    }

    // Show Interstitial ad
    public void ShowInterstitialAd()
    {
        // Check if the interstitial ad is loaded before showing
        if (interstitialAdLoaded)
        {
            Advertisement.Show(interstitialAdId, this);
        }
        else
        {
            Debug.LogWarning("Interstitial ad is not ready to show.");
            LoadInterstitialAd(); // Load the ad if it is not ready
        }
    }

    // Load Rewarded ad
    public void LoadRewardedAd()
    {
        Advertisement.Load(rewardedAdId, this);  // Load the rewarded ad
    }

    // Show Rewarded ad
    public void ShowRewardedAd(Action<bool> onComplete = null)
    {
        // Check if the rewarded ad is loaded before showing
        if (rewardedAdLoaded)
        {
            Advertisement.Show(rewardedAdId, this);
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready to show.");
            LoadRewardedAd(); // Load the rewarded ad again if it's not ready
        }
    }
    public void ShowRewarded( )
    {
        // Check if the rewarded ad is loaded before showing
        if (rewardedAdLoaded)
        {
            Advertisement.Show(rewardedAdId, this);
        }
        else
        {
            Debug.LogWarning("Rewarded ad is not ready to show.");
            LoadRewardedAd(); // Load the rewarded ad again if it's not ready
        }
    }
    // Unity Ads Initialization Listener Callbacks
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");
        LoadInterstitialAd();  // Load interstitial ad after initialization
        LoadRewardedAd();      // Load rewarded ad after initialization
        ShowBannerAd();        // Show banner ad at the bottom
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    // Unity Ads Load Listener Callbacks
    public void OnUnityAdsAdLoaded(string placementId)
    {
        Debug.Log($"{placementId} ad is loaded and ready to show.");
        if (placementId == interstitialAdId)
        {
            interstitialAdLoaded = true;
        }
        else if (placementId == rewardedAdId)
        {
            rewardedAdLoaded = true;
        }
    }

    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {placementId}: {error} - {message}");
    }

    // Unity Ads Show Listener Callbacks
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == interstitialAdId)
        {
            Debug.Log("Interstitial ad finished.");
            interstitialAdLoaded = false;
            LoadInterstitialAd();  // Load another interstitial ad after showing
        }
        else if (placementId == rewardedAdId && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Rewarded ad completed. Reward the player.");
            // Handle player reward here
            rewardedAdLoaded = false;
        }
    }

    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show ad {placementId}: {error} - {message}");
        if (placementId == interstitialAdId)
        {
            interstitialAdLoaded = false;
            LoadInterstitialAd();
        }
        else if (placementId == rewardedAdId)
        {
            rewardedAdLoaded = false;
            LoadRewardedAd();
        }
    }

    public void OnUnityAdsShowStart(string placementId) { }
    public void OnUnityAdsShowClick(string placementId) { }
}
