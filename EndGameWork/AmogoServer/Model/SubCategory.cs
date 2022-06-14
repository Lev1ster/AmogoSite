using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    [DataContract]
    public class SubCategory
    {
        [DataMember]
        string name;

        [DataMember]
        Type[] filters;

        [DataMember]
        Category mainCategory;

        [DataMember]
        string urlImage;

        public SubCategory(Category mainCategory, string name, Type[] filters, string urlImage)
        {
            this.mainCategory = mainCategory;
            this.name = name;
            this.filters = filters;
            this.urlImage = urlImage;
        }
    }
}