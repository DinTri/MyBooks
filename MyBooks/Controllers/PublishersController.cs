using Microsoft.AspNetCore.Mvc;
using MyBooks.Data.Services;
using MyBooks.Data.ViewModels;
using MyBooks.Exceptions;

namespace MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly PublishersService _publishersService;
        private readonly ILogger<PublishersController> _logger;
        public PublishersController(PublishersService publishersService, ILogger<PublishersController> logger)
        {
            _publishersService = publishersService;
            _logger = logger;
        }
        [HttpGet("get-all-publishers")]
        public IActionResult GetAllPublishers(string sortBy, string searchString, int pageSize)
        {
            try
            {
                _logger.LogInformation("This is a log in GetAllPublishers");
                var _result = _publishersService.GetAllPublishers(sortBy, searchString, pageSize);
                return Ok(_result);
            }
            catch (Exception)
            {

                return BadRequest("Sorry we could not load the pblishers");
            }
        }
        [HttpPost("add-publisher")]
        public IActionResult AddPublisher([FromBody] PublisherVm publisher)
        {
            try
            {
                var newPublisher = _publishersService.AddPublisher(publisher);
                return Created(nameof(AddPublisher), newPublisher);
            }
            catch (PublisherNameException ex)
            {
                return BadRequest($"{ex.Message}, Publisher Name {ex.PublisherName}");
            }
            catch (Exception ex)
            {

                BadRequest(ex.Message);
            }
            return null;

        }

        [HttpGet("get-publisher-by-id/{id}")]
        public IActionResult GetPublisherById(int id)
        {
            var _response = _publishersService.GetPublisherById(id);
            if (_response != null)
            {
                return Ok(_response);
            }
            else
            {
                return NotFound();
            }

        }

        [HttpGet("get-publishers-with-authors/{id}")]
        public IActionResult GetPublisherData(int id)
        {
            var _response = _publishersService.GetPublisherData(id);
            return Ok(_response);
        }
        [HttpDelete("delete-publisher-by-id/{id}")]
        public IActionResult DeletePbulisherById(int id)
        {
            try
            {
                _publishersService.DeletePbulisherById(id);
                return Ok();
            }
            catch (Exception ex)
            {

                BadRequest(ex.Message);
            }
            return BadRequest();
        }
    }
}
