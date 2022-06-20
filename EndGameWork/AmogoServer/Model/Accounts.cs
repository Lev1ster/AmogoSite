using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    [DataContract]
    public class Accounts
    {
        public static List<Accounts> accounts = new List<Accounts>();

        public static readonly SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static Accounts()
        {
            try
            {
                for (int i = 0; i < SubCategory.subCategories.Count; i++)
                {
                    using (var cmd = new SqlCommand("SELECT ID, Login, " +
                        "Password, Name, LastName, Telephone, avatar, isAdmin" +
                        " FROM Accounts", connection))
                    {
                        connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                accounts.Add(new Accounts(int.Parse(reader[0].ToString()), reader[1].ToString(), reader[2].ToString(), reader[3].ToString(),
                                    reader[4].ToString(), reader[5].ToString(), reader[6].ToString(), Equals(reader[7].ToString(), "1")));
                            }
                        }
                    }
                }
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static Product[] GetProduct(int ID)
        {
            List<Product> products = new List<Product>();

            foreach (var item in Product.products)
            {
                if (item.idAcc == ID)
                    products.Add(item);
            }

            return products.ToArray();
        }

        public static Product[] GetFavoriteProducts(int ID)
        {
            List<Product> products = new List<Product>();

            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("SELECT SubCat, ID_Product FROM Accounts_Favorit WHERE " +
                    "@ID = ID_Acc", connection))
                {
                    cmd.Parameters.AddWithValue("ID", ID);

                    using(var reader = cmd.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            products.Add(Product.GetProducts(reader[0].ToString()).First(prod => prod.id == int.Parse(reader[1].ToString())));
                        }
                    }
                }

                return products.ToArray();
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void Add(string login, string password, string name, string lastName,
            string telephone, byte isAdmin, string urlAvatar = "")
        {
            try
            {
                connection.Open();

                int id;
                bool admin = false;

                using (var cmd = new SqlCommand("INSERT INTO Accounts" +
                    $" VALUES(@Login, @Password, @Name, @LastName, @Telephone, @url, @isAdmin)", connection))
                {

                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("Login", login);
                    cmd.Parameters.AddWithValue("Password", password);
                    cmd.Parameters.AddWithValue("LastName", lastName);
                    cmd.Parameters.AddWithValue("Telephone", telephone);
                    if (isAdmin == 1)
                    {
                        admin = true;
                    }
                    cmd.Parameters.AddWithValue("isAdmin", admin);
                    cmd.Parameters.AddWithValue("url", urlAvatar);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"SELECT ID FROM Accounts WHERE @Login = [Login]";

                    var rdr = cmd.ExecuteReader();
                    rdr.Read();
                    id = int.Parse(rdr[0].ToString());
                    rdr.Close();
                    rdr.Dispose();
                }

                accounts.Add(new Accounts(id, login, password, name, lastName, telephone, urlAvatar, admin));
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        public static void Delete(int ID)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand($"DELETE FROM Accounts " +
                    "WHERE  ID = @ID", connection))
                {
                    cmd.Parameters.AddWithValue("ID", ID);

                    cmd.ExecuteNonQuery();
                }

                accounts.Remove(accounts.Find(acc => acc.id == ID));
            }
            catch (SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        [DataMember]
        public int id;

        [DataMember]
        public string login;

        [DataMember]
        public string password;

        [DataMember]
        string name;

        [DataMember]
        string lastName;

        [DataMember]
        string telephone;

        [DataMember]
        string urlAvatar;

        [DataMember]
        bool isAdmin;

        public Accounts(int id, string login, string password, string name, string lastName,
            string telephone, string urlAvatar, bool isAdmin)
        {
            this.id = id;
            this.login = login;
            this.password = password;
            this.name = name;
            this.lastName = lastName;
            this.telephone = telephone;
            this.urlAvatar = urlAvatar;
            this.isAdmin = isAdmin;
        }
    }
}