using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace Models.V1
{
    public class Brewery : IEntity, IEquatable<Brewery>
    {
        #region properties
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; private set; }

        [Required]
        public string Name { get; private set; }

        public IEnumerable<Beer> Beers { get; private set; }
        #endregion

        #region constructors
        private Brewery() 
        {
            Beers = new List<Beer>();
        }

        public Brewery(string name) : this()
        {
            SetName(name);
        }

        public Brewery(string name, params Beer[] beers) : this(name)
        {
            foreach(var beer in beers)
            {
                AddBeer(beer);
            }
        }
        #endregion

        #region methods
        public bool Equals(Brewery other)
        {
            if (other is null) return false;
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as Brewery);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        public override string ToString() => Name;

        public Brewery SetName(string name)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            return this;
        }

        public Brewery AddBeer(Beer beer)
        {
            if (beer is null) throw new ArgumentNullException(nameof(beer));

            if (!((List<Beer>)Beers).Contains(beer))
            {
                ((List<Beer>)Beers).Add(beer);
            }
            return this;
        }

        public Brewery RemoveBeer(Beer beer)
        {
            if (beer is null) throw new ArgumentNullException(nameof(beer));
            ((List<Beer>)Beers).Remove(beer);
            return this;
        }
        #endregion
    }
}
