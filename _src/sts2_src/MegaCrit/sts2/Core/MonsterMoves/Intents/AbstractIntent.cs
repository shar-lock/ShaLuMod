// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.MonsterMoves.Intents.AbstractIntent
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.MonsterMoves.Intents;

public abstract class AbstractIntent
{
  protected const string _locTable = "intents";
  protected string? _cachedAnimationName;

  public abstract IntentType IntentType { get; }

  public virtual bool HasIntentTip => true;

  protected virtual LocString? IntentLabelFormat => (LocString) null;

  protected abstract string IntentPrefix { get; }

  protected abstract string? SpritePath { get; }

  public virtual IEnumerable<string> AssetPaths
  {
    get
    {
      return this.SpritePath != null ? (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(ImageHelper.GetImagePath(this.SpritePath)) : (IEnumerable<string>) Array.Empty<string>();
    }
  }

  protected LocString IntentTitle => new LocString("intents", this.IntentPrefix + ".title");

  public virtual LocString GetIntentLabel(IEnumerable<Creature> targets, Creature owner)
  {
    return this.IntentLabelFormat ?? new LocString("intents", "FORMAT_EMPTY");
  }

  public virtual Texture2D? GetTexture(IEnumerable<Creature> targets, Creature owner)
  {
    return string.IsNullOrEmpty(this.SpritePath) ? (Texture2D) null : PreloadManager.Cache.GetTexture2D(ImageHelper.GetImagePath(this.SpritePath));
  }

  public virtual string GetAnimation(IEnumerable<Creature> targets, Creature owner)
  {
    return this._cachedAnimationName ?? (this._cachedAnimationName = this.IntentPrefix.ToLowerInvariant());
  }

  public HoverTip GetHoverTip(IEnumerable<Creature> targets, Creature owner)
  {
    Creature[] array = targets.ToArray<Creature>();
    return new HoverTip(this.IntentTitle, this.GetIntentDescription((IEnumerable<Creature>) array, owner), this.GetTexture((IEnumerable<Creature>) array, owner));
  }

  protected virtual LocString GetIntentDescription(IEnumerable<Creature> targets, Creature owner)
  {
    LocString intentDescription = new LocString("intents", this.IntentPrefix + ".description");
    LocString locString = intentDescription;
    ICombatState combatState = owner.CombatState;
    int num = combatState != null ? (combatState.RunState.Players.Count > 1 ? 1 : 0) : 0;
    locString.Add("IsMultiplayer", num != 0);
    return intentDescription;
  }
}
