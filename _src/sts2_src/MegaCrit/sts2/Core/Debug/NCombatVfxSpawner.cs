// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.NCombatVfxSpawner
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Commands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Vfx;
using MegaCrit.Sts2.Core.Nodes.Vfx.Ui;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

[ScriptPath("res://src/Core/Debug/NCombatVfxSpawner.cs")]
public class NCombatVfxSpawner : Control
{
  [Export]
  private Node2D _backCombatVfxContainer;
  [Export]
  private Control _combatVfxContainer;
  private WorldEnvironment _env;
  [Export]
  private Node2D _playerTopPosition;
  [Export]
  private Node2D _playerPosition;
  [Export]
  private Node2D _playerGroundPosition;
  [Export]
  private Node2D _enemyTopPosition;
  [Export]
  private Node2D _enemyPosition;
  [Export]
  private Node2D _enemyGroundPosition;
  [Export]
  private Node2D _defectEyePosition;
  [Export]
  private NLowHpBorderVfx _lowHpBorderVfx;
  [Export]
  private NGaseousScreenVfx _gaseousScreenVfx;
  private Decimal _damage = 10M;
  private bool _shiftPressed;

  public override void _Ready()
  {
  }

  public override void _Process(double delta)
  {
    ((Node) this)._Process(delta);
    this._shiftPressed = Input.IsKeyPressed((Key) 4194325L);
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (inputEvent is InputEventKey inputEventKey1 && inputEventKey1.Keycode == 81L && inputEventKey1.Pressed)
      this.TestFunctionA(this._shiftPressed);
    if (inputEvent is InputEventKey inputEventKey2 && inputEventKey2.Keycode == 87L && inputEventKey2.Pressed)
      this.TestFunctionB(this._shiftPressed);
    if (!(inputEvent is InputEventKey inputEventKey3) || inputEventKey3.Keycode != 69L || !inputEventKey3.Pressed)
      return;
    this.TestFunctionC(this._shiftPressed);
  }

  private static Color GetRandomColor()
  {
    return new Color(Mathf.Lerp(0.35f, 1f, GD.Randf()), Mathf.Lerp(0.35f, 1f, GD.Randf()), Mathf.Lerp(0.35f, 1f, GD.Randf()), 1f);
  }

  private void TestFunctionA(bool shiftPressed) => this.TestProjectileHandler();

  private void TestFunctionB(bool shiftPressed) => TaskHelper.RunSafely(this.PlayingGrandFinale());

  private void TestFunctionC(bool shiftPressed)
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NWormyImpactVfx.Create(this._playerGroundPosition.GlobalPosition, this._playerPosition.GlobalPosition));
  }

  private void TestProjectileHandler()
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NVfxProjectileHandler.Create("debug/vfx/vfx_test_projectile_handler", "debug/vfx/vfx_test_projectile", this._playerPosition.GlobalPosition, this._enemyPosition.GlobalPosition, new Callable()));
  }

  private void SpawnVfx()
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NGaseousImpactVfx.Create(this._enemyPosition.GlobalPosition, NCombatVfxSpawner.GetRandomColor()));
  }

  private async Task SpawningVfx()
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NItemThrowVfx.Create(Vector2.op_Addition(this._playerPosition.GlobalPosition, Vector2.op_Multiply(Vector2.Up, 150f)), this._enemyPosition.GlobalPosition, (Texture2D) null));
    await Cmd.Wait(0.55f);
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NSplashVfx.Create(this._enemyPosition.GlobalPosition, new Color(0.25f, 1f, 0.4f, 1f)));
  }

  private async Task Hyperbeaming()
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NHyperbeamVfx.Create(this._defectEyePosition.GlobalPosition, this._enemyPosition.GlobalPosition));
    await Cmd.Wait(NHyperbeamVfx.hyperbeamAnticipationDuration);
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NHyperbeamImpactVfx.Create(this._defectEyePosition.GlobalPosition, this._enemyPosition.GlobalPosition));
  }

  private async Task PlayingGrandFinale()
  {
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NGrandFinaleVfx.Create(this._playerPosition.GlobalPosition));
    await Cmd.Wait(NGrandFinaleVfx.totalAnticipationDuration);
    ((Node) this._combatVfxContainer).AddChildSafely((Node) NGrandFinaleImpactVfx.Create(this._enemyPosition.GlobalPosition, this._enemyGroundPosition.GlobalPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(9)
    {
      new MethodInfo(NCombatVfxSpawner.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.GetRandomColor, new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionA, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("shiftPressed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionB, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("shiftPressed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.TestFunctionC, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit("shiftPressed"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.TestProjectileHandler, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCombatVfxSpawner.MethodName.SpawnVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.GetRandomColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Color randomColor = NCombatVfxSpawner.GetRandomColor();
      ret = VariantUtils.CreateFrom<Color>(ref randomColor);
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionA) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TestFunctionA(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionB) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TestFunctionB(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionC) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.TestFunctionC(VariantUtils.ConvertTo<bool>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestProjectileHandler) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.TestProjectileHandler();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.SpawnVfx) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SpawnVfx();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.GetRandomColor) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Color randomColor = NCombatVfxSpawner.GetRandomColor();
      ret = VariantUtils.CreateFrom<Color>(ref randomColor);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Ready) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Process) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName._Input) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.GetRandomColor) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionA) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionB) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestFunctionC) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.TestProjectileHandler) || StringName.op_Equality(ref method, NCombatVfxSpawner.MethodName.SpawnVfx) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._backCombatVfxContainer))
    {
      this._backCombatVfxContainer = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._combatVfxContainer))
    {
      this._combatVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._env))
    {
      this._env = VariantUtils.ConvertTo<WorldEnvironment>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerTopPosition))
    {
      this._playerTopPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerPosition))
    {
      this._playerPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerGroundPosition))
    {
      this._playerGroundPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyTopPosition))
    {
      this._enemyTopPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyPosition))
    {
      this._enemyPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyGroundPosition))
    {
      this._enemyGroundPosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._defectEyePosition))
    {
      this._defectEyePosition = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._lowHpBorderVfx))
    {
      this._lowHpBorderVfx = VariantUtils.ConvertTo<NLowHpBorderVfx>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._gaseousScreenVfx))
    {
      this._gaseousScreenVfx = VariantUtils.ConvertTo<NGaseousScreenVfx>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._shiftPressed))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._shiftPressed = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._backCombatVfxContainer))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._backCombatVfxContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._combatVfxContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._combatVfxContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._env))
    {
      value = VariantUtils.CreateFrom<WorldEnvironment>(ref this._env);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerTopPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._playerTopPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._playerPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._playerGroundPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._playerGroundPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyTopPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._enemyTopPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._enemyPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._enemyGroundPosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._enemyGroundPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._defectEyePosition))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._defectEyePosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._lowHpBorderVfx))
    {
      value = VariantUtils.CreateFrom<NLowHpBorderVfx>(ref this._lowHpBorderVfx);
      return true;
    }
    if (StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._gaseousScreenVfx))
    {
      value = VariantUtils.CreateFrom<NGaseousScreenVfx>(ref this._gaseousScreenVfx);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCombatVfxSpawner.PropertyName._shiftPressed))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._shiftPressed);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._backCombatVfxContainer, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._combatVfxContainer, (PropertyHint) 34L, "Control", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._env, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._playerTopPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._playerPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._playerGroundPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._enemyTopPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._enemyPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._enemyGroundPosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._defectEyePosition, (PropertyHint) 34L, "Node2D", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._lowHpBorderVfx, (PropertyHint) 34L, "ColorRect", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NCombatVfxSpawner.PropertyName._gaseousScreenVfx, (PropertyHint) 34L, "AspectRatioContainer", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 1L, NCombatVfxSpawner.PropertyName._shiftPressed, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCombatVfxSpawner.PropertyName._backCombatVfxContainer, Variant.From<Node2D>(ref this._backCombatVfxContainer));
    info.AddProperty(NCombatVfxSpawner.PropertyName._combatVfxContainer, Variant.From<Control>(ref this._combatVfxContainer));
    info.AddProperty(NCombatVfxSpawner.PropertyName._env, Variant.From<WorldEnvironment>(ref this._env));
    info.AddProperty(NCombatVfxSpawner.PropertyName._playerTopPosition, Variant.From<Node2D>(ref this._playerTopPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._playerPosition, Variant.From<Node2D>(ref this._playerPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._playerGroundPosition, Variant.From<Node2D>(ref this._playerGroundPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._enemyTopPosition, Variant.From<Node2D>(ref this._enemyTopPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._enemyPosition, Variant.From<Node2D>(ref this._enemyPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._enemyGroundPosition, Variant.From<Node2D>(ref this._enemyGroundPosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._defectEyePosition, Variant.From<Node2D>(ref this._defectEyePosition));
    info.AddProperty(NCombatVfxSpawner.PropertyName._lowHpBorderVfx, Variant.From<NLowHpBorderVfx>(ref this._lowHpBorderVfx));
    info.AddProperty(NCombatVfxSpawner.PropertyName._gaseousScreenVfx, Variant.From<NGaseousScreenVfx>(ref this._gaseousScreenVfx));
    info.AddProperty(NCombatVfxSpawner.PropertyName._shiftPressed, Variant.From<bool>(ref this._shiftPressed));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._backCombatVfxContainer, ref variant1))
      this._backCombatVfxContainer = ((Variant) ref variant1).As<Node2D>();
    Variant variant2;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._combatVfxContainer, ref variant2))
      this._combatVfxContainer = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._env, ref variant3))
      this._env = ((Variant) ref variant3).As<WorldEnvironment>();
    Variant variant4;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerTopPosition, ref variant4))
      this._playerTopPosition = ((Variant) ref variant4).As<Node2D>();
    Variant variant5;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerPosition, ref variant5))
      this._playerPosition = ((Variant) ref variant5).As<Node2D>();
    Variant variant6;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._playerGroundPosition, ref variant6))
      this._playerGroundPosition = ((Variant) ref variant6).As<Node2D>();
    Variant variant7;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyTopPosition, ref variant7))
      this._enemyTopPosition = ((Variant) ref variant7).As<Node2D>();
    Variant variant8;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyPosition, ref variant8))
      this._enemyPosition = ((Variant) ref variant8).As<Node2D>();
    Variant variant9;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._enemyGroundPosition, ref variant9))
      this._enemyGroundPosition = ((Variant) ref variant9).As<Node2D>();
    Variant variant10;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._defectEyePosition, ref variant10))
      this._defectEyePosition = ((Variant) ref variant10).As<Node2D>();
    Variant variant11;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._lowHpBorderVfx, ref variant11))
      this._lowHpBorderVfx = ((Variant) ref variant11).As<NLowHpBorderVfx>();
    Variant variant12;
    if (info.TryGetProperty(NCombatVfxSpawner.PropertyName._gaseousScreenVfx, ref variant12))
      this._gaseousScreenVfx = ((Variant) ref variant12).As<NGaseousScreenVfx>();
    Variant variant13;
    if (!info.TryGetProperty(NCombatVfxSpawner.PropertyName._shiftPressed, ref variant13))
      return;
    this._shiftPressed = ((Variant) ref variant13).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName GetRandomColor = StringName.op_Implicit(nameof (GetRandomColor));
    public static readonly StringName TestFunctionA = StringName.op_Implicit(nameof (TestFunctionA));
    public static readonly StringName TestFunctionB = StringName.op_Implicit(nameof (TestFunctionB));
    public static readonly StringName TestFunctionC = StringName.op_Implicit(nameof (TestFunctionC));
    public static readonly StringName TestProjectileHandler = StringName.op_Implicit(nameof (TestProjectileHandler));
    public static readonly StringName SpawnVfx = StringName.op_Implicit(nameof (SpawnVfx));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _backCombatVfxContainer = StringName.op_Implicit(nameof (_backCombatVfxContainer));
    public static readonly StringName _combatVfxContainer = StringName.op_Implicit(nameof (_combatVfxContainer));
    public static readonly StringName _env = StringName.op_Implicit(nameof (_env));
    public static readonly StringName _playerTopPosition = StringName.op_Implicit(nameof (_playerTopPosition));
    public static readonly StringName _playerPosition = StringName.op_Implicit(nameof (_playerPosition));
    public static readonly StringName _playerGroundPosition = StringName.op_Implicit(nameof (_playerGroundPosition));
    public static readonly StringName _enemyTopPosition = StringName.op_Implicit(nameof (_enemyTopPosition));
    public static readonly StringName _enemyPosition = StringName.op_Implicit(nameof (_enemyPosition));
    public static readonly StringName _enemyGroundPosition = StringName.op_Implicit(nameof (_enemyGroundPosition));
    public static readonly StringName _defectEyePosition = StringName.op_Implicit(nameof (_defectEyePosition));
    public static readonly StringName _lowHpBorderVfx = StringName.op_Implicit(nameof (_lowHpBorderVfx));
    public static readonly StringName _gaseousScreenVfx = StringName.op_Implicit(nameof (_gaseousScreenVfx));
    public static readonly StringName _shiftPressed = StringName.op_Implicit(nameof (_shiftPressed));
  }

  public class SignalName : Control.SignalName
  {
  }
}
