using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Kurs.Enums
{
    public enum WorkStatus
    {
        NotStarted,     // ещё не взято
        InProgress,     // выполняется
        Completed,      // сотрудник отметил как завершено
        PendingReview,  // на проверке у добавившего
        Approved,       // админ/менеджер принял
        Rejected        // отклонено — вернётся исполнителю
    }

}
