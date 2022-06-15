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
    public class Product
    {
        public static List<Product> products = new List<Product>();

        static readonly SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static Product()
        {
            try
            {
                for (int i = 0; i < SubCategory.subCategories.Count; i++)
                {
                    using (var cmd = new SqlCommand($"SELECT * FROM {SubCategory.subCategories[i].name}", connection))
                    {
                        connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<object> list = new List<object>();

                                for (int j = 5; j < reader.FieldCount; j++)
                                {
                                    list.Add(reader[j]);
                                }

                                products.Add(new Product(int.Parse(reader[0].ToString()), reader[1].ToString(),
                                    reader[2].ToString(), decimal.Parse(reader[3].ToString()), reader[4].ToString(),
                                    SubCategory.subCategories[i], list.ToArray()));
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

        public static void Add(string name, string description, decimal price,
            string subCategory, object[] valueFilters, string urlImage = "")
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand($"INSERT INTO {subCategory}" +
                    " VALUES(@Name, @url)", connection))
                {
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("url", urlImage);

                    cmd.ExecuteNonQuery();
                }

                products.Add(new Product(name, urlImage));
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

        public static void Delete(string name)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("DELETE FROM dbo.Category " +
                    "WHERE NameCategory = @Name", connection))
                {
                    cmd.Parameters.AddWithValue("Name", name);

                    cmd.ExecuteNonQuery();
                }

                products.Remove(products.Find(cat => cat.name == name));
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
        int id;

        [DataMember]
        string name;

        [DataMember]
        string description;

        [DataMember]
        decimal price;

        [DataMember]
        SubCategory subCategory;

        [DataMember]
        string urlImage;

        [DataMember]
        object[] valueFilters;

        public Product(int id, string name, string description, decimal price, string urlImage, SubCategory subCategory, object[] valueFilters)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.urlImage = urlImage;
            this.description = description;
            this.subCategory = subCategory;
            this.valueFilters = valueFilters;
        }
    }
}