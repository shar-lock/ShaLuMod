// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Modding.ModManifest
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Saves;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Modding;

public record ModManifest()
{
  [JsonPropertyName("id")]
  public string? id;
  [JsonPropertyName("name")]
  public string? name;
  [JsonPropertyName("author")]
  public string? author;
  [JsonPropertyName("description")]
  public string? description;
  [JsonPropertyName("version")]
  public string? version;
  [JsonPropertyName("has_pck")]
  public bool hasPck;
  [JsonPropertyName("has_dll")]
  public bool hasDll;
  [JsonPropertyName("dependencies")]
  public List<ModDependency>? dependencies;
  [JsonPropertyName("affects_gameplay")]
  public bool affectsGameplay;
  [JsonPropertyName("min_game_version")]
  public string? minGameVersion;

  public static ModManifest? ReadFromStream(Stream stream, out List<LocString>? errors)
  {
    errors = (List<LocString>) null;
    JsonNode jsonNode1 = JsonNode.Parse(stream, new JsonNodeOptions?(), new JsonDocumentOptions());
    if (jsonNode1 == null)
      return (ModManifest) null;
    JsonNode jsonNode2 = jsonNode1["dependencies"];
    if (jsonNode2 != null && jsonNode2.GetValueKind() == 2)
    {
      JsonNode jsonNode3 = ((IEnumerable<JsonNode>) jsonNode2.AsArray()).FirstOrDefault<JsonNode>();
      if ((jsonNode3 != null ? (jsonNode3.GetValueKind() == 3 ? 1 : 0) : 0) != 0)
      {
        Log.Error("Detected old-style dependencies without min version specified! It works for now but this will be removed in a future release. Let the mod author know.");
        LocString locString = new LocString("main_menu_ui", "MOD_ERROR.MIGRATION_REQUIRED");
        locString.Add("id", jsonNode1["id"]?.GetValue<string>() ?? "<null>");
        if (errors == null)
          errors = new List<LocString>();
        errors.Add(locString);
        JsonArray jsonArray = new JsonArray(new JsonNodeOptions?());
        foreach (JsonNode jsonNode4 in jsonNode2.AsArray())
        {
          if (jsonNode4 != null)
          {
            // ISSUE: object of a compiler-generated type is created
            jsonArray.Add<JsonObject>(new JsonObject((IEnumerable<KeyValuePair<string, JsonNode>>) new \u003C\u003Ez__ReadOnlyArray<KeyValuePair<string, JsonNode>>(new KeyValuePair<string, JsonNode>[2]
            {
              new KeyValuePair<string, JsonNode>("id", JsonNode.op_Implicit(jsonNode4.GetValue<string>())),
              new KeyValuePair<string, JsonNode>("min_version", (JsonNode) null)
            }), new JsonNodeOptions?()));
          }
        }
        jsonNode1["dependencies"] = (JsonNode) jsonArray;
      }
    }
    return JsonSerializer.Deserialize<ModManifest>(jsonNode1, JsonSerializationUtility.GetTypeInfo<ModManifest>());
  }

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("id = ");
    builder.Append((object) this.id);
    builder.Append(", name = ");
    builder.Append((object) this.name);
    builder.Append(", author = ");
    builder.Append((object) this.author);
    builder.Append(", description = ");
    builder.Append((object) this.description);
    builder.Append(", version = ");
    builder.Append((object) this.version);
    builder.Append(", hasPck = ");
    builder.Append(this.hasPck.ToString());
    builder.Append(", hasDll = ");
    builder.Append(this.hasDll.ToString());
    builder.Append(", dependencies = ");
    builder.Append((object) this.dependencies);
    builder.Append(", affectsGameplay = ");
    builder.Append(this.affectsGameplay.ToString());
    builder.Append(", minGameVersion = ");
    builder.Append((object) this.minGameVersion);
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    return (((((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.id)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.name)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.author)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.description)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.version)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.hasPck)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.hasDll)) * -1521134295 + EqualityComparer<List<ModDependency>>.Default.GetHashCode(this.dependencies)) * -1521134295 + EqualityComparer<bool>.Default.GetHashCode(this.affectsGameplay)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.minGameVersion);
  }

  [CompilerGenerated]
  public virtual bool Equals(ModManifest? other)
  {
    if ((object) this == (object) other)
      return true;
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<string>.Default.Equals(this.id, other.id) && EqualityComparer<string>.Default.Equals(this.name, other.name) && EqualityComparer<string>.Default.Equals(this.author, other.author) && EqualityComparer<string>.Default.Equals(this.description, other.description) && EqualityComparer<string>.Default.Equals(this.version, other.version) && EqualityComparer<bool>.Default.Equals(this.hasPck, other.hasPck) && EqualityComparer<bool>.Default.Equals(this.hasDll, other.hasDll) && EqualityComparer<List<ModDependency>>.Default.Equals(this.dependencies, other.dependencies) && EqualityComparer<bool>.Default.Equals(this.affectsGameplay, other.affectsGameplay) && EqualityComparer<string>.Default.Equals(this.minGameVersion, other.minGameVersion);
  }

  [CompilerGenerated]
  protected ModManifest(ModManifest original)
  {
    this.id = original.id;
    this.name = original.name;
    this.author = original.author;
    this.description = original.description;
    this.version = original.version;
    this.hasPck = original.hasPck;
    this.hasDll = original.hasDll;
    this.dependencies = original.dependencies;
    this.affectsGameplay = original.affectsGameplay;
    this.minGameVersion = original.minGameVersion;
  }
}
