using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    [DataContract]
    public class Product
    {
        [DataMember]
        int id;

        [DataMember]
        string name;

        [DataMember]
        decimal price;

        [DataMember]
        SubCategory subCategory;

        public Product(int id, string name, decimal price, SubCategory subCategory)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.subCategory = subCategory;
        }
    }
}