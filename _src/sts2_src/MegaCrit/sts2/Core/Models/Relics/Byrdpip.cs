// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Byrdpip
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Byrdpip : RelicModel
{
  private string _skin = Byrdpip.SkinOptions[0];

  public override bool AddsPet => true;

  public override RelicRarity Rarity => RelicRarity.Event;

  public override bool HasUponPickupEffect => true;

  public override bool SpawnsPets => true;

  public static string[] SkinOptions
  {
    get
    {
      return new string[4]
      {
        "version1",
        "version2",
        "version3",
        "version4"
      };
    }
  }

  [SavedProperty]
  public string Skin
  {
    get => this._skin;
    set
    {
      this.AssertMutable();
      this._skin = value;
    }
  }

  protected override IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => HoverTipFactory.FromCardWithCardHoverTips<ByrdSwoop>();
  }

  public override async Task AfterObtained()
  {
    this.Skin = new Rng(this.Owner, this.Id).NextItem<string>((IEnumerable<string>) Byrdpip.SkinOptions);
    List<CardModel> list = PileType.Deck.GetPile(this.Owner).Cards.Where<CardModel>((Func<CardModel, bool>) (c => c is ByrdonisEgg)).ToList<CardModel>();
    if (CombatManager.Instance.IsInProgress)
      list.AddRange(this.Owner.PlayerCombatState.AllCards.Where<CardModel>((Func<CardModel, bool>) (c => c is ByrdonisEgg)));
    foreach (CardModel original in list)
    {
      CardPileAddResult? nullable = await CardCmd.TransformTo<ByrdSwoop>(original);
    }
    if (!CombatManager.Instance.IsInProgress)
      return;
    await this.SummonPet();
  }

  public override async Task BeforeCombatStart() => await this.SummonPet();

  private async Task SummonPet()
  {
    Creature creature = await PlayerCmd.AddPet<MegaCrit.Sts2.Core.Models.Monsters.Byrdpip>(this.Owner);
  }
}
