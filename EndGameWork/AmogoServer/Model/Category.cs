﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    //fetch('http://localhost:65388/Service/CategoryService.svc/Category', { mode: 'no-cors' });

    [DataContract]
    public class Category
    {
        [DataMember]
        int id;

        [DataMember]
        string name;

        public Category(int id, string name)
        {
            this.id = id;
            this.name = name;
        }
    }
}