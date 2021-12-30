using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeRMT
{
    class Schedule
    {
        /**
         * productCode = 제품코드의 앞 8자리
         * dateTime = 데이터가 있는 날짜의 yyyy-MM-dd 포맷 데이터
         * prodLine = 생산라인의 앞 6자리
         * amount = 데이터
         * amtUnit = 제품코드의 10자리부터 끝까지
         */
        public string productCode { get; set; }
        public string dateTime { get; set; }
        public string prodLine { get; set; }
        public string amount { get; set; }
        public string amtUnit { get; set; }
    }
}
