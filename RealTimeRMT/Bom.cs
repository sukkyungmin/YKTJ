using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeRMT
{
    class Bom
    {
        /**
         * compCode = TempBOM Sheet의 Component
         * productCode = Material
         * itemNum = Item
         * bomLevel = BOM Level
         * itemCategory = ICt
         * altBOM = AltBOM
         * quantity = Quantity
         * resultQuantity = Accumulated Scrap
         * unit = UOM
         * description = Component Description
         * unitQuantity
         * priceOfRoll
         * setDiaTar
         * spliceDiaTar
         * vendor
         */ 

         public string compCode { get; set; }
         public string productCode { get; set; }
         public string itemNum { get; set; }
         public string bomLevel { get; set; }
         public string itemCategory { get; set; }
         public string altBom  { get; set; }
         public string quantity { get; set; }
         public string resultQuantity { get; set; }
         public string unit { get; set; }
         public string description { get; set; }
         public string unitQuantity { get; set; }
         public string priceOfRoll { get; set; }
         public string setDiaTar { get; set; }
         public string spliceDiaTar { get; set; }
         public string vendor { get; set; }
    }
}
