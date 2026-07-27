// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.OrbModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Audio.Debug;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Models.Orbs;
using MegaCrit.Sts2.Core.Random;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class OrbModel : AbstractModel
{
  public const string locTable = "orbs";
  private static readonly ModelId[] _validOrbs = new ModelId[5]
  {
    ModelDb.GetId<LightningOrb>(),
    ModelDb.GetId<FrostOrb>(),
    ModelDb.GetId<DarkOrb>(),
    ModelDb.GetId<PlasmaOrb>(),
    ModelDb.GetId<GlassOrb>()
  };
  private OrbModel _canonicalInstance;
  private Player? _owner;

  public abstract Decimal PassiveVal { get; }

  public abstract Decimal EvokeVal { get; }

  public static OrbModel GetRandomOrb(Rng rng)
  {
    return ModelDb.GetById<OrbModel>(rng.NextItem<ModelId>((IEnumerable<ModelId>) OrbModel._validOrbs));
  }

  public bool HasBeenRemovedFromState { get; private set; }

  public LocString Title => new LocString("orbs", this.Id.Entry + ".title");

  public LocString Description => new LocString("orbs", this.Id.Entry + ".description");

  public bool HasSmartDescription => LocString.Exists("orbs", this.SmartDescriptionLocKey);

  private string SmartDescriptionLocKey => this.Id.Entry + ".smartDescription";

  public LocString SmartDescription
  {
    get
    {
      return !this.HasSmartDescription ? this.Description : new LocString("orbs", this.Id.Entry + ".smartDescription");
    }
  }

  private string DebugPassiveSfx => this.Id.Entry.ToLowerInvariant() + "_passive.mp3";

  private string DebugEvokeSfx => this.Id.Entry.ToLowerInvariant() + "_evoke.mp3";

  private string DebugChannelSfx => this.Id.Entry.ToLowerInvariant() + "_channel.mp3";

  protected virtual string PassiveSfx => "";

  protected virtual string EvokeSfx => "";

  protected virtual string ChannelSfx => "";

  protected void PlayPassiveSfx()
  {
    if (this.PassiveSfx != "")
      SfxCmd.Play(this.PassiveSfx);
    else
      NDebugAudioManager.Instance?.Play(this.DebugPassiveSfx);
  }

  protected void PlayEvokeSfx()
  {
    if (this.EvokeSfx != "")
      SfxCmd.Play(this.EvokeSfx);
    else
      NDebugAudioManager.Instance?.Play(this.DebugEvokeSfx);
  }

  public void PlayChannelSfx()
  {
    if (this.ChannelSfx != "")
      SfxCmd.Play(this.ChannelSfx);
    else
      NDebugAudioManager.Instance?.Play(this.DebugChannelSfx);
  }

  public static HoverTip EmptySlotHoverTipHoverTip
  {
    get
    {
      return new HoverTip(new LocString("orbs", "EMPTY_SLOT.title"), new LocString("orbs", "EMPTY_SLOT.description"));
    }
  }

  public HoverTip DumbHoverTip => new HoverTip(this, this.Description);

  protected virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      List<IHoverTip> list = this.ExtraHoverTips.ToList<IHoverTip>();
      if (this.HasSmartDescription && this.IsMutable)
      {
        LocString smartDescription = this.SmartDescription;
        smartDescription.Add("energyPrefix", this.Owner.Character.CardPool.Title);
        smartDescription.Add("Passive", this.PassiveVal);
        smartDescription.Add("Evoke", this.EvokeVal);
        list.Add((IHoverTip) new HoverTip(this, smartDescription));
      }
      else
        list.Add((IHoverTip) this.DumbHoverTip);
      return (IEnumerable<IHoverTip>) list;
    }
  }

  private string IconPath
  {
    get => ImageHelper.GetImagePath($"orbs/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  private string SpritePath
  {
    get => SceneHelper.GetScenePath("orbs/orb_visuals/" + this.Id.Entry.ToLowerInvariant());
  }

  public CompressedTexture2D Icon => PreloadManager.Cache.GetCompressedTexture2D(this.IconPath);

  public Node2D CreateSprite()
  {
    return PreloadManager.Cache.GetScene(this.SpritePath).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
  }

  public abstract Color DarkenedColor { get; }

  private OrbModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public OrbModel ToMutable(int initialAmount = 0)
  {
    this.AssertCanonical();
    OrbModel mutable = (OrbModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  public Player Owner
  {
    get
    {
      this.AssertMutable();
      return this._owner;
    }
    set
    {
      this.AssertMutable();
      this._owner = this._owner == null || value == null || value == this._owner ? value : throw new InvalidOperationException($"Card {this.Id.Entry} already has an owner.");
    }
  }

  public ICombatState CombatState => this.Owner.Creature.CombatState;

  protected void ActivatePassive()
  {
    Action passiveActivated = this.PassiveActivated;
    if (passiveActivated == null)
      return;
    passiveActivated();
  }

  public event Action? PassiveActivated;

  public void ActivateEvoke(Creature[] targets)
  {
    Action<Creature[]> evokeActivated = this.EvokeActivated;
    if (evokeActivated == null)
      return;
    evokeActivated(targets);
  }

  public event Action<Creature[]>? EvokeActivated;

  public virtual Task BeforeTurnEndOrbTrigger(PlayerChoiceContext choiceContext)
  {
    return Task.CompletedTask;
  }

  public virtual Task AfterTurnStartOrbTrigger(PlayerChoiceContext choiceContext)
  {
    return Task.CompletedTask;
  }

  public async Task TriggerPassive(PlayerChoiceContext choiceContext, Creature? target)
  {
    List<AbstractModel> modifyingModels;
    int triggerCount = Hook.ModifyOrbPassiveTriggerCount(this.Owner.Creature.CombatState, this, 1, out modifyingModels);
    await Hook.AfterModifyingOrbPassiveTriggerCount(this.Owner.Creature.CombatState, this, (IEnumerable<AbstractModel>) modifyingModels);
    for (int i = 0; i < triggerCount; ++i)
    {
      await this.Passive(choiceContext, target);
      if (LocalContext.IsMe(this.Owner))
        await Cmd.CustomScaledWait(0.1f, 0.25f);
      else
        await Cmd.Wait(0.05f);
    }
  }

  public virtual Task Passive(PlayerChoiceContext choiceContext, Creature? target)
  {
    return Task.CompletedTask;
  }

  public virtual Task<IEnumerable<Creature>> Evoke(PlayerChoiceContext playerChoiceContext)
  {
    return Task.FromResult<IEnumerable<Creature>>((IEnumerable<Creature>) Array.Empty<Creature>());
  }

  protected Decimal ModifyOrbValue(Decimal result)
  {
    return Hook.ModifyOrbValue(this.Owner.Creature.CombatState, this, result);
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.EvokeActivated = (Action<Creature[]>) null;
    this.PassiveActivated = (Action) null;
  }

  public override bool ShouldReceiveCombatHooks => true;

  public IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[2]
      {
        this.IconPath,
        this.SpritePath
      });
    }
  }

  public void RemoveInternal() => this.HasBeenRemovedFromState = true;
}
