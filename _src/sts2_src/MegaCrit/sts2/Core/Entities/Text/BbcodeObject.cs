// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Text.BbcodeObject
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Text;

public record BbcodeObject()
{
  public BbcodeObjectType type;
  public string? text;
  public string? tag;

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("type = ");
    builder.Append(this.type.ToString());
    builder.Append(", text = ");
    builder.Append((object) this.text);
    builder.Append(", tag = ");
    builder.Append((object) this.tag);
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    return ((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<BbcodeObjectType>.Default.GetHashCode(this.type)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.text)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.tag);
  }

  [CompilerGenerated]
  public virtual bool Equals(BbcodeObject? other)
  {
    if ((object) this == (object) other)
      return true;
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<BbcodeObjectType>.Default.Equals(this.type, other.type) && EqualityComparer<string>.Default.Equals(this.text, other.text) && EqualityComparer<string>.Default.Equals(this.tag, other.tag);
  }

  [CompilerGenerated]
  protected BbcodeObject(BbcodeObject original)
  {
    this.type = original.type;
    this.text = original.text;
    this.tag = original.tag;
  }
}
