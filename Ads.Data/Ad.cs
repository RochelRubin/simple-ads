using System;
using System.Collections.Generic;
using System.Text;

namespace Ads.Data
{
    public class Ad
    {
        public int Id { get; set; }
        public DateTime DateSubmitted { get; set; }
        public string PhoneNumber { get; set; }
        public string Text { get; set; }
        public string Name { get; set; }
        public int UserId { get; set; }
    }
}
