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
}