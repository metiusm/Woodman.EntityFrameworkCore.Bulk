# Woodman.EntityFrameworkCore.Bulk #

Entity Framework bulk transactions written for .NET Standard 2.0

Woodman.EntityFrameworkCore.Bulk provides a free set of extensions that provide a quick, simple way to perform bulk transactions
against an EntityFramwork DbContext. Currently, it supports SqlServer, NgpSql, and InMemory Providers.

````
   Install-Package Woodman.EntityFrameworkCore.Bulk
````

NOTE: Version 1 Requires a Primary Key. Support for Composite Primary Keys may come later.


#### Bulk Join ####

By default, EF will generate a WHERE IN command when you pass a list of IDs into a WHERE clause.

Wraps the SQL generated by the IQueryable, and add a JOIN against the primary key.

````
using(var dbContext = new DbContext(c))
{
   var ids = new int[] { 123, 1234, 12345 };
   
   var queryable = dbContext.Entity.Join(ids);
   
   var entities = await queryable.Entity.ToListAsync();
}
````

#### Bulk Remove ####

IQueryable.BulkRemoveAsnc(IEnumerable<id> id)

Similar to the Join Operation above, wraps the EF and performs a single statement.

````
using(var dbContext = new DbContext(c))
{
   var ids = new int[] { 123, 1234, 12345 };
	
   int numDeleted = await dbContext.Entity.BulkRemoveAsync(ids);
}
````

#### Bulk Add ####

````
using(var dbContext = new DbContext(c))
{
   var entities = new List<Entity>();

   entites.Add(new Entity { Name = "E1" };
   entites.Add(new Entity { Name = "E2" };
   
   // sets the PrimaryKey if generated by the DB
   await dbContext.Entity.BulkAddAsync(entities);
}
````

#### Bulk Update ####
````
using(var dbContext = new DbContext(c))
{
   // update a set of records using a constructor expression

   await dbContext.Entity.BulkUpdateAsync(() => new Entity
   {
      UpdatedDate = DateTime.UtcNow
   });
   
   // optionally pass in a set of ids to update records using the corresponding id
	
   var ids = new int[] { 123, 1234, 12345 };
	
   await dbContext.Entity.BulkUpdateAsync(ids, id => new Entity
   {
      UpdatedDate = id === 123 ? DateTime.UtcNow : DateTime.UtcNow.AddDays(1)
   });
}
````

#### Bulk Merge ####
````
using(var dbContext = new DbContext(c))
{
   var entities = new List<Entity>();

   entites.Add(new Entity { Id = 1, Name = "E1" };
   entites.Add(new Entity { Name = "E2" };
   
    // sets the PrimaryKey if generated by the DB
   var numRowsAffected = await dbContext.Entity.BulkMergeAsync(entities);
}
````
