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
    using UnityEditor;

    public class DailyRewardManager: MonoBehaviour
    {
        
        #region Options

        [Header ("Timer Text Options")]
        public Text timeCounterText;
        
        [Space(20)]
        [SerializeField] private UnityEvent onActiveBtnClicked;
        
        [Space(5)] [Header("Called Only if redeem is enabled")]
        [ShowIf("Editor_OnRedeemEnable")][SerializeField] private UnityEvent onRedeemActiveBtnClicked;

        [Space(5)]
        [Header("Custom Claim Button")]
        [ShowIf("Editor_CanuseCustomBtn")]
        [SerializeField] private Button customClaimBtn;

        #endregion

        #region Docs

        #if UNITY_EDITOR

        [Button ("Open DailyReward Config")]
        private void OpenConfig ()
        {
            EditorUtil.FocusOrCreateAsset("DailyRewardConfig t:DailyRewardConfigSO");
        }

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

        private bool Editor_OnRedeemEnable ()
        {
            var res = Resources.Load<DailyRewardConfigSO> ("DailyRewardConfig");
            if (res == null)
                return DailyRewardConfigSO.Instance.useRedeem;
            return res.useRedeem;
        }

        private bool Editor_CanuseCustomBtn ()
        {
            var res = Resources.Load<DailyRewardConfigSO>("DailyRewardConfig");
            if (res == null)
                return DailyRewardConfigSO.Instance.useCustomClaimButton;
            return res.useCustomClaimButton;
        }

        #endif

        #endregion

        #region Logic

        public static DailyRewardManager Instance { get; private set; }
        private bool isInitialized = false;
        private bool canRefreshUI  = true;
        private DailyRewardBtn activeBtn;
        private static List<DailyRewardBtn> dailyRewardBtns;
        private Action<DailyRewardBtn> _applyUiStyling;

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

        public void RedeemReward ()
        {
            activeBtn.onRedeemReward?.Invoke ();
        }
        
        /// <summary>
        /// Invokes Action On Btns
        /// </summary>
        void Init ()
        {
            foreach (var btn in dailyRewardBtns)
            {
                btn.Init ();
                var (canClaim, status) = DailyRewardInternal.GetDailyRewardStatus (btn.Day);
                btn.status = status;
                btn.redeemAvailable = DailyRewardInternal.CanRedeemReward;
                
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

                    var currBtn = DailyRewardConfigSO.Instance.useCustomClaimButton ?
                        customClaimBtn : btn.btn;

                    currBtn.onClick.RemoveAllListeners();
                    currBtn.onClick.AddListener(() => ClaimRewardOnBtnClick(btn));
                }

                _applyUiStyling?.Invoke(btn);
            }
        }   

        private void ClaimRewardOnBtnClick (DailyRewardBtn btn)
        {
            DailyRewardInternal.ClaimTodayReward(() =>
            {
                Init();
                btn.onClick?.Invoke();
                if (DailyRewardConfigSO.Instance.useRedeem && DailyRewardInternal.CanRedeemReward)
                    onRedeemActiveBtnClicked?.Invoke();
                else
                    onActiveBtnClicked?.Invoke();
            }, dailyRewardBtns.Count);
        }
        
        IEnumerator CountTimer ()
        {
            string newRewardStr = "";
            while (true)
            {
                yield return new WaitForSeconds (1f);
                
                newRewardStr = DailyRewardInternal.NextRewardTimer(
                    DailyRewardConfigSO.Instance.timerTextPrefix,
                    DailyRewardConfigSO.Instance.timerTextPostfix, 
                    DailyRewardConfigSO.Instance.textOnAvailable
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

        public bool IsRewardAvailable 
        {
            get  {
                return activeBtn != null;
            }
        }

        public event Action<DailyRewardBtn> OnRewardAvailable;

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

        public void ApplyUiStyling(Action<DailyRewardBtn> _action)
        {
            if (DailyRewardConfigSO.Instance.useDefaultStyling)
                throw new Exception(@"
                    You can't use ApplyUiStyling. 
                    since you are using default styling in config.
                    Please disable it first"
                );
            _applyUiStyling = _action;
            Init();
        }
        
        public TimeSpan NextRewardTime 
        {
            get => DailyRewardInternal.NextRewardTime;
        }
        
        #endregion

        #region Editor

        #if UNITY_EDITOR
        [MenuItem("Tools/DailyReward/DailyRewardManager")]
        private static void CreateInstance ()
        {
            var instance = GameObject.FindObjectOfType<DailyRewardManager> ();
            if(!instance) {
                var go = new GameObject("DailyRewardManager");
                instance = go.AddComponent<DailyRewardManager>(); 
            }

            Selection.activeObject = instance;
        }
        
        void OnDrawGizmos()
        {
            var instances = GameObject.FindObjectsOfType<DailyRewardManager> ();
            if (instances.Length <= 1) return;
            foreach (var instance in instances)
                if (instance != this)
                    Selection.activeObject = instance;
            DestroyImmediate (this);
            Debug.LogError(@"'DailyRewardManager' is a singleton. Please use the existing one");
        }
        #endif

        #endregion
    }
}



