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
    public class AccountService
    {
        [WebGet(UriTemplate = "/Accounts", ResponseFormat = WebMessageFormat.Json)]
        public Model.Accounts[] GetAllAccount() => Model.Accounts.accounts.ToArray();

        [WebGet(UriTemplate = "/Accounts/{idAcc}", ResponseFormat = WebMessageFormat.Json)]
        public Model.Accounts GetCategory(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.Accounts.accounts.Find(acc => acc.id == pid);
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddAccounts(string login, string password, string name, string lastName,
            string telephone, string isAdmin, string urlAvatar = "")
        {
            if (!byte.TryParse(isAdmin, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Accounts.Add(login, password, name, lastName, telephone, pid, urlAvatar);
        }

        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts/{idAcc}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteAccounts(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Accounts.Delete(pid);
        }


        [WebGet(UriTemplate = "/Accounts/{idAcc}/Products", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product[] GetAllProduct(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.Accounts.GetProduct(pid);
        }

        [WebGet(UriTemplate = "/Accounts/{idAcc}/FavoriteProuct", ResponseFormat = WebMessageFormat.Json)]
        public Model.Product[] GetAccounts(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.Accounts.GetFavoriteProducts(pid);
        }

        [WebGet(UriTemplate = "Accounts/Messages", ResponseFormat = WebMessageFormat.Json)]
        public Model.Message[] GetAllMessages() => Model.Message.messages.ToArray();

        [WebGet(UriTemplate = "Accounts/SupMessages", ResponseFormat = WebMessageFormat.Json)]
        public Model.SupMessage[] GetAllSupMessages() => Model.SupMessage.supMessages.ToArray();

        [WebGet(UriTemplate = "/Accounts/{idAcc}/Messages", ResponseFormat = WebMessageFormat.Json)]
        public Model.Message[] GetMessages(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.Message.GetMessages(pid);
        }

        [WebGet(UriTemplate = "/Accounts/{idAcc}/SupMessages", ResponseFormat = WebMessageFormat.Json)]
        public Model.SupMessage[] GetSupMessages(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return Model.SupMessage.GetSupMessages(pid);
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts/{idOwner}/Messages/{idSecond}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddMessage(string idOwner, string idSecond, string text)
        {

            if (!int.TryParse(idOwner, out var idOwn))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (!int.TryParse(idSecond, out var idSec))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Message.Add(idOwn, idSec, text);
        }

        [WebInvoke(Method = "POST", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts/{idAcc}/SupMessage", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void AddSupMessage(string idAcc, string text)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.SupMessage.Add(pid, text);
        }


        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts/{idOwner}/Message/{idSecond}", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteMessage(string idOwner, string idSecond)
        {
            if (!int.TryParse(idOwner, out var idOwn))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            if (!int.TryParse(idSecond, out var idSec))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.Message.Delete(idOwn, idSec);
        }


        [WebInvoke(Method = "DELETE", RequestFormat = WebMessageFormat.Json,
            UriTemplate = "/Accounts/{idAcc}/SupMessage", ResponseFormat = WebMessageFormat.Json,
            BodyStyle = WebMessageBodyStyle.Wrapped)]
        public void DeleteSupMessage(string idAcc)
        {
            if (!int.TryParse(idAcc, out var pid))
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            Model.SupMessage.Delete(pid);
        }
    }
}