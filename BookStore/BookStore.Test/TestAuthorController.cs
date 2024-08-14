using AutoMapper;
using BookStore.Controllers;
using BookStore.Entities;
using BookStore.Models;
using BookStore.Repository;
using BookStore.Test.MockData;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookStore.Test
{
    public class TestAuthorController
    {
        private readonly Mock<IMapper> mockMapper;
        private readonly Mock<ILogger<AuthorController>> mockLogger;
        private readonly Mock<IBookStoreRepository> mockBookService;
        private readonly AuthorController sut;

        public TestAuthorController()
        {
            mockMapper = new Mock<IMapper>();
            mockLogger = new Mock<ILogger<AuthorController>>();
            mockBookService = new Mock<IBookStoreRepository>();
            sut = new AuthorController(mockBookService.Object, mockMapper.Object, mockLogger.Object);
        }
        [Fact]
        public async Task GetAuthors_ReturnsOkResponse_WithAuthors()
        {
            // Arrange
            var authorsFromDb = new List<Entities.Author> // Assuming AuthorDbModel is the DB model
            {
                new Entities.Author { author_id = 1, name = "Author 1", author_image = new byte[] { } },
                new Entities.Author { author_id = 2, name = "Author 2", author_image = new byte[] { } }
            };

            var authors = new List<Models.Author>
            {
                new Models.Author { author_id = 1, name = "Author 1", author_image = new byte[] { } },
                new Models.Author { author_id = 2, name = "Author 2", author_image = new byte[] { } }
            };

            mockBookService.Setup(_ => _.GetAuthorAsync()).ReturnsAsync(authorsFromDb);
            mockMapper.Setup(mapper => mapper.Map<IEnumerable<Models.Author>>(authorsFromDb)).Returns(authors);

            // Act
            var result = await sut.GetAuthors();

            // Assert
            if (result.Result is OkObjectResult okResult)
            {
                var items = Assert.IsAssignableFrom<IEnumerable<Models.Author>>(okResult.Value);


            }
            else
            {
                Assert.Fail("Expected result to be OkObjectResult but it was not.");
            }

        }
        [Fact]
        public async Task GetAuthors_ReturnsNotFound_WhenNoAuthors()
        {
            mockBookService.Setup(_ => _.GetAuthorAsync()).ReturnsAsync(GenreMockData.GetAuthors());
            //mockMapper.Setup(mapper => mapper.Map<IEnumerable<Models.Author>>(GenreMockData.GetAuthors())).Returns(authors);

            // Act
            var result = await sut.GetAuthors();

            // Assert
            var NotFound = Assert.IsType<ActionResult<IEnumerable<Models.Author>>>(result);
            Assert.True(NotFound.Result is NotFoundObjectResult notFound);
        }
        [Fact]
        public async Task GetAuthors_returnBadRequest()
        {
            mockBookService.Setup(_ => _.GetAuthorAsync())
                      .ThrowsAsync(new InvalidOperationException("Simulated exception for testing."));

            // Act
            var result = await sut.GetAuthors();

            // Assert
            var badRequestResult = Assert.IsType<ActionResult<IEnumerable<Models.Author>>>(result);
            Assert.True(badRequestResult.Result is BadRequestObjectResult badRequest);

        }
        [Fact]
        public async Task GetAuthors_ReturnsOkResult_WithAuthor()
        {
            // Arrange
            var authorId = 1;
            var authorFromDb = new Entities.Author { author_id = 1, name = "Author 1", author_image = new byte[] { } };
            var authorDto = new Models.Author { author_id = 1, name = "Author 1", author_image = new byte[] { } };

            mockBookService.Setup(_ => _.GetAuthorByIdAsync(authorId))
                         .ReturnsAsync(authorFromDb);
            mockMapper.Setup(m => m.Map<Models.Author>(authorFromDb))
                      .Returns(authorDto);

            // Act
            var result = await sut.getAuthors(authorId);

            // Assert
            if (result.Result is OkObjectResult okResult)
            {
                var items = Assert.IsAssignableFrom<Models.Author>(okResult.Value);


            }
            else
            {
                Assert.Fail("Expected result to be OkObjectResult but it was not.");
            }

        }
        [Fact]
        public async Task GetAuthors_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 1;
            mockBookService.Setup(b => b.GetAuthorByIdAsync(authorId))
                         .ReturnsAsync(GenreMockData.GetAuthorsByIdNull(authorId));

            // Act
            var result = await sut.getAuthors(authorId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
            Assert.True(actionResult.Result is NotFoundObjectResult notfound);


        }
        [Fact]
        public async Task GetAuthors_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var authorId = 1;
            mockBookService.Setup(_ => _.GetAuthorByIdAsync(authorId))
                         .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.getAuthors(authorId);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
            Assert.True(actionResult.Result is BadRequestObjectResult badRequest);
        }
        [Fact]
        public async Task CreateAuthor_ReturnsCreatedAtRouteResult_WhenAuthorIsAddedSuccessfully()
        {
            // Arrange
            var authorModel = new Models.UpdateAuthor
            {
                name = "John Doe",
                biography = "An interesting author",
                author_image = new FormFile(new MemoryStream(new byte[0]), 0, 0, "name", "file.jpg")
            };
            var authorEntity = new Entities.Author
            {
                name = authorModel.name,
                biography = authorModel.biography,
                author_image = new byte[0] // Simulate image conversion
            };
            var authorDto = new Models.Author
            {
                author_id = 1,
                name = authorModel.name,
                biography = authorModel.biography
            };

            mockBookService.Setup(_ => _.CreateAuthor(authorEntity))
                         .Returns(Task.CompletedTask);
            mockBookService.Setup(_ => _.SyncDb())
                         .ReturnsAsync(true);
            mockMapper.Setup(m => m.Map<Models.Author>(It.IsAny<Entities.Author>()))
                      .Returns(authorDto);

            // Act
            var result = await sut.CreateAuthor(authorModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
           Assert.True(actionResult.Result is  CreatedAtRouteResult action);
           

        }
        [Fact]
        public async Task CreateAuthor_ReturnsOkResult_WhenAuthorAlreadyExists()
        {
            // Arrange
            var authorModel = new Models.UpdateAuthor
            {
                name = "John Doe",
                biography = "An interesting author"
            };
            var authorEntity = new Entities.Author
            {
                name = authorModel.name,
                biography = authorModel.biography
            };

            mockBookService.Setup(b => b.CreateAuthor(authorEntity))
                         .Returns(Task.CompletedTask);
            mockBookService.Setup(b => b.SyncDb())
                         .ReturnsAsync(false);

            // Act
            var result = await sut.CreateAuthor(authorModel);

            // Assert
           
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
           
            Assert.True(actionResult.Result is OkObjectResult okobject);
            //Assert.Equal("Author already Exists", actualResult);
            

          
        }
        [Fact]
        public async Task CreateAuthor_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var authorModel = new Models.UpdateAuthor
            {
                name = "John Doe",
                biography = "An interesting author"
            };

            mockBookService.Setup(b => b.CreateAuthor(It.IsAny<Entities.Author>()))
                         .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.CreateAuthor(authorModel);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
            Assert.True(actionResult.Result is BadRequestObjectResult badRequest);
        }
        [Fact]
        public async Task DeleteAuthor_ReturnsNoContent_WhenDeletionIsSuccessful()
        {
            // Arrange
            var authorId = 1;
            mockBookService.Setup(b => b.DeleteAuthorAsync(authorId))
                         .Returns(Task.CompletedTask);
            mockBookService.Setup(b => b.SyncDb())
                         .ReturnsAsync(true);

            // Act
            var result = await sut.DeleteAuthor(authorId);


            // Assert
            var actionResult = Assert.IsType<NoContentResult>(result);
            //mockLogger.Verify(log => log.LogWarning($"The Author with id '{authorId}' is deleted"), Times.Once);
        }
        [Fact]
        public async Task DeleteAuthor_ReturnsStatusCode405_WhenAuthorHasBooks()
        {
            // Arrange
            var authorId = 1;
            mockBookService.Setup(b => b.DeleteAuthorAsync(authorId))
                         .Returns(Task.CompletedTask);
            mockBookService.Setup(b => b.SyncDb())
                         .ReturnsAsync(false);

            // Act
            var result = await sut.DeleteAuthor(authorId);

            // Assert
            var actionResult = Assert.IsType<ObjectResult>(result);
            Assert.Equal(405, actionResult.StatusCode);
        }
        [Fact]
        public async Task DeleteAuthor_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var authorId = 1;
            mockBookService.Setup(b => b.DeleteAuthorAsync(authorId))
                         .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.DeleteAuthor(authorId);

            // Assert
            var actionResult = Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Database error", actionResult.Value);
        }
        [Fact]
        public async Task UpdateAuthor_ReturnsOk_WhenUpdateIsSuccessful()
        {
            // Arrange
            var authorId = 1;
            var updateAuthor = new UpdateAuthor
            {
                name = "Updated Name",
                biography = "Updated Biography",
                author_image = new FormFile(new MemoryStream(new byte[] { 1, 2, 3 }), 0, 3, "name", "file.jpg"),
                UpdatedAt = DateTime.UtcNow
            };
            var authorCurrent = new Entities.Author
            {
                author_id = authorId,
                name = "Original Name",
                biography = "Original Biography",
                author_image = new byte[0]
            };
            var updatedAuthor = new Entities.Author
            {
                author_id = authorId,
                name = updateAuthor.name,
                biography = updateAuthor.biography,
                author_image = new byte[] { 1, 2, 3 },
                UpdatedAt = updateAuthor.UpdatedAt
            };
            var authorDto = new Models.Author
            {
                author_id = authorId,
                name = updateAuthor.name,
                biography = updateAuthor.biography
            };

            mockBookService.Setup(b => b.GetAuthorByIdAsync(authorId))
                         .ReturnsAsync(authorCurrent);
            mockBookService.Setup(b => b.SyncDb())
                         .ReturnsAsync(true);
            mockMapper.Setup(m => m.Map<Models.Author>(updatedAuthor))
                      .Returns(authorDto);

            // Act
            var result = await sut.UpdateAuthor(authorId, updateAuthor);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author?>>(result);
            Assert.True(actionResult.Result is OkObjectResult objectResult);
            }

        [Fact]
        public async Task UpdateAuthor_ReturnsNotFound_WhenAuthorDoesNotExist()
        {
            // Arrange
            var authorId = 1;
            var updateAuthor = new UpdateAuthor
            {
                name = "Updated Name",
                biography = "Updated Biography"
            };
            mockBookService.Setup(b => b.GetAuthorByIdAsync(authorId))
                         .ReturnsAsync((Entities.Author)null);

            // Act
            var result = await sut.UpdateAuthor(authorId, updateAuthor);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
            Assert.True(actionResult.Result is NotFoundObjectResult notfoundobject);
        }
        [Fact]
        public async Task UpdateAuthor_ReturnsBadRequest_WhenExceptionIsThrown()
        {
            // Arrange
            var authorId = 1;
            var updateAuthor = new UpdateAuthor
            {
                name = "Updated Name",
                biography = "Updated Biography"
            };
            mockBookService.Setup(b => b.GetAuthorByIdAsync(authorId))
                         .ThrowsAsync(new Exception("Database error"));

            // Act
            var result = await sut.UpdateAuthor(authorId, updateAuthor);

            // Assert
            var actionResult = Assert.IsType<ActionResult<Models.Author>>(result);
            Assert.True(actionResult.Result is BadRequestObjectResult badobject);
        }
    }
}


