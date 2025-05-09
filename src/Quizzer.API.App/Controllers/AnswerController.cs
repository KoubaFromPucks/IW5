using Microsoft.AspNetCore.Mvc;
using Quizzer.API.BL.Facades;
using Quizzer.Common.Models;

namespace Quizzer.API.Controllers; 
[ApiController]
[Route("api/[controller]")]
public class AnswerController : ControllerBase{
    private readonly AnswerFacade _facade;

    public AnswerController(AnswerFacade facade) {
        _facade = facade;
    }

    [HttpGet(Name = "[controller]/GetAll")]
    public IEnumerable<AnswerDetailModel> GetAll() {
        return _facade.GetAll();
    }

    [HttpGet("{id:guid}", Name = "[controller]/GetById")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(AnswerDetailModel))]
    public IActionResult GetById(Guid id) {
        AnswerDetailModel? answer = _facade.GetDetailById(id);

        if (answer is null) return NotFound(id);

        return Ok(answer);
    }

    [HttpGet("ByQuestion/{questionId:guid}", Name = "[controller]/GetForQuestion")]
    public IEnumerable<AnswerDetailModel> GetForQuestion(Guid questionId) {
        return _facade.GetAnswersForQuestion(questionId);
    }

    [HttpDelete("{id:guid}", Name = "[controller]/Delete")]
    [ProducesResponseType(StatusCodes.Status404NotFound, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    public IActionResult Delete(Guid id) {
        if (!_facade.Exists(id)) return NotFound(id);

        try {
            _facade.Delete(id);
        }catch(Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }

        return Ok(id);
    }

    [HttpPut("{questionId:guid}", Name = "[controller]/Update")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Update([FromBody] AnswerDetailModel answerDetailModel, Guid questionId) {
        if (!_facade.Exists(answerDetailModel.Id)) return BadRequest("Entity doesn't exist");

        Guid updatedId;
        try {
            updatedId = _facade.Save(answerDetailModel, questionId);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(updatedId);
    }

    [HttpPost("{questionId:guid}", Name = "[controller]/Create")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(Guid))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(string))]
    public IActionResult Create([FromBody] AnswerDetailModel answerDetailModel, Guid questionId) {
        if (_facade.Exists(answerDetailModel.Id)) return BadRequest("Entity already exists");
        
        Guid createdId;
        try {
            createdId = _facade.Save(answerDetailModel, questionId);
        } catch (Exception ex) {
            return BadRequest("\"" + ex.Message + "\"");
        }
        return Ok(createdId);
    }

    [HttpGet("ByText/{text}/{exact:bool}", Name = "[controller]/GetByText")]
    public IEnumerable<AnswerDetailModel> GetByText(string text, bool exact) {
        return _facade.GetAnswerByText(text, exact);
    }
}