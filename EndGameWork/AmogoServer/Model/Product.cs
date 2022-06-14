﻿using System;
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
        string description;

        [DataMember]
        decimal price;

        [DataMember]
        SubCategory subCategory;

        [DataMember]
        string urlImage;

        public Product(int id, string name, string description, decimal price, SubCategory subCategory, string urlImage)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.subCategory = subCategory;
            this.urlImage = urlImage;
            this.description = description;
        }
    }
}