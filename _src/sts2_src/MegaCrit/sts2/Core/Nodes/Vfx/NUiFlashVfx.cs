// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Vfx.NUiFlashVfx
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Vfx;

[ScriptPath("res://src/Core/Nodes/Vfx/NUiFlashVfx.cs")]
public class NUiFlashVfx : Control
{
  private const string _scenePath = "res://scenes/vfx/ui_flash_vfx.tscn";
  private TextureRect _textureRect;
  private Texture2D _texture;
  private Color _modulate;
  private Tween? _spriteTween;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/vfx/ui_flash_vfx.tscn");
    }
  }

  public override void _Ready()
  {
    this._textureRect = ((Node) this).GetNode<TextureRect>(NodePath.op_Implicit("TextureRect"));
    this._textureRect.Texture = this._texture;
  }

  public async Task StartVfx()
  {
    TextureRect textureRect = this._textureRect;
    Color modulate = this._modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) textureRect).Modulate = color;
    ((Control) this._textureRect).PivotOffset = Vector2.op_Multiply(((Control) this._textureRect).Size, 0.5f);
    this._spriteTween = ((Node) this).CreateTween();
    this._spriteTween.SetParallel(true);
    this._spriteTween.TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("scale"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.One, 1.3f)), 0.5);
    this._spriteTween.TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 1L);
    this._spriteTween.TweenProperty((GodotObject) this._textureRect, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 1L).SetDelay(0.34999999403953552);
    bool flag = await this._spriteTween.AwaitFinished((Node) this);
    ((Node) this).QueueFreeSafely();
  }

  public static NUiFlashVfx? Create(Texture2D tex, Color modulate)
  {
    if (TestMode.IsOn)
      return (NUiFlashVfx) null;
    NUiFlashVfx nuiFlashVfx = (NUiFlashVfx) PreloadManager.Cache.GetScene("res://scenes/vfx/ui_flash_vfx.tscn").Instantiate((PackedScene.GenEditState) 0L);
    nuiFlashVfx._texture = tex;
    nuiFlashVfx._modulate = modulate;
    return nuiFlashVfx;
  }

  public override void _ExitTree() => this._spriteTween?.Kill();

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NUiFlashVfx.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NUiFlashVfx.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("tex"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Texture2D"), false),
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("modulate"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NUiFlashVfx.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUiFlashVfx.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NUiFlashVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NUiFlashVfx>(ref nuiFlashVfx);
      return true;
    }
    if (!StringName.op_Equality(ref method, NUiFlashVfx.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NUiFlashVfx.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      NUiFlashVfx nuiFlashVfx = NUiFlashVfx.Create(VariantUtils.ConvertTo<Texture2D>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = VariantUtils.CreateFrom<NUiFlashVfx>(ref nuiFlashVfx);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NUiFlashVfx.MethodName._Ready) || StringName.op_Equality(ref method, NUiFlashVfx.MethodName.Create) || StringName.op_Equality(ref method, NUiFlashVfx.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._textureRect))
    {
      this._textureRect = VariantUtils.ConvertTo<TextureRect>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._texture))
    {
      this._texture = VariantUtils.ConvertTo<Texture2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._modulate))
    {
      this._modulate = VariantUtils.ConvertTo<Color>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._spriteTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._spriteTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._textureRect))
    {
      value = VariantUtils.CreateFrom<TextureRect>(ref this._textureRect);
      return true;
    }
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._texture))
    {
      value = VariantUtils.CreateFrom<Texture2D>(ref this._texture);
      return true;
    }
    if (StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._modulate))
    {
      value = VariantUtils.CreateFrom<Color>(ref this._modulate);
      return true;
    }
    if (!StringName.op_Equality(ref name, NUiFlashVfx.PropertyName._spriteTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._spriteTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NUiFlashVfx.PropertyName._textureRect, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUiFlashVfx.PropertyName._texture, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 20L, NUiFlashVfx.PropertyName._modulate, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NUiFlashVfx.PropertyName._spriteTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NUiFlashVfx.PropertyName._textureRect, Variant.From<TextureRect>(ref this._textureRect));
    info.AddProperty(NUiFlashVfx.PropertyName._texture, Variant.From<Texture2D>(ref this._texture));
    info.AddProperty(NUiFlashVfx.PropertyName._modulate, Variant.From<Color>(ref this._modulate));
    info.AddProperty(NUiFlashVfx.PropertyName._spriteTween, Variant.From<Tween>(ref this._spriteTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NUiFlashVfx.PropertyName._textureRect, ref variant1))
      this._textureRect = ((Variant) ref variant1).As<TextureRect>();
    Variant variant2;
    if (info.TryGetProperty(NUiFlashVfx.PropertyName._texture, ref variant2))
      this._texture = ((Variant) ref variant2).As<Texture2D>();
    Variant variant3;
    if (info.TryGetProperty(NUiFlashVfx.PropertyName._modulate, ref variant3))
      this._modulate = ((Variant) ref variant3).As<Color>();
    Variant variant4;
    if (!info.TryGetProperty(NUiFlashVfx.PropertyName._spriteTween, ref variant4))
      return;
    this._spriteTween = ((Variant) ref variant4).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName _textureRect = StringName.op_Implicit(nameof (_textureRect));
    public static readonly StringName _texture = StringName.op_Implicit(nameof (_texture));
    public static readonly StringName _modulate = StringName.op_Implicit(nameof (_modulate));
    public static readonly StringName _spriteTween = StringName.op_Implicit(nameof (_spriteTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
