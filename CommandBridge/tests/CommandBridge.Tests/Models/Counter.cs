namespace CommandBridge.Tests.Models
{
    public class Counter
    {
        public Counter(int initialValue = 0)
        {
            CurrentValue = initialValue;
        }

        public int CurrentValue { get; private set; }

        public int GetNext()
        {
            lock (this) 
            {
                CurrentValue++;
                return CurrentValue;
            }
        }
    }
}