using System.Text.Json.Nodes;
using Persistence.Base;

namespace Persistence.Entities;

public class Post : BaseEntity
{
    public required JsonNode Node { get; init; }
}