using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Logging;
using System.Data.SqlClient;

namespace LoggerTest
{
    /// <summary>
    /// Summary description for LoggerDAOTest
    /// </summary>
    [TestClass]
    public class LoggerTest
    {
        public LoggerTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        private SqlConnection connectionSQL = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\Jack\\Documents\\Visual Studio 2015\\Projects\\SuperSimpleLogger\\Logger\\Log.mdf\"; Integrated Security = True; Connect Timeout = 30");

        [TestInitialize]
        public void DestroyDatabase()
        {
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "DELETE FROM log";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            sqlCommand.ExecuteNonQuery();
            connectionSQL.Close();
        }

        [ClassCleanup]
        public static void CallDestroyDatabase()
        {
            SqlConnection connectionSQL = new SqlConnection("Data Source = (LocalDB)\\MSSQLLocalDB; AttachDbFilename=\"C:\\Users\\Jack\\Documents\\Visual Studio 2015\\Projects\\SuperSimpleLogger\\Logger\\Log.mdf\"; Integrated Security = True; Connect Timeout = 30");
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "DELETE FROM log";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            sqlCommand.ExecuteNonQuery();
            connectionSQL.Close();
        }

        [TestMethod]
        public void TestGetInstance()
        {
            Logger logger = Logger.getInstance();
            Assert.AreEqual(Logger.getInstance(), logger);
        }

        [TestMethod]
        public void TestAddLog()
        {
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            // Use of mocks has been mentioned but is not necessary for this practice
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            String userName = null;
            String area = null;
            String type = null;
            while (sqlDataReader.Read())
            {
                userName = Convert.ToString(sqlDataReader["User"]).TrimEnd();
                area = Convert.ToString(sqlDataReader["Area"]).TrimEnd();
                type = Convert.ToString(sqlDataReader["Type"]).TrimEnd();
            }
            Assert.AreEqual("Usuario", userName);
            Assert.AreEqual("Area", area);
            Assert.AreEqual("Type", type);
        }

        [TestMethod]
        public void TestOrderByUser()
        {
            // Known bad practice but when you don't have time you make do.
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            System.Threading.Thread.Sleep(1000);
            logger.AddLog("Asuario", "Brea", "Dype");
            List<Log> logs = logger.OrderBy(Logger.Fields.User);
            // Use of mocks has been mentioned but is not necessary for this practice
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log ORDER BY(User) DESC";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int i = 0;
            while (sqlDataReader.Read())
            {
                Log log = logs[i];
                Assert.AreEqual(Convert.ToString(sqlDataReader["User"]).TrimEnd(), log.userName);
                Assert.AreEqual(Convert.ToString(sqlDataReader["area"]).TrimEnd(), log.area);
                Assert.AreEqual(Convert.ToString(sqlDataReader["type"]).TrimEnd(), log.type);
                Assert.AreEqual(Convert.ToDateTime(sqlDataReader["dateTime"]), log.dateTime);
                i++;
            }
        }

        [TestMethod]
        public void TestOrderByDateTime()
        {
            // Known bad practice but when you don't have time you make do.
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            System.Threading.Thread.Sleep(1000);
            logger.AddLog("Asuario", "Brea", "Dype");
            List<Log> logs = logger.OrderBy(Logger.Fields.DateTime);
            // Use of mocks has been mentioned but is not necessary for this practice
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log ORDER BY(DateTime) DESC";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int i = 0;
            while (sqlDataReader.Read())
            {
                Log log = logs[i];
                Assert.AreEqual(Convert.ToString(sqlDataReader["User"]).TrimEnd(), log.userName);
                Assert.AreEqual(Convert.ToString(sqlDataReader["area"]).TrimEnd(), log.area);
                Assert.AreEqual(Convert.ToString(sqlDataReader["type"]).TrimEnd(), log.type);
                Assert.AreEqual(Convert.ToDateTime(sqlDataReader["dateTime"]), log.dateTime);
                i++;
            }
        }
        [TestMethod]
        public void TestOrderByArea()
        {
            // Known bad practice but when you don't have time you make do.
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            System.Threading.Thread.Sleep(1000);
            logger.AddLog("Asuario", "Brea", "Dype");
            List<Log> logs = logger.OrderBy(Logger.Fields.Area);
            // Use of mocks has been mentioned but is not necessary for this practice
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log ORDER BY(Area) DESC";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int i = 0;
            while (sqlDataReader.Read())
            {
                Log log = logs[i];
                Assert.AreEqual(Convert.ToString(sqlDataReader["User"]).TrimEnd(), log.userName);
                Assert.AreEqual(Convert.ToString(sqlDataReader["area"]).TrimEnd(), log.area);
                Assert.AreEqual(Convert.ToString(sqlDataReader["type"]).TrimEnd(), log.type);
                Assert.AreEqual(Convert.ToDateTime(sqlDataReader["dateTime"]), log.dateTime);
                i++;
            }
        }

        [TestMethod]
        public void TestOrderByType()
        {
            // Known bad practice but when you don't have time you make do.
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            System.Threading.Thread.Sleep(1000);
            logger.AddLog("Asuario", "Brea", "Dype");
            List<Log> logs = logger.OrderBy(Logger.Fields.Type);
            // Use of mocks has been mentioned but is not necessary for this practice
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "SELECT * FROM Log ORDER BY(Type) DESC";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            int i = 0;
            while (sqlDataReader.Read())
            {
                Log log = logs[i];
                Assert.AreEqual(Convert.ToString(sqlDataReader["User"]).TrimEnd(), log.userName);
                Assert.AreEqual(Convert.ToString(sqlDataReader["area"]).TrimEnd(), log.area);
                Assert.AreEqual(Convert.ToString(sqlDataReader["type"]).TrimEnd(), log.type);
                Assert.AreEqual(Convert.ToDateTime(sqlDataReader["dateTime"]), log.dateTime);
                i++;
            }
            sqlDataReader.Close();
            connectionSQL.Close();
        }

        [TestMethod]
        public void TestDeleteLog()
        {
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area", "Type");
            List<Log> logs = logger.DumpAll();
            Assert.IsTrue(logger.DeleteLog("Jay", logs[0].id)); // Jay is the only superuser
            SqlCommand sqlCommand = new SqlCommand();
            sqlCommand.CommandText = "DELETE FROM log";
            sqlCommand.Connection = connectionSQL;
            connectionSQL.Open();
            Assert.AreEqual(1, sqlCommand.ExecuteNonQuery());
            connectionSQL.Close();
        }
        [TestMethod]
        public void TestLastUse()
        {
            Logger logger = Logger.getInstance();
            logger.AddLog("Usuario", "Area1", "Type");
            logger.AddLog("Asuario", "Brea1", "Dype");
            System.Threading.Thread.Sleep(1000);
            logger.AddLog("Usuario", "Area2", "Type");
            logger.AddLog("Asuario", "Brea2", "Dype");
            List<Log> logs = logger.LastUses();
            Assert.IsTrue(logs[0].area == "Area2" || logs[0].area == "Brea2");
            Assert.IsTrue(logs[1].area == "Area2" || logs[1].area == "Brea2");
        }
    }
}
