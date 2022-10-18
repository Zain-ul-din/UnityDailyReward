using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Randoms.DailyReward
{
    using Internals;

    public class DailyRewardManager: MonoBehaviour
    {
        
        [SerializeField] private Text timeCounterText;
        public static DailyRewardManager Instance {get; private set;}
        private bool isInitialized = false;
        private bool canRefreshUI  = true;
        
        void Awake ()
        {
            if (Instance) Destroy (this);
            Instance = this;
            Debug.Log (PlayerPrefs.GetString ("RANDOMS_DAILYREWARD_STORE"));
        }
        
        void Start()
        {
            Init ();
            isInitialized = true;
            StartCoroutine (CountTimer());
        }

        void OnEnable ()
        {
            if (!isInitialized) return;
            Init ();
        }

        void OnDisable()
        {
            StopAllCoroutines ();
        }

        void OnDestroy()
        {
            StopAllCoroutines ();
            DailyRewardBtn.dailyRewardBtns.Clear (); 
        }

        
        /// <summary>
        /// Invokes Action On Btns
        /// </summary>
        void Init ()
        {
            foreach (var btn in DailyRewardBtn.dailyRewardBtns)
            {
                var (canClaim, status) = DailyRewardInternal.GetDailyRewardStatus (btn.day);
                switch (status)
                {
                    case DailyRewardStatus.CLAIMED:  btn.OnClaimed?.Invoke (); break;
                    case DailyRewardStatus.UNCLAIMED_UNAVAILABLE:  btn.OnClaimUnAvailable?.Invoke();  break;
                }
                
                if (status == DailyRewardStatus.UNCLAIMED_AVAILABLE && canClaim)
                {
                    btn.OnClaim?.Invoke ();
                    btn.btn.onClick.AddListener (()=> DailyRewardInternal.ClaimTodayReward (()=> Init ()));
                }
            }
        }    
        
        IEnumerator CountTimer ()
        {
            while (true)
            {
                yield return new WaitForSeconds (1f);
                if (timeCounterText)
                {
                    timeCounterText.text = "" + DailyRewardInternal.NextRewardTimer();
                }

                if (DailyRewardInternal.CanClaimTodayReward () && canRefreshUI)
                {
                    canRefreshUI = false;
                    Init ();
                }
                else
                {
                    canRefreshUI = true;
                }
            }
        }
    }
}

