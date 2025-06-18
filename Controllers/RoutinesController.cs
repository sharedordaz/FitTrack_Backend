using Microsoft.AspNetCore.Mvc;
using Backend.Data;
using Backend.Models;

namespace Backend.Controllers
{
  [ApiController]
  [Route("api/[controller]")]
  public class RoutinesController : ControllerBase
  {
    private readonly FitTrackContext _context;

    public RoutinesController(FitTrackContext context)
    {
      _context = context;
    }

    [HttpGet]
    public IActionResult GetAllRoutines()
    {
      var routines = _context.Routines.ToList();
      return Ok(routines);
    }

    [HttpGet("{id}")]
    public IActionResult GetRoutineById(int id)
    {
      var routine = _context.Routines.Find(id);
      if (routine == null)
      {
        return NotFound();
      }
      return Ok(routine);
    }

    [HttpPost]
    public IActionResult CreateRoutine([FromBody] Routine routine)
    {
      if (!ModelState.IsValid)
      {
        return BadRequest(ModelState);
      }

      _context.Routines.Add(routine);
      _context.SaveChanges();
      return CreatedAtAction(nameof(GetRoutineById), new { id = routine.Id }, routine);
    }

    [HttpPut("{id}")]
    public IActionResult UpdateRoutine(int id, [FromBody] Routine updatedRoutine)
    {
      if (id != updatedRoutine.Id)
      {
        return BadRequest("Routine ID mismatch.");
      }

      var routine = _context.Routines.Find(id);
      if (routine == null)
      {
        return NotFound();
      }

      routine.Value = updatedRoutine.Value;
      routine.Unit = updatedRoutine.Unit;
      routine.Repetitions = updatedRoutine.Repetitions;
      routine.Date = updatedRoutine.Date;
      routine.Type = updatedRoutine.Type;

      _context.SaveChanges();
      return NoContent();
    }

    [HttpDelete("{id}")]
    public IActionResult DeleteRoutine(int id)
    {
      var routine = _context.Routines.Find(id);
      if (routine == null)
      {
        return NotFound();
      }

      _context.Routines.Remove(routine);
      _context.SaveChanges();
      return NoContent();
    }
  }
}