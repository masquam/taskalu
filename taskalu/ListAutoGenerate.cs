using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Taskalu
{
    public class ListAutoGenerate
    {
        public Int64 Id { get; set; }
        public Int64 Order { get; set; }
        public Int64 Type { get; set; }
        public string Name { get; set; }
        public string Priority { get; set; }
        public Int64 Template { get; set; }
        public Int64 Number1 { get; set; }
        public Int64 Number2 { get; set; }
        public Int64 Due_hour { get; set; }
        public Int64 Due_minute { get; set; }

        public enum TypeName : long {
            A_Day_Of_Every_Month,
            A_Weekday_In_Every_Week
        }
    }
}
