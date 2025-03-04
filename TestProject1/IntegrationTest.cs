using System.Net;
using System.Net.Http.Json;
using TestOurStore;
using Repositories;
using Entities;
using Services;
using Microsoft.Extensions.Logging;
using Moq;

namespace TestOurStore;
public class IntegrationTests : IClassFixture<DatabaseFixture>
{
    private readonly OrderService _service;
    private readonly OurStoreContext _context;
    private readonly Mock<ILogger<OrderService>> _loggerMock;
    private readonly UserRepository _userRepository;

    public IntegrationTests(DatabaseFixture fixture)
    {
        _loggerMock = new Mock<ILogger<OrderService>>();
        _context = fixture.Context;
        _service = new OrderService(new OrderRepository(_context), new ProductsRepository(_context), _loggerMock.Object);
        _userRepository = new UserRepository(_context);
    }
    [Fact]
    public async Task LogIn_ValidCredentials_ReturnsUser()
    {
        var user = new User { Email = "test@example.com", Password = "pass123@" ,FirstName="Efart"};
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userRepository = new UserRepository(_context);

        // Act
        var retrievedUser = await userRepository.login(user.Email,user.Password);

        // Assert
        Assert.NotNull(retrievedUser);
        Assert.Equal(user.Email, retrievedUser.Email);
    }

    [Fact]
    public async Task LogIn_InvalidPassword_ReturnsNoContent()
    {
        var user = new User { Email = "test@example.com", Password = "pass123@", FirstName = "Test" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        var userRepository = new UserRepository(_context);

        // Act
        var retrievedUser = await userRepository.login(user.Email, "wrongpassword");

        // Assert
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task LogIn_NonExistentUser_ReturnsNoContent()
    {
        var userRepository = new UserRepository(_context);

        // Act
        var retrievedUser = await userRepository.login("nonexistent@example.com", "somepassword");

        // Assert
        Assert.Null(retrievedUser);
    }

    [Fact]
    public async Task Post_ShouldAddUser_WhenUserIsValid()
    {
        // Arrange
        var user = new User { Email = "test@example.com", Password = "password123", FirstName = "John", LastName = "Doe" };

        // Act
        // var addedUser = await _context.Users.AddAsync(user);
        var addedUser = await _userRepository.addUser(user);


        //await _context.SaveChangesAsync();

        // Assert
        Assert.NotNull(addedUser);
        Assert.Equal(user.Email, addedUser.Email);
        Assert.True(addedUser.Id > 0); // נניח שהמזהה יוקצה אוטומטית
    }


    [Fact]
    public async Task Post_ShouldSaveOrder_WithCorrectTotalAmount()
    {
        // Arrange: יצירת קטגוריה עבור המוצרים (כי יש Foreign Key)
        var category = new Category { CategoryName = "Electronics" };
        _context.Categories.Add(category);
        await _context.SaveChangesAsync();

        // יצירת מוצרים עם קטגוריה תקפה
        var product1 = new Product { ProductName = "Laptop", Price = 10, Image = "laptop.jpg", Category = category ,Description="Test"};
        var product2 = new Product { ProductName = "Phone", Price = 20, Image = "phone.jpg", Category = category, Description = "Test" };
        var product3 = new Product { ProductName = "Tablet", Price = 15, Image = "tablet.jpg", Category = category , Description = "Test" };

        _context.Products.AddRange(product1, product2, product3);
        await _context.SaveChangesAsync();

        // יצירת משתמש (כי UserId חובה בהזמנה)
        var user = new User { Email = "test@example.com", Password = "password123", FirstName = "John", LastName = "Doe" };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();

        // יצירת הזמנה עם מוצרים תקפים ו-UserId
        var order = new Order
        {
            OrderDate = DateTime.UtcNow,
            OrderSum = 0,  // יחושב ע"י CheckSum
            UserId = user.Id,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = product1.ProductId, Quantity = 1 },
                new OrderItem { ProductId = product2.ProductId, Quantity = 1 },
                new OrderItem { ProductId = product3.ProductId, Quantity = 1 }
            }
        };

        // Act: שליחת ההזמנה לפונקציה הנבדקת
        var savedOrder = await _service.addOrder(order);

        // Assert: בדיקת תקינות הנתונים
        Assert.NotNull(savedOrder);
        Assert.Equal(45, savedOrder.OrderSum); // 10.5 + 20.0 + 15.75
        Assert.Equal(3, savedOrder.OrderItems.Count); // 3 מוצרים בהזמנה
        Assert.Equal(user.Id, savedOrder.UserId); // בדיקה שהמשתמש משויך להזמנה
    }

}
    