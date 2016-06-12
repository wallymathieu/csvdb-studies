using System.IO;
using System.Xml.Linq;
using SomeBasicCsvApp.Core;
using NUnit.Framework;
using Order = SomeBasicCsvApp.Core.Entities.Order;
using System.Linq;
using SomeBasicCsvApp.Core.Entities;
using System;
using With;
namespace SomeBasicCsvApp.Tests
{
    [TestFixture]
    public class CustomerDataTests
    {

        private ISessionFactory _sessionFactory;

        private ISession _session;


        [Test]
        public void CanGetCustomerById()
        {
            var customer = _session.Get<Customer>(1);

            Assert.IsNotNull(customer);
        }

        [Test]
        public void CustomerHasOrders()
        {
            var customer = _session.Get<Customer>(1);

            Assert.True(_session.Any<Order>(o => o.Customer == customer.Id));
        }

        [Test]
        public void ProductsArePartOfOrders()
        {
            var product = _session.Get<Product>(1);

            Assert.True(_session.Any<OrderProduct>(op => op.ProductId == product.Id));
        }

        [Test]
        public void CanGetCustomerByFirstname()
        {
            var customers = _session.Where<Customer>(c => c.Firstname == "Steve")
                .ToList();
            Assert.AreEqual(3, customers.Count);
        }

        [Test]
        public void CanUpdateCustomer()
        {
            Customer customer;
            int version;
            using (var session = _sessionFactory.OpenSession())
            {
                customer = session.Get<Customer>(1);
                customer.Lastname += "_Updated";
                version = customer.Version;
                session.Commit();
            }

            var c = _session.Get<Customer>(1);
            Assert.That(c.Lastname, Is.EqualTo(customer.Lastname));
            Assert.That(c.Version, Is.EqualTo(version + 1));
        }

        [Test]
        public void CanGetProductById()
        {
            var product = _session.Get<Product>(1);

            Assert.IsNotNull(product);
        }

        [Test]
        public void OrderContainsProduct()
        {
            var order = _session.Get<Order>(1);

            Assert.True(_session.Any<OrderProduct>(op => op.OrderId == order.Id));
        }

        [Test]
        public void OrderHasACustomer()
        {
            Assert.IsTrue(_session.Get<Order>(1).Customer > 0);
        }

        [SetUp]
        public void Setup()
        {
            _session = _sessionFactory.OpenSession();
        }


        [TearDown]
        public void TearDown()
        {
            _session.Dispose();
        }

        [TestFixtureSetUp]
        public void TestFixtureSetup()
        {
            if (Directory.Exists("CustomerDataTests")) { Directory.Delete("CustomerDataTests", recursive: true); }

            _sessionFactory = SessionFactory.CreateTestSessionFactory(new ConsoleMapPath().MapPath("CustomerDataTests"));
            var doc = XDocument.Load(Path.Combine("TestData", "TestData.xml"));
            var import = new XmlImport(doc, "http://tempuri.org/Database.xsd");
            using (var session = _sessionFactory.OpenSession())
            {
                import.Parse(new[] { typeof(Customer), typeof(Order), typeof(Product) },
                    (type, obj) =>
                    {
                        Switch.On(obj)
                            .Case((Customer c) => session.Save(c))
                            .Case((Order o) => session.Save(o))
                            .Case((Product p) => session.Save(p))
                            .ElseFail();
                    },
                    onIgnore: (type, property) =>
                    {
                        Console.WriteLine("ignoring property {1} on {0}", type.Name, property.PropertyType.Name);
                    });
                session.Commit();
            }
            using (var session = _sessionFactory.OpenSession())
            {
                int sequence = 0;
                import.ParseConnections("OrderProduct", "Product", "Order", (productId, orderId) =>
                {
                    var product = session.Get<Product>(productId);
                    var order = session.Get<Order>(orderId);
                    session.Save(new OrderProduct { OrderId = order.Id, ProductId = product.Id, Id = ++sequence });
                });

                import.ParseIntProperty("Order", "Customer", (orderId, customerId) =>
                {
                    session.Get<Order>(orderId).Customer = session.Get<Customer>(customerId).Id;
                });
                session.Commit();
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            _sessionFactory.Dispose();
        }
    }
}
