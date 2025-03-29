using System.Text.Json.Nodes;

namespace Application.Services.SearchText;

public interface ISearchTextProvider
{
    string GetSearchText(JsonNode node);
}