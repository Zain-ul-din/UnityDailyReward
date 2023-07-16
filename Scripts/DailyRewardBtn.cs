using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randoms.DailyReward
{
    using Inspector;
    [RequireComponent (typeof (IPointerClickHandler))]
    public class DailyRewardBtn : MonoBehaviour 
    {
        [HideInInspector] public Button btn;
        [SerializeField] private int day;
        internal DailyRewardStatus status;
        internal bool redeemAvailable;

        [Space (10)][Header ("Ui State")]
        public UnityEvent OnClaimState;
        public UnityEvent OnClaimedState;
        public UnityEvent OnClaimUnAvailableState;
        
        
        [Space (10)][Header ("Reward Events")]
        public UnityEvent onRewardCollect;
        public UnityEvent on2XRewardCollect;
        public UnityEvent onClick;

        [Header("Optional! Use Only if redeem is enable")][Space(5)]
        [ShowIf("Editor_OnRedeemEnable")]public UnityEvent onRedeemReward;
        
        internal void Init ()
        {
            btn = GetComponent <Button> ();

            if(DailyRewardConfigSO.Instance.useDefaultStyling)
                AddDefaultUiState ();
        }

        private void AddDefaultUiState ()
        {
            OnClaimUnAvailableState.AddListener (()=> {
                btn.interactable = false;
            });

            OnClaimedState.AddListener(()=> {
                btn.interactable = false;
            });
        }

        public DailyRewardStatus Status 
        {
            get=> status;
        }

        public int Day 
        {
            get=> day;
        }

        public bool RedeemAvailable 
        {
            get=> redeemAvailable;
        }

        private bool Editor_OnRedeemEnable ()
        {
            var res = Resources.Load<DailyRewardConfigSO> ("DailyRewardConfig");
            if (res == null)
                return DailyRewardConfigSO.Instance.useRedeem;
            return res.useRedeem;
        }
    }
}


