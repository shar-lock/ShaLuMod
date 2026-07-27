// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Wither
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Wither : CardModel
{
  private int _fakeUpgradeLevel;

  public Wither()
    : base(-1, CardType.Status, CardRarity.Status, TargetType.None)
  {
  }

  string[] CardModel.AllPortraitPaths
  {
    [PreserveBaseOverrides] get
    {
      return new string[3]
      {
        this.GetPortraitPath(0),
        this.GetPortraitPath(1),
        this.GetPortraitPath(2)
      };
    }
  }

  public override string PortraitPath => this.GetPortraitPath(this.FakeUpgradeLevel);

  protected override string PortraitPngPath => this.GetPortraitPngPath(this.FakeUpgradeLevel);

  private string GetPortraitPath(int witherLevel)
  {
    return ImageHelper.GetImagePath($"atlases/card_atlas.sprites/{this.Pool.Title.ToLowerInvariant()}/{this.GetPortraitFilename(witherLevel)}.tres");
  }

  private string GetPortraitPngPath(int witherLevel)
  {
    return ImageHelper.GetImagePath($"packed/card_portraits/{this.Pool.Title.ToLowerInvariant()}/{this.GetPortraitFilename(witherLevel)}.png");
  }

  private string GetPortraitFilename(int witherLevel)
  {
    if (witherLevel >= 2)
      return "wither3";
    return witherLevel >= 1 ? "wither2" : "wither1";
  }

  public override string Title
  {
    get
    {
      string title = base.Title;
      if (this.FakeUpgradeLevel <= 0)
        return title;
      return $"{title}+{this.FakeUpgradeLevel}";
    }
  }

  private int FakeUpgradeLevel
  {
    get => this._fakeUpgradeLevel;
    set
    {
      this.AssertMutable();
      this._fakeUpgradeLevel = value;
    }
  }

  public override int MaxUpgradeLevel => 0;

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(3M, ValueProp.Unpowered | ValueProp.Move));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Unplayable);
    }
  }

  public override bool HasTurnEndInHandEffect => true;

  protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

  protected override async Task OnTurnEndInHand(PlayerChoiceContext choiceContext)
  {
    IEnumerable<DamageResult> damageResults = await CreatureCmd.Damage(choiceContext, this.Owner.Creature, this.DynamicVars.Damage, (CardModel) this, (CardPlay) null);
  }

  public void FakeUpgrade()
  {
    ++this.FakeUpgradeLevel;
    this.DynamicVars.Damage.UpgradeValueBy(3M);
  }
}
