using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using ef_mssql_char_bug;
using Microsoft.EntityFrameworkCore;

TestcontainerDatabaseConfiguration containerConfig = new MsSqlTestcontainerConfiguration { Password = "yourStrong(!)Password" };
var dbContainer = new TestcontainersBuilder<MsSqlTestcontainer>()
  .WithDatabase(containerConfig)
  .Build();
await dbContainer.StartAsync().ConfigureAwait(false);



var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<SampleDbContext>(opt => opt.UseSqlServer(dbContainer.ConnectionString).UseLazyLoadingProxies());

var app = builder.Build();


//Migrate DB
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
  var context = serviceScope.ServiceProvider.GetRequiredService<SampleDbContext>();
  context.Database.Migrate();
  context.Database.EnsureCreated();
}


//Create
using (var serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
  var context = serviceScope.ServiceProvider.GetRequiredService<SampleDbContext>();

  OrderModel? order1 = new OrderModel
  {
    OrderId = "123456789",
    Info = "9 chars"
  };
  OrderModel? order2 = new OrderModel
  {
    OrderId = "0123456789",
    Info = "10 chars"
  };

  context.Order.Add(order1);
  context.Order.Add(order2);
  context.SaveChanges();

  OrderItemModel item1 = new OrderItemModel
  {
    OrderId = order1.OrderId,
    ItemName = "You-No-Poo",
    Quantity = 435345.0
  };
  OrderItemModel item2 = new OrderItemModel
  {
    OrderId = order1.OrderId,
    ItemName = "Portable swamp",
    Quantity = 1.2
  };
  context.OrderItem.Add(item1);
  context.OrderItem.Add(item2);
  context.SaveChanges();

  Console.WriteLine("Added models");


  await context.OrderItem.ForEachAsync(m => context.OrderItem.Remove(m));
  context.SaveChanges();

  await context.Order.ForEachAsync(m => context.Order.Remove(m));
  context.SaveChanges();

  Console.WriteLine("Removed models");

  foreach (var entry in context.ChangeTracker.Entries())
  {
    Console.WriteLine(entry);
  }

  context.Order.Remove(order1);
  context.SaveChanges();
}