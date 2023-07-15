using  UnityEngine;
using UnityEngine.UI;
using Randoms.DailyReward;

public class DailyRewardDemo : MonoBehaviour
{
    [SerializeField] private Button menuBtn;


    public void Start ()
    {
        // check if reward available
        if (!DailyRewardManager.Instance.IsRewardAvailable)
        {
            menuBtn.onClick.RemoveAllListeners();
            menuBtn.interactable = false;
        }
        else 
        {
            DailyRewardManager.Instance.AvailableRewardBtn.onClick.AddListener(() => {
                menuBtn.onClick.RemoveAllListeners();
                menuBtn.interactable = false;
            });
        }
    
        
        DailyRewardManager.Instance.OnRewardAvailable += (DailyRewardBtn btn)=>{
            // add listener here also update reward text here
        };
    }  
    
    /// Update Text Mesh Pro
    public void UpdateTextMeshProText ()
    {
        DailyRewardManager.Instance.AddTickListener((time_str)=> {
            // update text mesh pro here
            Debug.Log(time_str);
        });
    }
}

