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
        
        [HideInInspector] public Button btn;
        public int day;
        public UnityEvent OnClaim;
        public UnityEvent OnClaimed;
        public UnityEvent OnClaimUnAvailable;

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


