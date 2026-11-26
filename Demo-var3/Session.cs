using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Demo_var3
{
    public static class Session
    {
        public static string FullName;
        public static int Role; // 1 админ, 2 менеджер, 3 клиент, 4 гость
        public static bool isStaff => Role <= 2;
        public static bool isAdmin => Role == 1;
    }
}
