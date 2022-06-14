using System;
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
        string name;

        [DataMember]
        string urlImage;

        public Category(string name, string urlImage)
        {
            this.name = name;
            this.urlImage = urlImage;
        }
    }
}