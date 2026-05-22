using System;
using System.Collections.Generic;
using System.Text;

namespace Custom_Builds.Core.DTO
{
    public class HistoryOrderDTO : MiniOrderInfoDTO
    {
        public required decimal TotalPrice { get; set; }
        public required int Quantity { get; set; }
        public required List<string> specs { get; set; }
    }
}
