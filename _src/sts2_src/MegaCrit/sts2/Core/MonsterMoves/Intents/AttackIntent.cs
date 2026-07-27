// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.AttackIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.ValueProps;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public abstract class AttackIntent : AbstractIntent
{
  public override IntentType IntentType => IntentType.Attack;

  protected override string IntentPrefix => "ATTACK";

  public Func<Decimal>? DamageCalc { get; protected set; }

  public virtual int Repeats => 0;

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_attack.tres";

  public override IEnumerable<string> AssetPaths
  {
    get
    {
      List<string> assetPaths = new List<string>();
      for (int index = 1; index <= 5; ++index)
        assetPaths.Add(ImageHelper.GetImagePath($"atlases/intent_atlas.sprites/attack/intent_attack_{index}.tres"));
      return (IEnumerable<string>) assetPaths;
    }
  }

  public override Texture2D GetTexture(IEnumerable<Creature> targets, Creature owner)
  {
    int totalDamage = this.GetTotalDamage(targets, owner);
    string str = "";
    return PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath($"atlases/intent_atlas.sprites/attack/intent_attack_{(totalDamage >= 5 ? (totalDamage >= 10 ? (totalDamage >= 20 ? (totalDamage >= 40 ? str + "5" : str + "4") : str + "3") : str + "2") : str + "1")}.tres"));
  }

  public override string GetAnimation(IEnumerable<Creature> targets, Creature owner)
  {
    string animation = base.GetAnimation(targets, owner);
    int totalDamage = this.GetTotalDamage(targets, owner);
    return totalDamage >= 5 ? (totalDamage >= 10 ? (totalDamage >= 20 ? (totalDamage >= 40 ? animation + "_5" : animation + "_4") : animation + "_3") : animation + "_2") : animation + "_1";
  }

  protected override LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
  {
    LocString intentDescription = base.GetIntentDescription(targets, owner);
    intentDescription.Add("Damage", (Decimal) this.GetSingleDamage(targets, owner));
    intentDescription.Add("Repeat", (Decimal) this.Repeats);
    return intentDescription;
  }

  public abstract int GetTotalDamage(IEnumerable<Creature> targets, Creature owner);

  public int GetSingleDamage(IEnumerable<Creature> targets, Creature owner)
  {
    Decimal num = this.DamageCalc();
    Player me = LocalContext.GetMe(owner.CombatState);
    if (me != null)
      num = Hook.ModifyDamage(me.RunState, me.Creature.CombatState, me.Creature, owner, this.DamageCalc(), ValueProp.Move, (CardModel) null, (CardPlay) null, ModifyDamageHookType.All, CardPreviewMode.None, out IEnumerable<AbstractModel> _);
    return Math.Max(0, (int) num);
  }
}
