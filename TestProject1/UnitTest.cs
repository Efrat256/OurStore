using Entities;
using Moq;
using Moq.EntityFrameworkCore;
using Repositories;
using Services;
namespace TestOurStore;
using Microsoft.Extensions.Logging;

public class UnitTest
{
    [Fact]
    public async Task GetUser_ValidCredentials_ReturnsUser()
    {
        var user = new User { Email = "Efart@gmail.com", Password = "bg742tq7ubehuifuihfdshu" };
        var mockContext = new Mock<OurStoreContext>();
        var users = new List<User>() { user };
        mockContext.Setup(x => x.Users).ReturnsDbSet(users);
        var userRepository = new UserRepository(mockContext.Object);
        var result = await userRepository.login(user.Email, user.Password);

        Assert.Equal(user, result);
    }
    [Fact]
    public async Task GetUser_InvalidPassword_ReturnsNull()
    {
        // Arrange
        var user = new User { Email = "Efart@gmail.com", Password = "correctpassword" };
        var mockContext = new Mock<OurStoreContext>();
        var users = new List<User>() { user };
        mockContext.Setup(x => x.Users).ReturnsDbSet(users); // הגדרת המוק למסד הנתונים

        var userRepository = new UserRepository(mockContext.Object);
        var result = await userRepository.login(user.Email, "wrongpassword");  // סיסמה שגויה

        // Assert
        Assert.Null(result);  // אנחנו מצפים שהתשובה תהיה Null אם הסיסמה לא נכונה
    }
    [Fact]
    public async Task GetUser_InvalidEmail_ReturnsNull()
    {
        // Arrange
        var user = new User { Email = "Efart@gmail.com", Password = "bg742tq7ubehuifuihfdshu" };
        var mockContext = new Mock<OurStoreContext>();
        var users = new List<User>() { user };
        mockContext.Setup(x => x.Users).ReturnsDbSet(users); // הגדרת המוק למסד הנתונים

        var userRepository = new UserRepository(mockContext.Object);
        var result = await userRepository.login("nonexistentuser@gmail.com", "bg742tq7ubehuifuihfdshu");

        // Assert
        Assert.Null(result);  // אנחנו מצפים שהתשובה תהיה Null אם האימייל לא נמצא
    }

    [Fact]
    public async Task AddOrder_OrderSumMatches_ReturnsOrder()
    {
        // Arrange
        var order = new Order
        {
            OrderSum = 100,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            }
        };

        var _orderRepositoryMock = new Mock<IOrderRepository>();
        var _productsRepositoryMock = new Mock<IProductsRepository>();
        var _loggerMock = new Mock<ILogger<OrderService>>();
        var _orderService = new OrderService(_orderRepositoryMock.Object, _productsRepositoryMock.Object, _loggerMock.Object);
        _productsRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(new Product { Price = 50 });
        _productsRepositoryMock.Setup(r => r.GetProductById(2)).ReturnsAsync(new Product { Price = 50 });
        _orderRepositoryMock.Setup(r => r.addOrder(order)).ReturnsAsync(order);

        // Act
        var result = await _orderService.addOrder(order);

        // Assert
        Assert.Equal(100, result.OrderSum);
    }

    [Fact]
    public async Task AddOrder_OrderSumDoesNotMatch_FixesOrderSum()
    {
        // Arrange
        var order = new Order
        {
            OrderSum = 90,
            OrderItems = new List<OrderItem>
            {
                new OrderItem { ProductId = 1 },
                new OrderItem { ProductId = 2 }
            }
        };
        var _orderRepositoryMock = new Mock<IOrderRepository>();
        var _productsRepositoryMock = new Mock<IProductsRepository>();
        var _loggerMock = new Mock<ILogger<OrderService>>();
        var _orderService = new OrderService(_orderRepositoryMock.Object, _productsRepositoryMock.Object, _loggerMock.Object);
        _productsRepositoryMock.Setup(r => r.GetProductById(1)).ReturnsAsync(new Product { Price = 50 });
        _productsRepositoryMock.Setup(r => r.GetProductById(2)).ReturnsAsync(new Product { Price = 50 });
        _orderRepositoryMock.Setup(r => r.addOrder(order)).ReturnsAsync(order);

        // Act
        var result = await _orderService.addOrder(order);

        // Assert
        Assert.Equal(100, result.OrderSum);
        _loggerMock.Verify(
            x => x.Log(
                It.Is<LogLevel>(l => l == LogLevel.Critical),
                It.IsAny<EventId>(),
                It.Is<It.IsAnyType>((v, t) => v.ToString().Contains("The orderSum is not equals to the original sum.")),  // שגיאה אפשרית: אם ההודעה לא תואמת בדיוק את מה שנשלח ל-logger, זה יגרום לשגיאה
                It.IsAny<Exception>(),
                It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
            Times.Once);
    }
}
