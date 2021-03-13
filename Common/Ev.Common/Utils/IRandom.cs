namespace Ev.Common.Utils
{
    public interface IRandom
    {
        int Next(int maxValue);

        int Next(int minValue, int maxValue);

        double NextDouble();
    }
}
