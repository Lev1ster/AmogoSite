using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AmogoWebSite.Model
{
    //fetch('http://localhost:65388/Service/CategoryService.svc/Category', { mode: 'no-cors' });

    [DataContract]
    public class Category
    {
        public static List<Category> categories = new List<Category>();

        static readonly SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static Category()
        {
            try
            {
                using (var cmd = new SqlCommand("SELECT * FROM dbo.Category", connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            categories.Add(new Category(reader[0].ToString(), reader[1].ToString()));
                        }
                    }
                }
            }
            catch (SqlException e)
            {
                throw new Exception("Ошибка подключения к базе данных", e.InnerException);
            }
            finally
            {
                connection.Close();
            }
        }

        public static void Add(string name, string urlImage = "")
        {
            try
            {
                connection.Open();

                using(var cmd = new SqlCommand("INSERT INTO dbo.Category VALUES(@Name, @url)", connection))
                {
                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("url", urlImage);

                    cmd.ExecuteNonQuery();
                }

                categories.Add(new Category(name, urlImage));
            }
            catch(SqlException)
            {
                throw;
            }
            finally
            {
                connection.Close();
            }
        }

        [DataMember]
        string name;

        [DataMember]
        string urlImage;

        public Category(string name, string urlImage)
        {
            this.name = name;
            this.urlImage = urlImage;
        }
    }
}