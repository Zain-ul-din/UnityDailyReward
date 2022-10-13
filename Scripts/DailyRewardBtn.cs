using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Randoms.DailyReward
{
    using Editor;
    
    [RequireComponent (typeof (IPointerClickHandler))]
    public class DailyRewardBtn : MonoBehaviour
    {
        public static List <DailyRewardBtn> dailyRewardBtns;
        
        [HideInInspector] public Button btn;
        public int day;
        
        [Space (10)][Header ("Ui State")]
        public UnityEvent OnClaimState;
        public UnityEvent OnClaimedState;
        public UnityEvent OnClaimUnAvailableState;
        
        
        [Space (10)][Header ("Reward Events")]
        public UnityEvent onRewardCollect;
        public UnityEvent on2XRewardCollect;
        
        
        
        private void Awake ()
        {
            btn = GetComponent <Button> ();
            if (dailyRewardBtns == null)
            {
                dailyRewardBtns = new List<DailyRewardBtn> ();
            }
            dailyRewardBtns.Add (this);
        }
    }
}



