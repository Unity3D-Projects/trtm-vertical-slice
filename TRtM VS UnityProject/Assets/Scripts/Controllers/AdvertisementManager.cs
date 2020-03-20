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
        private PrefabManager _prefabManager;
        private Spawner _spawner;
        private SaveSystem _saveSystem;
        private GameController _controller;

        public bool IsAdsReady { get => Advertisement.IsReady(); private set => IsAdsReady = value; }
        public bool IsShowingAd { get; set; }

#if UNITY_IOS
        private string gameId = "1486551";
#elif UNITY_ANDROID
        private string gameId = "1486550";
#endif

        void Start()
        {
            _prefabManager = GetComponent<PrefabManager>();
            _spawner = GetComponent <Spawner>();
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

        public void ShowAd(string placementId)
        {
            Advertisement.Show(placementId);

            
        }

        //private IEnumerator ShowAdCoroutine()
        //{
        //
        //    //Debug.Log("This is where the ad is shown on the screen");
        //    //
        //    //var ad = Instantiate(_prefabManager.adMockPrefab, _prefabManager.canvas);
        //    //
        //    //double timePassed = (DateTime.Now - DateTime.Now).TotalMinutes;
        //    //Slider s = Instantiate(_prefabManager.sliderPrefab, ad.transform);
        //    //s.interactable = false;
        //    //s.value = (float)(timePassed / 0.25f);
        //    //
        //    //float remainingInMinutes = 0.25f;
        //    //while (remainingInMinutes > 0)
        //    //{
        //    //    remainingInMinutes -= Time.deltaTime / 60;
        //    //    s.value += Time.deltaTime / (0.25f * 60);
        //    //    yield return null;
        //    //}
        //    //Destroy(s.gameObject);
        //    //Destroy(ad.gameObject);
        //
        //}

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
                AnalyticsEvent.AdStart(true, AdvertisingNetwork.UnityAds, placementId);
                AnalyticsEvent.Custom("customAdStart");
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
                    _controller.SkipDelay(0.2f);
                    AnalyticsEvent.AdComplete(true, AdvertisingNetwork.UnityAds, placementId);
                }
            }
            else if (showResult == ShowResult.Skipped)
            {
                if (placementId == "rewardedVideo")
                {
                    AnalyticsEvent.AdSkip(true, AdvertisingNetwork.UnityAds, placementId);
                }
            }
            else if (showResult == ShowResult.Failed)
            {
                Debug.LogWarning("The ad did not finish due to an error.");
            }
        }
    }
}
