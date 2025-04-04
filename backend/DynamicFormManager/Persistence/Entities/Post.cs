﻿using System.Text.Json.Nodes;
using Persistence.Base;

namespace Persistence.Entities;

public class Post : BaseEntity
{
    public required JsonNode Node { get; init; }
    public required string SearchText { get; init; }
    public required Guid UserId { get; init; }
    public User? User { get; init; }
}