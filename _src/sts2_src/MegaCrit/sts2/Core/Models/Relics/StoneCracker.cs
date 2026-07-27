// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.StoneCracker
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Extensions;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Rooms;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class StoneCracker : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Uncommon;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(2));
    }
  }

  public override async Task AfterRoomEntered(AbstractRoom room)
  {
    if (!(room is CombatRoom))
      return;
    this.Flash();
    List<CardModel> list = PileType.Draw.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c.IsUpgradable)).ToList<CardModel>().StableShuffle<CardModel>(this.Owner.RunState.Rng.CombatCardSelection).Take<CardModel>(this.DynamicVars.Cards.IntValue).ToList<CardModel>();
    CardCmd.Upgrade((IEnumerable<CardModel>) list, CardPreviewStyle.HorizontalLayout);
    CardCmd.Preview((IReadOnlyList<CardModel>) list);
    await Cmd.CustomScaledWait(0.5f, 1f);
  }
}
