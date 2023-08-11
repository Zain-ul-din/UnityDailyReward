using UnityEngine;

namespace Randoms.DailyReward 
{
    [CreateAssetMenu(fileName = "DailyRewardConfig", menuName = "DailyRewardConfig")]
    public class DailyRewardConfigSO : ScriptableObject
    {
        [Space(25)] [Header ("Timer Text Format")] 
        public string textOnAvailable = "AVAILABLE";
        public string timerTextPrefix = "Next Reward In";
        public string timerTextPostfix = "TIME";

        [Space(25)]
        [Header("Custom Claim Button")]
        public bool useCustomClaimButton = false;

        [Space(25)] [Header("Buttons Styling Options")]
        public bool useDefaultStyling = true; 

        [Space(25)][Header("Redeem Options")]
        
        [Tooltip("allow user to redeem already claimed reward")]
        public bool useRedeem = false;
        

        
        #region  Singleton

        private static DailyRewardConfigSO _instance;

        public static DailyRewardConfigSO Instance
        {
            get
            {
                if (!_instance)
                {
                    _instance = Resources.Load<DailyRewardConfigSO>("DailyRewardConfig");
                    if (!_instance)
                        _instance = ScriptableObject.CreateInstance<DailyRewardConfigSO>();
                }

                return _instance;
            }
        }

        #endregion
    }
}




