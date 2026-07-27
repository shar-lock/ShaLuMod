// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.MainMenu.NPatchNotesScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.Linq;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.MainMenu;

[ScriptPath("res://src/Core/Nodes/Screens/MainMenu/NPatchNotesScreen.cs")]
public class NPatchNotesScreen : Control, IScreenContext
{
  private NScrollableContainer _screenContents;
  private MarginContainer _marginContainer;
  private NButton _prevButton;
  private NButton _nextButton;
  private NButton _patchNotesToggle;
  private NButton _backButton;
  private MegaLabel _dateLabel;
  private MegaRichTextLabel _patchText;
  private Tween? _tween;
  private PackedScene _cachedScene;
  private const string _engPatchNotesPath = "res://localization/eng/patch_notes";
  private List<string>? _patchNotePaths;
  private int _index;
  private int _currentScrollLine;

  public bool IsOpen { get; private set; }

  public override void _Ready()
  {
    this._cachedScene = ResourceLoader.Load<PackedScene>("res://scenes/screens/patch_screen_contents.tscn", (string) null, (ResourceLoader.CacheMode) 1L);
    this.CreateNewPatchEntry();
    this._prevButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("PrevButton"));
    ((GodotObject) this._prevButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.PreviousPatchNote())), 0U);
    this._nextButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("NextButton"));
    ((GodotObject) this._nextButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.NextPatchNote())), 0U);
    ((CanvasItem) this._nextButton).Visible = false;
    this._patchNotesToggle = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%PatchNotesToggle"));
    ((GodotObject) this._patchNotesToggle).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.Close())), 0U);
    this._patchNotesToggle.Disable();
    this._backButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%BackButton"));
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.Close())), 0U);
  }

  private void CreateNewPatchEntry()
  {
    this._screenContents = this._cachedScene.Instantiate<NScrollableContainer>((PackedScene.GenEditState) 0L);
    ((Node) this).AddChildSafely((Node) this._screenContents);
    ((Node) this).MoveChildSafely((Node) this._screenContents, 0);
    this._marginContainer = ((Node) this._screenContents).GetNode<MarginContainer>(NodePath.op_Implicit("Content"));
    this._patchText = ((Node) this._screenContents).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("Content/PatchText"));
    this._dateLabel = ((Node) this._patchText).GetNode<MegaLabel>(NodePath.op_Implicit("DateLabel"));
    if (this._patchNotePaths == null)
      return;
    this.LoadPatchNoteText(this._patchNotePaths[this._index]);
  }

  private void NextPatchNote()
  {
    if (!((CanvasItem) this._nextButton).Visible)
      return;
    if (this._patchNotePaths == null)
    {
      Log.Error("NPatchNotesScreen: No patch paths available!");
    }
    else
    {
      --this._index;
      ((CanvasItem) this._prevButton).Visible = true;
      if (this._index == 0)
        ((CanvasItem) this._nextButton).Visible = false;
      ((Node) this._screenContents).QueueFreeSafely();
      this.CreateNewPatchEntry();
    }
  }

  private void PreviousPatchNote()
  {
    if (this._patchNotePaths == null)
    {
      Log.Error("NPatchNotesScreen: No patch paths available!");
    }
    else
    {
      ++this._index;
      ((CanvasItem) this._nextButton).Visible = true;
      if (this._index == this._patchNotePaths.Count - 1)
        ((CanvasItem) this._prevButton).Visible = false;
      ((Node) this._screenContents).QueueFreeSafely();
      this.CreateNewPatchEntry();
    }
  }

  public void Open()
  {
    this.IsOpen = true;
    NGame.Instance.MainMenu?.EnableBackstop();
    this._patchNotesToggle.Enable();
    this._backButton.Enable();
    ((CanvasItem) this).Visible = true;
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.25);
    if (this._patchNotePaths == null)
      this._patchNotePaths = ((IEnumerable<string>) DirAccess.GetFilesAt("res://localization/eng/patch_notes")).Select<string, string>((Func<string, string>) (fileName => "res://localization/eng/patch_notes/" + fileName)).Reverse<string>().ToList<string>();
    this.LoadPatchNoteText(this._patchNotePaths[this._index]);
    ActiveScreenContext.Instance.Update();
    NHotkeyManager.Instance.PushHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.PreviousPatchNote));
    NHotkeyManager.Instance.PushHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.NextPatchNote));
    NHotkeyManager.Instance.PushHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
  }

  private void Close()
  {
    NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.left), new Action(this.PreviousPatchNote));
    NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.right), new Action(this.NextPatchNote));
    NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.pauseAndBack), new Action(this.Close));
    this._patchNotesToggle.Disable();
    this._backButton.Disable();
    NGame.Instance.MainMenu?.DisableBackstop();
    Tween tween = this._tween;
    if (tween != null)
      tween.FastForwardToCompletion();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 0.25);
    this._tween.TweenCallback(Callable.From((Action) (() =>
    {
      this.IsOpen = false;
      ((CanvasItem) this).SetVisible(false);
      ActiveScreenContext.Instance.Update();
    })));
  }

  private void LoadPatchNoteText(string patchNotePath)
  {
    this._patchText.ScrollToLine(0);
    this._currentScrollLine = 0;
    this._patchText.SetTextAutoSize(NPatchNotesScreen.ReadPatchNoteFile(patchNotePath));
    this.UpdateDateLabel(patchNotePath);
  }

  private static string ReadPatchNoteFile(string engPatchNotePath)
  {
    string language = LocManager.Instance.Language;
    if (language != "eng")
    {
      string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(engPatchNotePath);
      string str = $"res://localization/{language}/patch_notes/{fileNameFromPath}";
      if (FileAccess.FileExists(str))
      {
        using (FileAccess fileAccess = FileAccess.Open(str, (FileAccess.ModeFlags) 1L))
        {
          if (fileAccess != null)
            return fileAccess.GetAsText(false);
          Log.Warn("Failed to open localized patch notes: " + str);
        }
      }
    }
    using (FileAccess fileAccess = FileAccess.Open(engPatchNotePath, (FileAccess.ModeFlags) 1L))
    {
      if (fileAccess != null)
        return fileAccess.GetAsText(false);
      Log.Error("Failed to open patch notes: " + engPatchNotePath);
      return "";
    }
  }

  private void UpdateDateLabel(string patchNotePath)
  {
    string dateString = NPatchNotesScreen.RemoveFileExtension(NPatchNotesScreen.GetFileNameFromPath(patchNotePath));
    string formattedDate;
    if (NPatchNotesScreen.TryParseDate(dateString, out formattedDate))
      this._dateLabel.SetTextAutoSize(formattedDate);
    else
      Log.Error("Invalid date format in file name: " + dateString);
  }

  private static string GetFileNameFromPath(string path)
  {
    string str = path;
    int startIndex = path.LastIndexOf('/') + 1;
    return str.Substring(startIndex, str.Length - startIndex);
  }

  private static string RemoveFileExtension(string fileName)
  {
    return fileName.Split('.', StringSplitOptions.None)[0];
  }

  private static bool TryParseDate(string dateString, out string formattedDate)
  {
    DateTime result;
    if (DateTime.TryParseExact(dateString, "yyyy_MM_d", (IFormatProvider) CultureInfo.InvariantCulture, DateTimeStyles.None, out result))
    {
      formattedDate = result.ToString("MMMM d, yyyy", (IFormatProvider) CultureInfo.InvariantCulture);
      return true;
    }
    formattedDate = string.Empty;
    return false;
  }

  public Control? DefaultFocusedControl => (Control) null;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(11)
    {
      new MethodInfo(NPatchNotesScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.CreateNewPatchEntry, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.NextPatchNote, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.PreviousPatchNote, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.LoadPatchNoteText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("patchNotePath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.ReadPatchNoteFile, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("engPatchNotePath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.UpdateDateLabel, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("patchNotePath"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.GetFileNameFromPath, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("path"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NPatchNotesScreen.MethodName.RemoveFileExtension, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("fileName"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.CreateNewPatchEntry) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CreateNewPatchEntry();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.NextPatchNote) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.NextPatchNote();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.PreviousPatchNote) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PreviousPatchNote();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.LoadPatchNoteText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.LoadPatchNoteText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.ReadPatchNoteFile) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NPatchNotesScreen.ReadPatchNoteFile(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.UpdateDateLabel) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.UpdateDateLabel(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.GetFileNameFromPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref fileNameFromPath);
      return true;
    }
    if (!StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.RemoveFileExtension) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    string str1 = NPatchNotesScreen.RemoveFileExtension(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref str1);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.ReadPatchNoteFile) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NPatchNotesScreen.ReadPatchNoteFile(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.GetFileNameFromPath) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string fileNameFromPath = NPatchNotesScreen.GetFileNameFromPath(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref fileNameFromPath);
      return true;
    }
    if (StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.RemoveFileExtension) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NPatchNotesScreen.RemoveFileExtension(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NPatchNotesScreen.MethodName._Ready) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.CreateNewPatchEntry) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.NextPatchNote) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.PreviousPatchNote) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.Open) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.Close) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.LoadPatchNoteText) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.ReadPatchNoteFile) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.UpdateDateLabel) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.GetFileNameFromPath) || StringName.op_Equality(ref method, NPatchNotesScreen.MethodName.RemoveFileExtension) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName.IsOpen))
    {
      this.IsOpen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._screenContents))
    {
      this._screenContents = VariantUtils.ConvertTo<NScrollableContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._marginContainer))
    {
      this._marginContainer = VariantUtils.ConvertTo<MarginContainer>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._prevButton))
    {
      this._prevButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._nextButton))
    {
      this._nextButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._patchNotesToggle))
    {
      this._patchNotesToggle = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._dateLabel))
    {
      this._dateLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._patchText))
    {
      this._patchText = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._cachedScene))
    {
      this._cachedScene = VariantUtils.ConvertTo<PackedScene>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._index))
    {
      this._index = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._currentScrollLine))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._currentScrollLine = VariantUtils.ConvertTo<int>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName.IsOpen))
    {
      ref godot_variant local = ref value;
      bool isOpen = this.IsOpen;
      godot_variant from = VariantUtils.CreateFrom<bool>(ref isOpen);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._screenContents))
    {
      value = VariantUtils.CreateFrom<NScrollableContainer>(ref this._screenContents);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._marginContainer))
    {
      value = VariantUtils.CreateFrom<MarginContainer>(ref this._marginContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._prevButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._prevButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._nextButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._nextButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._patchNotesToggle))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._patchNotesToggle);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._dateLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._dateLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._patchText))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._patchText);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._cachedScene))
    {
      value = VariantUtils.CreateFrom<PackedScene>(ref this._cachedScene);
      return true;
    }
    if (StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._index))
    {
      value = VariantUtils.CreateFrom<int>(ref this._index);
      return true;
    }
    if (!StringName.op_Equality(ref name, NPatchNotesScreen.PropertyName._currentScrollLine))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<int>(ref this._currentScrollLine);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._screenContents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._marginContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._prevButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._nextButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._patchNotesToggle, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._dateLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._patchText, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName._cachedScene, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NPatchNotesScreen.PropertyName.IsOpen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPatchNotesScreen.PropertyName._index, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NPatchNotesScreen.PropertyName._currentScrollLine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NPatchNotesScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    GodotSerializationInfo serializationInfo = info;
    StringName isOpen1 = NPatchNotesScreen.PropertyName.IsOpen;
    bool isOpen2 = this.IsOpen;
    Variant variant = Variant.From<bool>(ref isOpen2);
    serializationInfo.AddProperty(isOpen1, variant);
    info.AddProperty(NPatchNotesScreen.PropertyName._screenContents, Variant.From<NScrollableContainer>(ref this._screenContents));
    info.AddProperty(NPatchNotesScreen.PropertyName._marginContainer, Variant.From<MarginContainer>(ref this._marginContainer));
    info.AddProperty(NPatchNotesScreen.PropertyName._prevButton, Variant.From<NButton>(ref this._prevButton));
    info.AddProperty(NPatchNotesScreen.PropertyName._nextButton, Variant.From<NButton>(ref this._nextButton));
    info.AddProperty(NPatchNotesScreen.PropertyName._patchNotesToggle, Variant.From<NButton>(ref this._patchNotesToggle));
    info.AddProperty(NPatchNotesScreen.PropertyName._backButton, Variant.From<NButton>(ref this._backButton));
    info.AddProperty(NPatchNotesScreen.PropertyName._dateLabel, Variant.From<MegaLabel>(ref this._dateLabel));
    info.AddProperty(NPatchNotesScreen.PropertyName._patchText, Variant.From<MegaRichTextLabel>(ref this._patchText));
    info.AddProperty(NPatchNotesScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NPatchNotesScreen.PropertyName._cachedScene, Variant.From<PackedScene>(ref this._cachedScene));
    info.AddProperty(NPatchNotesScreen.PropertyName._index, Variant.From<int>(ref this._index));
    info.AddProperty(NPatchNotesScreen.PropertyName._currentScrollLine, Variant.From<int>(ref this._currentScrollLine));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName.IsOpen, ref variant1))
      this.IsOpen = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._screenContents, ref variant2))
      this._screenContents = ((Variant) ref variant2).As<NScrollableContainer>();
    Variant variant3;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._marginContainer, ref variant3))
      this._marginContainer = ((Variant) ref variant3).As<MarginContainer>();
    Variant variant4;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._prevButton, ref variant4))
      this._prevButton = ((Variant) ref variant4).As<NButton>();
    Variant variant5;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._nextButton, ref variant5))
      this._nextButton = ((Variant) ref variant5).As<NButton>();
    Variant variant6;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._patchNotesToggle, ref variant6))
      this._patchNotesToggle = ((Variant) ref variant6).As<NButton>();
    Variant variant7;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._backButton, ref variant7))
      this._backButton = ((Variant) ref variant7).As<NButton>();
    Variant variant8;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._dateLabel, ref variant8))
      this._dateLabel = ((Variant) ref variant8).As<MegaLabel>();
    Variant variant9;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._patchText, ref variant9))
      this._patchText = ((Variant) ref variant9).As<MegaRichTextLabel>();
    Variant variant10;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._tween, ref variant10))
      this._tween = ((Variant) ref variant10).As<Tween>();
    Variant variant11;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._cachedScene, ref variant11))
      this._cachedScene = ((Variant) ref variant11).As<PackedScene>();
    Variant variant12;
    if (info.TryGetProperty(NPatchNotesScreen.PropertyName._index, ref variant12))
      this._index = ((Variant) ref variant12).As<int>();
    Variant variant13;
    if (!info.TryGetProperty(NPatchNotesScreen.PropertyName._currentScrollLine, ref variant13))
      return;
    this._currentScrollLine = ((Variant) ref variant13).As<int>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName CreateNewPatchEntry = StringName.op_Implicit(nameof (CreateNewPatchEntry));
    public static readonly StringName NextPatchNote = StringName.op_Implicit(nameof (NextPatchNote));
    public static readonly StringName PreviousPatchNote = StringName.op_Implicit(nameof (PreviousPatchNote));
    public static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName LoadPatchNoteText = StringName.op_Implicit(nameof (LoadPatchNoteText));
    public static readonly StringName ReadPatchNoteFile = StringName.op_Implicit(nameof (ReadPatchNoteFile));
    public static readonly StringName UpdateDateLabel = StringName.op_Implicit(nameof (UpdateDateLabel));
    public static readonly StringName GetFileNameFromPath = StringName.op_Implicit(nameof (GetFileNameFromPath));
    public static readonly StringName RemoveFileExtension = StringName.op_Implicit(nameof (RemoveFileExtension));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName IsOpen = StringName.op_Implicit(nameof (IsOpen));
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _screenContents = StringName.op_Implicit(nameof (_screenContents));
    public static readonly StringName _marginContainer = StringName.op_Implicit(nameof (_marginContainer));
    public static readonly StringName _prevButton = StringName.op_Implicit(nameof (_prevButton));
    public static readonly StringName _nextButton = StringName.op_Implicit(nameof (_nextButton));
    public static readonly StringName _patchNotesToggle = StringName.op_Implicit(nameof (_patchNotesToggle));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _dateLabel = StringName.op_Implicit(nameof (_dateLabel));
    public static readonly StringName _patchText = StringName.op_Implicit(nameof (_patchText));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _cachedScene = StringName.op_Implicit(nameof (_cachedScene));
    public static readonly StringName _index = StringName.op_Implicit(nameof (_index));
    public static readonly StringName _currentScrollLine = StringName.op_Implicit(nameof (_currentScrollLine));
  }

  public class SignalName : Control.SignalName
  {
  }
}
