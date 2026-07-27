// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.DecimillipedeSegmentBack
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Nodes.Animation;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class DecimillipedeSegmentBack : DecimillipedeSegment
{
  public override void SegmentAttack()
  {
    this.Creature.GetCreatureNode()?.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/SegmentDriver")?.AttackShake();
  }
}
