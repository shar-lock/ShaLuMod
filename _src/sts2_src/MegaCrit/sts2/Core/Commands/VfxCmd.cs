// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Commands.VfxCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Combat;
using MegaCrit.Sts2.Core.Entities.Creatures;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Bestiary;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Cards;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Commands;

public static class VfxCmd
{
  public const string adrenalinePath = "vfx/vfx_adrenaline";
  public const string bitePath = "vfx/vfx_bite";
  public const string blockPath = "vfx/vfx_block";
  public const string bloodyImpactPath = "vfx/vfx_bloody_impact";
  public const string bluntPath = "vfx/vfx_attack_blunt";
  public const string chainPath = "vfx/vfx_chain";
  public const string coinExplosionSmallPath = "vfx/vfx_coin_explosion_small";
  public const string coinExplosionRegularPath = "vfx/vfx_coin_explosion_regular";
  public const string coinExplosionJumboPath = "vfx/vfx_coin_explosion_jumbo";
  public const string daggerThrowPath = "vfx/vfx_dagger_throw";
  public const string daggerSprayPath = "vfx/vfx_dagger_spray";
  public const string dramaticStabPath = "vfx/vfx_dramatic_stab";
  public const string flyingSlashPath = "vfx/vfx_flying_slash";
  public const string gazePath = "vfx/vfx_gaze";
  public const string giantHorizontalSlashPath = "vfx/vfx_giant_horizontal_slash";
  public const string healPath = "vfx/vfx_cross_heal";
  public const string lightningPath = "vfx/vfx_attack_lightning";
  public const string thrashPath = "vfx/vfx_thrash";
  public const string rockShatterPath = "vfx/vfx_rock_shatter";
  public const string sandyImpactPath = "vfx/vfx_sandy_impact";
  public const string scratchPath = "vfx/vfx_scratch";
  public const string slashPath = "vfx/vfx_attack_slash";
  public const string slimeImpactVfxPath = "vfx/vfx_slime_impact";
  public const string hellraiserSwordVfxPath = "vfx/hellraiser_attack_vfx";
  public const string heavyBluntPath = "vfx/vfx_heavy_blunt";
  public const string starryImpactVfx = "vfx/vfx_starry_impact";
  public const string screamVfx = "vfx/vfx_scream";
  public const string spookyScreamVfx = "vfx/vfx_spooky_scream";

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) new string[27]
      {
        "vfx/vfx_block",
        "vfx/vfx_attack_slash",
        "vfx/vfx_attack_blunt",
        "vfx/vfx_gaze",
        "vfx/vfx_bloody_impact",
        "vfx/vfx_bite",
        "vfx/vfx_chain",
        "vfx/vfx_flying_slash",
        "vfx/vfx_coin_explosion_small",
        "vfx/vfx_coin_explosion_regular",
        "vfx/vfx_coin_explosion_jumbo",
        "vfx/vfx_adrenaline",
        "vfx/vfx_rock_shatter",
        "vfx/vfx_scratch",
        "vfx/vfx_sandy_impact",
        "vfx/vfx_attack_lightning",
        "vfx/vfx_giant_horizontal_slash",
        "vfx/vfx_dagger_throw",
        "vfx/vfx_dagger_spray",
        "vfx/vfx_dramatic_stab",
        "vfx/vfx_slime_impact",
        "vfx/vfx_thrash",
        "vfx/vfx_cross_heal",
        "vfx/vfx_heavy_blunt",
        "vfx/vfx_starry_impact",
        "vfx/vfx_scream",
        "vfx/vfx_spooky_scream"
      }).Select<string, string>(VfxCmd.\u003C\u003EO.\u003C0\u003E__GetScenePath ?? (VfxCmd.\u003C\u003EO.\u003C0\u003E__GetScenePath = new Func<string, string>(SceneHelper.GetScenePath))).Concat<string>((IEnumerable<string>) new \u003C\u003Ez__ReadOnlyArray<string>(new string[28]
      {
        NBigSlashImpactVfx.scenePath,
        NBigSlashVfx.scenePath,
        NCardExhaustVfx.scenePath,
        NCardExhaustQuickVfx.scenePath,
        NCardRemoveVfx.scenePath,
        NCardTransformShineVfx.scenePath,
        NDaggerSprayFlurryVfx.scenePath,
        NDaggerSprayImpactVfx.scenePath,
        NFireBurningVfx.scenePath,
        NFireBurstVfx.scenePath,
        NGaseousImpactVfx.scenePath,
        NGoopyImpactVfx.scenePath,
        NHyperbeamImpactVfx.scenePath,
        NHyperbeamVfx.scenePath,
        NItemThrowVfx.scenePath,
        NLargeMagicMissileVfx.scenePath,
        NLiquidOverlayVfx.scenePath,
        NLowHpBorderVfx.scenePath,
        NMinionDiveBombVfx.scenePath,
        NPoisonImpactVfx.scenePath,
        NScratchVfx.scenePath,
        NShivThrowVfx.scenePath,
        NSmallMagicMissileVfx.scenePath,
        NSplashVfx.scenePath,
        NSporeImpactVfx.scenePath,
        NSweepingBeamImpactVfx.scenePath,
        NSweepingBeamVfx.scenePath,
        NWormyImpactVfx.scenePath
      })).Concat<string>(NHealNumVfx.AssetPaths);
    }
  }

  public static void PlayFullScreenInCombat(string path, Creature? spawner)
  {
    if (TestMode.IsOn)
      return;
    Control vfxContainer = spawner?.GetVfxContainer();
    if (vfxContainer == null)
      return;
    Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(path)).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    ((Node) vfxContainer).AddChildSafely((Node) child);
    Node2D node2D = child;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 vector2 = Vector2.op_Multiply(((Rect2) ref viewportRect).Size, 0.5f);
    node2D.GlobalPosition = vector2;
  }

  public static Vector2? GetSideCenter(CombatSide side, ICombatState combatState)
  {
    if (TestMode.IsOn)
      return new Vector2?();
    if (!combatState.IsLiveCombat())
      return side == CombatSide.Enemy && NBestiary.Instance != null ? new Vector2?(NBestiary.Instance.GetSideCenter()) : new Vector2?(Vector2.Zero);
    Vector2 vector2 = Vector2.Zero;
    IReadOnlyList<Creature> list = (IReadOnlyList<Creature>) combatState.GetCreaturesOnSide(side).Where<Creature>((Func<Creature, bool>) (c => c.IsHittable)).ToList<Creature>();
    foreach (Creature creature in (IEnumerable<Creature>) list)
    {
      NCreature creatureNode = creature.GetCreatureNode();
      if (creatureNode != null)
        vector2 = Vector2.op_Addition(vector2, creatureNode.VfxSpawnPosition);
    }
    return new Vector2?(Vector2.op_Division(vector2, (float) list.Count));
  }

  public static Vector2? GetSideCenterFloor(CombatSide side, ICombatState combatState)
  {
    if (TestMode.IsOn)
      return new Vector2?();
    if (!combatState.IsLiveCombat())
      return side == CombatSide.Enemy && NBestiary.Instance != null ? new Vector2?(NBestiary.Instance.GetSideFloor()) : new Vector2?(Vector2.Zero);
    Vector2 zero = Vector2.Zero;
    IReadOnlyList<Creature> list = (IReadOnlyList<Creature>) combatState.GetCreaturesOnSide(side).Where<Creature>((Func<Creature, bool>) (c => c.IsHittable)).ToList<Creature>();
    foreach (Creature creature in (IEnumerable<Creature>) list)
    {
      NCreature creatureNode = creature.GetCreatureNode();
      if (creatureNode != null)
      {
        if ((double) creatureNode.GetBottomOfHitbox().Y > (double) zero.Y)
          zero.Y = creatureNode.GetBottomOfHitbox().Y;
        zero.X += creatureNode.GetBottomOfHitbox().X;
      }
    }
    zero.X /= (float) list.Count;
    return new Vector2?(zero);
  }

  public static void PlayOnSide(CombatSide side, string path, ICombatState combatState)
  {
    if (TestMode.IsOn || !combatState.IsLiveCombat() && side != CombatSide.Enemy)
      return;
    Vector2? sideCenter = VfxCmd.GetSideCenter(side, combatState);
    if (!sideCenter.HasValue)
      return;
    Control vfxContainer = !combatState.IsLiveCombat() ? NBestiary.Instance?.VfxContainer : NCombatRoom.Instance?.CombatVfxContainer;
    VfxCmd.PlayVfx(sideCenter.Value, path, vfxContainer);
  }

  public static void PlayOnCreatureCenters(IEnumerable<Creature> targets, string path)
  {
    foreach (Creature target in targets)
      VfxCmd.PlayOnCreatureCenter(target, path);
  }

  public static void PlayOnCreatureCenter(Creature target, string path)
  {
    if (TestMode.IsOn || target.IsDead)
      return;
    NCreature creatureNode = target.GetCreatureNode();
    if (creatureNode == null)
      return;
    VfxCmd.PlayVfx(creatureNode.VfxSpawnPosition, path, target.GetVfxContainer());
  }

  public static void PlayOnCreatures(IEnumerable<Creature> targets, string path)
  {
    foreach (Creature target in targets)
      VfxCmd.PlayOnCreature(target, path);
  }

  public static void PlayOnCreature(Creature target, string path)
  {
    if (TestMode.IsOn || target.IsDead)
      return;
    NCreature creatureNode = target.GetCreatureNode();
    if (creatureNode == null)
      return;
    VfxCmd.PlayVfx(creatureNode.GlobalPosition, path, target.GetVfxContainer());
  }

  public static void PlayVfx(Vector2 position, string path, Control? vfxContainer)
  {
    if (TestMode.IsOn)
      return;
    Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(path)).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    if (vfxContainer != null)
      ((Node) vfxContainer).AddChildSafely((Node) child);
    child.GlobalPosition = position;
  }

  public static Node2D? PlayNonCombatVfx(Node container, Vector2 position, string path)
  {
    if (TestMode.IsOn)
      return (Node2D) null;
    Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath(path)).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    container.AddChildSafely((Node) child);
    child.GlobalPosition = position;
    return child;
  }
}
