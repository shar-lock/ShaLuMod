// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NMapCircleVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Map;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NMapCircleVfx.cs")]
public class NMapCircleVfx : Control
{
  private TextureRect _image;
  private const string _path = "res://scenes/vfx/map_circle_vfx.tscn";
  private static readonly string[] _textures = new string[5]
  {
    "res://images/atlases/compressed.sprites/map/map_circle_0.tres",
    "res://images/atlases/compressed.sprites/map/map_circle_1.tres",
    "res://images/atlases/compressed.sprites/map/map_circle_2.tres",
    "res://images/atlases/compressed.sprites/map/map_circle_3.tres",
    "res://images/atlases/compressed.sprites/map/map_circle_4.tres"
  };
  private const double _animInterval = 0.041666666666666664;
  private bool _playAnim;
  private MapCoord _mapCoord;
  private IRunState _runState;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return ((IEnumerable<string>) NMapCircleVfx._textures).Append<string>("res://scenes/vfx/map_circle_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    this._image = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._image.Texture = PreloadManager.Cache.GetTexture2D(NMapCircleVfx._textures[0]);
    Rng rng = new Rng((ulong) ((long) this._runState.Rng.Seed + (long) this._mapCoord.row + 131L * (long) this._mapCoord.row));
    this.RotationDegrees = rng.NextFloat(360f);
    Vector2 vector2 = Vector2.op_Multiply(Vector2.One, rng.NextFloat(0.85f, 0.9f));
    if (this._playAnim)
    {
      Tween tween = ((Node) this).CreateTween().SetParallel(true);
      tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(vector2), 1.0).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(Vector2.op_Multiply(vector2, 2f)));
      tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.95f), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L).From(Variant.op_Implicit(0.0f));
      TaskHelper.RunSafely(this.AnimateSprite());
    }
    else
    {
      this.Scale = vector2;
      ((CanvasItem) this).Modulate = new Color(((CanvasItem) this).Modulate, 0.95f);
      this._image.Texture = PreloadManager.Cache.GetTexture2D(((IEnumerable<string>) NMapCircleVfx._textures).Last<string>());
    }
  }

  private async Task AnimateSprite()
  {
    string[] strArray = NMapCircleVfx._textures;
    for (int index = 0; index < strArray.Length; ++index)
    {
      this._image.Texture = PreloadManager.Cache.GetTexture2D(strArray[index]);
      await ((GodotObject) ((Node) this).GetTree().CreateTimer(1.0 / 24.0, true, false, false)).AwaitSignal(SceneTreeTimer.SignalName.Timeout, (Node) this);
    }
    strArray = (string[]) null;
  }

  public static NMapCircleVfx? Create(IRunState runState, MapCoord mapCoord, bool playAnim)
  {
    if (TestMode.IsOn)
      return (NMapCircleVfx) null;
    NMapCircleVfx nmapCircleVfx = PreloadManager.Cache.GetScene("res://scenes/vfx/map_circle_vfx.tscn").Instantiate<NMapCircleVfx>((PackedScene.GenEditState) 0L);
    nmapCircleVfx._playAnim = playAnim;
    nmapCircleVfx._mapCoord = mapCoord;
    nmapCircleVfx._runState = runState;
    return nmapCircleVfx;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NMapCircleVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (!StringName.op_Equality(ref method, NMapCircleVfx.MethodName._Ready) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._Ready();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NMapCircleVfx.MethodName._Ready) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapCircleVfx.PropertyName._image))
    {
      this._image = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapCircleVfx.PropertyName._playAnim))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._playAnim = VariantUtils.ConvertTo<bool>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NMapCircleVfx.PropertyName._image))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._image);
      return true;
    }
    if (!StringName.op_Equality(ref name, NMapCircleVfx.PropertyName._playAnim))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<bool>(ref this._playAnim);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NMapCircleVfx.PropertyName._image, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NMapCircleVfx.PropertyName._playAnim, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NMapCircleVfx.PropertyName._image, Variant.From<TextureRect>(ref this._image));
    info.AddProperty(NMapCircleVfx.PropertyName._playAnim, Variant.From<bool>(ref this._playAnim));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NMapCircleVfx.PropertyName._image, ref variant1))
      this._image = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (!info.TryGetProperty(NMapCircleVfx.PropertyName._playAnim, ref variant2))
      return;
    this._playAnim = ((Variant) ref variant2).As<bool>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _image = StringName.op_Implicit(nameof (_image));
    public static readonly StringName _playAnim = StringName.op_Implicit(nameof (_playAnim));
  }

  public class SignalName : Control.SignalName
  {
  }
}
