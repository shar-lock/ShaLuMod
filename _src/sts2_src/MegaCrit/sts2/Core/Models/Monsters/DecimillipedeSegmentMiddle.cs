// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Monsters.DecimillipedeSegmentMiddle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Nodes.Animation;
using MegaCrit.Sts2.Core.Nodes.Combat;

#nullable disable
namespace MegaCrit.Sts2.Core.Models.Monsters;

public sealed class DecimillipedeSegmentMiddle : DecimillipedeSegment
{
  public override void SegmentAttack()
  {
    NCreature creatureNode = this.Creature.GetCreatureNode();
    creatureNode?.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/RightSegmentDriver")?.AttackShake();
    creatureNode?.GetSpecialNode<NDecimillipedeSegmentDriver>("%Visuals/LeftSegmentDriver")?.AttackShake();
  }
}
