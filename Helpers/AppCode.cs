using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Nigeria_Reg.Models;

namespace Nigeria_Reg.Helpers
{
    public class AppCode
    {
        public int GetSitesCount(List<tblSite> lst)
        {
            return lst.Count();
        }

        public int GetSurveysCount(List<tblSurvey> lst)
        {
            return lst.Count();
        }
    }
}