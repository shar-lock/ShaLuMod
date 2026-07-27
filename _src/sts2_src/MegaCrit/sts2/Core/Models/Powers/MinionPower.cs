// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Powers.MinionPower
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Entities.Powers;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Powers;

public sealed class MinionPower : PowerModel
{
  public override PowerType Type => PowerType.Buff;

  public override PowerStackType StackType => PowerStackType.Single;

  public override bool ShouldPlayVfx => false;

  public override bool OwnerIsSecondaryEnemy => true;

  public override bool ShouldPowerBeRemovedAfterOwnerDeath() => false;

  public override bool ShouldOwnerDeathTriggerFatal() => false;
}
