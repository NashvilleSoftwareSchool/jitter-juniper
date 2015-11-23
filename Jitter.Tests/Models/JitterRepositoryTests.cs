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
        [TestMethod]
        public void JitterContextEnsureICanCreateInstance()
        {
            JitterContext context = new JitterContext();
            Assert.IsNotNull(context);
        }

        [TestMethod]
        public void JitterRepositoryEnsureICanCreatInstance()
        {
            JitterRepository repository = new JitterRepository();
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
            Mock<JitterContext> mock_context = new Mock<JitterContext>();
            Mock<DbSet<JitterUser>> mock_set = new Mock<DbSet<JitterUser>>();

            mock_set.Object.AddRange(expected);
            var data_source = expected.AsQueryable();

            // Convince LINQ that our Mock DbSet is a (relational) Data store.
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Provider).Returns(data_source.Provider);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.Expression).Returns(data_source.Expression);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.ElementType).Returns(data_source.ElementType);
            mock_set.As<IQueryable<JitterUser>>().Setup(data => data.GetEnumerator()).Returns(data_source.GetEnumerator());

            // This is Stubbing the JitterUsers property getter
            mock_context.Setup(a => a.JitterUsers).Returns(mock_set.Object);
            JitterRepository repository = new JitterRepository(mock_context.Object);
            // Act
            var actual = repository.GetAllUsers();
            // Assert
            //Assert.AreEqual("adam1", actual.First().Handle);
            CollectionAssert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void JitterRepositoryEnsureIHaveAContext()
        {
            // Arrange
            JitterRepository repository = new JitterRepository();
            // Act
            var actual = repository.Context;
            // Assert
            Assert.IsInstanceOfType(actual, typeof(JitterContext));
        }


    }
}
