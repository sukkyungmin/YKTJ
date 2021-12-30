using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeRMT
{
    class Product
    {
        /**
         * productCode = 자재
         * prodLine = 생산라인
         * prodVerDate = Cell(1,1)의 Date
         * prodUpDate = 현재시간
         * description = 자재내역
         * prodName = Description 파싱
         * prodGender = Description 파싱
         * prodSize = Description 파싱
         * domestic = Description 파싱
         * country = Description 파싱
         * prodCountPerBag = Description 파싱
         * bagCountPerCase = Description 파싱
         * prodTotal = Description 파싱
         */

         public string productCode { get; set; }
         public string prodLine { get; set; }
         public string prodVerDate { get; set; }
         public string prodUpDate { get; set; }
         public string description { get; set; }
         public string prodName { get; set; }
         public string prodGender { get; set; }
         public string prodSize { get; set; }
         public string domestic { get; set; }
         public string country  { get; set; }
         public string prodCountPerBag { get; set; }
         public string bagCountPerCase { get; set; }
         public string prodTotal { get; set; }
    }
}
