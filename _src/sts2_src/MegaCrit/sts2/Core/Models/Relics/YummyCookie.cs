// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Relics.YummyCookie
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Relics;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Models.Characters;
using MegaCrit.Sts2.Core.Random;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Relics;

public sealed class YummyCookie : RelicModel
{
  private static CharacterModel? _cachedRandomCharacter;

  public override RelicRarity Rarity => RelicRarity.Ancient;

  public override bool HasUponPickupEffect => true;

  protected override string IconBaseName
  {
    get
    {
      CharacterModel characterModel;
      if (this.IsCanonical || this.Owner == null)
      {
        if (YummyCookie._cachedRandomCharacter == null)
          YummyCookie._cachedRandomCharacter = Rng.Chaotic.NextItem<CharacterModel>(ModelDb.AllCharacters);
        characterModel = YummyCookie._cachedRandomCharacter;
      }
      else
        characterModel = this.Owner.Character;
      string iconBaseName;
      switch (characterModel)
      {
        case Ironclad _:
          iconBaseName = "yummy_cookie_ironclad";
          break;
        case Silent _:
          iconBaseName = "yummy_cookie_silent";
          break;
        case Regent _:
          iconBaseName = "yummy_cookie_regent";
          break;
        case Necrobinder _:
          iconBaseName = "yummy_cookie_necro";
          break;
        case Defect _:
          iconBaseName = "yummy_cookie_defect";
          break;
        default:
          iconBaseName = "yummy_cookie_ironclad";
          break;
      }
      return iconBaseName;
    }
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.RelicIconChanged();
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new CardsVar(4));
    }
  }

  public override async Task AfterObtained()
  {
    foreach (CardModel card in (await CardSelectCmd.FromDeckForUpgrade(this.Owner, new CardSelectorPrefs(CardSelectorPrefs.UpgradeSelectionPrompt, this.DynamicVars.Cards.IntValue))).ToList<CardModel>())
      CardCmd.Upgrade(card);
  }
}
