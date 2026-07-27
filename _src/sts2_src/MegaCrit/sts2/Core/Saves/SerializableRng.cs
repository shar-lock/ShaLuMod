// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Saves.SerializableRng
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Multiplayer.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.Json.Serialization;

#nullable enable
namespace MegaCrit.Sts2.Core.Saves;

public record SerializableRng() : IPacketSerializable
{
  [JsonPropertyName("counter")]
  public int counter;
  [JsonPropertyName("s0")]
  public ulong state0;
  [JsonPropertyName("s1")]
  public ulong state1;
  [JsonPropertyName("s2")]
  public ulong state2;
  [JsonPropertyName("s3")]
  public ulong state3;

  public void Serialize(PacketWriter writer)
  {
    writer.WriteInt(this.counter);
    writer.WriteULong(this.state0);
    writer.WriteULong(this.state1);
    writer.WriteULong(this.state2);
    writer.WriteULong(this.state3);
  }

  public void Deserialize(PacketReader reader)
  {
    this.counter = reader.ReadInt();
    this.state0 = reader.ReadULong();
    this.state1 = reader.ReadULong();
    this.state2 = reader.ReadULong();
    this.state3 = reader.ReadULong();
  }

  public override string ToString()
  {
    return $"Counter: {this.counter} State: {this.state0} {this.state1} {this.state2} {this.state3}";
  }

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("counter = ");
    builder.Append(this.counter.ToString());
    builder.Append(", state0 = ");
    builder.Append(this.state0.ToString());
    builder.Append(", state1 = ");
    builder.Append(this.state1.ToString());
    builder.Append(", state2 = ");
    builder.Append(this.state2.ToString());
    builder.Append(", state3 = ");
    builder.Append(this.state3.ToString());
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    return ((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<int>.Default.GetHashCode(this.counter)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state0)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state1)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state2)) * -1521134295 + EqualityComparer<ulong>.Default.GetHashCode(this.state3);
  }

  [CompilerGenerated]
  public virtual bool Equals(SerializableRng? other)
  {
    if ((object) this == (object) other)
      return true;
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<int>.Default.Equals(this.counter, other.counter) && EqualityComparer<ulong>.Default.Equals(this.state0, other.state0) && EqualityComparer<ulong>.Default.Equals(this.state1, other.state1) && EqualityComparer<ulong>.Default.Equals(this.state2, other.state2) && EqualityComparer<ulong>.Default.Equals(this.state3, other.state3);
  }

  [CompilerGenerated]
  protected SerializableRng(SerializableRng original)
  {
    this.counter = original.counter;
    this.state0 = original.state0;
    this.state1 = original.state1;
    this.state2 = original.state2;
    this.state3 = original.state3;
  }
}
