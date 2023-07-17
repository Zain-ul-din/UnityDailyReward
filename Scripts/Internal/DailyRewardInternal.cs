using System;
using UnityEngine;

namespace Randoms.DailyReward.Internals
{
    using DailyReward;
    
    internal static class DailyRewardInternal
    {
        internal static      readonly string      dailyRewardStoreKey = "RANDOMS_DAILY_REWARD_STORE";
        static      DailyRewardStore     store;
        static      bool                 isInitialized;

        internal static event Action OnClaimed; 
        
        static DailyRewardInternal ()
        {
            // cache values
            store = Util.LoadStore (dailyRewardStoreKey);
            if (store == null) 
            {
                isInitialized = true;
                store = DailyRewardStore.GetDefault ();
                return;
            }
            // check if the user does not log in more than 1 day
            if (Mathf.Abs(Util.GetTimeSpanFromNow (store.lastTime).Days) > 1)
            {
                isInitialized = true;
                store = DailyRewardStore.GetDefault ();
                return;
            }
            DailyRewardInternal.UpdateStore ();
        }


        /// <summary>
        /// Return DailyReward Status 
        /// Item1: canClaim , Item2: currentDay 
        /// </summary>
        public static (bool,DailyRewardStatus) GetDailyRewardStatus (int day)
        {
            bool canClaim = store.currDay == day && store.canClaimReward;
            if (day == store.currDay)
            {
                return (canClaim , DailyRewardStatus.UNCLAIMED_AVAILABLE);
            }
            else if (day < store.currDay)
            {
                return (canClaim , DailyRewardStatus.CLAIMED);
            }
            else if (day > store.currDay)
            {
                return (canClaim , DailyRewardStatus.UNCLAIMED_UNAVAILABLE);
            }
            return (false, DailyRewardStatus.UNCLAIMED_AVAILABLE); // intentionally return false to prevent warning
        }


        /// <summary>
        /// Claim Daily Reward
        /// </summary>
        public static void ClaimTodayReward (Action refreshUI, int maxDays = 7)
        {
            UpdateStore ();
            if (!store.canClaimReward) return;
            store.lastTime = DateTime.Now.ToString ();
            store.currDay  = store.currDay + 1;
            // all rewards has been collected
            if (store.currDay > maxDays) {
                store.canRedeemReward = true;
                store.currDay = 1;
            }
            Util.SaveStore (store, dailyRewardStoreKey);
            UpdateStore ();
            refreshUI ();
            isInitialized = false;

            OnClaimed?.Invoke();
        }
        
        /// <summary>
        /// Returns time left for next reward
        /// </summary>
        public static string NextRewardTimer (
            string prefix, string postfix,
            string onAvailableText = "AVAILABLE"
        )
        {
            var timeDiff = NextRewardTime; 
            return 
            (
                Math.Abs(timeDiff.Days) <= 0 && !isInitialized
                ?
                $"{prefix} {23 - Math.Abs(timeDiff.Hours)}:{59 - Math.Abs(timeDiff.Minutes)}:{60 - Math.Abs(timeDiff.Seconds)} {postfix}"
                :
                onAvailableText
            ); 
        }

        /// <Summary>
        /// Returns Next Reward Time Span
        /// </Summary>
        public static TimeSpan NextRewardTime 
        {
            get {
                var timeNow = DateTime.Now;
                var lastTime = DateTime.Parse (store.lastTime);
                TimeSpan timeDiff = lastTime - timeNow; 
                return timeDiff;
            }
        }
        
        /// <summary>
        /// Returns can Claim today reward
        /// </summary>
        public static bool CanClaimTodayReward ()
        {
            UpdateStore ();
            return store.canClaimReward;
        }        
        
        /// <summary>
        /// Updates Store Value WRT Time Span
        /// </summary>
        static void UpdateStore ()
        {
            if (isInitialized) return;
            store.canClaimReward = Mathf.Abs(Util.GetTimeSpanFromNow (store.lastTime).Days) > 0; 
        }
        
        /// <summary>
        /// Returns true if can Redeem Reward
        /// </summary>
        public static bool CanRedeemReward 
        {
            get {
                return store.canRedeemReward;
            }
        }
    }
    
    // Utilities
    internal static class Util
    {
        public static DailyRewardStore LoadStore (string key) => JsonUtility.FromJson <DailyRewardStore>(PlayerPrefs.GetString(key));
        public static void SaveStore (DailyRewardStore store, string key) => PlayerPrefs.SetString (key, JsonUtility.ToJson (store));
        public static TimeSpan GetTimeSpanFromNow (string lastTimeStr)
        {
            var timeNow = DateTime.Now;
            var lastTime = DateTime.Parse (lastTimeStr);
            TimeSpan timeDiff = lastTime - timeNow;
            return timeDiff;
        }
    }

    
    // Custom Store for remembering state
    internal class DailyRewardStore
    {
        public static DailyRewardStore GetDefault ()
        {
            return new DailyRewardStore 
            {
                lastTime = DateTime.Now.ToString (),
                currDay  = 1,
                canClaimReward = true,
                canRedeemReward = false
            };
        }
        
        public string lastTime;
        public int currDay;
        public bool   canClaimReward;
        public bool   canRedeemReward;
    }
}





