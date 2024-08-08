using Microsoft.AspNetCore.Mvc;
using Moq;
using Xunit;
using BookStore.Repository;
using BookStore.Test.MockData;
using BookStore.Controllers;
using AutoMapper;
using Microsoft.Identity.Client;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
namespace BookStore.Test
{
    public class TestGenreController
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILogger<GenreController>> mockLogger;
        private readonly Mock<IBookStoreRepository> mockBookService;
        private readonly GenreController sut;

        public TestGenreController() {
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<GenreController>>();
            mockBookService = new Mock<IBookStoreRepository>();
            sut = new GenreController(mockBookService.Object, mockMapper.Object, mockLogger.Object);
        }
       
        [Fact]
        public async Task GetGenres_ShouldReturn200Ok()
        {

            //Arrange
           
           mockBookService
                .Setup(_=>_.GetGenresAsync())
                .ReturnsAsync(GenreMockData.GetGenres());
            //var Model = mapper.Map<Models.Genre>(mockBookService.Object);
            
           
            //Act
            var result = await sut.GetGenres();
            ///Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.Genre>>>(result);

            // If the result is OkObjectResult, we need to further assert the value
            if (actionResult.Result is OkObjectResult okResult)
            {
                var items = Assert.IsAssignableFrom<IEnumerable<Models.Genre>>(okResult.Value);

                
            }
            else
            {
                Assert.Fail("Expected result to be OkObjectResult but it was not.");
            }
            
        }
        [Fact]
        public async Task GetGenres_return404NotFound()
        {
            mockBookService
               .Setup(_ => _.GetGenresAsync())
               .ReturnsAsync(GenreMockData.GetGenresNull());
            //var Model = mapper.Map<Models.Genre>(mockBookService.Object);

            
            //Act
            var result = await sut.GetGenres();
            ///Assert
            var actionResult = Assert.IsType<ActionResult<IEnumerable<Models.Genre>>>(result);
            Assert.True(actionResult.Result is NotFoundObjectResult notFound);
            


        }
        [Fact]
        public async Task GetGenres_returnBadRequest()
        {
            mockBookService.Setup(_ => _.GetGenresAsync())
                      .ThrowsAsync(new InvalidOperationException("Simulated exception for testing."));

            // Act
            var result = await sut.GetGenres();

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<IEnumerable<Models.Genre>>>(result);
            Assert.True(badRequestResult.Result is BadRequestObjectResult badRequest);

        }
        [Fact]
        public async Task GetGenreById_ShouldReturnOk_WhenGenreExists()
        {
            // Arrange
            int genreId = 1;
            var genre = new Entities.Genre { genre_id = genreId, genre_name = "Fantasy" };
            var genreModel = new Models.Genre { genre_id = genreId, genre_name = "Fantasy" };

            mockBookService.Setup(_=>_.GetGenresByIdAsync(genreId))
                             .ReturnsAsync(genre);

            mockMapper.Setup(mapper => mapper.Map<Models.Genre>(genre))
                       .Returns(genreModel);

            // Act
            var result = await sut.getGenreById(genreId);

            // Assert
            var okResult = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(okResult.Result is OkObjectResult okobject);
        }
        [Fact]
        public async Task GetGenreById_ShouldReturnNotFound_WhenGenreDoesNotExist()
        {
            // Arrange
            int genreId = 1;
           
            mockBookService.Setup(_ => _.GetGenresByIdAsync(genreId))
                             .ReturnsAsync(GenreMockData.GetGenresByIdNull(genreId));

            // Act
            var result = await sut.getGenreById(genreId);

            // Assert
            var notFoundResult = Assert.IsType<ActionResult<Models.Genre>> (result);
            Assert.True(notFoundResult.Result is NotFoundObjectResult notFound);
           
        }
        [Fact]
        public async Task GetGenreById_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            int genreId = 1;
            mockBookService.Setup(_ => _.GetGenresByIdAsync(genreId))
                             .ThrowsAsync(new InvalidOperationException("Simulated exception for testing."));

            // Act
            var result = await sut.getGenreById(genreId);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(badRequestResult.Result is BadRequestObjectResult badRequestObject);
        }
        [Fact]
        public async Task CreateGenre_ShouldReturnBadRequest_WhenGenreIsNull()
        {
            // Arrange
            Models.Genre genre = null;

            // Act
            var result = await sut.CreateGenre(genre);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(badRequestResult.Result is BadRequestObjectResult badRequestObject);
        }
        [Fact]
        public async Task CreateGenre_ShouldReturnBadRequest_WhenCreationFails()
        {
            // Arrange
            var genreModel = new Models.Genre { genre_id = 1, genre_name = "Fantasy" };
            var genreEntity = new Entities.Genre { genre_id = 1, genre_name = "Fantasy" };

            mockMapper.Setup(mapper => mapper.Map<Entities.Genre>(genreModel))
                       .Returns(genreEntity);

            mockBookService.Setup(service => service.CreateGenre(genreEntity))
                             .Returns(Task.CompletedTask);

            mockBookService.Setup(service => service.SyncDb())
                             .ReturnsAsync(false); // Simulate creation failure

            // Act
            var result = await sut.CreateGenre(genreModel);

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(badRequestResult.Result is BadRequestObjectResult badRequestObject);
        }
        [Fact]
        public async Task CreateGenre_ShouldReturnCreated_WhenGenreIsSuccessfullyCreated()
        {
            // Arrange
            var genreModel = new Models.Genre { genre_id = 1, genre_name = "Fantasy" };
            var genreEntity = new Entities.Genre { genre_id = 1, genre_name = "Fantasy" };

            mockMapper.Setup(mapper => mapper.Map<Entities.Genre>(genreModel))
                       .Returns(genreEntity);

            mockBookService.Setup(service => service.CreateGenre(genreEntity))
                             .Returns(Task.CompletedTask);

            mockBookService.Setup(service => service.SyncDb())
                             .ReturnsAsync(true); // Simulate successful creation

            mockMapper.Setup(mapper => mapper.Map<Models.Genre>(genreEntity))
                       .Returns(genreModel);

            // Act
            var result = await sut.CreateGenre(genreModel);

            // Assert
            var createdAtRouteResult = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(createdAtRouteResult.Result is CreatedAtRouteResult created);
        }
        [Fact]
        public async Task CreateGenre_ShouldReturnBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var genreModel = new Models.Genre { genre_id = 1, genre_name = "Fantasy" };
            var genreEntity = new Entities.Genre { genre_id = 1, genre_name = "Fantasy" };

            mockMapper.Setup(mapper => mapper.Map<Entities.Genre>(genreModel))
                       .Returns(genreEntity);

            mockBookService.Setup(service => service.CreateGenre(genreEntity))
                             .ThrowsAsync(new InvalidOperationException("Simulated exception for testing."));

            // Act
            var result = await sut.CreateGenre(genreModel);

            // Assert
            var BadRequest = Assert.IsType<ActionResult<Models.Genre>>(result);
            Assert.True(BadRequest.Result is BadRequestObjectResult badRequest);
        }
    }
}

