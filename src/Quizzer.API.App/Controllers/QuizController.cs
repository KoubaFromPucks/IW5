using Microsoft.AspNetCore.Mvc;
using Quizzer.API.BL.Facades;
using Quizzer.Common.Models;

namespace Quizzer.API.Controllers; 
[ApiController]
[Route("api/[controller]")]
public class QuizController : ControllerBase {
    private readonly QuizFacade _facade;

    public QuizController(QuizFacade facade) {
        _facade = facade;
    }

    [HttpGet(Name = "[controller]/GetAll")]
    public IEnumerable<QuizDetailModel> GetAll() {
        return _facade.GetAll();
    }

    [HttpGet("AllForUser/{userId:guid}", Name = "[controller]/GetAllForUser")]
    public IEnumerable<QuizDetailModel> GetAllForUser(Guid userId) {
        return _facade.GetAllForUser(userId);
    }

    [HttpGet("{id:guid}", Name = "[controller]/GetById")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizDetailModel))]
    public IActionResult GetById(Guid id) {
        QuizDetailModel? model = _facade.GetDetailById(id);

        if (model is null) return NotFound(id);

        return Ok(model);
    }

    [HttpPut(Name = "[controller]/Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Update([FromBody] QuizDetailModel quizDetailModel) {
        if (!_facade.Exists(quizDetailModel.Id)) return BadRequest("Entity doesn't exist");

        Guid updatedId;
        try {
            updatedId = _facade.Save(quizDetailModel, DateTime.Now);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(updatedId);
    }

    [HttpPost(Name = "[controller]/Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Create([FromBody] QuizDetailModel quizDetailModel) {
        if (_facade.Exists(quizDetailModel.Id)) return BadRequest("Entity already exists");

        Guid createdId;
        try {
            createdId = _facade.Save(quizDetailModel, DateTime.Now);
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

    [HttpGet("ByContent/{name}/{description},{exact:bool}", Name = "[controller]/GetByContent")]
    public IEnumerable<QuizListModel> GetByContent(string name, string description, bool exact) {
        return _facade.GetQuizByContent(name, description, exact);
    }

    [HttpGet("Score/{quizId:guid}/{userId:guid}", Name = "[controller]/GetQuizScore")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(double))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult GetQuizScore(Guid quizId, Guid userId) {
        var score = _facade.GetQuizScore(quizId, userId);

        if (score is double.NaN) return BadRequest("Could not get score.");

        return Ok(score);
    }

    [HttpPatch("Start/{quizId:guid}/{userId:guid}", Name = "[controller]/StartQuiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public IActionResult StartQuiz(Guid quizId, Guid userId) {
        try {
            _facade.StartQuiz(userId, quizId, DateTime.Now);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(quizId);
    }

    [HttpPatch("Complete/{quizId:guid}/{userId:guid}", Name = "[controller]/CompleteQuiz")]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public IActionResult CompleteQuiz(Guid quizId, Guid userId) {
        try {
            _facade.CompleteQuiz(quizId, userId);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(quizId);
    }

    [HttpGet("Result/{quizId:guid}/{userId:guid}", Name = "[controller]/GetResult")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuizResultDetailModel))]
    public IActionResult GetResult(Guid quizId, Guid userId) {
        try {
            QuizResultDetailModel quizResultDetailModel = _facade.GetResultModel(quizId, userId, DateTime.Now);
            return Ok(quizResultDetailModel);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
    }
}