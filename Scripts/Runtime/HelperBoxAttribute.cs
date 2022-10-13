using System;

namespace Randoms.DailyReward.Editor
{
    [AttributeUsage (AttributeTargets.Class)]
    public class HelperBoxAttribute: Attribute
    {
        public HelperBoxAttribute (string helperText)
        {
            this.helperText = helperText;
        }
        
        public string helperText;
    }
}

