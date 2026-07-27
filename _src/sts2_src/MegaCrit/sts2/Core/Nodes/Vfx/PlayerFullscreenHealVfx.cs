// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.PlayerFullscreenHealVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx.Utilities;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

public static class PlayerFullscreenHealVfx
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("vfx/vfx_cross_heal_fullscreen");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(PlayerFullscreenHealVfx._scenePath);
    }
  }

  public static void Play(Player player, Decimal healAmount, Control? vfxContainer)
  {
    if (TestMode.IsOn || healAmount < 1M || vfxContainer == null)
      return;
    float num1 = Ease.QuadOut((float) (healAmount / (Decimal) player.Creature.MaxHp));
    Color green = StsColors.green;
    green.A = Mathf.Max(num1 * 0.8f, 0.4f);
    NSmokyVignetteVfx child1 = NSmokyVignetteVfx.Create(green, new Color(0.0f, 1f, 0.0f, 0.33f));
    if (child1 != null)
      ((Node) vfxContainer).AddChildSafely((Node) child1);
    NVfxParticleSystem child2 = PreloadManager.Cache.GetScene(PlayerFullscreenHealVfx._scenePath).Instantiate<NVfxParticleSystem>((PackedScene.GenEditState) 0L);
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    child2.GlobalPosition = Vector2.op_Multiply(((Rect2) ref viewportRect).Size, 0.5f);
    GpuParticles2D node = ((Node) child2).GetNode<GpuParticles2D>(NodePath.op_Implicit("beam"));
    ((ParticleProcessMaterial) node.ProcessMaterial).EmissionBoxExtents = new Vector3(((Rect2) ref viewportRect).Size.X / 2f, ((Rect2) ref viewportRect).Size.Y / 2f, 1f);
    int num2 = Mathf.RoundToInt((float) ((double) num1 * 40.0 + 10.0));
    node.Amount = num2;
    ((Node) vfxContainer).AddChildSafely((Node) child2);
  }
}
