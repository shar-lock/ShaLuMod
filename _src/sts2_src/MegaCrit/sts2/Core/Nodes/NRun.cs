// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.NRun
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Multiplayer.Game.PeerInput;
using MegaCrit.Sts2.Core.Nodes.Audio;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.Rooms;
using MegaCrit.Sts2.Core.Nodes.Screens.Capstones;
using MegaCrit.Sts2.Core.Nodes.Screens.GameOverScreen;
using MegaCrit.Sts2.Core.Nodes.Screens.Map;
using MegaCrit.Sts2.Core.Nodes.Screens.Overlays;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using System.Collections.Generic;
using System.ComponentModel;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes;

[ScriptPath("res://src/Core/Nodes/NRun.cs")]
public class NRun : Control
{
  private const string _scenePath = "res://scenes/run.tscn";
  [Export]
  private PackedScene _cardScene;
  private RunState _state;
  private NSceneContainer _roomContainer;
  private Button _testButton;
  private MegaLabel _seedLabel;

  public static IEnumerable<string> AssetPaths
  {
    get
    {
      return (IEnumerable<string>) new \u003C\u003Ez__ReadOnlySingleElementList<string>("res://scenes/run.tscn");
    }
  }

  public static NRun? Instance => NGame.Instance?.CurrentRunNode;

  public NCombatRoom? CombatRoom
  {
    get
    {
      NCombatRoom combatRoom;
      switch (this._roomContainer.CurrentScene)
      {
        case NCombatRoom ncombatRoom:
          combatRoom = ncombatRoom;
          break;
        case NEventRoom neventRoom:
          combatRoom = neventRoom.EmbeddedCombatRoom;
          break;
        default:
          combatRoom = (NCombatRoom) null;
          break;
      }
      return combatRoom;
    }
  }

  public NTreasureRoom? TreasureRoom => this._roomContainer.CurrentScene as NTreasureRoom;

  public NEventRoom? EventRoom => this._roomContainer.CurrentScene as NEventRoom;

  public NRestSiteRoom? RestSiteRoom => this._roomContainer.CurrentScene as NRestSiteRoom;

  public NMapRoom? MapRoom => this._roomContainer.CurrentScene as NMapRoom;

  public NMerchantRoom? MerchantRoom => this._roomContainer.CurrentScene as NMerchantRoom;

  public NGlobalUi GlobalUi { get; private set; }

  public NRunMusicController RunMusicController { get; private set; }

  public ScreenStateTracker ScreenStateTracker { get; private set; }

  public static NRun Create(RunState state)
  {
    NRun nrun = PreloadManager.Cache.GetScene("res://scenes/run.tscn").Instantiate<NRun>((PackedScene.GenEditState) 0L);
    nrun._state = state;
    return nrun;
  }

  public override void _Ready()
  {
    this._roomContainer = ((Node) this).GetNode<NSceneContainer>(NodePath.op_Implicit("%RoomContainer"));
    this.GlobalUi = ((Node) this).GetNode<NGlobalUi>(NodePath.op_Implicit("%GlobalUi"));
    this.GlobalUi.Initialize(this._state);
    this.ScreenStateTracker = new ScreenStateTracker(this.GlobalUi.MapScreen, this.GlobalUi.CapstoneContainer, this.GlobalUi.Overlays);
    this.RunMusicController = ((Node) this).GetNode<NRunMusicController>(NodePath.op_Implicit("%RunMusicController"));
    this.RunMusicController.SetRunState((IRunState) this._state);
    this.RunMusicController.UpdateMusic();
    this._seedLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%DebugSeed"));
    this._seedLabel.SetTextAutoSize(this._state.Rng.StringSeed);
  }

  public override void _Process(double delta) => RunManager.Instance.NetService.Update();

  public override void _Notification(int what)
  {
    if (what != 1006)
      return;
    RunManager.Instance.CleanUp(false);
  }

  public void SetCurrentRoom(Control? node)
  {
    if (node == null)
      return;
    this._roomContainer.SetCurrentScene(node);
    ActiveScreenContext.Instance.Update();
  }

  public void ShowGameOverScreen(SerializableRun serializableRun)
  {
    NCapstoneContainer.Instance.Close();
    NMapScreen.Instance.Close();
    NOverlayStack.Instance.Push((IOverlayScreen) NGameOverScreen.Create(this._state, serializableRun));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(4)
    {
      new MethodInfo(NRun.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NRun.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRun.MethodName._Notification, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("what"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NRun.MethodName.SetCurrentRoom, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NRun.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRun.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NRun.MethodName._Notification) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((GodotObject) this)._Notification(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NRun.MethodName.SetCurrentRoom) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.SetCurrentRoom(VariantUtils.ConvertTo<Control>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NRun.MethodName._Ready) || StringName.op_Equality(ref method, NRun.MethodName._Process) || StringName.op_Equality(ref method, NRun.MethodName._Notification) || StringName.op_Equality(ref method, NRun.MethodName.SetCurrentRoom) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRun.PropertyName.GlobalUi))
    {
      this.GlobalUi = VariantUtils.ConvertTo<NGlobalUi>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.RunMusicController))
    {
      this.RunMusicController = VariantUtils.ConvertTo<NRunMusicController>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._cardScene))
    {
      this._cardScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._roomContainer))
    {
      this._roomContainer = VariantUtils.ConvertTo<NSceneContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._testButton))
    {
      this._testButton = VariantUtils.ConvertTo<Button>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRun.PropertyName._seedLabel))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._seedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NRun.PropertyName.CombatRoom))
    {
      ref godot_variant local = ref value;
      NCombatRoom combatRoom = this.CombatRoom;
      godot_variant from = VariantUtils.CreateFrom<NCombatRoom>(ref combatRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.TreasureRoom))
    {
      ref godot_variant local = ref value;
      NTreasureRoom treasureRoom = this.TreasureRoom;
      godot_variant from = VariantUtils.CreateFrom<NTreasureRoom>(ref treasureRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.EventRoom))
    {
      ref godot_variant local = ref value;
      NEventRoom eventRoom = this.EventRoom;
      godot_variant from = VariantUtils.CreateFrom<NEventRoom>(ref eventRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.RestSiteRoom))
    {
      ref godot_variant local = ref value;
      NRestSiteRoom restSiteRoom = this.RestSiteRoom;
      godot_variant from = VariantUtils.CreateFrom<NRestSiteRoom>(ref restSiteRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.MapRoom))
    {
      ref godot_variant local = ref value;
      NMapRoom mapRoom = this.MapRoom;
      godot_variant from = VariantUtils.CreateFrom<NMapRoom>(ref mapRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.MerchantRoom))
    {
      ref godot_variant local = ref value;
      NMerchantRoom merchantRoom = this.MerchantRoom;
      godot_variant from = VariantUtils.CreateFrom<NMerchantRoom>(ref merchantRoom);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.GlobalUi))
    {
      ref godot_variant local = ref value;
      NGlobalUi globalUi = this.GlobalUi;
      godot_variant from = VariantUtils.CreateFrom<NGlobalUi>(ref globalUi);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName.RunMusicController))
    {
      ref godot_variant local = ref value;
      NRunMusicController runMusicController = this.RunMusicController;
      godot_variant from = VariantUtils.CreateFrom<NRunMusicController>(ref runMusicController);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._cardScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._cardScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._roomContainer))
    {
      value = VariantUtils.CreateFrom<NSceneContainer>(ref this._roomContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NRun.PropertyName._testButton))
    {
      value = VariantUtils.CreateFrom<Button>(ref this._testButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NRun.PropertyName._seedLabel))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<MegaLabel>(ref this._seedLabel);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName._cardScene, (PropertyHint) 17L, "PackedScene", (PropertyUsageFlags) 4102L, true),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName._roomContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName._testButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName._seedLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.CombatRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.TreasureRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.EventRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.RestSiteRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.MapRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.MerchantRoom, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.GlobalUi, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NRun.PropertyName.RunMusicController, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo1 = info;
    StringName globalUi1 = NRun.PropertyName.GlobalUi;
    NGlobalUi globalUi2 = this.GlobalUi;
    Variant variant1 = Variant.From<NGlobalUi>(ref globalUi2);
    serializationInfo1.AddProperty(globalUi1, variant1);
    GodotSerializationInfo serializationInfo2 = info;
    StringName runMusicController1 = NRun.PropertyName.RunMusicController;
    NRunMusicController runMusicController2 = this.RunMusicController;
    Variant variant2 = Variant.From<NRunMusicController>(ref runMusicController2);
    serializationInfo2.AddProperty(runMusicController1, variant2);
    info.AddProperty(NRun.PropertyName._cardScene, Variant.From<PackedScene>(ref this._cardScene));
    info.AddProperty(NRun.PropertyName._roomContainer, Variant.From<NSceneContainer>(ref this._roomContainer));
    info.AddProperty(NRun.PropertyName._testButton, Variant.From<Button>(ref this._testButton));
    info.AddProperty(NRun.PropertyName._seedLabel, Variant.From<MegaLabel>(ref this._seedLabel));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NRun.PropertyName.GlobalUi, ref variant1))
      this.GlobalUi = ((Variant) ref variant1).As<NGlobalUi>();
    Variant variant2;
    if (info.TryGetProperty(NRun.PropertyName.RunMusicController, ref variant2))
      this.RunMusicController = ((Variant) ref variant2).As<NRunMusicController>();
    Variant variant3;
    if (info.TryGetProperty(NRun.PropertyName._cardScene, ref variant3))
      this._cardScene = ((Variant) ref variant3).As<PackedScene>();
    Variant variant4;
    if (info.TryGetProperty(NRun.PropertyName._roomContainer, ref variant4))
      this._roomContainer = ((Variant) ref variant4).As<NSceneContainer>();
    Variant variant5;
    if (info.TryGetProperty(NRun.PropertyName._testButton, ref variant5))
      this._testButton = ((Variant) ref variant5).As<Button>();
    Variant variant6;
    if (!info.TryGetProperty(NRun.PropertyName._seedLabel, ref variant6))
      return;
    this._seedLabel = ((Variant) ref variant6).As<MegaLabel>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName _Notification = StringName.op_Implicit(nameof (_Notification));
    public static readonly StringName SetCurrentRoom = StringName.op_Implicit(nameof (SetCurrentRoom));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName CombatRoom = StringName.op_Implicit(nameof (CombatRoom));
    public static readonly StringName TreasureRoom = StringName.op_Implicit(nameof (TreasureRoom));
    public static readonly StringName EventRoom = StringName.op_Implicit(nameof (EventRoom));
    public static readonly StringName RestSiteRoom = StringName.op_Implicit(nameof (RestSiteRoom));
    public static readonly StringName MapRoom = StringName.op_Implicit(nameof (MapRoom));
    public static readonly StringName MerchantRoom = StringName.op_Implicit(nameof (MerchantRoom));
    public static readonly StringName GlobalUi = StringName.op_Implicit(nameof (GlobalUi));
    public static readonly StringName RunMusicController = StringName.op_Implicit(nameof (RunMusicController));
    public static readonly StringName _cardScene = StringName.op_Implicit(nameof (_cardScene));
    public static readonly StringName _roomContainer = StringName.op_Implicit(nameof (_roomContainer));
    public static readonly StringName _testButton = StringName.op_Implicit(nameof (_testButton));
    public static readonly StringName _seedLabel = StringName.op_Implicit(nameof (_seedLabel));
  }

  public class SignalName : Control.SignalName
  {
  }
}
