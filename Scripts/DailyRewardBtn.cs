using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randoms.DailyReward
{
    
    [RequireComponent (typeof (IPointerClickHandler))]
    public class DailyRewardBtn : MonoBehaviour 
    {
        [HideInInspector] public Button btn;
        public int day;
        public bool overRideUiState = false;

        [Space (10)][Header ("Ui State")]
        public UnityEvent OnClaimState;
        public UnityEvent OnClaimedState;
        public UnityEvent OnClaimUnAvailableState;
        
        
        [Space (10)][Header ("Reward Events")]
        public UnityEvent onRewardCollect;
        public UnityEvent on2XRewardCollect;
        public UnityEvent onClick;
        
        internal void Init ()
        {
            btn = GetComponent <Button> ();

            if(!overRideUiState)
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
    }
}
