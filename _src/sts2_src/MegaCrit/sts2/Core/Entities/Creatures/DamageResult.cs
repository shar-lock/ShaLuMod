// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.Creatures.DamageResult
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.ValueProps;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.Creatures;

public class DamageResult
{
  public Creature Receiver { get; }

  public ValueProp Props { get; }

  public int BlockedDamage { get; set; }

  public int UnblockedDamage { get; init; }

  public int OverkillDamage { get; init; }

  public int TotalDamage => this.BlockedDamage + this.UnblockedDamage;

  public bool WasBlockBroken { get; set; }

  public bool WasFullyBlocked { get; set; }

  public bool WasTargetKilled { get; init; }

  public DamageResult(Creature receiver, ValueProp props)
  {
    this.Receiver = receiver;
    this.Props = props;
  }
}
