using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Server
{
    public partial class URL
    {
        public string RetrievePassedUrl()
        {
            return HttpContext.Current.Server.UrlDecode(HttpContext.Current.Request.QueryString["url"]);
        }
    }
}
