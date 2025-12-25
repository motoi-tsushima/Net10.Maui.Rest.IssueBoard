using Shared.Rest.IssueBoard;
using System.Net.Http.Json;

namespace Net10.Maui.Rest.IssueBoard.Services;

public class IssueService
{
    private readonly HttpClient _httpClient;
    private const string ApiPath = "api/issues";

    public IssueService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<IssueDto>> GetAllIssuesAsync()
    {
        try
        {
            var issues = await _httpClient.GetFromJsonAsync<List<IssueDto>>(ApiPath);
            return issues ?? new List<IssueDto>();
        }
        catch (Exception ex)
        {
            throw new Exception("â€ëËÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩ", ex);
        }
    }

    public async Task<IssueDto?> GetIssueAsync(int id)
    {
        try
        {
            return await _httpClient.GetFromJsonAsync<IssueDto>($"{ApiPath}/{id}");
        }
        catch (Exception ex)
        {
            throw new Exception("â€ëËÇÃéÊìæÇ…é∏îsÇµÇ‹ÇµÇΩ", ex);
        }
    }

    public async Task<IssueDto> CreateIssueAsync(CreateIssueDto dto)
    {
        try
        {
            var response = await _httpClient.PostAsJsonAsync(ApiPath, dto);
            response.EnsureSuccessStatusCode();
            var issue = await response.Content.ReadFromJsonAsync<IssueDto>();
            return issue ?? throw new Exception("â€ëËÇÃçÏê¨Ç…é∏îsÇµÇ‹ÇµÇΩ");
        }
        catch (Exception ex)
        {
            throw new Exception("â€ëËÇÃçÏê¨Ç…é∏îsÇµÇ‹ÇµÇΩ", ex);
        }
    }

    public async Task UpdateIssueAsync(int id, UpdateIssueDto dto)
    {
        try
        {
            var response = await _httpClient.PutAsJsonAsync($"{ApiPath}/{id}", dto);
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("â€ëËÇÃçXêVÇ…é∏îsÇµÇ‹ÇµÇΩ", ex);
        }
    }

    public async Task DeleteIssueAsync(int id)
    {
        try
        {
            var response = await _httpClient.DeleteAsync($"{ApiPath}/{id}");
            response.EnsureSuccessStatusCode();
        }
        catch (Exception ex)
        {
            throw new Exception("â€ëËÇÃçÌèúÇ…é∏îsÇµÇ‹ÇµÇΩ", ex);
        }
    }
}
