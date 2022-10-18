using System;

namespace Randoms.DailyReward.Editor
{
    [AttributeUsage (AttributeTargets.Method)]
    public class BtnAttribute: Attribute
    {
        public BtnAttribute (string btnText)
        {
            this.btnText = btnText;
        }

        public string btnText;
    } 
}

