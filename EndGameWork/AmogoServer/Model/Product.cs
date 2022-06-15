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

                                for (int j = 6; j < reader.FieldCount; j++)
                                {
                                    list.Add(reader[j]);
                                }

                                products.Add(new Product(int.Parse(reader[0].ToString()), reader[1].ToString(),
                                    reader[2].ToString(), decimal.Parse(reader[3].ToString()), DateTime.Parse(reader[4].ToString()), reader[5].ToString(),
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

        public static Product[] GetProducts(string subCategory)
        {
            List<Product> list = new List<Product>();

            var sub = SubCategory.subCategories.Find(subs => subs.name == subCategory);

            foreach (var item in products)
            {
                if (item.subCategory == sub)
                    list.Add(item);
            }

            return list.ToArray();
        }

        public static void Add(string name, string description, decimal price,
            string subCategory, object[] valueFilters, string urlImage = "")
        {
            try
            {
                connection.Open();

                int id;

                using (var cmd = new SqlCommand($"INSERT INTO {subCategory}" +
                    $" VALUES(@Name, @Descr, @Cost, @Date, @url, {string.Join(", ", valueFilters as string[])})", connection))
                {
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("url", urlImage);

                    cmd.ExecuteNonQueryAsync();

                    cmd.CommandText = $"SELECT ID FROM {subCategory} WHERE @Name = Name AND Created = @Date";

                    var rdr = cmd.ExecuteReader();
                    rdr.Read();
                    id = int.Parse(rdr[0].ToString());
                    rdr.Close();
                    rdr.Dispose();
                }

                products.Add(new Product(id, name, description, price, DateTime.Now, urlImage, SubCategory.subCategories.Find(sub => sub.name == subCategory), valueFilters));
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

        public static void Delete(string subCategory, int ID)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand($"DELETE FROM {subCategory} " +
                    "WHERE  ID = @ID", connection))
                {
                    cmd.Parameters.AddWithValue("ID", ID);

                    cmd.ExecuteNonQuery();
                }

                products.Remove(products.Find(cat => cat.id == ID));
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
        DateTime created;

        [DataMember]
        string urlImage;

        [DataMember]
        object[] valueFilters;

        public Product(int id, string name, string description, decimal price, DateTime created, string urlImage, SubCategory subCategory, object[] valueFilters)
        {
            this.id = id;
            this.name = name;
            this.price = price;
            this.created = created;
            this.urlImage = urlImage;
            this.description = description;
            this.subCategory = subCategory;
            this.valueFilters = valueFilters;
        }
    }
}