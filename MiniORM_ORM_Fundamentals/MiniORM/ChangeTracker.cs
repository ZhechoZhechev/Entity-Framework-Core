using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Reflection;
using MiniORM;

namespace MiniORM
{
    // Entity classes must be non-static reference types
    internal class ChangeTracker<TEntity>
        where TEntity : class, new()
    {
        private readonly IList<TEntity> allEntities;

        private readonly IList<TEntity> added;

        private readonly IList<TEntity> removed;

        private ChangeTracker()
        {
            this.added = new List<TEntity>();
            this.removed = new List<TEntity>();
        }

        public ChangeTracker(IEnumerable<TEntity> entities)
            : this()
        {
            this.allEntities = this.CloneEntities(entities);
        }

        public IReadOnlyCollection<TEntity> AllEntities =>
            (IReadOnlyCollection<TEntity>)this.allEntities;

        public IReadOnlyCollection<TEntity> Added =>
            (IReadOnlyCollection<TEntity>)added;

        public IReadOnlyCollection<TEntity> Removed =>
            (IReadOnlyCollection<TEntity>)removed;

        public void Add(TEntity entity) => this.added.Add(entity);

        public void Remove(TEntity entity) => this.removed.Add(entity);

        public IEnumerable<TEntity> GetModifiedEntities(DbSet<TEntity> dbSet)
        {
            IList<TEntity> modifiedEntities = new List<TEntity>();

            var primaryKeys = typeof(TEntity)
                .GetProperties()
                .Where(pi => pi.HasAttribute<KeyAttribute>())
                .ToArray();

            //var primaryKeys2 = typeof(TEntity).GetProperties().Where(pi => pi.GetCustomAttribute<KeyAttribute>() != null).ToList();

            foreach (var proxyEntity in this.AllEntities)
            {
                var proxyEntityPrimaryKeyValues =
                    GetPrimaryKeyValues(primaryKeys, proxyEntity).ToArray();

                var originalEntity = dbSet.Entities
                    .Single(e => GetPrimaryKeyValues(primaryKeys, e)
                    .SequenceEqual(proxyEntityPrimaryKeyValues));

                var isModified = IsModified(originalEntity, proxyEntity);
                if (isModified)
                {
                    modifiedEntities.Add(originalEntity);
                }
            }

            return modifiedEntities;
        }

        private IList<TEntity> CloneEntities(IEnumerable<TEntity> entities)
        {
            IList<TEntity> clonedEntities = new List<TEntity>();

            var propertiesToClone = typeof(TEntity)
            .GetProperties()
            .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType))
            .ToArray();

            foreach (var originalEntity in entities)
            {
                var clonedEntity = Activator.CreateInstance<TEntity>();
                foreach (var property in propertiesToClone)
                {
                    var originalValue = property.GetValue(originalEntity);
                    property.SetValue(clonedEntity, originalValue);
                }

                clonedEntities.Add(clonedEntity);
            }

            return clonedEntities;
        }

        private static IEnumerable<object> GetPrimaryKeyValues(IEnumerable<PropertyInfo> primaryKeys, TEntity entity)
            => primaryKeys.Select(pk => pk.GetValue(entity));

        private static bool IsModified(TEntity original, TEntity proxy)
        {
            var monitoredPorperties = typeof(TEntity).GetProperties()
                 .Where(pi => DbContext.AllowedSqlTypes.Contains(pi.PropertyType));

            var modifiedProperties = monitoredPorperties
                .Where(pi => !Equals(pi.GetValue(proxy), pi.GetValue(original)))
                .ToArray();

            return modifiedProperties.Any();
        }
    }
}