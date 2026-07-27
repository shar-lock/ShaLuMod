// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.Nightmare
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.CardSelection;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models.Powers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class Nightmare : CardModel
{
  public Nightmare()
    : base(3, CardType.Skill, CardRarity.Rare, TargetType.Self)
  {
  }

  public override bool CanBeGeneratedInCombat => false;

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NNightmareHandsVfx.AssetPaths;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    CardSelectorPrefs prefs = new CardSelectorPrefs(this.SelectionScreenPrompt, 1);
    IEnumerable<CardModel> cards = await CardSelectCmd.FromHand(choiceContext, this.Owner, prefs, (Func<CardModel, bool>) null, (AbstractModel) this);
    if (TestMode.IsOff && LocalContext.IsMe(this.Owner))
    {
      ((Node) NGame.Instance.CurrentRunNode.GlobalUi).AddChildSafely((Node) NSmokyVignetteVfx.Create(new Color(0.8f, 0.3f, 0.8f, 0.66f), new Color(0.0f, 0.0f, 4f, 0.33f)));
      ((Node) NGame.Instance.CurrentRunNode.GlobalUi).AddChildSafely((Node) NNightmareHandsVfx.Create());
      await Cmd.CustomScaledWait(0.1f, 0.25f);
    }
    await CreatureCmd.TriggerAnim(this.Owner.Creature, "Cast", this.Owner.Character.CastAnimDelay);
    CardModel selectedCard = cards.FirstOrDefault<CardModel>();
    if (selectedCard == null)
    {
      cards = (IEnumerable<CardModel>) null;
      selectedCard = (CardModel) null;
    }
    else
    {
      (await PowerCmd.Apply<NightmarePower>(choiceContext, this.Owner.Creature, 3M, this.Owner.Creature, (CardModel) this)).SetSelectedCard(selectedCard);
      cards = (IEnumerable<CardModel>) null;
      selectedCard = (CardModel) null;
    }
  }

  protected override void OnUpgrade() => this.EnergyCost.UpgradeBy(-1);
}
