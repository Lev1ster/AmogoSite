using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    [DataContract]
    public class Accounts
    {
        [DataMember]
        int id;

        [DataMember]
        string login;

        [DataMember]
        string password;

        [DataMember]
        string name;

        [DataMember]
        string lastName;

        [DataMember]
        string telephone;

        [DataMember]
        string urlAvatar;

        public Accounts(int id, string login, string password, string name, string lastName, string telephone, string urlAvatar)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.name = name;
            this.lastName = lastName;
            this.telephone = telephone;
            this.urlAvatar = urlAvatar;
        }
    }
}