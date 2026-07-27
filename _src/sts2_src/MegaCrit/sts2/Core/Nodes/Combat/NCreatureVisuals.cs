// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Combat.NCreatureVisuals
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Bindings.MegaSpine;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Combat;

[ScriptPath("res://src/Core/Nodes/Combat/NCreatureVisuals.cs")]
public class NCreatureVisuals : Node2D
{
  private static readonly StringName _overlayInfluence = new StringName("overlay_influence");
  private static readonly StringName _h = new StringName("h");
  private static readonly StringName _tint = new StringName("tint");
  private const double _baseLiquidOverlayDuration = 1.0;
  private Node2D _body;
  private Node2D? _phobiaModeBody;
  private float _hue = 1f;
  private double _liquidOverlayTimer;
  private Material? _savedNormalMaterial;
  private ShaderMaterial? _currentLiquidOverlayMaterial;
  private CancellationTokenSource _cts = new CancellationTokenSource();

  public Node2D GetCurrentBody()
  {
    Node2D phobiaModeBody = this._phobiaModeBody;
    return phobiaModeBody == null || !((CanvasItem) phobiaModeBody).Visible ? this._body : this._phobiaModeBody;
  }

  public Control Bounds { get; private set; }

  public Marker2D IntentPosition { get; private set; }

  public Marker2D OrbPosition { get; private set; }

  public Marker2D? TalkPosition { get; private set; }

  private bool IsSpineNode
  {
    get
    {
      return GodotObject.IsInstanceValid((GodotObject) this._body) && ((GodotObject) this._body).GetClass() == "SpineSprite";
    }
  }

  public bool HasSpineAnimation => this.SpineBody != null;

  public bool IsUsingPhobiaModeBody => this._phobiaModeBody == this.GetCurrentBody();

  public MegaSprite? SpineBody { get; private set; }

  public SpineAnimationAccess SpineAnimation => new SpineAnimationAccess(this.SpineBody);

  public Marker2D VfxSpawnPosition { get; private set; }

  public float DefaultScale { get; set; } = 1f;

  public override void _Ready()
  {
    this._body = ((Node) this).GetNode<Node2D>(NodePath.op_Implicit("%Visuals"));
    this._phobiaModeBody = ((Node) this).GetNodeOrNull<Node2D>(NodePath.op_Implicit("%PhobiaModeVisuals"));
    this.Bounds = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%Bounds"));
    this.IntentPosition = ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("%IntentPos"));
    this.VfxSpawnPosition = ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("%CenterPos"));
    this.OrbPosition = ((Node) this).HasNode(NodePath.op_Implicit("%OrbPos")) ? ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("%OrbPos")) : this.IntentPosition;
    this.TalkPosition = ((Node) this).HasNode(NodePath.op_Implicit("%TalkPos")) ? ((Node) this).GetNode<Marker2D>(NodePath.op_Implicit("%TalkPos")) : (Marker2D) null;
    if (this.IsSpineNode)
    {
      this.SpineBody = new MegaSprite(Variant.op_Implicit((GodotObject) this._body));
      if (this.SpineBody.GetSkeleton()?.GetData() == null)
      {
        GD.PushWarning($"Spine skeleton data failed to load for {((Node) this).Name}, disabling spine animation.");
        this.SpineBody = (MegaSprite) null;
      }
    }
    this._savedNormalMaterial = (Material) null;
    this._currentLiquidOverlayMaterial = (ShaderMaterial) null;
  }

  public override void _EnterTree() => this._cts = new CancellationTokenSource();

  public override void _ExitTree() => this._cts.Cancel();

  public void UpdatePhobiaMode(MonsterModel? model)
  {
    if (this._phobiaModeBody != null)
    {
      ((CanvasItem) this._phobiaModeBody).Visible = SaveManager.Instance.PrefsSave.PhobiaMode;
      ((CanvasItem) this._body).Visible = !((CanvasItem) this._phobiaModeBody).Visible;
    }
    if (this.SpineBody == null)
      return;
    MegaSkeleton skeleton = this.SpineBody.GetSkeleton();
    if (skeleton == null || model == null)
      return;
    model.OnPhobiaModeToggled(SaveManager.Instance.PrefsSave.PhobiaMode, this.SpineBody, skeleton);
  }

  public void SetUpSkin(MonsterModel model)
  {
    if (this.SpineBody == null)
      return;
    MegaSkeleton skeleton = this.SpineBody.GetSkeleton();
    if (skeleton == null)
      return;
    model.SetupSkins(this.SpineBody, skeleton);
  }

  public void SetScaleAndHue(float scale, float hue)
  {
    this.DefaultScale = scale;
    this.Scale = Vector2.op_Multiply(Vector2.One, scale);
    this._hue = hue;
    if (Mathf.IsEqualApprox(hue, 0.0f) || this.SpineBody == null)
      return;
    Material normalMaterial = this.SpineBody.GetNormalMaterial();
    ShaderMaterial shaderMaterial;
    if (normalMaterial == null)
    {
      shaderMaterial = (ShaderMaterial) ((Resource) PreloadManager.Cache.GetMaterial("res://materials/vfx/hsv.tres")).Duplicate(false);
      this.SpineBody.SetNormalMaterial((Material) shaderMaterial);
    }
    else
      shaderMaterial = (ShaderMaterial) normalMaterial;
    shaderMaterial.SetShaderParameter(NCreatureVisuals._h, Variant.op_Implicit(hue));
  }

  public bool IsPlayingHurtAnimation() => this.SpineAnimation.GetCurrentAnimationName() == "hurt";

  public void TryApplyLiquidOverlay(Color tint)
  {
    if (this._currentLiquidOverlayMaterial != null)
    {
      this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._tint, Variant.op_Implicit(tint));
      this._liquidOverlayTimer = 1.0;
    }
    else
      TaskHelper.RunSafely(this.ApplyLiquidOverlayInternal(tint));
  }

  private async Task ApplyLiquidOverlayInternal(Color tint)
  {
    if (this.SpineBody == null)
      return;
    this._savedNormalMaterial = this.SpineBody.GetNormalMaterial();
    this._currentLiquidOverlayMaterial = (ShaderMaterial) ((Resource) PreloadManager.Cache.GetMaterial("res://materials/vfx/potion/potion_liquid_overlay.tres")).Duplicate(false);
    this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._tint, Variant.op_Implicit(tint));
    this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._h, Variant.op_Implicit(this._hue));
    this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._overlayInfluence, Variant.op_Implicit(1f));
    this.SpineBody.SetNormalMaterial((Material) this._currentLiquidOverlayMaterial);
    this._liquidOverlayTimer = 1.0;
    while (this._liquidOverlayTimer > 0.0)
    {
      double num1 = (1.0 - this._liquidOverlayTimer) / 1.0;
      this._currentLiquidOverlayMaterial.SetShaderParameter(NCreatureVisuals._overlayInfluence, Variant.op_Implicit(1.0 - num1));
      this._liquidOverlayTimer -= ((Node) this).GetProcessDeltaTime();
      double num2 = (double) await ((Node) this).AwaitProcessFrame(this._cts.Token);
    }
    this.SpineBody.SetNormalMaterial(this._savedNormalMaterial);
    this._currentLiquidOverlayMaterial = (ShaderMaterial) null;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(7)
    {
      new MethodInfo(NCreatureVisuals.MethodName.GetCurrentBody, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node2D"), false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName.SetScaleAndHue, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("scale"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false),
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("hue"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName.IsPlayingHurtAnimation, new PropertyInfo((Variant.Type) 1L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreatureVisuals.MethodName.TryApplyLiquidOverlay, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("tint"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName.GetCurrentBody) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      Node2D currentBody = this.GetCurrentBody();
      ret = VariantUtils.CreateFrom<Node2D>(ref currentBody);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName.SetScaleAndHue) && ((NativeVariantPtrArgs) ref args).Count == 2)
    {
      this.SetScaleAndHue(VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[0]), VariantUtils.ConvertTo<float>(ref ((NativeVariantPtrArgs) ref args)[1]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreatureVisuals.MethodName.IsPlayingHurtAnimation) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      bool flag = this.IsPlayingHurtAnimation();
      ret = VariantUtils.CreateFrom<bool>(ref flag);
      return true;
    }
    if (!StringName.op_Equality(ref method, NCreatureVisuals.MethodName.TryApplyLiquidOverlay) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.TryApplyLiquidOverlay(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCreatureVisuals.MethodName.GetCurrentBody) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName._Ready) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName._EnterTree) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName._ExitTree) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName.SetScaleAndHue) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName.IsPlayingHurtAnimation) || StringName.op_Equality(ref method, NCreatureVisuals.MethodName.TryApplyLiquidOverlay) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.Bounds))
    {
      this.Bounds = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.IntentPosition))
    {
      this.IntentPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.OrbPosition))
    {
      this.OrbPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.TalkPosition))
    {
      this.TalkPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.VfxSpawnPosition))
    {
      this.VfxSpawnPosition = VariantUtils.ConvertTo<Marker2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.DefaultScale))
    {
      this.DefaultScale = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._body))
    {
      this._body = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._phobiaModeBody))
    {
      this._phobiaModeBody = VariantUtils.ConvertTo<Node2D>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._hue))
    {
      this._hue = VariantUtils.ConvertTo<float>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._liquidOverlayTimer))
    {
      this._liquidOverlayTimer = VariantUtils.ConvertTo<double>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._savedNormalMaterial))
    {
      this._savedNormalMaterial = VariantUtils.ConvertTo<Material>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentLiquidOverlayMaterial = VariantUtils.ConvertTo<ShaderMaterial>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.Bounds))
    {
      ref godot_variant local = ref value;
      Control bounds = this.Bounds;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref bounds);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.IntentPosition))
    {
      ref godot_variant local = ref value;
      Marker2D intentPosition = this.IntentPosition;
      godot_variant from = VariantUtils.CreateFrom<Marker2D>(ref intentPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.OrbPosition))
    {
      ref godot_variant local = ref value;
      Marker2D orbPosition = this.OrbPosition;
      godot_variant from = VariantUtils.CreateFrom<Marker2D>(ref orbPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.TalkPosition))
    {
      ref godot_variant local = ref value;
      Marker2D talkPosition = this.TalkPosition;
      godot_variant from = VariantUtils.CreateFrom<Marker2D>(ref talkPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.IsSpineNode))
    {
      ref godot_variant local = ref value;
      bool isSpineNode = this.IsSpineNode;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isSpineNode);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.HasSpineAnimation))
    {
      ref godot_variant local = ref value;
      bool hasSpineAnimation = this.HasSpineAnimation;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref hasSpineAnimation);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.IsUsingPhobiaModeBody))
    {
      ref godot_variant local = ref value;
      bool usingPhobiaModeBody = this.IsUsingPhobiaModeBody;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref usingPhobiaModeBody);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.VfxSpawnPosition))
    {
      ref godot_variant local = ref value;
      Marker2D vfxSpawnPosition = this.VfxSpawnPosition;
      godot_variant from = VariantUtils.CreateFrom<Marker2D>(ref vfxSpawnPosition);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName.DefaultScale))
    {
      ref godot_variant local = ref value;
      float defaultScale = this.DefaultScale;
      godot_variant from = VariantUtils.CreateFrom<float>(ref defaultScale);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._body))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._body);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._phobiaModeBody))
    {
      value = VariantUtils.CreateFrom<Node2D>(ref this._phobiaModeBody);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._hue))
    {
      value = VariantUtils.CreateFrom<float>(ref this._hue);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._liquidOverlayTimer))
    {
      value = VariantUtils.CreateFrom<double>(ref this._liquidOverlayTimer);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._savedNormalMaterial))
    {
      value = VariantUtils.CreateFrom<Material>(ref this._savedNormalMaterial);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<ShaderMaterial>(ref this._currentLiquidOverlayMaterial);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName._body, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName._phobiaModeBody, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName.Bounds, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName.IntentPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName.OrbPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName.TalkPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreatureVisuals.PropertyName.IsSpineNode, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreatureVisuals.PropertyName.HasSpineAnimation, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreatureVisuals.PropertyName.IsUsingPhobiaModeBody, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName.VfxSpawnPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCreatureVisuals.PropertyName.DefaultScale, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCreatureVisuals.PropertyName._hue, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCreatureVisuals.PropertyName._liquidOverlayTimer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName._savedNormalMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName bounds1 = NCreatureVisuals.PropertyName.Bounds;
    Control bounds2 = this.Bounds;
    Variant variant1 = Variant.From<Control>(ref bounds2);
    serializationInfo1.AddProperty(bounds1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName intentPosition1 = NCreatureVisuals.PropertyName.IntentPosition;
    Marker2D intentPosition2 = this.IntentPosition;
    Variant variant2 = Variant.From<Marker2D>(ref intentPosition2);
    serializationInfo2.AddProperty(intentPosition1, variant2);
    GodotSerializationInfo serializationInfo3 = info;
    StringName orbPosition1 = NCreatureVisuals.PropertyName.OrbPosition;
    Marker2D orbPosition2 = this.OrbPosition;
    Variant variant3 = Variant.From<Marker2D>(ref orbPosition2);
    serializationInfo3.AddProperty(orbPosition1, variant3);
    GodotSerializationInfo serializationInfo4 = info;
    StringName talkPosition1 = NCreatureVisuals.PropertyName.TalkPosition;
    Marker2D talkPosition2 = this.TalkPosition;
    Variant variant4 = Variant.From<Marker2D>(ref talkPosition2);
    serializationInfo4.AddProperty(talkPosition1, variant4);
    GodotSerializationInfo serializationInfo5 = info;
    StringName vfxSpawnPosition1 = NCreatureVisuals.PropertyName.VfxSpawnPosition;
    Marker2D vfxSpawnPosition2 = this.VfxSpawnPosition;
    Variant variant5 = Variant.From<Marker2D>(ref vfxSpawnPosition2);
    serializationInfo5.AddProperty(vfxSpawnPosition1, variant5);
    GodotSerializationInfo serializationInfo6 = info;
    StringName defaultScale1 = NCreatureVisuals.PropertyName.DefaultScale;
    float defaultScale2 = this.DefaultScale;
    Variant variant6 = Variant.From<float>(ref defaultScale2);
    serializationInfo6.AddProperty(defaultScale1, variant6);
    info.AddProperty(NCreatureVisuals.PropertyName._body, Variant.From<Node2D>(ref this._body));
    info.AddProperty(NCreatureVisuals.PropertyName._phobiaModeBody, Variant.From<Node2D>(ref this._phobiaModeBody));
    info.AddProperty(NCreatureVisuals.PropertyName._hue, Variant.From<float>(ref this._hue));
    info.AddProperty(NCreatureVisuals.PropertyName._liquidOverlayTimer, Variant.From<double>(ref this._liquidOverlayTimer));
    info.AddProperty(NCreatureVisuals.PropertyName._savedNormalMaterial, Variant.From<Material>(ref this._savedNormalMaterial));
    info.AddProperty(NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, Variant.From<ShaderMaterial>(ref this._currentLiquidOverlayMaterial));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.Bounds, ref variant1))
      this.Bounds = ((Variant) ref variant1).As<Control>();
    Variant variant2;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.IntentPosition, ref variant2))
      this.IntentPosition = ((Variant) ref variant2).As<Marker2D>();
    Variant variant3;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.OrbPosition, ref variant3))
      this.OrbPosition = ((Variant) ref variant3).As<Marker2D>();
    Variant variant4;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.TalkPosition, ref variant4))
      this.TalkPosition = ((Variant) ref variant4).As<Marker2D>();
    Variant variant5;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.VfxSpawnPosition, ref variant5))
      this.VfxSpawnPosition = ((Variant) ref variant5).As<Marker2D>();
    Variant variant6;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName.DefaultScale, ref variant6))
      this.DefaultScale = ((Variant) ref variant6).As<float>();
    Variant variant7;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName._body, ref variant7))
      this._body = ((Variant) ref variant7).As<Node2D>();
    Variant variant8;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName._phobiaModeBody, ref variant8))
      this._phobiaModeBody = ((Variant) ref variant8).As<Node2D>();
    Variant variant9;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName._hue, ref variant9))
      this._hue = ((Variant) ref variant9).As<float>();
    Variant variant10;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName._liquidOverlayTimer, ref variant10))
      this._liquidOverlayTimer = ((Variant) ref variant10).As<double>();
    Variant variant11;
    if (info.TryGetProperty(NCreatureVisuals.PropertyName._savedNormalMaterial, ref variant11))
      this._savedNormalMaterial = ((Variant) ref variant11).As<Material>();
    Variant variant12;
    if (!info.TryGetProperty(NCreatureVisuals.PropertyName._currentLiquidOverlayMaterial, ref variant12))
      return;
    this._currentLiquidOverlayMaterial = ((Variant) ref variant12).As<ShaderMaterial>();
  }

  public class MethodName : Node2D.MethodName
  {
    public static readonly StringName GetCurrentBody = StringName.op_Implicit(nameof (GetCurrentBody));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName SetScaleAndHue = StringName.op_Implicit(nameof (SetScaleAndHue));
    public static readonly StringName IsPlayingHurtAnimation = StringName.op_Implicit(nameof (IsPlayingHurtAnimation));
    public static readonly StringName TryApplyLiquidOverlay = StringName.op_Implicit(nameof (TryApplyLiquidOverlay));
  }

  public class PropertyName : Node2D.PropertyName
  {
    public static readonly StringName Bounds = StringName.op_Implicit(nameof (Bounds));
    public static readonly StringName IntentPosition = StringName.op_Implicit(nameof (IntentPosition));
    public static readonly StringName OrbPosition = StringName.op_Implicit(nameof (OrbPosition));
    public static readonly StringName TalkPosition = StringName.op_Implicit(nameof (TalkPosition));
    public static readonly StringName IsSpineNode = StringName.op_Implicit(nameof (IsSpineNode));
    public static readonly StringName HasSpineAnimation = StringName.op_Implicit(nameof (HasSpineAnimation));
    public static readonly StringName IsUsingPhobiaModeBody = StringName.op_Implicit(nameof (IsUsingPhobiaModeBody));
    public static readonly StringName VfxSpawnPosition = StringName.op_Implicit(nameof (VfxSpawnPosition));
    public static readonly StringName DefaultScale = StringName.op_Implicit(nameof (DefaultScale));
    public static readonly StringName _body = StringName.op_Implicit(nameof (_body));
    public static readonly StringName _phobiaModeBody = StringName.op_Implicit(nameof (_phobiaModeBody));
    public static readonly StringName _hue = StringName.op_Implicit(nameof (_hue));
    public static readonly StringName _liquidOverlayTimer = StringName.op_Implicit(nameof (_liquidOverlayTimer));
    public static readonly StringName _savedNormalMaterial = StringName.op_Implicit(nameof (_savedNormalMaterial));
    public static readonly StringName _currentLiquidOverlayMaterial = StringName.op_Implicit(nameof (_currentLiquidOverlayMaterial));
  }

  public class SignalName : Node2D.SignalName
  {
  }
}
