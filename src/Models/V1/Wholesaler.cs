﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Models.V1
{
    public class Wholesaler : IEntity
    {
        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; }

        public IEnumerable<Stock> Stocks { get; private set; }
        #endregion

        #region constructors
        private Wholesaler() 
        {
            Stocks = new List<Stock>();
        }

        public Wholesaler(string name) : this()
        {
            SetName(name);
        }

        public Wholesaler(string name, params Stock[] stocks) : this(name)
        {
            foreach(var stock in stocks)
            {
                AddStock(stock);
            }
        }
        #endregion

        #region methods
        public Wholesaler SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            return this;
        }

        public bool Equals(Wholesaler other)
        {
            if (other == null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Wholesaler);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString() => Name;

        public Wholesaler AddStock(int quantity, Beer beer)
        {
            return AddStock(new Stock(quantity, beer, this));
        }

        private Wholesaler AddStock(Stock stock)
        {
            if (stock is null) throw new ArgumentNullException(nameof(stock));

            if (!((List<Stock>)Stocks).Contains(stock))
            {
                ((List<Stock>)Stocks).Add(stock);
            }
            return this;
        }

        public Wholesaler AddQuantity(int quantity, Beer beer)
        {
            if (beer is null) throw new ArgumentNullException(nameof(beer));

            var stockToUpdate = ((List<Stock>)Stocks).Find(s => s.Beer == beer);
            if (stockToUpdate is null) throw new ArgumentNullException(nameof(stockToUpdate));
           
            stockToUpdate.SetQuantity(stockToUpdate.Quantity + quantity);
            return this;
        }

        public Wholesaler RemoveStock(Beer beer)
        {
            if (beer is null) throw new ArgumentNullException(nameof(beer));

            var stockToRemove = ((List<Stock>)Stocks).Find(s => s.Beer == beer);
            
            return RemoveStock(stockToRemove);
        }
        private Wholesaler RemoveStock(Stock stock)
        {
            if (stock is null) throw new ArgumentNullException(nameof(stock));
            ((List<Stock>)Stocks).Remove(stock);
            return this;
        }
        #endregion
    }
}
