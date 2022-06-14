using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;

namespace AmogoWebSite.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class CategoryService
    {
        [WebGet(UriTemplate = "/Category", ResponseFormat = WebMessageFormat.Json)]
        public Model.Category[] GetAllCategory() => Model.Category.categories.ToArray();

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void Add(string name, string urlImage = "") => Model.Category.Add(name, urlImage);
    }
}
