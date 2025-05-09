using Microsoft.AspNetCore.Mvc;
using Quizzer.API.BL.Facades;
using Quizzer.Common.Models;

namespace Quizzer.API.Controllers; 
[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase {
    private readonly UserFacade _facade;

    public UserController(UserFacade facade) {
        _facade = facade;
    }

    [HttpGet(Name = "[controller]/GetAll")]
    public IEnumerable<UserDetailModel> GetAll() {
        return _facade.GetAll();
    }

    [HttpGet("{id:guid}", Name = "[controller]/GetById")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(UserDetailModel))]
    public IActionResult GetById(Guid id) {
        UserDetailModel? userDetailModel = _facade.GetDetailById(id);

        if (userDetailModel is null) return NotFound(id);

        return Ok(userDetailModel);
    }

    [HttpPut(Name = "[controller]/Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Update([FromBody] UserDetailModel userDetailModel) {
        if (!_facade.Exists(userDetailModel.Id)) return BadRequest("Entity doesn't exist");

        Guid updatedId;
        try {
            updatedId = _facade.Save(userDetailModel);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(updatedId);
    }

    [HttpPost(Name = "[controller]/Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Create([FromBody] UserDetailModel userDetailModel) {
        if (_facade.Exists(userDetailModel.Id)) return BadRequest("Entity already exists");

        Guid createdId;
        try {
            createdId = _facade.Save(userDetailModel);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(createdId);
    }

    [HttpDelete("{id:guid}", Name = "[controller]/Delete")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public IActionResult Delete(Guid id) {
        if (!_facade.Exists(id)) return NotFound(id);

        _facade.Delete(id);
        return Ok(id);
    }

    [HttpGet("ByName/{name}/{exact:bool}", Name = "[controller]/GetByName")]
    public IEnumerable<UserListModel> GetByName(string name, bool exact) {
        return _facade.GetByName(name, exact);
    }
}