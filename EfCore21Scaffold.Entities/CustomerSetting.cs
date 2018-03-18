using System;
using System.Collections.Generic;

namespace EfCore21Scaffold.Entities
{
    public partial class CustomerSetting
    {
        public string CustomerId { get; set; }
        public string Setting { get; set; }

        public Customer Customer { get; set; }
    }
}
