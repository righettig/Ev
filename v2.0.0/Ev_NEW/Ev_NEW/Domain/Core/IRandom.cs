namespace Ev_NEW
{
    public interface IRandom
    {
        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        double NextDouble();
    }
}