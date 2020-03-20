using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Controllers
{
    public class AdvertisementManager : MonoBehaviour
    {
        private PrefabManager _prefabManager;
        private Spawner _spawner;
        private SaveSystem _saveSystem;
        public bool IsShowingAd { get; set; }

        void Start()
        {
            _prefabManager = GetComponent<PrefabManager>();
            _spawner = GetComponent <Spawner>();
            _saveSystem = GetComponent<SaveSystem>();
        }

        public void ShowAd(Action callback)
        {
            var coroutine = new CoroutineObject<Action>(this, ShowAdCoroutine);
            coroutine.Start(callback);
        }

        private IEnumerator ShowAdCoroutine(Action callback)
        {
            Debug.Log("This is where the ad is shown on the screen");

            var ad = Instantiate(_prefabManager.adMockPrefab, _prefabManager.canvas);

            double timePassed = (DateTime.Now - DateTime.Now).TotalMinutes;
            Slider s = Instantiate(_prefabManager.sliderPrefab, ad.transform);
            s.interactable = false;
            s.value = (float)(timePassed / 0.25f);

            float remainingInMinutes = 0.25f;
            while (remainingInMinutes > 0)
            {
                remainingInMinutes -= Time.deltaTime / 60;
                s.value += Time.deltaTime / (0.25f * 60);
                yield return null;
            }
            Destroy(s.gameObject);

            IsShowingAd = true;

            Destroy(ad.gameObject);

            IsShowingAd = false;

            _saveSystem.SaveAdWatchedTime();

            callback();
        }
    }
}
