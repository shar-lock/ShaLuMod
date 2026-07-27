// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Cards.NCardBundle
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Cards;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Cards;

[ScriptPath("res://src/Core/Nodes/Cards/NCardBundle.cs")]
public class NCardBundle : Control
{
  private const float _cardSeparation = 45f;
  private readonly Vector2 _hoverScale = Vector2.op_Multiply(Vector2.One, 0.85f);
  public readonly Vector2 smallScale = Vector2.op_Multiply(Vector2.One, 0.8f);
  private Control _cardHolder;
  private readonly List<NCard> _cardNodes = new List<NCard>();
  private Tween? _hoverTween;
  private Tween? _cardTween;
  private 
  #nullable disable
  NCardBundle.ClickedEventHandler backing_Clicked;

  public 
  #nullable enable
  NClickableControl Hitbox { get; private set; }

  public IReadOnlyList<CardModel> Bundle { get; private set; }

  public IReadOnlyList<NCard> CardNodes => (IReadOnlyList<NCard>) this._cardNodes;

  private static string ScenePath => SceneHelper.GetScenePath("/cards/card_bundle");

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>(NCardBundle.ScenePath);
    }
  }

  public static NCardBundle? Create(IReadOnlyList<CardModel> bundle)
  {
    if (TestMode.IsOn)
      return (NCardBundle) null;
    NCardBundle ncardBundle = PreloadManager.Cache.GetScene(NCardBundle.ScenePath).Instantiate<NCardBundle>((PackedScene.GenEditState) 0L);
    ((Node) ncardBundle).Name = StringName.op_Implicit(nameof (NCardBundle));
    ncardBundle.Scale = ncardBundle.smallScale;
    ncardBundle.Bundle = bundle;
    return ncardBundle;
  }

  public override void _Ready()
  {
    this.Hitbox = (NClickableControl) ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%Hitbox"));
    this._cardHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cards"));
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnFocused)), 0U);
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnUnfocused)), 0U);
    ((GodotObject) this.Hitbox).Connect(NClickableControl.SignalName.Released, Callable.From<NClickableControl>(new Action<NClickableControl>(this.OnClicked)), 0U);
    for (int index = 0; index < this.Bundle.Count; ++index)
    {
      NCard child = NCard.Create(this.Bundle[index]);
      this._cardHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cards"));
      ((Node) this._cardHolder).AddChildSafely((Node) child);
      child.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      NCard ncard = child;
      ncard.Position = Vector2.op_Addition(ncard.Position, Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(-1f, 1f), 45f), (float) index - (float) this.Bundle.Count / 2f));
      float num = (float) (0.5 + (double) index / (double) (this.Bundle.Count - 1) * 0.5);
      ((CanvasItem) child).Modulate = new Color(num, num, num, 1f);
      this._cardNodes.Add(child);
    }
  }

  public IReadOnlyList<NCard> RemoveCardNodes()
  {
    this._cardTween?.Kill();
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    foreach (GodotObject cardNode in this._cardNodes)
      this._cardTween.TweenProperty(cardNode, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.15000000596046448);
    return this.CardNodes;
  }

  public void ReAddCardNodes()
  {
    this._cardTween?.Kill();
    this._cardTween = ((Node) this).CreateTween().SetParallel(true);
    for (int index = 0; index < this._cardNodes.Count; ++index)
    {
      NCard cardNode = this._cardNodes[index];
      Vector2 globalPosition = cardNode.GlobalPosition;
      Node parent = ((Node) cardNode).GetParent();
      if (parent != null)
        parent.RemoveChildSafely((Node) cardNode);
      this._cardHolder = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Cards"));
      ((Node) this._cardHolder).AddChildSafely((Node) cardNode);
      cardNode.GlobalPosition = globalPosition;
      cardNode.UpdateVisuals(PileType.None, CardPreviewMode.Normal);
      this._cardTween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("position"), Variant.op_Implicit(Vector2.op_Multiply(Vector2.op_Multiply(new Vector2(-1f, 1f), 45f), (float) index - (float) this._cardNodes.Count / 2f)), 0.40000000596046448).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
      float num = (float) (0.5 + (double) index / (double) (this._cardNodes.Count - 1) * 0.5);
      this._cardTween.TweenProperty((GodotObject) cardNode, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(num, num, num, 1f)), 0.40000000596046448);
    }
  }

  private void OnClicked(NClickableControl _)
  {
    ((GodotObject) this).EmitSignal(NCardBundle.SignalName.Clicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) this)
    });
  }

  private void OnFocused(NClickableControl _)
  {
    this._hoverTween?.Kill();
    this.Scale = this._hoverScale;
  }

  private void OnUnfocused(NClickableControl _)
  {
    this._hoverTween?.Kill();
    this._hoverTween = ((Node) this).CreateTween();
    this._hoverTween.TweenProperty((GodotObject) this, NodePath.op_Implicit("scale"), Variant.op_Implicit(this.smallScale), 0.5).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 5L);
  }

  public override void _ExitTree()
  {
    foreach (NCard cardNode in this._cardNodes)
    {
      if (((Node) this).IsAncestorOf((Node) cardNode))
        ((Node) cardNode).QueueFreeSafely();
    }
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(6)
    {
      new MethodInfo(NCardBundle.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardBundle.MethodName.ReAddCardNodes, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCardBundle.MethodName.OnClicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardBundle.MethodName.OnFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardBundle.MethodName.OnUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCardBundle.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCardBundle.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardBundle.MethodName.ReAddCardNodes) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ReAddCardNodes();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardBundle.MethodName.OnClicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnClicked(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardBundle.MethodName.OnFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnFocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCardBundle.MethodName.OnUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnUnfocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCardBundle.MethodName._ExitTree) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    ((Node) this)._ExitTree();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCardBundle.MethodName._Ready) || StringName.op_Equality(ref method, NCardBundle.MethodName.ReAddCardNodes) || StringName.op_Equality(ref method, NCardBundle.MethodName.OnClicked) || StringName.op_Equality(ref method, NCardBundle.MethodName.OnFocused) || StringName.op_Equality(ref method, NCardBundle.MethodName.OnUnfocused) || StringName.op_Equality(ref method, NCardBundle.MethodName._ExitTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName.Hitbox))
    {
      this.Hitbox = VariantUtils.ConvertTo<NClickableControl>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName._cardHolder))
    {
      this._cardHolder = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName._hoverTween))
    {
      this._hoverTween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardBundle.PropertyName._cardTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._cardTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName.Hitbox))
    {
      ref godot_variant local = ref value;
      NClickableControl hitbox = this.Hitbox;
      godot_variant from = VariantUtils.CreateFrom<NClickableControl>(ref hitbox);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName._hoverScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._hoverScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName.smallScale))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this.smallScale);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName._cardHolder))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._cardHolder);
      return true;
    }
    if (StringName.op_Equality(ref name, NCardBundle.PropertyName._hoverTween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._hoverTween);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCardBundle.PropertyName._cardTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._cardTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 5L, NCardBundle.PropertyName._hoverScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NCardBundle.PropertyName.smallScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardBundle.PropertyName.Hitbox, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardBundle.PropertyName._cardHolder, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardBundle.PropertyName._hoverTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCardBundle.PropertyName._cardTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName hitbox1 = NCardBundle.PropertyName.Hitbox;
    NClickableControl hitbox2 = this.Hitbox;
    Variant variant = Variant.From<NClickableControl>(ref hitbox2);
    serializationInfo.AddProperty(hitbox1, variant);
    info.AddProperty(NCardBundle.PropertyName._cardHolder, Variant.From<Control>(ref this._cardHolder));
    info.AddProperty(NCardBundle.PropertyName._hoverTween, Variant.From<Tween>(ref this._hoverTween));
    info.AddProperty(NCardBundle.PropertyName._cardTween, Variant.From<Tween>(ref this._cardTween));
    info.AddSignalEventDelegate(NCardBundle.SignalName.Clicked, (Delegate) this.backing_Clicked);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCardBundle.PropertyName.Hitbox, ref variant1))
      this.Hitbox = ((Variant) ref variant1).As<NClickableControl>();
    Variant variant2;
    if (info.TryGetProperty(NCardBundle.PropertyName._cardHolder, ref variant2))
      this._cardHolder = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NCardBundle.PropertyName._hoverTween, ref variant3))
      this._hoverTween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NCardBundle.PropertyName._cardTween, ref variant4))
      this._cardTween = ((Variant) ref variant4).As<Tween>();
    NCardBundle.ClickedEventHandler clickedEventHandler;
    if (!info.TryGetSignalEventDelegate<NCardBundle.ClickedEventHandler>(NCardBundle.SignalName.Clicked, ref clickedEventHandler))
      return;
    this.backing_Clicked = clickedEventHandler;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<MethodInfo> GetGodotSignalList()
  {
    return new List<MethodInfo>(1)
    {
      new MethodInfo(NCardBundle.SignalName.Clicked, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("cardHolder"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  public event NCardBundle.ClickedEventHandler Clicked
  {
    add => this.backing_Clicked += value;
    remove => this.backing_Clicked -= value;
  }

  protected void EmitSignalClicked(NCardBundle cardHolder)
  {
    ((GodotObject) this).EmitSignal(NCardBundle.SignalName.Clicked, new Variant[1]
    {
      Variant.op_Implicit((GodotObject) cardHolder)
    });
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RaiseGodotClassSignalCallbacks(
    in godot_string_name signal,
    NativeVariantPtrArgs args)
  {
    if (StringName.op_Equality(ref signal, NCardBundle.SignalName.Clicked) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      NCardBundle.ClickedEventHandler backingClicked = this.backing_Clicked;
      if (backingClicked == null)
        return;
      backingClicked(VariantUtils.ConvertTo<NCardBundle>(ref ((NativeVariantPtrArgs) ref args)[0]));
    }
    else
      ((GodotObject) this).RaiseGodotClassSignalCallbacks(ref signal, args);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassSignal(in godot_string_name signal)
  {
    return StringName.op_Equality(ref signal, NCardBundle.SignalName.Clicked) || base.HasGodotClassSignal(ref signal);
  }

  [Signal]
  public delegate void ClickedEventHandler(
  #nullable enable
  NCardBundle cardHolder);

  public class MethodName : Control.MethodName
  {
    public static readonly 
    #nullable disable
    StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName ReAddCardNodes = StringName.op_Implicit(nameof (ReAddCardNodes));
    public static readonly StringName OnClicked = StringName.op_Implicit(nameof (OnClicked));
    public static readonly StringName OnFocused = StringName.op_Implicit(nameof (OnFocused));
    public static readonly StringName OnUnfocused = StringName.op_Implicit(nameof (OnUnfocused));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName Hitbox = StringName.op_Implicit(nameof (Hitbox));
    public static readonly StringName _hoverScale = StringName.op_Implicit(nameof (_hoverScale));
    public static readonly StringName smallScale = StringName.op_Implicit(nameof (smallScale));
    public static readonly StringName _cardHolder = StringName.op_Implicit(nameof (_cardHolder));
    public static readonly StringName _hoverTween = StringName.op_Implicit(nameof (_hoverTween));
    public static readonly StringName _cardTween = StringName.op_Implicit(nameof (_cardTween));
  }

  public class SignalName : Control.SignalName
  {
    public static readonly StringName Clicked = StringName.op_Implicit(nameof (Clicked));
  }
}
