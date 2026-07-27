// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.Pomander
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class Pomander : RelicModel
{
  public override RelicRarity Rarity => RelicRarity.Ancient;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(1));
    }
  }

  public override async Task AfterObtained()
  {
    foreach (CardModel card in (await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>())
      CardCmd.Upgrade(card);
  }
}
