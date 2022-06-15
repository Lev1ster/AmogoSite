﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;

namespace AmogoWebSite.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ProductService
    {
        [WebGet(UriTemplate = "/Category/SubCategory/Products", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product[] GetAllProducts() => Model.Product.products.ToArray();
    }
}
