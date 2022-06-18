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
                    using (var cmd = new SqlCommand($"SELECT * FROM [{SubCategory.subCategories[i].name}]", connection))
                    {
                        connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                List<object> list = new List<object>();

                                for (int j = 7; j < reader.FieldCount; j++)
                                {
                                    list.Add(reader[j]);
                                }

                                products.Add(new Product(int.Parse(reader[0].ToString()), int.Parse(reader[1].ToString()),
                                    reader[2].ToString(), reader[3].ToString(), decimal.Parse(reader[4].ToString()), DateTime.Parse(reader[5].ToString()).ToString(), reader[6].ToString(),
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

        public static void Add(int idAcc, string name, string description, decimal price,
            string subCategory, object[] valueFilters, string urlImage = "")
        {
            try
            {
                connection.Open();

                int id;

                using (var cmd = new SqlCommand($"INSERT INTO [{subCategory}]" +
                    $" VALUES(@idAcc, @Name, @Descr, @Cost, @Date, @url, {string.Join(", ", valueFilters)})", connection))
                {
                    cmd.Parameters.AddWithValue("idAcc", idAcc);
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("url", urlImage);
                    cmd.Parameters.AddWithValue("Descr", description);
                    cmd.Parameters.AddWithValue("Cost", price);
                    cmd.Parameters.AddWithValue("Date", DateTime.Now.ToString());


                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"SELECT ID FROM [{subCategory}] WHERE @idAcc = ID_Acc AND @Name = Name AND Created = @Date";

                    var rdr = cmd.ExecuteReader();
                    rdr.Read();
                    id = int.Parse(rdr[0].ToString());
                    rdr.Close();
                    rdr.Dispose();
                }

                products.Add(new Product(id, idAcc, name, description, price, DateTime.Now.ToString(), urlImage, SubCategory.subCategories.Find(sub => sub.name == subCategory), valueFilters));
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

                using (var cmd = new SqlCommand($"DELETE FROM [{subCategory}] " +
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
        public int id;

        [DataMember]
        public int idAcc;

        [DataMember]
        string name;

        [DataMember]
        string description;

        [DataMember]
        decimal price;

        [DataMember]
        SubCategory subCategory;

        [DataMember]
        string created;

        [DataMember]
        string urlImage;

        [DataMember]
        object[] valueFilters;

        public Product(int id, int idAcc, string name, string description, decimal price, string created, string urlImage, SubCategory subCategory, object[] valueFilters)
        {
            this.id = id;
            this.idAcc = idAcc;
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