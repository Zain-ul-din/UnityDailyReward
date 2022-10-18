using System;
using UnityEngine;

namespace Randoms.DailyReward.Internals
{
    
    internal static class DailyRewardInternal
    {
        static      readonly string      dailyRewardStoreKey = "RANDOMS_DAILYREWARD_STORE";
        static      DailyRewardStore     store;
        static      bool                 isInitialized;
        
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
            
            if (store.currDay > maxDays)
            {
                store.currDay = 1;
            }

            Util.SaveStore (store, dailyRewardStoreKey);
            UpdateStore ();
            refreshUI ();
            isInitialized = false;
        }

        
        /// <summary>
        /// Returns time left for next reward
        /// </summary>
        public static string NextRewardTimer ()
        {
            var timeNow = DateTime.Now;
            var lastTime = DateTime.Parse (store.lastTime);
            TimeSpan timeDiff = lastTime - timeNow; 
            
            return 
            (
                Math.Abs(timeDiff.Days) <= 0 && !isInitialized
                ?
                $"NEXT REWARD IN {23 - Math.Abs(timeDiff.Hours)}H {59 - Math.Abs(timeDiff.Minutes)}M {60 - Math.Abs(timeDiff.Seconds)}SEC"
                :
                "AVAILABLE"
            ); 
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

            var timeNow = DateTime.Now;
            var lastTime = DateTime.Parse (store.lastTime);
            TimeSpan timeDiff = lastTime - timeNow;
            store.canClaimReward = Math.Abs (timeDiff.Days) > 0; 
        }
        
    }
    

    // Utilities
    internal static class Util
    {
        public static DailyRewardStore LoadStore (string key) => JsonUtility.FromJson <DailyRewardStore>(PlayerPrefs.GetString(key));
        public static void SaveStore (DailyRewardStore store, string key) => PlayerPrefs.SetString (key, JsonUtility.ToJson (store));
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
            };
        }
        
        public string lastTime;
        public int currDay;
        public bool   canClaimReward;
    }

    public enum DailyRewardStatus 
    {
        UNCLAIMED_AVAILABLE,
        UNCLAIMED_UNAVAILABLE,
        CLAIMED
    }
}




