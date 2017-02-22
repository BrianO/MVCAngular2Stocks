using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Data;
using System.Data.SqlClient;
using SocialStocks2.Models;

namespace SocialStocks2
{
    public class Data
    {
        private SqlDataReader getReader(string sql)
        {
            SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();

            SqlCommand cmd =
                new SqlCommand(sql, conn);

            SqlDataReader reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);

            return reader;
        }

        public string UserIdFromName(string name)
        {
            SqlDataReader reader = getReader("Select Id from AspNetUsers Where UserName='" +
                name + "'");

            if (reader.Read())
            {
                string returnVal = reader["Id"].ToString();
                reader.Close();
                return returnVal;
            }
            else
            {
                reader.Close();
                return "";
            }
           
        }

        public IEnumerable<Stock> GetUserStocks(string userId)
        {
            List<Stock> stocks = new List<Stock>();

            using (SqlDataReader reader =
                  getReader("Select * From Stocks Where UserId = '" + userId + "' ORDER BY SYMBOL"))
            {

                while (reader.Read())
                {
                    stocks.Add(new Stock()
                    {
                        Id = reader["Id"].ToString(),
                        Symbol = reader["Symbol"].ToString(),
                        Color = "Green",
                        Price = ""
                    });
                }

                reader.Close();
            }

            return stocks;
        }

        public void AddCustomer(string name, string address, string phone)
        {
            SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();

            SqlCommand cmd =
               new SqlCommand("INSERT INTO CUSTOMER (Name,Address,Phone) Values (@Name,@Address,@Phone)", conn);
            cmd.CommandType = CommandType.Text;

            SqlParameter p1 = new SqlParameter("@Name", name);
            SqlParameter p2 = new SqlParameter("@Address", address);
            SqlParameter p3 = new SqlParameter("@Phone", phone);
            
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);          
            cmd.ExecuteNonQuery();
            
            conn.Close();
        }


        public IEnumerable<Customer> GetCustomers()
        {
            List<Customer> customers = new List<Customer>();

            SqlDataReader reader = getReader("Select * From Customer ORDER BY NAME");

            while (reader.Read())
            {
                customers.Add(new Customer()
                {
                    Id = reader["Id"].ToString(),
                    Name = reader["Name"].ToString(),
                    Address = reader["Address"].ToString(),
                    Phone = reader["Phone"].ToString()
                });
            }

            reader.Close();

            return customers;
        }

        public IEnumerable<Feed> GetUserFeeds(string userId)
        {
            List<Feed> feeds = new List<Feed>();

            SqlDataReader reader = getReader("Select * From Feeds Where UserId = '" + userId + "' ORDER BY TITLE");

            while (reader.Read())
            {
                feeds.Add(new Feed()
                {
                    Id = reader["Id"].ToString(),
                    Title = reader["Title"].ToString(),
                    Url = reader["Url"].ToString()
                });            
            }

            reader.Close();

            return feeds;
        }
        
        public void DeleteFeed(string id)
        {
            SqlConnection conn =
               new SqlConnection(
                   ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM FEEDS WHERE Id = @Id", conn);
            cmd.CommandType = CommandType.Text;
            SqlParameter p1 = new SqlParameter("@Id", id);
            cmd.Parameters.Add(p1);

            cmd.ExecuteNonQuery();

            conn.Close();
        }

        public void DeleteStock(string id)
        {
            SqlConnection conn =
               new SqlConnection(
                   ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();
            SqlCommand cmd = new SqlCommand("DELETE FROM STOCKS WHERE Id = @Id", conn);
            cmd.CommandType = CommandType.Text;
            SqlParameter p1 = new SqlParameter("@Id", id);
            cmd.Parameters.Add(p1);

            cmd.ExecuteNonQuery();

            conn.Close();
        }
        public void InsertFeed(string userId, string url, string title)
        {

            SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();
            
            System.Guid guid = System.Guid.NewGuid();

            SqlCommand cmd =
                new SqlCommand("INSERT INTO FEEDS (Id,UserId,Title,URL) Values (@Id,@UserId,@Title,@URL)", conn);
            cmd.CommandType = CommandType.Text;

            SqlParameter p1 = new SqlParameter("@Id", guid.ToString());
            SqlParameter p2 = new SqlParameter("@UserId",userId);
            SqlParameter p3 = new SqlParameter("@Title",title);
            SqlParameter p4 = new SqlParameter("@URL", url);

            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);
            cmd.Parameters.Add(p4);
            cmd.ExecuteNonQuery();
            conn.Close();
        }


        public void UpdateFeed(string Id, string url, string title)
        {
            using (SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                conn.Open();

                SqlCommand cmd =
                    new SqlCommand("UPDATE FEEDS SET TITLE=@Title, URL=@URL WHERE ID=@ID", conn);
                cmd.CommandType = CommandType.Text;

                SqlParameter p1 = new SqlParameter("@Id", Id);
                SqlParameter p3 = new SqlParameter("@Title", title);
                SqlParameter p4 = new SqlParameter("@URL", url);

                cmd.Parameters.Add(p1);
                cmd.Parameters.Add(p3);
                cmd.Parameters.Add(p4);
                cmd.ExecuteNonQuery();
                conn.Close();
            }
        }

        public void InsertStock(string userId, string symbol)
        {
            SqlConnection conn =
                new SqlConnection(
                    ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);

            conn.Open();

            System.Guid guid = System.Guid.NewGuid();

            SqlCommand cmd =
                new SqlCommand("INSERT INTO STOCKS (Id,UserId,Symbol) Values (@Id,@UserId,@Symbol)", conn);
            cmd.CommandType = CommandType.Text;

            SqlParameter p1 = new SqlParameter("@Id", guid.ToString());
            SqlParameter p2 = new SqlParameter("@UserId", userId);
            SqlParameter p3 = new SqlParameter("@Symbol", symbol);
            
            cmd.Parameters.Add(p1);
            cmd.Parameters.Add(p2);
            cmd.Parameters.Add(p3);

            cmd.ExecuteNonQuery();
            conn.Close();
        }
    }
}