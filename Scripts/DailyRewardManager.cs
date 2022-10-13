using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Randoms.DailyReward
{
    using Internals;
    using Editor;

    [HelperBox (" Public APIS:\n DailyRewardManager.CollectReward() \n ||\n DailyRewardManager.Collect2XReward()")]
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

        public void CollectReward ()
        {
            activeBtn.onRewardCollect?.Invoke ();
        }

        public void Collect2XReward ()
        {
            activeBtn.on2XRewardCollect?.Invoke ();
        }

        // Helpers

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
                    case DailyRewardStatus.CLAIMED:  btn.OnClaimedState?.Invoke (); break;
                    case DailyRewardStatus.UNCLAIMED_UNAVAILABLE:  btn.OnClaimUnAvailableState?.Invoke();  break;
                }
                
                if (status == DailyRewardStatus.UNCLAIMED_AVAILABLE && canClaim)
                {
                    activeBtn = btn;
                    btn.OnClaimState?.Invoke ();
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

