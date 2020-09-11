using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BiendeoCHLib.Wrappers;

namespace CHelgaBotv23
{
    public class CHelgaChart
    {

        public static List<CHelgaChart> Charts { get; } = new List<CHelgaChart>();

        public MoonChartWrapper MoonChart;
        public List<Activation> Activations;

        public CHelgaChart(MoonChartWrapper _moonChart, List<Activation> _activations)
        {
            MoonChart = _moonChart;
            Activations = _activations;
        }

        public static void AddChart(MoonChartWrapper _moonChart, List<Activation> _activations)
        {
            Charts.Add(new CHelgaChart(_moonChart, _activations));
        }

        public static void ClearCharts()
        {
            Charts.Clear();
        }

        public override bool Equals(object obj)
        {
            try
            {
                CHelgaChart chartToCompare = (CHelgaChart)obj;

                if (MoonChart.Equals(chartToCompare.MoonChart))
                    return true;
            } catch(InvalidCastException)
            {
                return false;
            }
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

    }
}
