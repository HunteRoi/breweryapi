using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Models.V1
{
    public class Stock : IEntity, IEquatable<Stock>
    {
        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public Wholesaler Wholesaler { get; private set; }

        [Required]
        public Beer Beer { get; private set; }

        public int Quantity { get; private set; }
        #endregion

        #region constructors
        private Stock() { }

        public Stock(int quantity, Beer beer, Wholesaler wholesaler)
        {
            SetQuantity(quantity);
            SetBeer(beer);
            SetWholesaler(wholesaler);
        }
        #endregion

        #region methods
        public bool Equals(Stock other)
        {
            if (other == null) return false;
            return Beer.Equals(other.Beer);
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Stock);
        }

        public override int GetHashCode()
        {
            return Beer.GetHashCode();
        }

        public Stock SetQuantity(int quantity)
        {
            if (quantity < 0) throw new ArgumentOutOfRangeException(nameof(quantity), "The quantity must be greater or equal to 0");
            Quantity = quantity;
            return this;
        }

        public Stock SetBeer(Beer beer)
        {
            Beer = beer ?? throw new ArgumentNullException(nameof(beer));
            return this;
        }

        public Stock SetWholesaler(Wholesaler wholesaler)
        {
            Wholesaler = wholesaler ?? throw new ArgumentNullException(nameof(wholesaler));
            return this;
        }
        #endregion
    }
}
