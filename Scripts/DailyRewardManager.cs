using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;


namespace Randoms.DailyReward
{
    using Internals;
    using Inspector;

    public class DailyRewardManager: MonoBehaviour
    {
        
        #region Options

        [Header ("Timer Text Options")]
        public Text timeCounterText;
        public string availableText = "AVAILABLE"; 
        public string timerTextPrefix = "Next Reward In";
        public string timerTextPostfix = "TIME";

        [Space(20)]
        [SerializeField] private UnityEvent onActiveBtnClicked;

        #endregion

        #region Docs

        [Button("Clear All DailyReward Player Prefs")]
        private void ClearPlayerPrefs ()
        {
            PlayerPrefs.DeleteKey(DailyRewardInternal.dailyRewardStoreKey);
        }

        [Button("Ask Q&A here OR Mention Issue here")]
        private void Help ()
        {
            Application.OpenURL("https://github.com/login?return_to=https%3A%2F%2Fgithub.com%2FZain-ul-din%2FDailyReward%2Fissues%2Fnew");
        }

        #endregion

        #region Logic

        public static DailyRewardManager Instance { get; private set; }
        private bool isInitialized = false;
        private bool canRefreshUI  = true;
        private DailyRewardBtn activeBtn;
        private static List<DailyRewardBtn> dailyRewardBtns;

        
        void Awake ()
        {
            if (Instance) Destroy (this);
            Instance = this;
            dailyRewardBtns = new List<DailyRewardBtn>();
            _tickListeners = new List<Action<string>>();
            
            foreach (var btn in Resources.FindObjectsOfTypeAll<DailyRewardBtn>())
                dailyRewardBtns.Add (btn);

            Init ();                
        }
        
        void Start()
        {
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
            dailyRewardBtns.Clear (); 
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
            foreach (var btn in dailyRewardBtns)
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
                    OnRewardAvailable?.Invoke(activeBtn);
                    btn.OnClaimState?.Invoke ();
                    btn.btn.onClick.AddListener (()=> DailyRewardInternal.ClaimTodayReward (()=> {
                        Init ();
                        btn.onClick?.Invoke (); 
                        onActiveBtnClicked?.Invoke();
                    }, dailyRewardBtns.Count));
                }
            }
        }   
        
        IEnumerator CountTimer ()
        {
            string newRewardStr = "";
            while (true)
            {
                yield return new WaitForSeconds (1f);
                
                newRewardStr = DailyRewardInternal.NextRewardTimer(
                    timerTextPrefix,timerTextPostfix, availableText
                );

                _tickListeners.ForEach(listener => {
                    listener(newRewardStr);
                });

                if (timeCounterText)
                    timeCounterText.text = newRewardStr;

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
        
        #endregion

        #region APIS

        /// <Summary>
        /// Returns active btn if reward available
        /// </Summary>
        public DailyRewardBtn AvailableRewardBtn 
        {
            get  {
                return activeBtn;
            }
        }

        public event Action<DailyRewardBtn> OnRewardAvailable;

        /// Text Mesh Pro 
        private  List<Action<string>>  _tickListeners;
 
        public void RemoveAllTickListeners ()
        {
            _tickListeners.Clear();
        }


        public void AddTickListener (Action<string> _action)
        {
            _tickListeners.Add(_action);
        }

        public void RemoveTickListener (Action<string> _action)
        {
            _tickListeners.Remove(_action);
        }

        #endregion
    }
}

