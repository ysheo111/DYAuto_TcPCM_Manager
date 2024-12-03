using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.DataVisualization.Charting;

namespace TcPCM_Connect_Global
{
    public class CustomColor
    {
        public static void GetChartColor(Chart chart)
        {
            List<System.Drawing.Color> color = new List<System.Drawing.Color>();
            color.Add(System.Drawing.ColorTranslator.FromHtml("#55efc4"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#74b9ff"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#81ecec"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#fd79a8"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#a29bfe"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#fab1a0"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#ff7675"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#FC60F"));

            color.Add(System.Drawing.ColorTranslator.FromHtml("#00b894"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#0984e3"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#00cec9"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#e84393"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#6c5ce7"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#e17055"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#d63031"));
            color.Add(System.Drawing.ColorTranslator.FromHtml("#fdcb6e"));

            chart.Palette = ChartColorPalette.None;
            chart.PaletteCustomColors = color.ToArray();
        }
    }
}
