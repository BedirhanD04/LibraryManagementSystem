using LibraryManagementSystem.Business.DTOs;
using LibraryManagementSystem.DataAccess.Repositories;
using LibraryManagementSystem.Entities.Models;

namespace LibraryManagementSystem.Business.Services;

public class MemberService : IMemberService
{
    private readonly IRepository<Member> _memberRepository;

    public MemberService(IRepository<Member> memberRepository)
    {
        _memberRepository = memberRepository;
    }

    public async Task<List<MemberDto>> GetAllAsync()
    {
        var members = await _memberRepository.GetAllAsync();
        return members.Select(MapToDto).ToList();
    }

    public async Task<MemberDto?> GetByIdAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        return member == null ? null : MapToDto(member);
    }

    public async Task<MemberDto> CreateAsync(CreateMemberDto createMemberDto)
    {
        var member = new Member
        {
            Name = createMemberDto.Name,
            Email = createMemberDto.Email,
            MembershipDate = DateTime.UtcNow
        };

        await _memberRepository.AddAsync(member);
        await _memberRepository.SaveChangesAsync(); 

        return MapToDto(member);
    }

    public async Task<MemberDto?> UpdateAsync(int id, CreateMemberDto dto)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return null;

        member.Name = dto.Name;
        member.Email = dto.Email;

        _memberRepository.Update(member);
        await _memberRepository.SaveChangesAsync();

        return MapToDto(member);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var member = await _memberRepository.GetByIdAsync(id);
        if (member == null) return false;

        _memberRepository.Remove(member);
        await _memberRepository.SaveChangesAsync();

        return true;
    }

    // Centralizing Entity -> DTO mapping in one place to prevent code duplication
    private static MemberDto MapToDto(Member member) => new()
    {
        Id = member.Id,
        Name = member.Name,
        Email = member.Email,
        MembershipDate = member.MembershipDate
    };
}