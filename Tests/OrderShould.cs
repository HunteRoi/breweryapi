using Business;
using DAL;
using DAL.Repositories;
using DTO.V1.WholesalerRequest;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;

namespace Tests
{
    public class OrderShould
    {
        private Order _order;
        private BreweryDbContext _context;

        #region when class init
        [OneTimeSetUp]
        public void OneTimeSetup()
        {
            var optionsBuilder = new DbContextOptionsBuilder<BreweryDbContext>();
            optionsBuilder.UseSqlServer("Data Source=DESKTOP-HIOFRPI\\SQLEXPRESS;Initial Catalog=breweryapi;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
            _context = new BreweryDbContext(optionsBuilder.Options);
        }

        [OneTimeTearDown]
        public void OneTimeTeardown()
        {
            _context?.Dispose();
        }
        #endregion

        #region when test init
        [SetUp]
        public void Setup()
        {
            _order = new Order();
        }

        [TearDown]
        public void Teardown()
        {
            _order.OrderDetails.Clear();
        }
        #endregion

        [Test]
        public void ThrowBusinessExceptionOrderEmpty()
        {
            var sut = _order.OrderDetails;

            Assert.That(() => BusinessRequirements.EnsureOrderDetailsIsNotEmpty(sut),
                Throws.TypeOf<BusinessException>().With.Message.EqualTo("ORDER_CANNOT_BE_EMPTY")
            );
        }

        [Test]
        [Category("RelyingOnDatabase")]
        public void ThrowBusinessExceptionWholesalerDoesNotExist()
        {
            var breweryId = 0;
            var breweryRepo = new BreweryRepository(_context, null);

            Assert.That(() => BusinessRequirements.EnsureBreweryExistsAsync(breweryId, breweryRepo),
                Throws.TypeOf<BusinessException>().With.Message.EqualTo("BREWERY_MUST_EXIST")
            );
        }

        [Test]
        public void ThrowBusinessExceptionOrderHasDuplicate()
        {
            var sut = _order.OrderDetails;
            sut.Add(new Stock() { BeerId = 1 });
            sut.Add(new Stock() { BeerId = 1 });

            Assert.That(() => BusinessRequirements.EnsureOrderDetailsAreUnique(sut),
                Throws.TypeOf<BusinessException>().With.Message.EqualTo("NO_DUPLICATE_IN_ORDER")
            );
        }

        [Test]
        [Category("RelyingOnDatabase")]
        public void ThrowBusinessExceptionQuantityNotGreaterThanInStock()
        {
            var wholesalerId = 1;
            var sut = _order.OrderDetails = new List<Stock>
            {
                new Stock { BeerId = 1, Quantity = 159 }
            };
            var wholesalerRepo = new WholesalerRepository(_context, null);

            Assert.That(() => BusinessRequirements.EnsureQuantitiesAreNotGreaterThanStockAsync(wholesalerId, sut, wholesalerRepo),
                Throws.TypeOf<BusinessException>().With.Message.EqualTo("NOT_GREATER_NUMBER_THAN_IN_STOCK")
            );
        }

        [Test]
        [Category("RelyingOnDatabase")]
        public void ThrowBusinessExceptionBeerNotSoldByWholesaler()
        {
            var wholesalerId = 1;
            var sut = _order.OrderDetails = new List<Stock>
            {
                new Stock { BeerId = 6, Quantity = 1 }
            };
            var wholesalerRepo = new WholesalerRepository(_context, null);

            Assert.That(() => BusinessRequirements.EnsureBeersAreSoldByWholesalerAsync(wholesalerId, sut, wholesalerRepo),
                Throws.TypeOf<BusinessException>().With.Message.EqualTo("BEER_NOT_SOLD_BY_WHOLESALER")
            );
        }
    }
}
