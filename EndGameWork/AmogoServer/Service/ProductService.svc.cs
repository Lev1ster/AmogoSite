using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.Text;
using System.Web.Http;

namespace AmogoWebSite.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ProductService
    {
        [WebGet(UriTemplate = "/Category/SubCategory/Product", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product[] GetAllProducts() => Model.Product.products.ToArray();

        [WebGet(UriTemplate = "/Category/SubCategory/{subCategory}/Products", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product[] GetProducts(string subCategory) => Model.Product.GetProducts(subCategory);

        [WebGet(UriTemplate = "/Category/SubCategory/{subCategory}/{ID}", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product GetProduct(string subCategory, string ID)
        {
            if (!int.TryParse(ID, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.Product.GetProducts(subCategory).First(prod=>prod.id == pid);
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category/SubCategory/{subCategory}/{idAcc}/Product", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddProduct(string idAcc, string name, string description, decimal price,
            string subCategory, object[] valueFilters, string urlImage = "")
        {
            if (!int.TryParse(idAcc, out var IdAcc))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Product.Add(IdAcc, name, description, price, subCategory, valueFilters, urlImage);
        }

        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Category/SubCategory/{subCategory}/Product/{ID}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteProduct(string subCategory, string ID)
        {
            if (!int.TryParse(ID, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Product.Delete(subCategory, pid);
        }
    }
}
