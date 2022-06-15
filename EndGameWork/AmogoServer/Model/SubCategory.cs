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
    public class SubCategory
    {
        public static List<SubCategory> subCategories = new List<SubCategory>();

        static readonly SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static SubCategory()
        {
            try
            {
                using (var cmd = new SqlCommand("SELECT NameSubCategory, image, Category FROM dbo.SubCategory", connection))
                {
                    connection.Open();

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            subCategories.Add(new SubCategory(Category.categories.Find(cat => cat.name == reader[2].ToString()),
                                    reader[0].ToString(), null, null, reader[1].ToString()));
                        }
                    }

                    for (int i = 0; i < subCategories.Count; i++)
                    {
                        cmd.CommandText = "SELECT DATA_TYPE, COLUMN_NAME FROM INFORMATION_SCHEMA.COLUMNS " +
                            $"WHERE table_name = '{subCategories[i].name}'";

                        List<string> types = new List<string>();
                        List<string> names = new List<string>();

                        using (var rdr = cmd.ExecuteReader())
                        {
                            int j = 0;
                            while (rdr.Read())
                            {
                                if (j >= 5)
                                {
                                    types.Add(rdr[0].ToString());
                                    names.Add(rdr[1].ToString());
                                }

                                j++;
                            }
                        }

                        subCategories[i].filtersType = types.ToArray();
                        subCategories[i].filtersName = names.ToArray();
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

        public static SubCategory[] GetSubCategories(Category category)
        {
            List<SubCategory> subs = new List<SubCategory>();

            for (int i = 0; i < subCategories.Count; i++)
            {
                if (subCategories[i].mainCategory == category)
                    subs.Add(subCategories[i]);
            }

            return subs.ToArray();
        }
        public static void Add(string mainCategory, string name, string[] filterName, string[] filterType, string urlImage = "")
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("CREATE TABLE " + name +
                    "(" +
                        "ID int IDENTITY PRIMARY KEY, " +
                        "[Name] nvarchar(50) NOT NULL, " +
                        "[Description] TEXT, " +
                        "[Cost] decimal NOT NULL, " +
                        "url text" +
                    ")", connection))
                {

                    cmd.ExecuteNonQuery();

                    for (int i = 0; i < filterType.Length; i++)
                    {
                        cmd.CommandText = "ALTER TABLE " + name + $" ADD {filterName[i]} {filterType[i]} NOT NULL";

                        cmd.ExecuteNonQuery();
                    }

                    cmd.CommandText = "INSERT INTO dbo.SubCategory VALUES(@Name, @url, @Category)";

                    cmd.Parameters.AddWithValue("Name", name);
                    cmd.Parameters.AddWithValue("url", urlImage);
                    cmd.Parameters.AddWithValue("Category", mainCategory);

                    cmd.ExecuteNonQuery();
                }

                subCategories.Add(new SubCategory(Category.categories.Find(cat => cat.name == mainCategory),
                    name, filterName, filterType, urlImage));
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

                using (var cmd = new SqlCommand("DELETE FROM dbo.SubCategory " +
                    "WHERE NameSubCategory = @Name", connection))
                {
                    cmd.Parameters.AddWithValue("Name", name);

                    cmd.ExecuteNonQuery();

                    cmd.CommandText = $"DROP TABLE {name}";

                    cmd.ExecuteNonQuery();
                }

                subCategories.Remove(subCategories.Find(cat => cat.name == name));
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
        public string name;

        [DataMember]
        string[] filtersName;

        [DataMember]
        string[] filtersType;

        [DataMember]
        Category mainCategory;

        [DataMember]
        string urlImage;

        public SubCategory(Category mainCategory, string name, string[] filtersName,
            string[] filtersType, string urlImage = "")
        {
            this.mainCategory = mainCategory;
            this.name = name;
            this.urlImage = urlImage;
            this.filtersName = filtersName;
            this.filtersType = filtersType;
        }
    }
}