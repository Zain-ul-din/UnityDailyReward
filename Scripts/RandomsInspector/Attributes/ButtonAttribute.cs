using System;
using UnityEngine;

namespace Randoms.Inspector 
{
    public class ButtonAttribute: IRandomsAttribute 
    {
        public string buttonName;

        public ButtonAttribute (string buttonName) 
        {
            this.buttonName = buttonName;
        }
    }
}


