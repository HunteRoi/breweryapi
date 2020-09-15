

using DAL;
using DAL.Repositories;
using DTO.V1.WholesalerRequest;
using Microsoft.EntityFrameworkCore;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Tests
{
    public class QuoteShould
    {
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

        [Test]
        [TestCase(11, 21.78, "10%")]
        [TestCase(21, 36.96, "20%")]
        public async Task ApplyPercentPromo(int quantity, decimal cost, string percentMsg)
        {
            var beerRepo = new BeerRepository(_context, null);
            var order = new Order
            {
                OrderDetails = new List<Stock>
                {
                    new Stock { BeerId = 1, Quantity = quantity }
                }
            };
            var beers = await beerRepo.ReadAllFromListAsync(order.OrderDetails.Select(od => od.BeerId));
            var sut = new Quote(order, beers);

            Assert.That(sut.Cost, Is.EqualTo(cost));
            Assert.That(sut.Summary, Has.Exactly(1).Matches<string>(s => s.Contains(percentMsg)));
        }
    }
}
