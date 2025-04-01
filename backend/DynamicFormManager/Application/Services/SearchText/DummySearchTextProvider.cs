using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Application.Services.SearchText;

public class DummySearchTextProvider : ISearchTextProvider
{
    public string GetSearchText(JsonNode node)
    {
        var searchTextBuilder = new StringBuilder();
        foreach (var (_, value) in node.AsObject())
        {
            if (value is null)
            {
                continue;
            }

            if (value.GetValueKind() != JsonValueKind.String)
            {
                continue;
            }

            var propertyValue = value.GetValue<string>();
            if (Base64.IsValid(propertyValue))
            {
                continue;
            }

            searchTextBuilder.AppendLine(propertyValue);
        }

        return searchTextBuilder.ToString();
    }
}