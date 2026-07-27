// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.EternalFeather
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class EternalFeather : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlyArray<DynamicVar>(new DynamicVar[2]
      {
        (DynamicVar) new CardsVar(5),
        (DynamicVar) new HealVar(3M)
      });
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is RestSiteRoom))
      return;
    this.Flash();
    Decimal healAmount = this.DynamicVars.Heal.BaseValue * (Decimal) (PileType.Deck.GetPile(this.Owner).Cards.Count / this.DynamicVars.Cards.IntValue);
    await CreatureCmd.Heal(this.Owner.Creature, healAmount);
    if (!LocalContext.IsMe(this.Owner))
      return;
    PlayerFullscreenHealVfx.Play(this.Owner, healAmount, (Control) NRestSiteRoom.Instance);
  }
}
