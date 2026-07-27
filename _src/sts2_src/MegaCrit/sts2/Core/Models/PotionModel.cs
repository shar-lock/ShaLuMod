// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Models.PotionModel
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Context;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Entities.Multiplayer;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Entities.Potions;
using MegaCrit.Sts2.Core.GameActions;
using MegaCrit.Sts2.Core.GameActions.Multiplayer;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.HoverTips;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Localization.DynamicVars;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Models;

public abstract class PotionModel : AbstractModel
{
  public const string locTable = "potions";
  private Player? _owner;
  private DynamicVarSet? _dynamicVars;
  private PotionModel _canonicalInstance;

  public event Action? BeforeUse;

  public LocString Title => new LocString("potions", this.Id.Entry + ".title");

  private LocString Description => new LocString("potions", this.Id.Entry + ".description");

  public LocString SelectionScreenPrompt
  {
    get => new LocString("potions", this.Id.Entry + ".selectionScreenPrompt");
  }

  public LocString DynamicDescription
  {
    get
    {
      LocString description = this.Description;
      this.DynamicVars.AddTo(description);
      string prefix = EnergyIconHelper.GetPrefix((AbstractModel) this);
      description.Add("energyPrefix", EnergyIconHelper.GetPrefix((AbstractModel) this));
      foreach (KeyValuePair<string, object> variable in (IEnumerable<KeyValuePair<string, object>>) description.Variables)
      {
        if (variable.Value is EnergyVar energyVar)
          energyVar.ColorPrefix = prefix;
      }
      return description;
    }
  }

  private string PackedImagePath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/potion_atlas.sprites/{this.Id.Entry.ToLowerInvariant()}.tres");
    }
  }

  private string PackedOutlinePath
  {
    get
    {
      return ImageHelper.GetImagePath($"atlases/potion_outline_atlas.sprites/{this.Id.Entry.ToLowerInvariant()}.tres");
    }
  }

  public string ImagePath => this.PackedImagePath;

  public Texture2D Image
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(this.PackedImagePath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public string? OutlinePath
  {
    get
    {
      return !ResourceLoader.Exists(this.PackedOutlinePath, "") ? (string) null : this.PackedOutlinePath;
    }
  }

  public Texture2D? Outline
  {
    get
    {
      return this.OutlinePath == null ? (Texture2D) null : ResourceLoader.Load<Texture2D>(this.OutlinePath, (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public string LargeImagePath
  {
    get => ImageHelper.GetImagePath($"potions/large/{this.Id.Entry.ToLowerInvariant()}.png");
  }

  public Texture2D LargeImage
  {
    get
    {
      return ResourceLoader.Load<Texture2D>(ResourceLoader.Exists(this.LargeImagePath, "") ? this.LargeImagePath : ImageHelper.GetImagePath("potions/missing_potion.png"), (string) null, (ResourceLoader.CacheMode) 1L);
    }
  }

  public abstract PotionRarity Rarity { get; }

  public abstract PotionUsage Usage { get; }

  public abstract TargetType TargetType { get; }

  public PotionPoolModel Pool
  {
    get
    {
      return ModelDb.AllPotionPools.First<PotionPoolModel>((Func<PotionPoolModel, bool>) (p => p.AllPotionIds.Contains<ModelId>(this.Id)));
    }
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
      this._owner = this._owner == null || this._owner == value ? value : throw new InvalidOperationException($"Cannot move potion {this.Id.Entry} from one owner to another");
    }
  }

  public DynamicVarSet DynamicVars
  {
    get
    {
      if (this._dynamicVars != null)
        return this._dynamicVars;
      this._dynamicVars = new DynamicVarSet(this.CanonicalVars);
      this._dynamicVars.InitializeWithOwner((AbstractModel) this);
      return this._dynamicVars;
    }
  }

  protected virtual IEnumerable<DynamicVar> CanonicalVars
  {
    get => (IEnumerable<DynamicVar>) Array.Empty<DynamicVar>();
  }

  public bool IsQueued { get; private set; }

  public virtual bool CanBeGeneratedInCombat => true;

  public virtual bool PassesCustomUsabilityCheck => true;

  public HoverTip HoverTip
  {
    get
    {
      HoverTip hoverTip = new HoverTip(this.Title, this.DynamicDescription);
      hoverTip.SetCanonicalModel((AbstractModel) this.CanonicalInstance);
      return hoverTip;
    }
  }

  public IEnumerable<IHoverTip> HoverTips
  {
    get
    {
      return ((IEnumerable<IHoverTip>) new IHoverTip[1]
      {
        (IHoverTip) this.HoverTip
      }).Concat<IHoverTip>(this.ExtraHoverTips);
    }
  }

  public virtual IEnumerable<IHoverTip> ExtraHoverTips
  {
    get => (IEnumerable<IHoverTip>) Array.Empty<IHoverTip>();
  }

  public PotionModel CanonicalInstance
  {
    get => !this.IsMutable ? this : this._canonicalInstance;
    private set
    {
      this.AssertMutable();
      this._canonicalInstance = value;
    }
  }

  public override bool ShouldReceiveCombatHooks => true;

  public bool HasBeenRemovedFromState { get; private set; }

  public PotionModel ToMutable()
  {
    this.AssertCanonical();
    PotionModel mutable = (PotionModel) this.MutableClone();
    mutable.CanonicalInstance = this;
    return mutable;
  }

  protected override void AfterCloned()
  {
    base.AfterCloned();
    this.HasBeenRemovedFromState = false;
    this.BeforeUse = (Action) null;
  }

  public void Discard()
  {
    this.Owner.DiscardPotionInternal(this);
    this.HasBeenRemovedFromState = true;
  }

  public void RemoveBeforeUse()
  {
    this.Owner.RemoveUsedPotionInternal(this);
    this.HasBeenRemovedFromState = true;
  }

  public void EnqueueManualUse(Creature? target)
  {
    this.AssertMutable();
    Action beforeUse = this.BeforeUse;
    if (beforeUse != null)
      beforeUse();
    if (target == null && this.IsValidTarget(this.Owner.Creature))
      target = this.Owner.Creature;
    UsePotionAction action = new UsePotionAction(this, target, CombatManager.Instance.IsInProgress);
    this.IsQueued = true;
    RunManager.Instance.ActionQueueSynchronizer.RequestEnqueue((GameAction) action);
  }

  public bool IsValidTarget(Creature? target)
  {
    if (target == null)
      return this.TargetType == TargetType.TargetedNoCreature || !this.TargetType.IsSingleTarget();
    if (!target.IsAlive)
      return false;
    if (this.TargetType == TargetType.AnyEnemy)
      return target.Side != this.Owner.Creature.Side;
    if (this.TargetType == TargetType.AnyAlly)
      return target.Side == this.Owner.Creature.Side && target != this.Owner.Creature;
    if (this.TargetType == TargetType.AnyPlayer)
      return target.IsPlayer;
    return this.TargetType == TargetType.Self && target == this.Owner.Creature;
  }

  public async Task OnUseWrapper(PlayerChoiceContext choiceContext, Creature? target)
  {
    this.RemoveBeforeUse();
    ICombatState combatState = this.Owner.Creature.CombatState;
    choiceContext.PushModel((AbstractModel) this);
    await CombatManager.Instance.WaitForUnpause();
    await Hook.BeforePotionUsed(this.Owner.RunState, combatState, this, target);
    if (TestMode.IsOff && combatState != null)
    {
      NCreature creatureNode1 = NCombatRoom.Instance?.GetCreatureNode(this.Owner.Creature);
      Vector2 vector2 = Vector2.Zero;
      Vector2 targetPosition;
      if (this.TargetType.IsSingleTarget())
      {
        NCreature creatureNode2 = NCombatRoom.Instance?.GetCreatureNode(target);
        targetPosition = creatureNode2 != null ? creatureNode2.GetBottomOfHitbox() : Vector2.Zero;
      }
      else
      {
        IReadOnlyList<Creature> creatureList = this.TargetType != TargetType.AllEnemies ? (IReadOnlyList<Creature>) combatState.GetCreaturesOnSide(CombatSide.Player).Where<Creature>((Func<Creature, bool>) (c => c.IsHittable)).ToList<Creature>() : (IReadOnlyList<Creature>) combatState.GetCreaturesOnSide(CombatSide.Enemy).Where<Creature>((Func<Creature, bool>) (c => c.IsHittable)).ToList<Creature>();
        foreach (Creature creature in (IEnumerable<Creature>) creatureList)
        {
          NCreature creatureNode3 = NCombatRoom.Instance?.GetCreatureNode(creature);
          vector2 = Vector2.op_Addition(vector2, creatureNode3 != null ? creatureNode3.VfxSpawnPosition : Vector2.Zero);
        }
        targetPosition = Vector2.op_Division(vector2, (float) creatureList.Count);
      }
      NItemThrowVfx child = NItemThrowVfx.Create(creatureNode1 != null ? creatureNode1.VfxSpawnPosition : Vector2.Zero, targetPosition, this.Image);
      NCombatRoom instance = NCombatRoom.Instance;
      if (instance != null)
        ((Node) instance.CombatVfxContainer).AddChildSafely((Node) child);
      await Cmd.Wait(0.5f);
    }
    CombatManager.Instance.BeginCardOrPotionEffect(this.Owner);
    try
    {
      BranchingPlayerChoiceContext choiceContext1 = new BranchingPlayerChoiceContext(LocalContext.NetId.Value, GameActionType.Combat, choiceContext);
      choiceContext1.PushModel((AbstractModel) this);
      Task task = this.OnUse((PlayerChoiceContext) choiceContext1, target);
      await choiceContext1.AssignTaskAndWaitForPauseOrCompletion(task);
    }
    finally
    {
      await CombatManager.Instance.EndCardOrPotionEffect(this.Owner);
    }
    if (this.Owner.Creature.IsDead)
    {
      combatState = (ICombatState) null;
    }
    else
    {
      this.InvokeExecutionFinished();
      if (combatState != null && CombatManager.Instance.IsInProgress)
        CombatManager.Instance.History.PotionUsed(combatState, this, target);
      await Hook.AfterPotionUsed(this.Owner.RunState, combatState, this, target);
      this.Owner.RunState.CurrentMapPointHistoryEntry?.GetEntry(this.Owner.NetId).PotionUsed.Add(this.Id);
      await CombatManager.Instance.CheckForEmptyHand(choiceContext, this.Owner);
      choiceContext.PopModel((AbstractModel) this);
      combatState = (ICombatState) null;
    }
  }

  public void AfterUsageCanceled() => this.IsQueued = false;

  protected virtual Task OnUse(PlayerChoiceContext choiceContext, Creature? target)
  {
    return Task.CompletedTask;
  }

  public SerializablePotion ToSerializable(int slotIndex)
  {
    this.AssertMutable();
    return new SerializablePotion()
    {
      Id = this.Id,
      SlotIndex = slotIndex
    };
  }

  public static PotionModel FromSerializable(SerializablePotion save)
  {
    return SaveUtil.PotionOrDeprecated(save.Id).ToMutable();
  }

  protected static void AssertValidForTargetedPotion([NotNull] Creature? target)
  {
    if (target == null)
      throw new ArgumentNullException(nameof (target), "Target must be present for targeted potions.");
  }

  public bool CanThrowAtAlly()
  {
    return this.TargetType == TargetType.AnyPlayer && this.Owner.RunState.Players.Count > 1 && CombatManager.Instance.IsInProgress;
  }
}
