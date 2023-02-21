using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.SqlClient;
using System.Linq;
using System.Reflection;

namespace MiniORM
{
	// responsible for retrieving entities from the database (DbSet<TEntity>)
	// and mapping the relations between them (through the so-called navigation properties)

	public abstract class DbContext
	{
		private readonly DatabaseConnection connection;

		private readonly IDictionary<Type, PropertyInfo> dbSetProperties;

		internal static readonly Type[] AllowedSqlTypes =
		{
			typeof(int),
			typeof(uint),
			typeof(long),
			typeof(ulong),
			typeof(short),
			typeof(ushort),
			typeof(byte),
			typeof(sbyte),
			typeof(decimal),
			typeof(double),
			typeof(float),
			typeof(bool),
			typeof(string),
			typeof(DateTime),
			typeof(TimeSpan),					
		};

        protected DbContext(string connectionString)
        {
			this.connection = new DatabaseConnection(connectionString);

			this.dbSetProperties = this.DiscoverDbSets();

			using var connectionManager = new ConnectionManager(this.connection);
			this.InitializeDbSets();

			this.MapAllRelations();
        }

		public void SaveChanges()
        {
            // object[] where each object is a collection of entity classes
			var dbSets = this.dbSetProperties
				.Select(kvp => kvp.Value.GetValue(this))
				.ToArray();

            foreach (IEnumerable<object> dbSet in dbSets)
            {
				var invalidEntities = dbSet
					.Where(e => !IsObjectValid(e))
					.ToArray();

				if (invalidEntities.Any())
                {
					throw new InvalidOperationException(
						string.Format(ExceptionMessages.InvalidEntitiesInContext,
						invalidEntities.Length, dbSet.GetType().Name));
                }			
            }

			using var connectionManager = new ConnectionManager(this.connection);
			using var transaction = this.connection.StartTransaction();
            foreach (IEnumerable dbSet in dbSets)
            {
				var dbSetType = dbSet.GetType().GetGenericArguments().First();
				var persistGenericMethod = typeof(DbContext)
					.GetMethod("Persist", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);

                try 
				{
					persistGenericMethod.Invoke(this, new object[] { dbSet });
				}
				catch(TargetInvocationException tie) // the invoked method itself throws an exception
                {
					throw tie.InnerException; // the actual exception, which occurred within the method invokation
                }
				catch(InvalidOperationException)
                {
					transaction.Rollback();
					throw;
                }
				catch(SqlException)
                {
					transaction.Rollback();
					throw;
                }		
            }

			transaction.Commit();
		}

		private void Persist<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class, new()
        {
			var tableName = this.GetTableName(typeof(TEntity));
			var columns = this.connection
				.FetchColumnNames(tableName)
				.ToArray();

			if (dbSet.ChangeTracker.Added.Any())
            {
				this.connection.InsertEntities(dbSet.ChangeTracker.Added, tableName, columns);
            }

			var modifiedEntities = dbSet.ChangeTracker
				.GetModifiedEntities(dbSet).ToArray();
			if(modifiedEntities.Any())
            {
				this.connection.UpdateEntities(modifiedEntities, tableName, columns);
            }

			if(dbSet.ChangeTracker.Removed.Any())
            {
				this.connection.DeleteEntities(dbSet.ChangeTracker.Removed, tableName, columns);
            }			
        }

		// Create generic method for populating each DbSet
		private void InitializeDbSets()
        {
            foreach (var dbSetKvp in this.dbSetProperties)
            {
				var dbSetType = dbSetKvp.Key;
				var dbSetProperty = dbSetKvp.Value;

				var populateDbSetGeneric = typeof(DbContext)
					.GetMethod("PopulateDbSet", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);

				populateDbSetGeneric.Invoke(this, new object[] { dbSetProperty });
            }
        }

		private void PopulateDbSet<TEntity>(PropertyInfo dbSetProperty)
			where TEntity : class, new()
        {
			var entityValues = this.LoadTableEntities<TEntity>(); // Load all data from the DB	
			var dbSetInstance = new DbSet<TEntity>(entityValues); // Create DbSet<TEntity> instance with loaded entites
			ReflectionHelper.ReplaceBackingField(this, dbSetProperty.Name, dbSetInstance); // Replace user-defined DbSet<TEntity> with DbSet<TEntity> populated with data
		}

		private void MapAllRelations()
        {
			foreach(var dbSetKvp in this.dbSetProperties)
            {
				var dbSetType = dbSetKvp.Key;

				var mapRelationsGeneric = typeof(DbContext)
					.GetMethod("MapRelations", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(dbSetType);

				var dbSet = dbSetKvp.Value.GetValue(this); // populated DbSet<TEntity>
				mapRelationsGeneric.Invoke(this, new object[] { dbSet });
            }
        }

		private void MapRelations<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class, new()
        {
			var entityType = typeof(TEntity);

			this.MapNavigationProperties(dbSet);

			var collections = entityType
				.GetProperties()
				.Where(pi =>
				    pi.PropertyType.IsGenericType &&
				    pi.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>))
				.ToArray();

            foreach (var collection in collections)
            {
				var collectionType = collection.PropertyType.GenericTypeArguments.First();

				var mapCollectionGeneric = typeof(DbContext)
					.GetMethod("MapCollection", BindingFlags.Instance | BindingFlags.NonPublic)
					.MakeGenericMethod(entityType, collectionType);

				mapCollectionGeneric.Invoke(this, new object[] { dbSet, collection });
            }
        }

		private void MapCollection<TEntity, TCollection>(DbSet<TEntity> dbSet, PropertyInfo collectionProperty)
			where TEntity : class, new()
			where TCollection : class, new()
        {
			var entityType = typeof(TEntity);
			var collectionType = typeof(TCollection);

			var referredEntityPrimaryKeys = collectionType.GetProperties()
				.Where(pi => pi.HasAttribute<KeyAttribute>())
				.ToArray();

			var primaryKey = referredEntityPrimaryKeys.First();
			var foreignKey = entityType.GetProperties()
				.First(pi => pi.HasAttribute<KeyAttribute>());

			bool isManyToMany = referredEntityPrimaryKeys.Length >= 2;
			if (isManyToMany)
            {
				primaryKey = collectionType.GetProperties()
					.First(pi => 
					 collectionType
	                    .GetProperty(pi.GetCustomAttribute<ForeignKeyAttribute>().Name)
				        .PropertyType == entityType);
            }

			var navigationDbSet = (DbSet<TCollection>) this.dbSetProperties[collectionType].GetValue(this);

            foreach (var entity in dbSet)
            {
				var primaryKeyValue = foreignKey.GetValue(entity);

				var navigationEntities = navigationDbSet
					.Where(ne => primaryKey.GetValue(ne).Equals(primaryKeyValue)) // foreignKey???
					.ToArray();

				ReflectionHelper.ReplaceBackingField(entity, collectionProperty.Name, navigationEntities);
            }
		}

		private void MapNavigationProperties<TEntity>(DbSet<TEntity> dbSet)
			where TEntity : class, new()
        {
			var entityType = typeof(TEntity);

			var foreignKeys = entityType.GetProperties()
				.Where(pi => pi.HasAttribute<ForeignKeyAttribute>())
				.ToArray();

            foreach (var foreignKey in foreignKeys)
            {
				string navigationPropName = foreignKey
					.GetCustomAttribute<ForeignKeyAttribute>().Name;
				var navProp = entityType.GetProperty(navigationPropName);

				var navDbSet = this.dbSetProperties[navProp.PropertyType].GetValue(this);

				var navPropPrimaryKey = navProp.PropertyType
					.GetProperties().First(pi => pi.HasAttribute<KeyAttribute>());

                foreach (var entity in dbSet)
                {
					var foreignKeyValue = foreignKey.GetValue(entity);
					var navPropValue = ((IEnumerable<object>)navDbSet)
						.First(currNavProp => navPropPrimaryKey.GetValue(currNavProp).Equals(foreignKeyValue));

					navProp.SetValue(entity, navPropValue);
                }
            }
        }

		private Dictionary<Type, PropertyInfo> DiscoverDbSets()
        {
			var dbSets = this.GetType()
				.GetProperties().Where(pi => pi.PropertyType.GetGenericTypeDefinition() == typeof(DbSet<>))
				.ToDictionary(pi => pi.PropertyType.GenericTypeArguments.First(), pi => pi);

			return dbSets;
        }

		private static bool IsObjectValid(object obj)
		{
			var validationContext = new ValidationContext(obj);
			var validationErrors = new List<ValidationResult>();

			var validationResult = Validator
				.TryValidateObject(obj, validationContext, validationErrors, true);

			return validationResult;
		}

		private IEnumerable<TEntity> LoadTableEntities<TEntity>()
			where TEntity : class
        {
			var entityType = typeof(TEntity);
			var columnNames = this.GetEntityColumnNames(entityType);
			var tableName = this.GetTableName(entityType);

			var fetchedRows = this.connection.FetchResultSet<TEntity>(tableName, columnNames);

			return fetchedRows;
        }

		private string GetTableName(Type entityType)
		{
			string tableName = ((TableAttribute)Attribute
				.GetCustomAttribute(entityType, typeof(TableAttribute))).Name;

			if (tableName == null)
            {
				tableName = this.dbSetProperties[entityType].Name;
            }

			return tableName;
		}

		private string[] GetEntityColumnNames(Type entityType)
        {
			string tableName = this.GetTableName(entityType);
			var dbColumns = this.connection.FetchColumnNames(tableName);

			var columns = entityType.GetProperties()
				.Where(pi => dbColumns.Contains(pi.Name) && 
				              !pi.HasAttribute<NotMappedAttribute>() &&
							  AllowedSqlTypes.Contains(pi.PropertyType))
				.Select(pi => pi.Name).ToArray();

			return columns;
		}
	}
}