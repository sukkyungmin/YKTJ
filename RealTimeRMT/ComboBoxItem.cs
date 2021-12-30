using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RealTimeRMT
{
    class ComboBoxItem
    {
        public string Text { get; set; }
        public object Value { get; set; }

        public override string ToString()
        {
            return Text;
        }

        public ComboBoxItem(string TextParam, object ValueParam)
        {
            Text = TextParam;
            Value = ValueParam;
        }
    }
}
