using System.Collections.Generic;
using System.Linq;

namespace IAddOneSecondForElder
{
    public class TemperatureInfo
    {
        public TemperatureInfo(List<double> temperatureList)
        {
            if (temperatureList.Any())
            {
                Min = temperatureList.Min();
                Max = temperatureList.Max();
                Average = temperatureList.Sum() / temperatureList.Count;
            }

        }
        public double Min { get; }
        public double Max { get; }
        public double Average { get; }
    }
}
