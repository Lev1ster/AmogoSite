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
    public class Message
    {
        public static List<Message> messages = new List<Message>();

        readonly static SqlConnection connection =
            new SqlConnection(ConfigurationManager.ConnectionStrings["DbConnection"].ConnectionString);

        static Message()
        {
            try
            {
                for (int i = 0; i < SubCategory.subCategories.Count; i++)
                {
                    using (var cmd = new SqlCommand("SELECT ID_owner, ID_second, " +
                        "Message, Date" +
                        " FROM Messages " +
                        "ORDER BY Date ASC", connection))
                    {
                        connection.Open();

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                messages.Add(new Message(int.Parse(reader[0].ToString()), int.Parse(reader[1].ToString()), 
                                    reader[2].ToString(), DateTime.Parse(reader[3].ToString()).ToString()));
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

        public static Message[] GetMessages(int ID_Acc)
        {
            List<Message> listResult = new List<Message>();

            foreach (var item in messages)
            {
                if(item.idOwner == ID_Acc || item.idSecond == ID_Acc)
                {
                    listResult.Add(item);
                }
            }

            return listResult.ToArray();
        }

        public static void Add(int ID_Own, int ID_Sec, string text)
        {
            try
            {
                connection.Open();

                using (var cmd = new SqlCommand("INSERT INTO Messages " +
                    "VALUES(@ID_Own, @ID_Sec, @Text, @Date)", connection))
                {
                    cmd.Parameters.AddWithValue("ID_Own", ID_Own);
                    cmd.Parameters.AddWithValue("ID_Sec", ID_Sec);
                    cmd.Parameters.AddWithValue("Text", text);
                    cmd.Parameters.AddWithValue("Date", DateTime.Now.ToString());

                    cmd.ExecuteNonQuery();

                    messages.Add(new Message(ID_Own, ID_Sec, text, DateTime.Now.ToString()));
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

        public static void Delete(int ID_Own, int ID_Sec)
        {
            try
            {
                connection.Open();

                using(var cmd = new SqlCommand("DELETE FROM Messages " +
                    "WHERE ID_owner = @ID_Own AND ID_second = @ID_Sec", connection))
                {
                    cmd.Parameters.AddWithValue("ID_Own", ID_Own);
                    cmd.Parameters.AddWithValue("ID_Sec", ID_Sec);

                    cmd.ExecuteNonQuery();

                    messages.RemoveAll(mes => (mes.idOwner == ID_Own && mes.idSecond == ID_Sec));
                }

                using (var cmd = new SqlCommand("DELETE FROM Messages " +
                    "WHERE ID_owner = @ID_Sec AND ID_second = @ID_Own", connection))
                {
                    cmd.Parameters.AddWithValue("ID_Own", ID_Own);
                    cmd.Parameters.AddWithValue("ID_Sec", ID_Sec);

                    cmd.ExecuteNonQuery();

                    messages.RemoveAll(mes => (mes.idOwner == ID_Sec && mes.idSecond == ID_Own));
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

        [DataMember]
        public int idOwner;

        [DataMember]
        public int idSecond;

        [DataMember]
        string text;

        [DataMember]
        string date;

        public Message(int idOwner, int idSecond, string text, string date)
        {
            this.idOwner = idOwner;
            this.idSecond = idSecond;
            this.text = text;
            this.date = date;
        }
    }
}