using Microsoft.AspNetCore.Mvc;
using Quizzer.API.BL.Facades;
using Quizzer.Common.Models;

namespace Quizzer.API.Controllers; 
[ApiController]
[Route("api/[controller]")]
public class QuestionController : ControllerBase {
    private readonly QuestionFacade _facade;

    public QuestionController(QuestionFacade facade) {
        _facade = facade;
    }

    [HttpGet(Name = "[controller]/GetAll")]
    public IEnumerable<QuestionDetailModel> GetAll() {
        return _facade.GetAll();
    }

    [HttpGet("{id:guid}", Name = "[controller]/GetById")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(QuestionDetailModel))]
    public IActionResult GetById(Guid id) {
        QuestionDetailModel? questionDetailModel = _facade.GetDetailById(id);

        if (questionDetailModel is null) return NotFound(id);

        return Ok(questionDetailModel);
    }

    [HttpPut("{quizId:guid}", Name = "[controller]/Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Update([FromBody] QuestionDetailModel questionDetailModel, Guid quizId) {
        if (!_facade.Exists(questionDetailModel.Id)) return BadRequest("Entity doesn't exist");

        Guid updatedId;
        try {
            updatedId = _facade.Save(questionDetailModel, quizId, DateTime.Now);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(updatedId);
    }

    [HttpPost("{quizId:guid}", Name = "[controller]/Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Create([FromBody] QuestionDetailModel questionDetailModel, Guid quizId) {
        if (_facade.Exists(questionDetailModel.Id)) return BadRequest("Entity already exists");

        Guid createdId;
        try {
            createdId = _facade.Save(questionDetailModel, quizId, DateTime.Now);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(createdId);
    }

    [HttpDelete("{id:guid}", Name = "[controller]/Delete")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Delete(Guid id) {
        if (!_facade.Exists(id)) return NotFound(id);

        try {
            _facade.Delete(id);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }

        return Ok(id);
    }

    [HttpGet("ByName/{name}/{exact:bool}", Name = "[controller]/GetByName")]
    public IEnumerable<QuestionDetailModel> GetByName(string name, bool exact) {
        return _facade.GetQuestionByName(name, exact);
    }

    [HttpGet("ResultsForUser/{quizId:guid}/{userId:guid}", Name = "[controller]/GetResultsForUser")]
    public IEnumerable<QuestionResultModel> GetResultsForUser(Guid quizId, Guid userId) {
        return _facade.GetResultsForUser(quizId, userId);
    }

    [HttpPost("AnswerQuestion/{answerId:guid}/{userId:guid}/{order:int}", Name = "[controller]/AnswerQuestion")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult AnswerQuestion(Guid answerId, Guid userId, int order) {
        try {
            _facade.AnswerQuestion(answerId, userId, DateTime.Now, order);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(answerId);
    }
}