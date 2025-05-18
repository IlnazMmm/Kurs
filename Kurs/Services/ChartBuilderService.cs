using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Kurs.Enums;
using Kurs.Models;
using Microcharts;
using SkiaSharp;

namespace Kurs.Services
{
    public static class ChartBuilderService
    {
        public static Chart BuildWorkTypeChart(List<ExtraWork> works, List<WorkType> types)
        {
            var entries = types
                .Select(t =>
                {
                    int count = works.Count(w => w.WorkTypeId == t.Id);
                    if (count == 0) return null;

                    return new ChartEntry(count)
                    {
                        Label = t.Description,
                        ValueLabel = count.ToString(),
                        Color = SKColor.Parse("#2196F3")
                    };
                })
                .Where(e => e != null)
                .ToList();

            return new DonutChart { Entries = entries };
        }

        public static Chart BuildDateChart(List<ExtraWork> works, string chartType)
        {
            var entries = works
                .GroupBy(w => w.StartDate.Date)
                .OrderBy(g => g.Key)
                .Select(g => new ChartEntry(g.Count())
                {
                    Label = g.Key.ToString("dd.MM"),
                    ValueLabel = g.Count().ToString(),
                    Color = SKColor.Parse("#4caf50")
                })
                .ToList();

            return chartType switch
            {
                "Pie" => new PieChart { Entries = entries },
                "Line" => new LineChart { Entries = entries, LineSize = 6 },
                "Donut" => new DonutChart { Entries = entries },
                _ => new BarChart { Entries = entries }
            };
        }
    }
}
