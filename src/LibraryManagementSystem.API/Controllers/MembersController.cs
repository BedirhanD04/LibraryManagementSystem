using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.Business.Services;
using Microsoft.AspNetCore.Mvc;

namespace LibraryManagementSystem.API.Controllers;

[ApiController] 
[Route("api/[controller]")] 
public class MembersController : ControllerBase
{
    private readonly IMemberService _memberService;

    public MembersController(IMemberService memberService)
    {
        _memberService = memberService;
    }

    [HttpGet]
    public async Task<ActionResult<List<MemberDto>>> GetAll()
    {
        var members = await _memberService.GetAllAsync();
        return Ok(members); 
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MemberDto>> Get(int id)
    {
        var member = await _memberService.GetByIdAsync(id);
        if (member == null)
            return NotFound();
        
        return Ok(member);
    }

    [HttpPost]
    public async Task<ActionResult<MemberDto>> Create([FromBody] CreateMemberDto dto)
    {
        var created = await _memberService.CreateAsync(dto);
        return CreatedAtAction(nameof(Get), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<MemberDto>> Update(int id, [FromBody] CreateMemberDto dto)
    {
        var updated = await _memberService.UpdateAsync(id, dto);
        if (updated == null)
            return NotFound();
        
        return Ok(updated);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _memberService.DeleteAsync(id);
        if (!deleted)
            return NotFound();
        
        return NoContent(); // 204
    }
}