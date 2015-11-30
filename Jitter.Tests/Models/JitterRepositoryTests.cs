using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Jitter.Models;
using System.Collections.Generic;
using Moq;
using System.Data.Entity;
using System.Linq;

namespace Jitter.Tests.Models
{
    [TestClass]
    public class JitterRepositoryTests
    {
        private Mock<JitterContext> mock_context;
        private Mock<DbSet<JitterUser>> mock_set;
        private Mock<DbSet<Jot>> mock_jot_set;
        private JitterRepository repository;

        private void ConnectMocksToDataStore(IEnumerable<JitterUser> data_store)
        {
            var data_source = data_store.AsQueryable<JitterUser>();
            // HINT HINT: var data_source = (data_store as IEnumerable<JitterUser>).AsQueryable();
            // Convince LINQ that our Mock DbSet is a (relational) Data store.
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());
            
            // This is Stubbing the JitterUsers property getter
            mock_context.Setup(a => a.JitterUsers).Returns(mock_set.Object);
        }

        private void ConnectMocksToDataStore(IEnumerable<Jot> data_store)
        {
            var data_source = data_store.AsQueryable<Jot>();
            // HINT HINT: var data_source = (data_store as IEnumerable<Jot>).AsQueryable();
            // Convince LINQ that our Mock DbSet is a (relational) Data store.
            mock_jot_set.As<IQueryable<Jot>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_jot_set.As<IQueryable<Jot>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_jot_set.As<IQueryable<Jot>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_jot_set.As<IQueryable<Jot>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());

            // This is Stubbing the Jots property getter
            mock_context.Setup(a => a.Jots).Returns(mock_jot_set.Object);
        }

        [TestInitialize]
        public void Initialize()
        {
            mock_context = new Mock<JitterContext>();
            mock_set = new Mock<DbSet<JitterUser>>();
            mock_jot_set = new Mock<DbSet<Jot>>();
            repository = new JitterRepository(mock_context.Object);
        }

        [TestCleanup]
        public void Cleanup()
        {
            mock_context = null;
            mock_set = null;
            mock_jot_set = null;
            repository = null;
        }

        [TestMethod]
        public void JitterContextEnsureICanCreateInstance()
        {
            JitterContext context = mock_context.Object;
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanCreatInstance()
        {
            Assert.IsNotNull(repository);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanGetAllUsers()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);

            // Act
            var actual = repository.GetAllUsers();
            // Assert

            Assert.AreEqual("adam1", actual.First().Handle);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JitterRepositoryEnsureIHaveAContext()
        {
            // Arrange
            // Act
            var actual = repository.Context;
            // Assert
            Assert.IsInstanceOfType(actual, typeof(JitterContext));
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanGenUserByHandle()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "rumbadancer2";
            JitterUser actual_user = repository.GetUserByHandle(handle);
            // Assert
            Assert.AreEqual("rumbadancer2", actual_user.Handle);
        }

        [TestMethod]
        public void JitterRepositoryGetUserByHandleUserDoesNotExist()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "bogus";
            JitterUser actual_user = repository.GetUserByHandle(handle);
            // Assert
            Assert.IsNull(actual_user);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void JitterRepositoryGetUserByHandleFailsMultipleUsers()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "adam1"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "adam1";
            JitterUser actual_user = repository.GetUserByHandle(handle);
            // Assert
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsAvailable()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "bogus";
            bool is_available = repository.IsHandleAvailable(handle);
            // Assert
            Assert.IsTrue(is_available);
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsNotAvailable()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "adam1";
            bool is_available = repository.IsHandleAvailable(handle);
            // Assert
            Assert.IsFalse(is_available);
           
        }

        [TestMethod]
        public void JitterRepositoryEnsureHandleIsNotAvailableMultipleUsers()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser {Handle = "adam1" },
                new JitterUser { Handle = "adam1"}
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "adam1";
            bool is_available = repository.IsHandleAvailable(handle);
            // Assert
            Assert.IsFalse(is_available);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanSearchByHandle()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser { Handle = "adam1" },
                new JitterUser { Handle = "rumbadancer2"},
                new JitterUser { Handle = "treehugger" },
                new JitterUser { Handle = "treedancer"}
                
            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string handle = "tree";
            List<JitterUser> expected_users = new List<JitterUser>
            {
                new JitterUser { Handle = "treedancer"},
                new JitterUser { Handle = "treehugger" }
            };
            List<JitterUser> actual_users = repository.SearchByHandle(handle);
            // Assert
            // There be :dragon:. Collection Assert Doesn't use CompareTo underneath the hood. 
            // CollectionAssert.AreEqual(expected_users, actual_users);
            Assert.AreEqual(expected_users[0].Handle, actual_users[0].Handle);
            Assert.AreEqual(expected_users[1].Handle, actual_users[1].Handle);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanSearchByName()
        {
            // Arrange
            var expected = new List<JitterUser>
            {
                new JitterUser { Handle = "adam1", FirstName = "Adam", LastName = "Sandler" },
                new JitterUser { Handle = "rumbadancer2", FirstName = "Sandy", LastName = "Mathis"},
                new JitterUser { Handle = "treehugger", FirstName = "Popeye", LastName = "The Sailor Man" },
                new JitterUser { Handle = "treedancer", FirstName = "Richard", LastName = "Sandiehook"}

            };
            mock_set.Object.AddRange(expected);

            ConnectMocksToDataStore(expected);
            // Act
            string search_term = "sand";
            List<JitterUser> expected_users = new List<JitterUser>
            {
                new JitterUser { Handle = "adam1", FirstName = "Adam", LastName = "Sandler" },
                new JitterUser { Handle = "rumbadancer2", FirstName = "Sandy", LastName = "Mathis"},
                new JitterUser { Handle = "treedancer", FirstName = "Richard", LastName = "Sandiehook"}
            };

            List<JitterUser> actual_users = repository.SearchByName(search_term);

            // Assert
            Assert.AreEqual(expected_users[0].Handle, actual_users[0].Handle);
            Assert.AreEqual(expected_users[1].Handle, actual_users[1].Handle);
            Assert.AreEqual(expected_users[2].Handle, actual_users[2].Handle);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanGetAllJots()
        {
            // Arrange
            DateTime base_time = DateTime.Now;
            List<Jot> expected_jots = new List<Jot>
            {
                new Jot { Content = "Hello World!", Date = base_time.AddSeconds(-30) },
                new Jot { Content = "I'm Hungry", Date = base_time.AddMinutes(-5)},
                new Jot { Content = "Sweet Potato Pies >>>> Pumpkin Pies", Date = base_time.AddHours(-1)}
            };
            mock_jot_set.Object.AddRange(expected_jots);
            ConnectMocksToDataStore(expected_jots);
            // Act
            List<Jot> actual_jots = repository.GetAllJots();
            expected_jots.Sort();
            actual_jots.Sort();
            
            // Assert
            Assert.AreEqual(expected_jots[0].Content, actual_jots[0].Content);
            Assert.AreEqual(expected_jots[1].Content, actual_jots[1].Content);
            Assert.AreEqual(expected_jots[2].Content, actual_jots[2].Content);
            Assert.AreEqual("Hello World!", actual_jots[0].Content); // Just to check ourselves

        }

        [TestMethod]
        public void JitterRepositoryEnsureICanCreateAJot()
        {
            // Arrange
            DateTime base_time = DateTime.Now;
            List<Jot> expected_jots = new List<Jot>(); // This is our database
            ConnectMocksToDataStore(expected_jots);
            JitterUser jitter_user1 = new JitterUser { Handle = "popeye1"};
            string content = "Hello Jitter World!";
            mock_jot_set.Setup(j => j.Add(It.IsAny<Jot>())).Callback((Jot s) => expected_jots.Add(s));
            // Act
            bool successful = repository.CreateJot(jitter_user1, content);

            // Assert
            Assert.AreEqual(1, repository.GetAllJots().Count);
            // Should this return true?
            //Assert.IsTrue(successful);
        }

    }
}
