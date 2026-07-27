// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.ModifierModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Modifiers;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class ModifierModel : AbstractModel
{
  private const string _locTable = "modifiers";
  private RunState? _runState;

  public override bool ShouldReceiveCombatHooks => true;

  public virtual bool ClearsPlayerDeck => false;

  public virtual IEnumerable<IHoverTip> HoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public virtual LocString Title => new LocString("modifiers", this.Id.Entry + ".title");

  public virtual LocString Description
  {
    get => new LocString("modifiers", this.Id.Entry + ".description");
  }

  public virtual LocString NeowOptionTitle => this.Title;

  public virtual LocString NeowOptionDescription => this.Description;

  protected LocString? AdditionalRestSiteHealText
  {
    get => LocString.GetIfExists("modifiers", this.Id.Entry + ".additionalRestSiteHealText");
  }

  public Texture2D Icon
  {
    get
    {
      return ResourceLoader.Exists(this.IconPath, "") ? PreloadManager.Cache.GetTexture2D(this.IconPath) : PreloadManager.Cache.GetTexture2D(ModifierModel.MissingIconPath);
    }
  }

  protected virtual string IconPath
  {
    get => ImageHelper.GetImagePath($"packed/modifiers/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  private static string MissingIconPath => ImageHelper.GetImagePath("powers/missing_power.png");

  protected RunState RunState
  {
    get => this._runState ?? throw new InvalidOperationException("Modifier was never initialized!");
  }

  public void OnRunCreated(RunState runState)
  {
    this.AssertMutable();
    this._runState = runState;
    if (this.ClearsPlayerDeck)
    {
      foreach (Player player in (IEnumerable<Player>) runState.Players)
        player.Deck.Clear();
    }
    this.AfterRunCreated(runState);
  }

  public void OnRunLoaded(RunState runState)
  {
    this.AssertMutable();
    this._runState = runState;
    this.AfterRunLoaded(runState);
  }

  public virtual Func<Task>? GenerateNeowOption(EventModel eventModel) => (Func<Task>) null;

  protected virtual void AfterRunCreated(RunState runState)
  {
  }

  protected virtual void AfterRunLoaded(RunState runState)
  {
  }

  public virtual bool IsEquivalent(ModifierModel other)
  {
    return this.IsCanonical == other.IsCanonical && this.GetType() == other.GetType();
  }

  public ModifierModel ToMutable()
  {
    this.AssertCanonical();
    return (ModifierModel) this.MutableClone();
  }

  public SerializableModifier ToSerializable()
  {
    this.AssertMutable();
    return new SerializableModifier()
    {
      Id = this.Id,
      Props = SavedProperties.From((AbstractModel) this)
    };
  }

  public static ModifierModel FromSerializable(SerializableModifier serializable)
  {
    ModifierModel mutable = SaveUtil.ModifierOrDeprecated(serializable.Id).ToMutable();
    serializable.Props?.Fill((AbstractModel) mutable);
    return mutable;
  }

  public static IReadOnlyCollection<ModifierModel> Pick2Good1Bad(
    Rng rng,
    IEnumerable<CharacterModel> excludedCharacters)
  {
    List<ModifierModel> modifierModelList = new List<ModifierModel>();
    List<ModifierModel> list1 = ModelDb.GoodModifiers.ToList<ModifierModel>();
    List<CharacterModel> list2 = ModelDb.AllCharacters.Except<CharacterModel>(excludedCharacters).ToList<CharacterModel>();
    if (list2.Count <= 0)
      list1.RemoveAll((Predicate<ModifierModel>) (m => m is CharacterCards));
    for (int index = 0; index < 2; ++index)
    {
      ModifierModel canonicalModifier = rng.NextItem<ModifierModel>((IEnumerable<ModifierModel>) list1);
      ModifierModel modifierModel1 = canonicalModifier != null ? canonicalModifier.ToMutable() : throw new InvalidOperationException("There were not enough good modifiers to fill the daily!");
      if (modifierModel1 is CharacterCards characterCards)
        characterCards.CharacterModel = rng.NextItem<CharacterModel>((IEnumerable<CharacterModel>) list2).Id;
      modifierModelList.Add(modifierModel1);
      list1.Remove(canonicalModifier);
      IReadOnlySet<ModifierModel> ireadOnlySet = ModelDb.MutuallyExclusiveModifiers.FirstOrDefault<IReadOnlySet<ModifierModel>>((Func<IReadOnlySet<ModifierModel>, bool>) (s => s.Contains(canonicalModifier)));
      if (ireadOnlySet != null)
      {
        foreach (ModifierModel modifierModel2 in (IEnumerable<ModifierModel>) ireadOnlySet)
          list1.Remove(modifierModel2);
      }
    }
    modifierModelList.Add(rng.NextItem<ModifierModel>((IEnumerable<ModifierModel>) ModelDb.BadModifiers).ToMutable());
    return (IReadOnlyCollection<ModifierModel>) modifierModelList;
  }
}
