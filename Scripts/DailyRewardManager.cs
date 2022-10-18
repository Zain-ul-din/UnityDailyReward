using System.Collections;
using System.Collections.Generic;
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
        public DailyRewardBtn activeBtn {get; private set;}

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
            if (DailyRewardBtn.dailyRewardBtns == null)
            {
                DailyRewardBtn.dailyRewardBtns = new List<DailyRewardBtn> ();
                foreach (var btn in FindObjectsOfType <DailyRewardBtn> ())
                {
                    btn.Init ();
                    DailyRewardBtn.dailyRewardBtns.Add (btn);
                }
            }

            foreach (var btn in DailyRewardBtn.dailyRewardBtns)
            {
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

