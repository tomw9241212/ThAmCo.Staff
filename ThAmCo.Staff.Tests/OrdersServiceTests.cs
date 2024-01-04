using Microsoft.Extensions.Configuration;
using Moq;
using Moq.Protected;
using Newtonsoft.Json;
using System.Net;
using System.Text;
using ThAmCo.Staff.Models;
using ThAmCo.Staff.Services;

namespace ThAmCo.Staff.Tests {
    public class OrdersServiceTests {

        private Mock<HttpMessageHandler> CreateTokenHttpMock() {
            var mockResponse = new HttpResponseMessage(HttpStatusCode.OK) {
                Content = new StringContent(JsonConvert.SerializeObject(new {
                    access_token = "mocked_access_token",
                    token_type = "Bearer",
                    expires_in = 3600
                }), Encoding.UTF8, "application/json")
            };

            var mock = new Mock<HttpMessageHandler>();
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.Is<HttpRequestMessage>(req => req.RequestUri.ToString().Contains("/oauth/token")),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(mockResponse)
                .Verifiable();

            return mock;
        }


        private Mock<HttpMessageHandler> CreateHttpMock(HttpStatusCode expectedCode, string expectedJson) {
            var response = new HttpResponseMessage {
                StatusCode = expectedCode
            };
            if (expectedJson != null) {
                response.Content = new StringContent(expectedJson,
                                                     Encoding.UTF8,
                                                     "application/json");
            }
            var mock = new Mock<HttpMessageHandler>(MockBehavior.Strict);
            mock.Protected()
                .Setup<Task<HttpResponseMessage>>(
                    "SendAsync",
                    ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(response)
                .Verifiable();
            return mock;
        }

        private IOrdersService CreateOrdersService(Mock<HttpMessageHandler> httpMock, Mock<HttpMessageHandler> tokenHttpMock) {
            var mockFactory = new Mock<IHttpClientFactory>();
            var orderClient = new HttpClient(httpMock.Object) { BaseAddress = new Uri("https://localhost:7153/") };
            var tokenClient = new HttpClient(tokenHttpMock.Object) { BaseAddress = new Uri("https://dev-watchmychops.uk.auth0.com") };
            var mockConfiguration = new Mock<IConfiguration>();
            mockConfiguration.Setup(c => c["WebServices:Orders:BaseAddress"]).Returns("https://localhost:7153/");
            mockFactory.Setup(f => f.CreateClient("OrdersClient")).Returns(orderClient);
            mockFactory.Setup(f => f.CreateClient("TokenClient")).Returns(tokenClient);
            return new OrdersService(mockFactory.Object, mockConfiguration.Object);
        }

        [Fact]
        public async Task GetOrdersAsync_WithValid_ShouldOkEntities() {

            // Arrange
            var expectedResult = new List<OrderGetDto> {
                new OrderGetDto {
                    Id = 1,
                    CustomerId = 1,
                    SubmittedDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail> {
                        new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                    }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await service.GetOrdersAsync();

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.First().Id, result.First().Id);
            Assert.Equal(expectedResult.First().CustomerId, result.First().CustomerId);
            Assert.Equal(expectedResult.First().SubmittedDate, result.First().SubmittedDate);
            Assert.Equal(expectedResult.First().OrderDetails.First().ProductId, result.First().OrderDetails.First().ProductId);
            Assert.Equal(expectedResult.First().OrderDetails.First().Quantity, result.First().OrderDetails.First().Quantity);
            Assert.Equal(expectedResult.First().OrderDetails.First().UnitPrice, result.First().OrderDetails.First().UnitPrice);

            httpMock.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get
                      && req.RequestUri == new Uri("https://localhost:7153/api/Orders")),
                   ItExpr.IsAny<CancellationToken>());
        }

        [Fact]
        public async Task GetOrdersAsync_WithInvalid_ShouldThrowException() {
            // Arrange
            var expectedResult = new List<OrderGetDto> {
                new OrderGetDto {
                    Id = 1,
                    CustomerId = 1,
                    SubmittedDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail> {
                        new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                    }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.BadRequest, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await Assert.ThrowsAsync<HttpRequestException>(service.GetOrdersAsync);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Response status code does not indicate success: 400 (Bad Request).", result.Message);

            httpMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(req => req.Method == HttpMethod.Get
                    && req.RequestUri == new Uri("https://localhost:7153/api/Orders")),
                ItExpr.IsAny<CancellationToken>());
        }


        [Fact]
        public async Task GetOrderAsync_WithValid_ShouldOkEntity() {
            // Arrange
            var expectedResult = new OrderGetDto {
                Id = 1,
                CustomerId = 1,
                SubmittedDate = DateTime.Now,
                OrderDetails = new List<OrderDetail> {
                    new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await service.GetOrderAsync(1);

            // Assert
            Assert.NotNull(result);
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Id, result.Id);
            Assert.Equal(expectedResult.CustomerId, result.CustomerId);
            Assert.Equal(expectedResult.SubmittedDate, result.SubmittedDate);
            Assert.Equal(expectedResult.OrderDetails.First().ProductId, result.OrderDetails.First().ProductId);
            Assert.Equal(expectedResult.OrderDetails.First().Quantity, result.OrderDetails.First().Quantity);
            Assert.Equal(expectedResult.OrderDetails.First().UnitPrice, result.OrderDetails.First().UnitPrice);

            httpMock.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(
                        req => req.Method == HttpMethod.Get
                               && req.RequestUri == new Uri("https://localhost:7153/api/Orders/1")),
                    ItExpr.IsAny<CancellationToken>());
        }

        // A test for GetOrderAsync with an invalid id
        [Fact]
        public async Task GetOrderAsync_WithInvalid_ShouldThrowException() {
            // Arrange
            var expectedResult = new OrderGetDto {
                Id = 1,
                CustomerId = 1,
                SubmittedDate = DateTime.Now,
                OrderDetails = new List<OrderDetail> {
                    new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.BadRequest, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetOrderAsync(1));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Response status code does not indicate success: 400 (Bad Request).", result.Message);

            httpMock.Protected().Verify(
                "SendAsync",
                Times.Once(),
                ItExpr.Is<HttpRequestMessage>(
                        req => req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri("https://localhost:7153/api/Orders/1")),
                ItExpr.IsAny<CancellationToken>());
        }

        // A test for GetOrderAsync with a non-existent id
        [Fact]
        public async Task GetOrderAsync_WithNonExistent_ShouldReturnNull() {
            // Arrange
            var expectedResult = new OrderGetDto {
                Id = 1,
                CustomerId = 1,
                SubmittedDate = DateTime.Now,
                OrderDetails = new List<OrderDetail> {
                    new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.NotFound, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await service.GetOrderAsync(1);

            // Assert
            Assert.Null(result);

            httpMock.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(
                        req => req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri("https://localhost:7153/api/Orders/1")),
                    ItExpr.IsAny<CancellationToken>());
        }

        // Test for GetOrdersByStatusAsync with a valid status
        [Fact]
        public async Task GetOrdersByStatusAsync_WithValid_ShouldOkEntities() {
            // Arrange
            var expectedResult = new List<OrderGetDto> {
                new OrderGetDto {
                    Id = 1,
                    CustomerId = 1,
                    Status = OrderStatus.Confirmed,
                    SubmittedDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail> {
                        new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                    }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.OK, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await service.GetOrdersByStatusAsync(OrderStatus.Confirmed);

            // Assert
            Assert.NotNull(result);
            Assert.Equal(expectedResult.Count, result.Count);
            Assert.Equal(expectedResult.First().Id, result.First().Id);
            Assert.Equal(expectedResult.First().CustomerId, result.First().CustomerId);
            Assert.Equal(expectedResult.First().SubmittedDate, result.First().SubmittedDate);
            Assert.Equal(expectedResult.First().OrderDetails.First().ProductId, result.First().OrderDetails.First().ProductId);
            Assert.Equal(expectedResult.First().OrderDetails.First().Quantity, result.First().OrderDetails.First().Quantity);
            Assert.Equal(expectedResult.First().OrderDetails.First().UnitPrice, result.First().OrderDetails.First().UnitPrice);

            httpMock.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(
                        req => req.Method == HttpMethod.Get
                        && req.RequestUri == new Uri("https://localhost:7153/api/Orders?orderStatus=Confirmed")),
                    ItExpr.IsAny<CancellationToken>());
        }

        // Test a GetOrdersByStatusAsync with an invalid status
        [Fact]
        public async Task GetOrdersByStatusAsync_WithInvalid_ShouldThrowException() {
            // Arrange
            var expectedResult = new List<OrderGetDto> {
                new OrderGetDto {
                    Id = 1,
                    CustomerId = 1,
                    Status = OrderStatus.Confirmed,
                    SubmittedDate = DateTime.Now,
                    OrderDetails = new List<OrderDetail> {
                        new OrderDetail { OrderId = 1, ProductId = 1, Quantity = 10, UnitPrice = 0.05f }
                    }
                }
            };
            var expectedJson = JsonConvert.SerializeObject(expectedResult);
            var httpMock = CreateHttpMock(HttpStatusCode.BadRequest, expectedJson);
            var tokenHttpMock = CreateTokenHttpMock();
            var service = CreateOrdersService(httpMock, tokenHttpMock);

            // Act
            var result = await Assert.ThrowsAsync<HttpRequestException>(() => service.GetOrdersByStatusAsync(OrderStatus.Confirmed));

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Response status code does not indicate success: 400 (Bad Request).", result.Message);

            httpMock.Protected().Verify(
                    "SendAsync",
                    Times.Once(),
                    ItExpr.Is<HttpRequestMessage>(
                               req => req.Method == HttpMethod.Get
                               && req.RequestUri == new Uri("https://localhost:7153/api/Orders?orderStatus=Confirmed")),
                    ItExpr.IsAny<CancellationToken>());
        }

    }
}