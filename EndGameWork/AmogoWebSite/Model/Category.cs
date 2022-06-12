using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel.Web;
using System.Web;

namespace AmogoWebSite.Model
{
    public class Category
    {
        [WebGet(UriTemplate = "/Category")]

        public string GetAllCategory() => "Category";
    }
}