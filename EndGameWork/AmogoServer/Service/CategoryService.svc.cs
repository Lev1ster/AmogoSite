using System;
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
    public class CategoryService
    {
        List<Model.Category> list = new List<Model.Category>() { new Model.Category(1, "Kukaracha"), 
            new Model.Category(2, "Mebel"), new Model.Category(3, "Electrical") };

        [WebGet(UriTemplate = "/Category", ResponseFormat = WebMessageFormat.Json)]
        public Model.Category[] GetAllCategory() => list.ToArray();
    }
}
