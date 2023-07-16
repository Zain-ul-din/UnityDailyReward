namespace Randoms.Inspector
{
    public class ShowIfAttribute : IRandomsAttribute 
    {
        public string Condition { get; private set; }

        public ShowIfAttribute(string condition)
        {
            Condition = condition;
        }
    } 
}
