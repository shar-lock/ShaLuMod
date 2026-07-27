// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Multiplayer.ConnectionFailureExtraInfo
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Platform;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Multiplayer;

public record ConnectionFailureExtraInfo()
{
  public List<string>? missingModsOnLocal;
  public List<string>? missingModsOnHost;
  public string? hostVersion;
  public PlatformBranch? hostBranch;
  public ulong? hostHash;
  public string? localVersion;
  public PlatformBranch? localBranch;
  public ulong? localHash;

  [CompilerGenerated]
  protected virtual bool PrintMembers(StringBuilder builder)
  {
    RuntimeHelpers.EnsureSufficientExecutionStack();
    builder.Append("missingModsOnLocal = ");
    builder.Append((object) this.missingModsOnLocal);
    builder.Append(", missingModsOnHost = ");
    builder.Append((object) this.missingModsOnHost);
    builder.Append(", hostVersion = ");
    builder.Append((object) this.hostVersion);
    builder.Append(", hostBranch = ");
    builder.Append(this.hostBranch.ToString());
    builder.Append(", hostHash = ");
    builder.Append(this.hostHash.ToString());
    builder.Append(", localVersion = ");
    builder.Append((object) this.localVersion);
    builder.Append(", localBranch = ");
    builder.Append(this.localBranch.ToString());
    builder.Append(", localHash = ");
    builder.Append(this.localHash.ToString());
    return true;
  }

  [CompilerGenerated]
  public override int GetHashCode()
  {
    return (((((((EqualityComparer<Type>.Default.GetHashCode(this.EqualityContract) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.missingModsOnLocal)) * -1521134295 + EqualityComparer<List<string>>.Default.GetHashCode(this.missingModsOnHost)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.hostVersion)) * -1521134295 + EqualityComparer<PlatformBranch?>.Default.GetHashCode(this.hostBranch)) * -1521134295 + EqualityComparer<ulong?>.Default.GetHashCode(this.hostHash)) * -1521134295 + EqualityComparer<string>.Default.GetHashCode(this.localVersion)) * -1521134295 + EqualityComparer<PlatformBranch?>.Default.GetHashCode(this.localBranch)) * -1521134295 + EqualityComparer<ulong?>.Default.GetHashCode(this.localHash);
  }

  [CompilerGenerated]
  public virtual bool Equals(ConnectionFailureExtraInfo? other)
  {
    if ((object) this == (object) other)
      return true;
    return (object) other != null && this.EqualityContract == other.EqualityContract && EqualityComparer<List<string>>.Default.Equals(this.missingModsOnLocal, other.missingModsOnLocal) && EqualityComparer<List<string>>.Default.Equals(this.missingModsOnHost, other.missingModsOnHost) && EqualityComparer<string>.Default.Equals(this.hostVersion, other.hostVersion) && EqualityComparer<PlatformBranch?>.Default.Equals(this.hostBranch, other.hostBranch) && EqualityComparer<ulong?>.Default.Equals(this.hostHash, other.hostHash) && EqualityComparer<string>.Default.Equals(this.localVersion, other.localVersion) && EqualityComparer<PlatformBranch?>.Default.Equals(this.localBranch, other.localBranch) && EqualityComparer<ulong?>.Default.Equals(this.localHash, other.localHash);
  }

  [CompilerGenerated]
  protected ConnectionFailureExtraInfo(ConnectionFailureExtraInfo original)
  {
    this.missingModsOnLocal = original.missingModsOnLocal;
    this.missingModsOnHost = original.missingModsOnHost;
    this.hostVersion = original.hostVersion;
    this.hostBranch = original.hostBranch;
    this.hostHash = original.hostHash;
    this.localVersion = original.localVersion;
    this.localBranch = original.localBranch;
    this.localHash = original.localHash;
  }
}
