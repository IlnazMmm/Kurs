using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Enums
{
    public enum ReportFilter
    {
        All,           // Все работы (по умолчанию)
        OnlyMine,      // Только мои работы
        ToReview,      // Работы на проверку (созданные мной)
        NotCompleted   // Незавершённые (не выполнены или отклонены)
    }
}
