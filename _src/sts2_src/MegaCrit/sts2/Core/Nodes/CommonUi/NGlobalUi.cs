// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.CommonUi.NGlobalUi
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Nodes.Cards;
using MegaCrit.Sts2.Core.Nodes.Combat;
using MegaCrit.Sts2.Core.Nodes.Multiplayer;
using MegaCrit.Sts2.Core.Nodes.Relics;
using MegaCrit.Sts2.Core.Nodes.Screens;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Settings;
using System;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.CommonUi;

[ScriptPath("res://src/Core/Nodes/CommonUi/NGlobalUi.cs")]
public class NGlobalUi : Control
{
  private const float _maxNarrowRatio = 1.33333337f;
  private const float _maxWideRatio = 2.38888884f;
  private Window _window;

  public NTopBar TopBar { get; private set; }

  public NOverlayStack Overlays { get; private set; }

  public NCapstoneContainer CapstoneContainer { get; private set; }

  public NRelicInventory RelicInventory { get; private set; }

  public Control EventCardPreviewContainer { get; private set; }

  public Control CardPreviewContainer { get; private set; }

  public NMessyCardPreviewContainer MessyCardPreviewContainer { get; private set; }

  public NGridCardPreviewContainer GridCardPreviewContainer { get; private set; }

  public Control AboveTopBarVfxContainer { get; private set; }

  public NMapScreen MapScreen { get; private set; }

  public NMultiplayerPlayerStateContainer MultiplayerPlayerContainer { get; private set; }

  public NMultiplayerTimeoutOverlay TimeoutOverlay { get; private set; }

  public NCapstoneSubmenuStack SubmenuStack { get; private set; }

  public NTargetManager TargetManager { get; private set; }

  public override void _Ready()
  {
    this._window = ((Node) this).GetTree().Root;
    ((GodotObject) this._window).Connect(Viewport.SignalName.SizeChanged, Callable.From(new Action(this.OnWindowChange)), 0U);
    this.EventCardPreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%EventCardPreviewContainer"));
    this.CardPreviewContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%CardPreviewContainer"));
    this.MessyCardPreviewContainer = ((Node) this).GetNode<NMessyCardPreviewContainer>(NodePath.op_Implicit("%MessyCardPreviewContainer"));
    this.GridCardPreviewContainer = ((Node) this).GetNode<NGridCardPreviewContainer>(NodePath.op_Implicit("%GridCardPreviewContainer"));
    this.TopBar = ((Node) this).GetNode<NTopBar>(NodePath.op_Implicit("%TopBar"));
    this.Overlays = ((Node) this).GetNode<NOverlayStack>(NodePath.op_Implicit("%OverlayScreensContainer"));
    this.CapstoneContainer = ((Node) this).GetNode<NCapstoneContainer>(NodePath.op_Implicit("%CapstoneScreenContainer"));
    this.MapScreen = ((Node) this).GetNode<NMapScreen>(NodePath.op_Implicit("%MapScreen"));
    this.SubmenuStack = ((Node) this).GetNode<NCapstoneSubmenuStack>(NodePath.op_Implicit("%CapstoneSubmenuStack"));
    this.RelicInventory = ((Node) this).GetNode<NRelicInventory>(NodePath.op_Implicit("%RelicInventory"));
    this.MultiplayerPlayerContainer = ((Node) this).GetNode<NMultiplayerPlayerStateContainer>(NodePath.op_Implicit("%MultiplayerPlayerContainer"));
    this.TargetManager = ((Node) this).GetNode<NTargetManager>(NodePath.op_Implicit("TargetManager"));
    this.TimeoutOverlay = ((Node) this).GetNode<NMultiplayerTimeoutOverlay>(NodePath.op_Implicit("%MultiplayerTimeoutOverlay"));
    this.AboveTopBarVfxContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%AboveTopBarVfxContainer"));
  }

  private void OnWindowChange()
  {
    if (SaveManager.Instance.SettingsSave.AspectRatioSetting != AspectRatioSetting.Auto)
      return;
    float num = (float) this._window.Size.X / (float) this._window.Size.Y;
    if ((double) num > 2.3888888359069824)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 2L;
      this._window.ContentScaleSize = new Vector2I(2580, 1080);
    }
    else if ((double) num < 1.3333333730697632)
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 3L;
      this._window.ContentScaleSize = new Vector2I(1680, 1260);
    }
    else
    {
      this._window.ContentScaleAspect = (Window.ContentScaleAspectEnum) 4L;
      this._window.ContentScaleSize = new Vector2I(1680, 1080);
    }
  }

  public void ReparentCard(NCard card)
  {
    Vector2 globalPosition = card.GlobalPosition;
    Node parent = ((Node) card).GetParent();
    if (parent != null)
      parent.RemoveChildSafely((Node) card);
    this.TopBar.TrailContainer.AddChildSafely((Node) card);
    card.GlobalPosition = globalPosition;
  }

  public void Initialize(RunState runState)
  {
    this.TopBar.Initialize((IRunState) runState);
    this.MultiplayerPlayerContainer.Initialize(runState);
    this.RelicInventory.Initialize(runState);
    this.MapScreen.Initialize(runState);
    this.TimeoutOverlay.Initialize(RunManager.Instance.NetService, false);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(3)
    {
      new MethodInfo(NGlobalUi.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalUi.MethodName.OnWindowChange, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NGlobalUi.MethodName.ReparentCard, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("card"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NGlobalUi.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NGlobalUi.MethodName.OnWindowChange) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnWindowChange();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NGlobalUi.MethodName.ReparentCard) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.ReparentCard(VariantUtils.ConvertTo<NCard>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NGlobalUi.MethodName._Ready) || StringName.op_Equality(ref method, NGlobalUi.MethodName.OnWindowChange) || StringName.op_Equality(ref method, NGlobalUi.MethodName.ReparentCard) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TopBar))
    {
      this.TopBar = VariantUtils.ConvertTo<NTopBar>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.Overlays))
    {
      this.Overlays = VariantUtils.ConvertTo<NOverlayStack>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.CapstoneContainer))
    {
      this.CapstoneContainer = VariantUtils.ConvertTo<NCapstoneContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.RelicInventory))
    {
      this.RelicInventory = VariantUtils.ConvertTo<NRelicInventory>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.EventCardPreviewContainer))
    {
      this.EventCardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.CardPreviewContainer))
    {
      this.CardPreviewContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MessyCardPreviewContainer))
    {
      this.MessyCardPreviewContainer = VariantUtils.ConvertTo<NMessyCardPreviewContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.GridCardPreviewContainer))
    {
      this.GridCardPreviewContainer = VariantUtils.ConvertTo<NGridCardPreviewContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.AboveTopBarVfxContainer))
    {
      this.AboveTopBarVfxContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MapScreen))
    {
      this.MapScreen = VariantUtils.ConvertTo<NMapScreen>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MultiplayerPlayerContainer))
    {
      this.MultiplayerPlayerContainer = VariantUtils.ConvertTo<NMultiplayerPlayerStateContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TimeoutOverlay))
    {
      this.TimeoutOverlay = VariantUtils.ConvertTo<NMultiplayerTimeoutOverlay>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.SubmenuStack))
    {
      this.SubmenuStack = VariantUtils.ConvertTo<NCapstoneSubmenuStack>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TargetManager))
    {
      this.TargetManager = VariantUtils.ConvertTo<NTargetManager>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlobalUi.PropertyName._window))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._window = VariantUtils.ConvertTo<Window>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TopBar))
    {
      ref godot_variant local = ref value;
      NTopBar topBar = this.TopBar;
      godot_variant from = VariantUtils.CreateFrom<NTopBar>(ref topBar);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.Overlays))
    {
      ref godot_variant local = ref value;
      NOverlayStack overlays = this.Overlays;
      godot_variant from = VariantUtils.CreateFrom<NOverlayStack>(ref overlays);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.CapstoneContainer))
    {
      ref godot_variant local = ref value;
      NCapstoneContainer capstoneContainer = this.CapstoneContainer;
      godot_variant from = VariantUtils.CreateFrom<NCapstoneContainer>(ref capstoneContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.RelicInventory))
    {
      ref godot_variant local = ref value;
      NRelicInventory relicInventory = this.RelicInventory;
      godot_variant from = VariantUtils.CreateFrom<NRelicInventory>(ref relicInventory);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.EventCardPreviewContainer))
    {
      ref godot_variant local = ref value;
      Control previewContainer = this.EventCardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.CardPreviewContainer))
    {
      ref godot_variant local = ref value;
      Control previewContainer = this.CardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MessyCardPreviewContainer))
    {
      ref godot_variant local = ref value;
      NMessyCardPreviewContainer previewContainer = this.MessyCardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<NMessyCardPreviewContainer>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.GridCardPreviewContainer))
    {
      ref godot_variant local = ref value;
      NGridCardPreviewContainer previewContainer = this.GridCardPreviewContainer;
      godot_variant from = VariantUtils.CreateFrom<NGridCardPreviewContainer>(ref previewContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.AboveTopBarVfxContainer))
    {
      ref godot_variant local = ref value;
      Control topBarVfxContainer = this.AboveTopBarVfxContainer;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref topBarVfxContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MapScreen))
    {
      ref godot_variant local = ref value;
      NMapScreen mapScreen = this.MapScreen;
      godot_variant from = VariantUtils.CreateFrom<NMapScreen>(ref mapScreen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.MultiplayerPlayerContainer))
    {
      ref godot_variant local = ref value;
      NMultiplayerPlayerStateContainer multiplayerPlayerContainer = this.MultiplayerPlayerContainer;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerPlayerStateContainer>(ref multiplayerPlayerContainer);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TimeoutOverlay))
    {
      ref godot_variant local = ref value;
      NMultiplayerTimeoutOverlay timeoutOverlay = this.TimeoutOverlay;
      godot_variant from = VariantUtils.CreateFrom<NMultiplayerTimeoutOverlay>(ref timeoutOverlay);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.SubmenuStack))
    {
      ref godot_variant local = ref value;
      NCapstoneSubmenuStack submenuStack = this.SubmenuStack;
      godot_variant from = VariantUtils.CreateFrom<NCapstoneSubmenuStack>(ref submenuStack);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NGlobalUi.PropertyName.TargetManager))
    {
      ref godot_variant local = ref value;
      NTargetManager targetManager = this.TargetManager;
      godot_variant from = VariantUtils.CreateFrom<NTargetManager>(ref targetManager);
      local = from;
      return true;
    }
    if (!StringName.op_Equality(ref name, NGlobalUi.PropertyName._window))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Window>(ref this._window);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName._window, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.TopBar, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.Overlays, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.CapstoneContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.RelicInventory, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.EventCardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.CardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.MessyCardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.GridCardPreviewContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.AboveTopBarVfxContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.MapScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.MultiplayerPlayerContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.TimeoutOverlay, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.SubmenuStack, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NGlobalUi.PropertyName.TargetManager, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName topBar1 = NGlobalUi.PropertyName.TopBar;
    NTopBar topBar2 = this.TopBar;
    Variant variant1 = Variant.From<NTopBar>(ref topBar2);
    serializationInfo1.AddProperty(topBar1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName overlays1 = NGlobalUi.PropertyName.Overlays;
    NOverlayStack overlays2 = this.Overlays;
    Variant variant2 = Variant.From<NOverlayStack>(ref overlays2);
    serializationInfo2.AddProperty(overlays1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName capstoneContainer1 = NGlobalUi.PropertyName.CapstoneContainer;
    NCapstoneContainer capstoneContainer2 = this.CapstoneContainer;
    Variant variant3 = Variant.From<NCapstoneContainer>(ref capstoneContainer2);
    serializationInfo3.AddProperty(capstoneContainer1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName relicInventory1 = NGlobalUi.PropertyName.RelicInventory;
    NRelicInventory relicInventory2 = this.RelicInventory;
    Variant variant4 = Variant.From<NRelicInventory>(ref relicInventory2);
    serializationInfo4.AddProperty(relicInventory1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName previewContainer1 = NGlobalUi.PropertyName.EventCardPreviewContainer;
    Control previewContainer2 = this.EventCardPreviewContainer;
    Variant variant5 = Variant.From<Control>(ref previewContainer2);
    serializationInfo5.AddProperty(previewContainer1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName previewContainer3 = NGlobalUi.PropertyName.CardPreviewContainer;
    Control previewContainer4 = this.CardPreviewContainer;
    Variant variant6 = Variant.From<Control>(ref previewContainer4);
    serializationInfo6.AddProperty(previewContainer3, variant6);
    GodotSerializationInfo serializationInfo7 = info;
    StringName previewContainer5 = NGlobalUi.PropertyName.MessyCardPreviewContainer;
    NMessyCardPreviewContainer previewContainer6 = this.MessyCardPreviewContainer;
    Variant variant7 = Variant.From<NMessyCardPreviewContainer>(ref previewContainer6);
    serializationInfo7.AddProperty(previewContainer5, variant7);
    GodotSerializationInfo serializationInfo8 = info;
    StringName previewContainer7 = NGlobalUi.PropertyName.GridCardPreviewContainer;
    NGridCardPreviewContainer previewContainer8 = this.GridCardPreviewContainer;
    Variant variant8 = Variant.From<NGridCardPreviewContainer>(ref previewContainer8);
    serializationInfo8.AddProperty(previewContainer7, variant8);
    GodotSerializationInfo serializationInfo9 = info;
    StringName topBarVfxContainer1 = NGlobalUi.PropertyName.AboveTopBarVfxContainer;
    Control topBarVfxContainer2 = this.AboveTopBarVfxContainer;
    Variant variant9 = Variant.From<Control>(ref topBarVfxContainer2);
    serializationInfo9.AddProperty(topBarVfxContainer1, variant9);
    GodotSerializationInfo serializationInfo10 = info;
    StringName mapScreen1 = NGlobalUi.PropertyName.MapScreen;
    NMapScreen mapScreen2 = this.MapScreen;
    Variant variant10 = Variant.From<NMapScreen>(ref mapScreen2);
    serializationInfo10.AddProperty(mapScreen1, variant10);
    GodotSerializationInfo serializationInfo11 = info;
    StringName multiplayerPlayerContainer1 = NGlobalUi.PropertyName.MultiplayerPlayerContainer;
    NMultiplayerPlayerStateContainer multiplayerPlayerContainer2 = this.MultiplayerPlayerContainer;
    Variant variant11 = Variant.From<NMultiplayerPlayerStateContainer>(ref multiplayerPlayerContainer2);
    serializationInfo11.AddProperty(multiplayerPlayerContainer1, variant11);
    GodotSerializationInfo serializationInfo12 = info;
    StringName timeoutOverlay1 = NGlobalUi.PropertyName.TimeoutOverlay;
    NMultiplayerTimeoutOverlay timeoutOverlay2 = this.TimeoutOverlay;
    Variant variant12 = Variant.From<NMultiplayerTimeoutOverlay>(ref timeoutOverlay2);
    serializationInfo12.AddProperty(timeoutOverlay1, variant12);
    GodotSerializationInfo serializationInfo13 = info;
    StringName submenuStack1 = NGlobalUi.PropertyName.SubmenuStack;
    NCapstoneSubmenuStack submenuStack2 = this.SubmenuStack;
    Variant variant13 = Variant.From<NCapstoneSubmenuStack>(ref submenuStack2);
    serializationInfo13.AddProperty(submenuStack1, variant13);
    GodotSerializationInfo serializationInfo14 = info;
    StringName targetManager1 = NGlobalUi.PropertyName.TargetManager;
    NTargetManager targetManager2 = this.TargetManager;
    Variant variant14 = Variant.From<NTargetManager>(ref targetManager2);
    serializationInfo14.AddProperty(targetManager1, variant14);
    info.AddProperty(NGlobalUi.PropertyName._window, Variant.From<Window>(ref this._window));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NGlobalUi.PropertyName.TopBar, ref variant1))
      this.TopBar = ((Variant) ref variant1).As<NTopBar>();
    Variant variant2;
    if (info.TryGetProperty(NGlobalUi.PropertyName.Overlays, ref variant2))
      this.Overlays = ((Variant) ref variant2).As<NOverlayStack>();
    Variant variant3;
    if (info.TryGetProperty(NGlobalUi.PropertyName.CapstoneContainer, ref variant3))
      this.CapstoneContainer = ((Variant) ref variant3).As<NCapstoneContainer>();
    Variant variant4;
    if (info.TryGetProperty(NGlobalUi.PropertyName.RelicInventory, ref variant4))
      this.RelicInventory = ((Variant) ref variant4).As<NRelicInventory>();
    Variant variant5;
    if (info.TryGetProperty(NGlobalUi.PropertyName.EventCardPreviewContainer, ref variant5))
      this.EventCardPreviewContainer = ((Variant) ref variant5).As<Control>();
    Variant variant6;
    if (info.TryGetProperty(NGlobalUi.PropertyName.CardPreviewContainer, ref variant6))
      this.CardPreviewContainer = ((Variant) ref variant6).As<Control>();
    Variant variant7;
    if (info.TryGetProperty(NGlobalUi.PropertyName.MessyCardPreviewContainer, ref variant7))
      this.MessyCardPreviewContainer = ((Variant) ref variant7).As<NMessyCardPreviewContainer>();
    Variant variant8;
    if (info.TryGetProperty(NGlobalUi.PropertyName.GridCardPreviewContainer, ref variant8))
      this.GridCardPreviewContainer = ((Variant) ref variant8).As<NGridCardPreviewContainer>();
    Variant variant9;
    if (info.TryGetProperty(NGlobalUi.PropertyName.AboveTopBarVfxContainer, ref variant9))
      this.AboveTopBarVfxContainer = ((Variant) ref variant9).As<Control>();
    Variant variant10;
    if (info.TryGetProperty(NGlobalUi.PropertyName.MapScreen, ref variant10))
      this.MapScreen = ((Variant) ref variant10).As<NMapScreen>();
    Variant variant11;
    if (info.TryGetProperty(NGlobalUi.PropertyName.MultiplayerPlayerContainer, ref variant11))
      this.MultiplayerPlayerContainer = ((Variant) ref variant11).As<NMultiplayerPlayerStateContainer>();
    Variant variant12;
    if (info.TryGetProperty(NGlobalUi.PropertyName.TimeoutOverlay, ref variant12))
      this.TimeoutOverlay = ((Variant) ref variant12).As<NMultiplayerTimeoutOverlay>();
    Variant variant13;
    if (info.TryGetProperty(NGlobalUi.PropertyName.SubmenuStack, ref variant13))
      this.SubmenuStack = ((Variant) ref variant13).As<NCapstoneSubmenuStack>();
    Variant variant14;
    if (info.TryGetProperty(NGlobalUi.PropertyName.TargetManager, ref variant14))
      this.TargetManager = ((Variant) ref variant14).As<NTargetManager>();
    Variant variant15;
    if (!info.TryGetProperty(NGlobalUi.PropertyName._window, ref variant15))
      return;
    this._window = ((Variant) ref variant15).As<Window>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName OnWindowChange = StringName.op_Implicit(nameof (OnWindowChange));
    public static readonly StringName ReparentCard = StringName.op_Implicit(nameof (ReparentCard));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName TopBar = StringName.op_Implicit(nameof (TopBar));
    public static readonly StringName Overlays = StringName.op_Implicit(nameof (Overlays));
    public static readonly StringName CapstoneContainer = StringName.op_Implicit(nameof (CapstoneContainer));
    public static readonly StringName RelicInventory = StringName.op_Implicit(nameof (RelicInventory));
    public static readonly StringName EventCardPreviewContainer = StringName.op_Implicit(nameof (EventCardPreviewContainer));
    public static readonly StringName CardPreviewContainer = StringName.op_Implicit(nameof (CardPreviewContainer));
    public static readonly StringName MessyCardPreviewContainer = StringName.op_Implicit(nameof (MessyCardPreviewContainer));
    public static readonly StringName GridCardPreviewContainer = StringName.op_Implicit(nameof (GridCardPreviewContainer));
    public static readonly StringName AboveTopBarVfxContainer = StringName.op_Implicit(nameof (AboveTopBarVfxContainer));
    public static readonly StringName MapScreen = StringName.op_Implicit(nameof (MapScreen));
    public static readonly StringName MultiplayerPlayerContainer = StringName.op_Implicit(nameof (MultiplayerPlayerContainer));
    public static readonly StringName TimeoutOverlay = StringName.op_Implicit(nameof (TimeoutOverlay));
    public static readonly StringName SubmenuStack = StringName.op_Implicit(nameof (SubmenuStack));
    public static readonly StringName TargetManager = StringName.op_Implicit(nameof (TargetManager));
    public static readonly StringName _window = StringName.op_Implicit(nameof (_window));
  }

  public class SignalName : Control.SignalName
  {
  }
}
