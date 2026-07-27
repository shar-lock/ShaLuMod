// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMonsterDeathVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NMonsterDeathVfx.cs")]
public class NMonsterDeathVfx : Node2D
{
  private const string _deathSfx = "event:/sfx/enemy/enemy_fade";
  private const float _refLength = 0.1f;
  private const float _refTweenDuration = 2.5f;
  private const float _minTweenDuration = 2.5f;
  private const float _tweenStartValue = 0.0f;
  private const string _shaderParamThreshold = "shader_parameter/threshold";
  private List<NCreature> _creatureNodes;
  private List<Control> _hitboxes;
  private CancellationToken _cancelToken;

  private static string ScenePath => SceneHelper.GetScenePath("vfx/vfx_monster_death");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NMonsterDeathVfx.ScenePath);
    }
  }

  public static NMonsterDeathVfx? Create(NCreature creatureNode, CancellationToken cancelToken)
  {
    if (TestMode.IsOn)
      return (NMonsterDeathVfx) null;
    if (SaveManager.Instance.PrefsSave.FastMode == FastModeType.Instant)
      return (NMonsterDeathVfx) null;
    if (cancelToken.IsCancellationRequested)
      return (NMonsterDeathVfx) null;
    NMonsterDeathVfx nmonsterDeathVfx1 = PreloadManager.Cache.GetScene(NMonsterDeathVfx.ScenePath).Instantiate<NMonsterDeathVfx>((PackedScene.GenEditState) 0L);
    NMonsterDeathVfx nmonsterDeathVfx2 = nmonsterDeathVfx1;
    int capacity1 = 1;
    List<NCreature> ncreatureList = new List<NCreature>(capacity1);
    CollectionsMarshal.SetCount<NCreature>(ncreatureList, capacity1);
    CollectionsMarshal.AsSpan<NCreature>(ncreatureList)[0] = creatureNode;
    nmonsterDeathVfx2._creatureNodes = ncreatureList;
    nmonsterDeathVfx1._cancelToken = cancelToken;
    NMonsterDeathVfx nmonsterDeathVfx3 = nmonsterDeathVfx1;
    int capacity2 = 1;
    List<Control> controlList = new List<Control>(capacity2);
    CollectionsMarshal.SetCount<Control>(controlList, capacity2);
    CollectionsMarshal.AsSpan<Control>(controlList)[0] = creatureNode.Hitbox;
    nmonsterDeathVfx3._hitboxes = controlList;
    return nmonsterDeathVfx1;
  }

  public static NMonsterDeathVfx? Create(List<NCreature> creatureNodes)
  {
    if (TestMode.IsOn)
      return (NMonsterDeathVfx) null;
    NMonsterDeathVfx nmonsterDeathVfx = PreloadManager.Cache.GetScene(NMonsterDeathVfx.ScenePath).Instantiate<NMonsterDeathVfx>((PackedScene.GenEditState) 0L);
    nmonsterDeathVfx._creatureNodes = creatureNodes;
    nmonsterDeathVfx._cancelToken = new CancellationToken();
    nmonsterDeathVfx._hitboxes = creatureNodes.Select<NCreature, Control>((Func<NCreature, Control>) (c => c.Hitbox)).ToList<Control>();
    return nmonsterDeathVfx;
  }

  public async Task PlayVfx()
  {
    if (this._cancelToken.IsCancellationRequested)
      return;
    NCombatRoom instance = NCombatRoom.Instance;
    if (instance == null)
    {
      ((Node) this).QueueFreeSafely();
    }
    else
    {
      SubViewport node1 = ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("Viewport"));
      Rect2? nullable = new Rect2?();
      for (int index = 0; index < this._creatureNodes.Count; ++index)
      {
        NCreature creatureNode = this._creatureNodes[index];
        NCreatureVisuals visuals = creatureNode.Visuals;
        Vector2 scale1 = visuals.GetCurrentBody().Scale;
        MonsterModel monster = creatureNode.Entity.Monster;
        Vector2 vector2_1 = monster != null ? monster.ExtraDeathVfxPadding : MonsterModel.defaultDeathVfxPadding;
        if (visuals != null && visuals.HasSpineAnimation && !visuals.IsUsingPhobiaModeBody)
        {
          MegaSkeleton skeleton = visuals.SpineBody.GetSkeleton();
          if (skeleton != null)
          {
            Vector2 scale2 = instance.SceneContainer.Scale;
            Rect2 bounds = skeleton.GetBounds();
            Rect2 rect2_1;
            // ISSUE: explicit constructor call
            ((Rect2) ref rect2_1).\u002Ector(Vector2.op_Multiply(Vector2.op_Multiply(((Rect2) ref bounds).Position, scale1), scale2), Vector2.op_Multiply(Vector2.op_Multiply(((Rect2) ref bounds).Size, scale1), scale2));
            Vector2 vector2_2;
            // ISSUE: explicit constructor call
            ((Vector2) ref vector2_2).\u002Ector(Math.Min(((Rect2) ref rect2_1).Position.X, ((Rect2) ref rect2_1).End.X), Math.Min(((Rect2) ref rect2_1).Position.Y, ((Rect2) ref rect2_1).End.Y));
            Vector2 vector2_3;
            // ISSUE: explicit constructor call
            ((Vector2) ref vector2_3).\u002Ector(Math.Max(((Rect2) ref rect2_1).Position.X, ((Rect2) ref rect2_1).End.X), Math.Max(((Rect2) ref rect2_1).Position.Y, ((Rect2) ref rect2_1).End.Y));
            Vector2 vector2_4 = Vector2.op_Subtraction(vector2_3, vector2_2);
            Vector2 vector2_5 = Vector2.op_Multiply(vector2_4, vector2_1);
            Vector2 vector2_6 = Vector2.op_Multiply(Vector2.op_Subtraction(vector2_5, vector2_4), 0.5f);
            Rect2 rect2_2;
            // ISSUE: explicit constructor call
            ((Rect2) ref rect2_2).\u002Ector(Vector2.op_Subtraction(Vector2.op_Addition(visuals.GetCurrentBody().GlobalPosition, vector2_2), vector2_6), vector2_5);
            if (!nullable.HasValue)
            {
              nullable = new Rect2?(rect2_2);
            }
            else
            {
              ref Rect2? local = ref nullable;
              Rect2 rect2_3 = nullable.Value;
              Rect2 rect2_4 = ((Rect2) ref rect2_3).Merge(rect2_2);
              local = new Rect2?(rect2_4);
            }
          }
        }
        else
        {
          Control hitbox = this._hitboxes[index];
          Vector2 vector2_7 = Vector2.op_Multiply(hitbox.Size, hitbox.Scale);
          Vector2 vector2_8 = Vector2.op_Multiply(vector2_7, vector2_1);
          Vector2 vector2_9 = Vector2.op_Multiply(Vector2.op_Subtraction(vector2_8, vector2_7), 0.5f);
          Rect2 rect2_5;
          // ISSUE: explicit constructor call
          ((Rect2) ref rect2_5).\u002Ector(Vector2.op_Subtraction(hitbox.GlobalPosition, vector2_9), vector2_8);
          if (!nullable.HasValue)
          {
            nullable = new Rect2?(rect2_5);
          }
          else
          {
            ref Rect2? local = ref nullable;
            Rect2 rect2_6 = nullable.Value;
            Rect2 rect2_7 = ((Rect2) ref rect2_6).Merge(rect2_5);
            local = new Rect2?(rect2_7);
          }
        }
      }
      Rect2 rect2_8 = nullable.Value;
      Vector2 position = ((Rect2) ref rect2_8).Position;
      Rect2 rect2_9 = nullable.Value;
      Vector2 vector2_10 = Vector2.op_Multiply(((Rect2) ref rect2_9).Size, 0.5f);
      this.GlobalPosition = Vector2.op_Addition(position, vector2_10);
      Rect2 rect2_10 = nullable.Value;
      Vector2 size = ((Rect2) ref rect2_10).Size;
      int num = Mathf.RoundToInt(Mathf.Max(size.X, size.Y));
      // ISSUE: explicit constructor call
      ((Vector2) ref size).\u002Ector((float) num, (float) num);
      node1.Size = Vector2I.op_Multiply(new Vector2I(num, num), 2);
      node1.Size2DOverride = new Vector2I(num, num);
      Sprite2D node2 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Visual"));
      ((Node2D) node2).Scale = Vector2.op_Division(((Node2D) node2).Scale, Vector2.op_Multiply(2f, instance.SceneContainer.Scale));
      Vector2 vector2_11 = Vector2.op_Subtraction(this.GlobalPosition, Vector2.op_Multiply(size, 0.5f));
      foreach (NCreature creatureNode in this._creatureNodes)
      {
        if (GodotObject.IsInstanceValid((GodotObject) creatureNode.Visuals.GetCurrentBody()))
        {
          Vector2 globalPosition = creatureNode.Visuals.GetCurrentBody().GlobalPosition;
          ((Node) creatureNode.Visuals.GetCurrentBody()).Reparent((Node) node1, true);
          if (GodotObject.IsInstanceValid((GodotObject) creatureNode.Visuals.GetCurrentBody()))
            creatureNode.Visuals.GetCurrentBody().Position = Vector2.op_Subtraction(globalPosition, vector2_11);
        }
      }
      node1.RenderTargetUpdateMode = (SubViewport.UpdateMode) 1L;
      await this.PlayVfxInternal();
    }
  }

  private async Task PlayVfxInternal()
  {
    SfxCmd.Play("event:/sfx/enemy/enemy_fade");
    SubViewport node1 = ((Node) this).GetNode<SubViewport>(NodePath.op_Implicit("Viewport"));
    Sprite2D node2 = ((Node) this).GetNode<Sprite2D>(NodePath.op_Implicit("Visual"));
    CpuParticles2D node3 = ((Node) this).GetNode<CpuParticles2D>(NodePath.op_Implicit("%Particles"));
    node3.EmissionSphereRadius = (float) node1.Size.Y / 4f;
    node3.Emitting = true;
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    float num1 = (float) (0.10000000149011612 * (double) ((Rect2) ref viewportRect).Size.X);
    float num2 = Math.Min((float) ((double) node1.Size.X / (double) num1 * 2.5), 2.5f);
    using (Tween tween = ((Node) this).CreateTween())
    {
      tween.TweenProperty((GodotObject) ((CanvasItem) node2).Material, NodePath.op_Implicit("shader_parameter/threshold"), Variant.op_Implicit(0.0f), (double) num2).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
      bool flag = await tween.AwaitFinished((Node) this);
      ((Node) this).QueueFreeSafely();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(
  #nullable disable
  GodotSerializationInfo info) => ((GodotObject) this).SaveGodotObjectData(info);

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
  }

  public class MethodName : Node2D.MethodName
  {
  }

  public class PropertyName : Node2D.PropertyName
  {
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
