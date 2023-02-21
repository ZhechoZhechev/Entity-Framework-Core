using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace MiniORM
{
	public class DbSet<TEntity> : ICollection<TEntity>
        where TEntity : class, new()
    {
        // Read from the DB
        // Stored in-memory (not in the DB itself)
        internal DbSet(IEnumerable<TEntity> entities)
        {
            this.Entities = entities.ToList();
            this.ChangeTracker = new ChangeTracker<TEntity>(entities);
        }

        internal ChangeTracker<TEntity> ChangeTracker { get; set; }

        internal IList<TEntity> Entities { get; set; }

        public int Count => this.Entities.Count;

        public bool IsReadOnly => this.Entities.IsReadOnly;

        public void Add(TEntity item)
        {         
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), ExceptionMessages.ItemNullException);
            }

            this.Entities.Add(item);
            this.ChangeTracker.Add(item);
        }

        public bool Remove(TEntity item)
        {          
            if (item == null)
            {
                throw new ArgumentNullException(nameof(item), ExceptionMessages.ItemNullException);
            }

            bool removedSuccessfully = this.Entities.Remove(item);
            if (removedSuccessfully)
            {
                this.ChangeTracker.Remove(item);
            }

            return removedSuccessfully;
        }

        public void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
            {
                this.Remove(entity);
            }
        }

        public void Clear()
        {
            while (this.Entities.Any())
            {
                var currenEntity = this.Entities.First();
                this.Remove(currenEntity);
            }
        }

        public bool Contains(TEntity item)
            => this.Entities.Contains(item);

        public void CopyTo(TEntity[] array, int arrayIndex)    
            => this.Entities.CopyTo(array, arrayIndex);

        public IEnumerator<TEntity> GetEnumerator() => this.Entities.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}