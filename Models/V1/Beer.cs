using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.V1
{
    public class Beer : IEntity, IEquatable<Beer>
    {
        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }
        
        [Required]
        public string Name { get; private set; }

        public decimal AlcoholLevel { get; private set; }

        public decimal Price { get; private set; }

        [Required]
        public Brewery Brewery { get; private set; }

        public IEnumerable<Stock> Stocks { get; set; }
        #endregion

        #region constructors
        private Beer() 
        {
            Stocks = new List<Stock>();
        }

        public Beer(string name, decimal alcoholLevel, decimal price, Brewery brewery) : this()
        {
            SetName(name);
            SetAlcoholLevel(alcoholLevel);
            SetPrice(price);
            SetBrewery(brewery);
        }
        #endregion

        #region methods
        public bool Equals(Beer other)
        {
            if (other is null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Beer);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString() => Name;

        public Beer SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            return this;
        }

        public Beer SetPrice(decimal price)
        {
            if (price < 0) throw new ArgumentOutOfRangeException(nameof(price), "The price must be greater or equal to 0");
            Price = price;
            return this;
        }

        public Beer SetAlcoholLevel(decimal alcoholLevel)
        {
            if (alcoholLevel < 0) throw new ArgumentOutOfRangeException(nameof(alcoholLevel), "The alcohol level must be greater or equal to 0");
            AlcoholLevel = alcoholLevel;
            return this;
        }

        public Beer SetBrewery(Brewery brewery)
        {
            Brewery = brewery ?? throw new ArgumentNullException(nameof(brewery));
            return this;
        }
        #endregion
    }
}
