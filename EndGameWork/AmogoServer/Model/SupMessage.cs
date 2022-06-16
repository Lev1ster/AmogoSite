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
    public class SupMessage
    {
        public static List<SupMessage> supMessages = new List<SupMessage>();
        readonly static SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static SupMessage()
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("SELECT ID_owner, [Message], Date " +
                    "FROM SupMessages", connection))
                {
                    using(var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            supMessages.Add(new SupMessage(int.Parse(reader[0].ToString()), reader[1].ToString(),
                                DateTime.Parse(reader[2].ToString())));
                        }
                    }
                }
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

        public static SupMessage[] GetSupMessages(int ID_Acc)
        {
            List<SupMessage> listResult = new List<SupMessage>();

            foreach (var item in supMessages)
            {
                if (item.ID_owner == ID_Acc)
                {
                    listResult.Add(item);
                }
            }

            return listResult.ToArray();
        }

        public static void Add(int ID_Own, string text)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("INSERT INTO SupMessages " +
                    "VALUES(@ID_Own, @Text, @Date)", connection))
                {
                    cmd.Parameters.AddWithValue("ID_Own", ID_Own);
                    cmd.Parameters.AddWithValue("Text", text);
                    cmd.Parameters.AddWithValue("Date", DateTime.Now);

                    cmd.ExecuteNonQuery();

                    supMessages.Add(new SupMessage(ID_Own, text, DateTime.Now));
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

        public static void Delete(int ID_Own)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("DELETE FROM Messages " +
                    "WHERE ID_owner = @ID_Own AND ID_second = @ID_Sec", connection))
                {
                    cmd.Parameters.AddWithValue("ID_Own", ID_Own);

                    cmd.ExecuteNonQuery();

                    supMessages.RemoveAll(mes => mes.ID_owner == ID_Own);
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

        [DataMember]
        public int ID_owner;

        [DataMember]
        string Text;

        [DataMember]
        DateTime date;

        public SupMessage(int iD_owner, string text, DateTime date)
        {
            ID_owner = iD_owner;
            Text = text;
            this.date = date;
        }
    }
}