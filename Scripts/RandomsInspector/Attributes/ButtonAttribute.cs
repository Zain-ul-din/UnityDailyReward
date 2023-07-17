namespace Randoms.Inspector 
{
    public class ButtonAttribute: IRandomsAttribute 
    {
        public string buttonName;
        public string doc;

        public ButtonAttribute (string buttonName, string doc = "") 
        {
            this.buttonName = buttonName;
            this.doc = doc;
        }
    }
}



