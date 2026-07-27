// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Platform.Null.NullMultiplayerName
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Platform.Null;

public record NullMultiplayerName()
{
  [JsonPropertyName("net_id")]
  public ulong netId;
  [JsonPropertyName("name")]
  public string name;

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("netId = ");
    builder.Append(this.netId.ToString());
    builder.Append(", name = ");
    builder.Append((object) this.name);
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    return (EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.netId)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.name);
  }

  [CompilerGenerated]
  public virtual bool Equals(NullMultiplayerName? other)
  {
    if ((object) this == (object) other)
      return true;
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<ulong>.Default.Equals(this.netId, other.netId) && EqualityComparer<string>.Default.Equals(this.name, other.name);
  }

  [CompilerGenerated]
  protected NullMultiplayerName(NullMultiplayerName original)
  {
    this.netId = original.netId;
    this.name = original.name;
  }
}
