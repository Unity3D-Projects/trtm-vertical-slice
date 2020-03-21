using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;

namespace Assets.Scripts.Controllers
{
    public class AdvertisementManager : MonoBehaviour, IUnityAdsListener
    {
        private SaveSystem _saveSystem;
        private GameController _controller;

        private Action RewardCallback;

        public bool IsAdsReady { get => Advertisement.IsReady(); private set => IsAdsReady = value; }
        public bool IsShowingAd { get; set; }

#if UNITY_IOS
        private string gameId = "1486551";
#elif UNITY_ANDROID
        private string gameId = "1486550";
#endif

        void Start()
        {
            _saveSystem = GetComponent<SaveSystem>();
            _controller = GetComponent<GameController>();

            Advertisement.AddListener(this);
            Advertisement.Initialize(gameId);
        }

        void Update()
        {
            if (Advertisement.IsReady() && _controller.SpeedUpButton != null && !_controller.SpeedUpButton.gameObject.activeSelf)
            {
                _controller.SpeedUpButton.gameObject.SetActive(true);
            }
        }

        public void ShowAd(string placementId, Action reward)
        {
            RewardCallback = reward;
            Advertisement.Show(placementId);
        }

        public void Test()
        {
            _controller.SkipDelay(1);
        }

        public void OnUnityAdsReady(string placementId)
        {
            if (placementId == "rewardedVideo")
            {
                if (_controller.SpeedUpButton != null)
                {
                    _controller.SpeedUpButton.gameObject.SetActive(true);
                }
            }
        }

        public void OnUnityAdsDidError(string message)
        {
            Debug.Log(message);
        }

        public void OnUnityAdsDidStart(string placementId)
        {
            IsShowingAd = true;

            if (placementId == "rewardedVideo")
            {
                AnalyticsEvent.AdStart(true, placementId);
            }
        }

        public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
        {
            IsShowingAd = false;

            _saveSystem.SaveAdWatchedTime();

            if (showResult == ShowResult.Finished)
            {
                if (placementId == "rewardedVideo")
                {
                    if (RewardCallback != null)
                    {
                        RewardCallback();
                        //Test();
                    }
                    else
                    {
                        throw new Exception("Reward callback was null");
                    }

                    AnalyticsEvent.AdComplete(true, placementId);
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                if (placementId == "rewardedVideo")
                {
                    AnalyticsEvent.AdSkip(true, placementId);
                }
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }
    }
}
