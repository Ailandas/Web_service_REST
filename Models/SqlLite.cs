using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Web;

namespace WebEndProject.Models
{
    public static class SqlLite
    {
        public static string kelias = System.Web.HttpContext.Current.Request.PhysicalApplicationPath + "\\Database.db;";
        public static List<Category> GetCategories()
        {
            List<Category> Categories = new List<Category>();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT DISTINCT Category FROM Categories", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        Category cat = new Category();
                        cat.SetName(Convert.ToString(reader3[0]));

                        Categories.Add(cat);


                    }
                }
                m_dbConnection.Close();
            }
            return Categories;
        }
        public static bool UpdateCategoryName(string newCategory, string oldCategory)
        {
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + kelias))
                {
                    string sql1 = $"UPDATE Categories SET Category='{newCategory}' WHERE Category='{oldCategory}'";
                    SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                    m_dbConnection.Open();
                    command1.ExecuteNonQuery();
                    m_dbConnection.Close();
                    return true;
                }
            }
            catch (Exception exc)
            {
                return false;
            }
        }

        public static bool UpdateSingleWord(string newWord, string oldWord)
        {
            using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + kelias))
            {
                string sql1 = $"UPDATE Categories SET Word='{newWord}' WHERE Word='{oldWord}'";
                SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                m_dbConnection.Open();
                command1.ExecuteNonQuery();
                m_dbConnection.Close();
                return true;
            }

        }

        public static bool CheckIfWordExists(string word)
        {
            string data = "";
            bool Egzists = false;
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {
                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT Word FROM Categories", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {

                        data = Convert.ToString(reader3[0]);
                        if (data == word)
                        {
                            Egzists = true;

                        }


                    }
                }
                m_dbConnection.Close();
            }

            return Egzists;

        }

        public static void InsertToDatabase(string Category, string Word, int DayTime)
        {
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + kelias))
                {


                    int ID = GetLatestID(); //Paima naujausia ID ir padidina
                    ID++;

                    string sql1 = $"insert into Categories (ID,Category,Word,DayTime) VALUES (@ID, @Category, @Word, @DayTime)";
                    SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                    command1.Parameters.AddWithValue("@ID", ID);
                    command1.Parameters.AddWithValue("@Category", Category);
                    command1.Parameters.AddWithValue("@Word", Word);
                    command1.Parameters.AddWithValue("@DayTime", DayTime);
                    m_dbConnection.Open();
                    command1.ExecuteNonQuery();
                    m_dbConnection.Close();
                }
            }
            catch (Exception exc)
            {
                Console.WriteLine(exc.Message);
            }
        }
        public static bool CheckIfCategoryEgzists(string Category)
        {
            string data = "";
            bool Egzists = false;
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT Category FROM Categories", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {

                        data = Convert.ToString(reader3[0]);
                        if (data == Category)
                        {
                            Egzists = true;
                           

                        }


                    }
                }
                m_dbConnection.Close();

            }
            return Egzists;
        }
        public static List<Time> GetTimes ()
        {
            List<Time> times = new List<Time>();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT * FROM DayTime", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        Time tempTime = new Time();
                        tempTime.SetID(Convert.ToInt16(reader3[0]));
                        tempTime.SetName(Convert.ToString(reader3[1]));
                        tempTime.SetAfter(Convert.ToString(reader3[2]));
                        tempTime.SetUpTo(Convert.ToString(reader3[3]));
                        times.Add(tempTime);

                    }
                }
                m_dbConnection.Close();
            }
            return times;
        }
        public static SingleWord GetSingleWord(int DayTime, string Category)
        {
            List<SingleWord> Words = new List<SingleWord>();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT * FROM Categories WHERE Category='{Category}' AND DayTime='{DayTime}' ", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        SingleWord word  = new SingleWord();
                        word.SetID(Convert.ToInt32(reader3[0]));
                        word.SetCategory(Convert.ToString(reader3[1]));
                        word.SetWord(Convert.ToString(reader3[2]));
                        word.SetDayTime(Convert.ToInt32(reader3[3]));
                        Words.Add(word);


                    }
                }
                m_dbConnection.Close();

            }
            Random rnd = new Random();
            int RandomNumber = rnd.Next(0, Words.Count);
            return Words[RandomNumber];
        }
        public static int GetLatestID() //Grazina paskutini ID is duomenu bazes
        {
            string data = "";
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT ID FROM Categories", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        data = Convert.ToString(reader3[0]);


                    }
                }
                m_dbConnection.Close();
            }
            return Convert.ToInt32(data);
        }
        public static List<SingleWord> GetEntriesByCategory(string category)
        {

            List<SingleWord> words = new List<SingleWord>();
            using (SQLiteConnection m_dbConnection = new SQLiteConnection(@"Data Source=" + kelias))
            {

                m_dbConnection.Open();
                using (SQLiteCommand command3 = new SQLiteCommand($"SELECT * FROM Categories WHERE Category='{category}'", m_dbConnection))
                {

                    SQLiteDataReader reader3 = command3.ExecuteReader();
                    while (reader3.Read())
                    {
                        SingleWord sw = new SingleWord();
                        sw.SetWord(Convert.ToString(reader3[2]));
                        
                        
                        words.Add(sw);


                    }
                }
                m_dbConnection.Close();
            }
            return words;
        }
        public static bool DeleteCategory(string category)
        {
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + kelias))
                {

                    string sql1 = $"DELETE FROM Categories WHERE Category='{category}';";
                    SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                    m_dbConnection.Open();
                    command1.ExecuteNonQuery();
                    m_dbConnection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }
            
         
        }
        public static bool DeleteWord(string word)
        {
            try
            {
                using (SQLiteConnection m_dbConnection = new SQLiteConnection("Data Source=" + kelias))
                {

                    string sql1 = $"DELETE FROM Categories WHERE Word='{word}';";
                    SQLiteCommand command1 = new SQLiteCommand(sql1, m_dbConnection);
                    m_dbConnection.Open();
                    command1.ExecuteNonQuery();
                    m_dbConnection.Close();
                    return true;
                }
            }
            catch
            {
                return false;
            }


        }
        public static string GetCurrentDirectory()
        {
            var enviroment = System.Environment.CurrentDirectory;
            string projectDirectory = Directory.GetParent(enviroment).Parent.FullName;

            return projectDirectory;
        }
    }
}