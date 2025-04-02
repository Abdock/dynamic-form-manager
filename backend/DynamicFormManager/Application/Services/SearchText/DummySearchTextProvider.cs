using System.Buffers.Text;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;

namespace Application.Services.SearchText;

public class DummySearchTextProvider : ISearchTextProvider
{
    public string GetSearchText(JsonNode node)
    {
        if (node.GetValueKind() == JsonValueKind.String)
        {
            return node.GetValue<string>();
        }

        var searchTextBuilder = new StringBuilder();
        if (node.GetValueKind() == JsonValueKind.Array)
        {
            foreach (var child in node.AsArray())
            {
                if (child is null)
                {
                    continue;
                }

                var value = GetSearchText(child);
                searchTextBuilder.AppendLine(value);
            }
        }
        else if (node.GetValueKind() == JsonValueKind.Object)
        {
            foreach (var (_, child) in node.AsObject())
            {
                if (child is null)
                {
                    continue;
                }

                var value = GetSearchText(child);
                searchTextBuilder.AppendLine(value);
            }
        }

        return searchTextBuilder.ToString();
    }
}