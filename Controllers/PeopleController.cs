//using DotnetApiPostgres.Api.Mappings;
//using DotnetApiPostgres.Api.Models.DTOs;
//using DotnetApiPostgres.Api.Models.Entities;
//using DotnetApiPostgres.Api.Repository;
//using DotnetApiPostgres.Api.Services;
//using Microsoft.AspNetCore.Mvc;

//namespace DotnetApiPostgres.Api.Models;

//[ApiController]
//[Route("api/[controller]/[action]")]
//public class PeopleController : ControllerBase
//{
//    private readonly IPersonService _personService;
//    private readonly IPostgresRepository<Person, int> _repository;
//    private readonly ILogger<PeopleController> _logger;

//    public PeopleController(IPersonService personService, IPostgresRepository<Person, int> repository, ILogger<PeopleController> logger)
//    {
//        _personService = personService;
//        _repository = repository;
//        _logger = logger;
//    }

//    [HttpPost]
//    public async Task<IActionResult> AddPersonAsync(CreatePersonDTO personToCreate)
//    {
//        try
//        {
//            var person = await _personService.AddPersonAsync(personToCreate);
//            return Ok(person);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex.Message);
//            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//        }
//    }

//    [HttpPut("{id}")]
//    public async Task<IActionResult> UpdatePersonAsync(int id, UpdatePersonDTO personToUpdate)
//    {
//        if (id != personToUpdate.Id)
//        {
//            return BadRequest($"id in parameter and id in body is different. id in parameter: {id}, id in body: {personToUpdate.Id}");
//        }
//        try
//        {
//            GetPersonDTO? person = await _personService.FindPersonByIdAsync(id);
//            if (person == null)
//            {
//                return NotFound();
//            }
//            await _personService.UpdatePersonAsync(personToUpdate);
//            return NoContent();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex.Message);
//            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//        }
//    }

//    [HttpGet("{id}")]
//    public async Task<IActionResult> GetPersonByIdAsync(int id)
//    {
//        try
//        {
//            GetPersonDTO? person = await _personService.FindPersonByIdAsync(id);
//            if (person == null)
//            {
//                return NotFound();
//            }
//            return Ok(person);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex.Message);
//            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//        }
//    }

//    [HttpGet]
//    public async Task<IActionResult> GetPeopleAsync()
//    {
//        try
//        {
//            IEnumerable<GetPersonDTO> peoples = await _personService.GetPeopleAsync();
//            return Ok(peoples);
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex.Message);
//            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//        }
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeleteByIdAsync(int id)
//    {
//        try
//        {
//            GetPersonDTO? person = await _personService.FindPersonByIdAsync(id);
//            if (person == null)
//            {
//                return NotFound();
//            }
//            await _personService.DeletePersonAsync(GetPersonDTO.ToPerson(person));
//            return NoContent();
//        }
//        catch (Exception ex)
//        {
//            _logger.LogError(ex.Message);
//            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
//        }
//    }

//    // V2

//    [HttpPost]
//    public async Task<ActionResult<GetPersonDTO>> AddPersonV2(CreatePersonDTO personToCreate)
//    {
//        var person = personToCreate.ToPerson();
//        person = await _repository.AddAsync(person);
//        return Ok(person.ToGetPersonDto());
//    }

//    [HttpGet("{id}")]
//    public async Task<ActionResult<GetPersonDTO?>> GetPersonV2(int id)
//    {
//        var person = await _repository.FindByIdAsync(id);
//        if (person == null)
//            return NotFound();
//        return Ok(person.ToGetPersonDto());
//    }

//    [HttpGet]
//    public async Task<ActionResult<IEnumerable<GetPersonDTO>>> GetAllPeopleV2()
//    {
//        var people = await _repository.GetAllAsync();
//        return Ok(people.Select(p => p.ToGetPersonDto()));
//    }

//    [HttpPut]
//    public async Task<IActionResult> UpdatePersonV2(UpdatePersonDTO personToUpdate)
//    {
//        var person = personToUpdate.ToPerson();
//        await _repository.UpdateAsync(person);
//        return NoContent();
//    }

//    [HttpDelete("{id}")]
//    public async Task<IActionResult> DeletePersonV2(int id)
//    {
//        var person = await _repository.FindByIdAsync(id);
//        if (person == null)
//            return NotFound();

//        await _repository.DeleteAsync(person);
//        return NoContent();
//    }
//}