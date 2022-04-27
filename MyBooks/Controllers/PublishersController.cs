using Microsoft.AspNetCore.Mvc;
using MyBooks.Data.Models;
using MyBooks.Data.Services;
using MyBooks.Data.ViewModels;
using MyBooks.Exceptions;

namespace MyBooks.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublishersController : ControllerBase
    {
        private readonly PublishersService? _publishersService;
        public PublishersController(PublishersService? publishersService)
        {
            _publishersService = publishersService;
        }

        [HttpPost("add-publisher")]
        public IActionResult? AddPublisher([FromBody] PublisherVm publisher)
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
        public ActionResult<Publisher> GetPublisherById(int id)
        {
            var _response = _publishersService.GetPublisherById(id);
            if (_response != null)
            {
                //return Ok(_response);
                return _response;
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
