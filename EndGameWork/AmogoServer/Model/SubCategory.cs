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
        int id;

        [DataMember]
        string name;

        [DataMember]
        Type[] filters;

        [DataMember]
        Category mainCategory;

        public SubCategory(int id, Category mainCategory, string name, Type[] filters)
        {
            this.id = id;
            this.mainCategory = mainCategory;
            this.name = name;
            this.filters = filters;
        }
    }
}