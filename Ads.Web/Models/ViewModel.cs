using Ads.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ads.Web.Models
{
    public class ViewModel
    {
        public bool IsAuthenticated { get; set; }
        public User CurrentUser { get; set; }
        public List<Ad> Ads { get; set; }
    }
}
