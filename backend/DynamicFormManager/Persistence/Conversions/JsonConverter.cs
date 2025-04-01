using System.Text.Json;
using System.Text.Json.Nodes;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Persistence.Conversions;

public class JsonConverter : ValueConverter<JsonNode, string>
{
    public JsonConverter() : base(node => node.ToJsonString(JsonSerializerOptions.Default), json => JsonNode.Parse(json, new JsonNodeOptions(), new JsonDocumentOptions())!)
    {
    }
}