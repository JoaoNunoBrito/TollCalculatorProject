namespace TollFeeCalculator.Entities
{
    public class TimePriceRange
    {
        public TimeSpan Start { get; }
        public TimeSpan End { get; }
        public int Price { get; }

        public TimePriceRange(TimeSpan start, TimeSpan end, int price)
        {
            Start = start;
            End = end;
            Price = price;
        }
    }
}
