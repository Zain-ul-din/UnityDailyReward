using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;


namespace Randoms.DailyReward
{
    
    [RequireComponent (typeof (IPointerClickHandler))]
    public class DailyRewardBtn : MonoBehaviour
    {
        public static List <DailyRewardBtn> dailyRewardBtns;
        
        
        public DailyRewardBtn ()
        {
            if (dailyRewardBtns == null)
            {
                dailyRewardBtns = new List<DailyRewardBtn> ();
            }

            if (dailyRewardBtns.Count > 0 && dailyRewardBtns[0] ==  null)  dailyRewardBtns = new List<DailyRewardBtn> ();   
        
            dailyRewardBtns.Add (this);
        }
 
        [HideInInspector] public Button btn;
        public int day;
        
        [Space (10)][Header ("Ui State")]
        public UnityEvent OnClaimState;
        public UnityEvent OnClaimedState;
        public UnityEvent OnClaimUnAvailableState;
        
        
        [Space (10)][Header ("Reward Events")]
        public UnityEvent onRewardCollect;
        public UnityEvent on2XRewardCollect;
        public UnityEvent onClick;
        
        internal bool isInitialized = false;

        private void Awake ()
        {
            Init ();
        }
        
        internal void Init ()
        {
            if (isInitialized) return;
            isInitialized = true;
            btn = GetComponent <Button> ();
        }
    }
}



