// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.DeathBlowIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public class DeathBlowIntent(Func<Decimal> damageCalc) : SingleAttackIntent(damageCalc)
{
  protected override string IntentPrefix => "DEATH_BLOW";

  protected override string SpritePath => "atlases/intent_atlas.sprites/intent_death_blow.tres";

  public override IntentType IntentType => IntentType.DeathBlow;

  public override Texture2D GetTexture(IEnumerable<Creature> targets, Creature owner)
  {
    return PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath(this.SpritePath));
  }

  public override string GetAnimation(IEnumerable<Creature> targets, Creature owner)
  {
    return this._cachedAnimationName ?? (this._cachedAnimationName = this.IntentPrefix.ToLowerInvariant());
  }
}
