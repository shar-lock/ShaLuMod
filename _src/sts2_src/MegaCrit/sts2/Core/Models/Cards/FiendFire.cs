// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.Cards.FiendFire
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Commands.Builders;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models.Cards;

public sealed class FiendFire : CardModel
{
  public FiendFire()
    : base(2, CardType.Attack, CardRarity.Rare, TargetType.AnyEnemy)
  {
  }

  protected override IEnumerable<DynamicVar> CanonicalVars
  {
    get
    {
      return (IEnumerable<DynamicVar>) new \u003C\u003Ez__ReadOnlySingleElementList<DynamicVar>((DynamicVar) new DamageVar(7M, ValueProp.Move));
    }
  }

  public override IEnumerable<CardKeyword> CanonicalKeywords
  {
    get
    {
      return (IEnumerable<CardKeyword>) new \u003C\u003Ez__ReadOnlySingleElementList<CardKeyword>(CardKeyword.Exhaust);
    }
  }

  protected override IEnumerable<string> ExtraRunAssetPaths => NGroundFireVfx.AssetPaths;

  protected override async Task OnPlay(PlayerChoiceContext choiceContext, CardPlay cardPlay)
  {
    ArgumentNullException.ThrowIfNull((object) cardPlay.Target, "cardPlay.Target");
    List<CardModel> list = PileType.Hand.GetPile(this.Owner).Cards.ToList<CardModel>();
    int cardCount = list.Count;
    foreach (CardModel card in list)
      await CardCmd.Exhaust(choiceContext, card);
    float scale = 0.8f;
    AttackCommand attackCommand = await DamageCmd.Attack(this.DynamicVars.Damage.BaseValue).WithHitCount(cardCount).FromCard((CardModel) this, cardPlay).Targeting(cardPlay.Target).BeforeDamage((Func<Task>) (() =>
    {
      NGroundFireVfx child = NGroundFireVfx.Create(cardPlay.Target);
      if (child == null)
        return Task.CompletedTask;
      SfxCmd.Play("event:/sfx/characters/attack_fire");
      child.Scale = Vector2.op_Multiply(Vector2.One, scale);
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
      scale += 0.1f;
      return Task.CompletedTask;
    })).Execute(choiceContext);
  }

  protected override void OnUpgrade() => this.DynamicVars.Damage.UpgradeValueBy(3M);
}
