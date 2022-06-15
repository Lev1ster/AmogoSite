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
        public void AddCategory(string name, string urlImage = "") => Model.Category.Add(name, urlImage);

        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category/{CategoryName}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteCategory(string CategoryName) => Model.Category.Delete(CategoryName);



        [WebGet(UriTemplate = "/Category/SubCategory", ResponseFormat = WebMessageFormat.Json)]
        public Model.SubCategory[] GetAllSubCategories() => Model.SubCategory.subCategories.ToArray();

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category/SubCategory", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddSubCategory(string mainCategory, string name, string[] filterName, string[] filterType, string urlImage = "")
            => Model.SubCategory.Add(mainCategory, name, filterName, filterType, urlImage);

        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category/SubCategory/{SubCategoryName}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteSubCategory(string SubCategoryName) => Model.SubCategory.Delete(SubCategoryName);
    }
}
