using UnityEngine;
using static UnityEngine.Debug;

#if UNITY_ANDROID
using Unity.Notifications.Android;
// #elif UNITY_IOS
// using Unity.Notifications.IOS;
#endif

namespace Randoms.DailyReward 
{

    public class DailyRewardNotificationManager : MonoBehaviour
    {
        static bool m_isInitialized = false;
        static int m_notification_id = -1;
        
        // Start is called before the first frame update
        void Start()
        {
            if(!DailyRewardConfigSO.Instance.useNotifications) 
            { 
                Destroy(this);
                return;
            }

            DontDestroyOnLoad(gameObject);    
            if (m_isInitialized)
                return;
                
            m_isInitialized = true;
            DailyRewardManager.Instance.OnRewardClaim += OnRewardClaim;
        }

        void OnRewardClaim ()
        {
            Debugger("DailyReward Notification: will be Sent after 24H");
            m_notification_id = SendNotification(((24* 60) * 60)); // send notification after 24 hours 
        }

        void OnApplicationQuit()
        {
            if(!DailyRewardManager.Instance.IsRewardAvailable) return;
            
            if (m_notification_id == -1) 
            {
                Debugger("DailyReward Notification: will be Sent after N time");
                m_notification_id = SendNotification(DailyRewardConfigSO.Instance.pendingRewardNotifyInterval);
                return;
            }

            NotificationStatus status = AndroidNotificationCenter.CheckScheduledNotificationStatus(m_notification_id);
            
            if(status == NotificationStatus.Scheduled)
                AndroidNotificationCenter.CancelNotification(m_notification_id);

            Debugger("DailyReward Notification: will be Sent after N time");
            m_notification_id = SendNotification(DailyRewardConfigSO.Instance.pendingRewardNotifyInterval);
        }
        
        void OnDestroy()
        {
            DailyRewardManager.Instance.OnRewardClaim -= OnRewardClaim;  
        }

        int SendNotification(int minutes)
        {
            var channel = new AndroidNotificationChannel()
            {
                Id = "channel_id",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };

            AndroidNotificationCenter.RegisterNotificationChannel(channel);
            
            var notification = new AndroidNotification();
            notification.Title = DailyRewardConfigSO.Instance.notificationTitle;
            notification.Text = DailyRewardConfigSO.Instance.notificationMessage;
            notification.FireTime = System.DateTime.Now.AddMinutes(minutes);
            return AndroidNotificationCenter.SendNotification(notification, "channel_id");
        }

        void Debugger (string text)
        {
            #if UNITY_EDITOR
            Log(text);
            #endif
        }
    }
}






