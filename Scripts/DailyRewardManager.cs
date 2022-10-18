using System.Collections;
using System.Collections.Generic;
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
        private DailyRewardBtn activeBtn;

        void Awake ()
        {
            if (Instance) Destroy (this);
            Instance = this;
        }
        
        void Start()
        {
            isInitialized = true;
            StartCoroutine (CountTimer());
            Init ();
        }

        void OnEnable ()
        {
            Debug.Log (PlayerPrefs.GetString ("RANDOMS_DAILYREWARD_STORE"));
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

        public void CollectReward ()
        {
            activeBtn.onRewardCollect?.Invoke ();
        }

        public void Collect2XReward ()
        {
            activeBtn.on2XRewardCollect?.Invoke ();
        }

        

        /// <summary>
        /// Invokes Action On Btns
        /// </summary>
        void Init ()
        {

            
            foreach (var btn in DailyRewardBtn.dailyRewardBtns)
            {
                btn.Init ();
                var (canClaim, status) = DailyRewardInternal.GetDailyRewardStatus (btn.day);
                switch (status)
                {
                    case DailyRewardStatus.CLAIMED:  btn.OnClaimedState?.Invoke (); break;
                    case DailyRewardStatus.UNCLAIMED_UNAVAILABLE:  btn.OnClaimUnAvailableState?.Invoke();  break;
                }
                
                // ative btn
                if (status == DailyRewardStatus.UNCLAIMED_AVAILABLE && canClaim)
                {
                    activeBtn = btn;
                    btn.OnClaimState?.Invoke ();
                    btn.btn.onClick.AddListener (()=> DailyRewardInternal.ClaimTodayReward (()=> {
                        Init ();
                        btn.onClick?.Invoke (); 
                    }));
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

