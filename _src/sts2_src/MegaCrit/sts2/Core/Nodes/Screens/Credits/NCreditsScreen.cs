// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.Credits.NCreditsScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Random;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.Credits;

[ScriptPath("res://src/Core/Nodes/Screens/Credits/NCreditsScreen.cs")]
public class NCreditsScreen : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/credits_screen");
  private bool _canClose;
  private bool _exitingScreen;
  private Tween? _tween;
  private Control _screenContents;
  private NBackButton _backButton;
  private const string _table = "credits";
  private float _targetPosition;
  private const float _scrollSpeed = 50f;
  private const float _trackpadScrollSpeed = 20f;
  private const float _autoScrollSpeed = 80f;
  private const float _lerpSmoothness = 20f;

  public static NCreditsScreen? Create()
  {
    return TestMode.IsOn ? (NCreditsScreen) null : PreloadManager.Cache.GetScene(NCreditsScreen._scenePath).Instantiate<NCreditsScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    NHotkeyManager.Instance.PushHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.back), new Action(this.CloseScreenDebug));
    this._screenContents = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%ScreenContents"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    ((CanvasItem) this._screenContents).Modulate = StsColors.transparentWhite;
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 2.0);
    this._targetPosition = this._screenContents.Position.Y;
    TaskHelper.RunSafely(this.EnableScreenExit());
    this.InitMegaCrit();
    this.InitComposer();
    this.InitAdditionalProgramming();
    this.InitAdditionalVfx();
    this.InitMarketingSupport();
    this.InitConsultants();
    this.InitVoices();
    this.InitLocalization();
    this.InitTwitchExtension();
    this.InitModdingSupport();
    this.InitPlaytesters();
    this.InitTrailer();
    this.InitFmod();
    this.InitSpine();
    this.InitGodot();
    this.InitExitMessage();
  }

  private void InitMegaCrit()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%MegaCritHeader")).Text = new LocString("credits", "MEGA_CRIT.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%CreatedByNames")).Text = new LocString("credits", "MEGA_CRIT.names").GetRawText();
    (string Roles, string Names) = this.SplitTwoColumn(new LocString("credits", "MEGA_CRIT_TEAM.names").GetRawText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%MegaCritTeamRoles")).Text = Roles;
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%MegaCritTeamNames")).Text = Names;
  }

  private void InitComposer()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ComposerHeader")).Text = new LocString("credits", "COMPOSER.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ComposerNames")).Text = new LocString("credits", "COMPOSER.names").GetRawText();
  }

  private void InitAdditionalProgramming()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AdditionalProgrammingHeader")).Text = new LocString("credits", "ADDITIONAL_PROGRAMMING.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%AdditionalProgrammingNames")).Text = new LocString("credits", "ADDITIONAL_PROGRAMMING.names").GetRawText();
  }

  private void InitAdditionalVfx()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%AdditionalVfxHeader")).Text = new LocString("credits", "ADDITIONAL_VFX.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%AdditionalVfxNames")).Text = new LocString("credits", "ADDITIONAL_VFX.names").GetRawText();
  }

  private void InitMarketingSupport()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%MarketingSupportHeader")).Text = new LocString("credits", "MARKETING_SUPPORT.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%MarketingSupportNames")).Text = new LocString("credits", "MARKETING_SUPPORT.names").GetRawText();
  }

  private void InitConsultants()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ConsultantsHeader")).Text = new LocString("credits", "CONSULTANTS.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ConsultantsNames")).Text = new LocString("credits", "CONSULTANTS.names").GetRawText();
  }

  private void InitVoices()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%VoicesHeader")).Text = new LocString("credits", "VOICES.header").GetRawText();
    (string left, string right) = NCreditsScreen.SplitTwoColumnMultiRole(new LocString("credits", "VOICES.names").GetRawText());
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%VoicesRoles")).Text = left;
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%VoicesNames")).Text = right;
  }

  private void InitLocalization()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%LocalizationHeader")).Text = new LocString("credits", "LOC.header").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ptbHeader")).Text = new LocString("credits", "LOC_PTB.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ptbNames")).Text = new LocString("credits", "LOC_PTB.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%zhsHeader")).Text = new LocString("credits", "LOC_ZHS.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%zhsRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%zhsNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_ZHS.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%fraHeader")).Text = new LocString("credits", "LOC_FRA.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%fraRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%fraNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_FRA.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%deuHeader")).Text = new LocString("credits", "LOC_DEU.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%deuNames")).Text = new LocString("credits", "LOC_DEU.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%itaHeader")).Text = new LocString("credits", "LOC_ITA.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%itaTeam")).Text = new LocString("credits", "LOC_ITA.team").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%itaRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%itaNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_ITA.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%jpnHeader")).Text = new LocString("credits", "LOC_JPN.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%jpnNames")).Text = new LocString("credits", "LOC_JPN.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%korHeader")).Text = new LocString("credits", "LOC_KOR.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%korNames")).Text = new LocString("credits", "LOC_KOR.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%polHeader")).Text = new LocString("credits", "LOC_POL.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%polNames")).Text = new LocString("credits", "LOC_POL.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%rusHeader")).Text = new LocString("credits", "LOC_RUS.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%rusRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%rusNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_RUS.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%spaHeader")).Text = new LocString("credits", "LOC_SPA.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%spaNames")).Text = new LocString("credits", "LOC_SPA.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%espHeader")).Text = new LocString("credits", "LOC_ESP.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%espTeam")).Text = new LocString("credits", "LOC_ESP.team").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%espRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%espNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_ESP.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%thaHeader")).Text = new LocString("credits", "LOC_THA.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%thaNames")).Text = new LocString("credits", "LOC_THA.names").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%turHeader")).Text = new LocString("credits", "LOC_TUR.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%turRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%turNames")).Text) = this.SplitTwoColumn(new LocString("credits", "LOC_TUR.names").GetRawText());
  }

  private void InitTwitchExtension()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TwitchHeader")).Text = new LocString("credits", "TWITCH.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TwitchRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TwitchNames")).Text) = this.SplitTwoColumn(new LocString("credits", "TWITCH.names").GetRawText());
  }

  private void InitModdingSupport()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%ModdingSupportHeader")).Text = new LocString("credits", "MODDING_SUPPORT.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ModdingSupportNames")).Text = NCreditsScreen.ShuffleOneColumn(new LocString("credits", "MODDING_SUPPORT.names").GetRawText());
  }

  private void InitPlaytesters()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%PlaytestersHeader")).Text = new LocString("credits", "PLAYTESTERS.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PlaytesterNames1")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PlaytesterNames2")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%PlaytesterNames3")).Text) = this.SplitThreeColumnPlaytesters(new LocString("credits", "PLAYTESTERS.names").GetRawText());
  }

  private void InitTrailer()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TrailerHeader")).Text = new LocString("credits", "TRAILER.header").GetRawText();
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TrailerAnimationTeam")).Text = new LocString("credits", "TRAILER_ANIMATION.team").GetRawText();
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TrailerAnimationHeader")).Text = new LocString("credits", "TRAILER_ANIMATION.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TrailerAnimationRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TrailerAnimationNames")).Text) = this.SplitTwoColumn(new LocString("credits", "TRAILER_ANIMATION.names").GetRawText());
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%TrailerEditorHeader")).Text = new LocString("credits", "TRAILER_EDITOR.header").GetRawText();
    (((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TrailerEditorRoles")).Text, ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%TrailerEditorNames")).Text) = this.SplitTwoColumn(new LocString("credits", "TRAILER_EDITOR.names").GetRawText());
  }

  private void InitFmod()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%FmodHeader")).Text = new LocString("credits", "FMOD").GetRawText();
  }

  private void InitSpine()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%SpineHeader")).Text = new LocString("credits", "SPINE").GetRawText();
  }

  private void InitGodot()
  {
    ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%GodotHeader")).Text = new LocString("credits", "GODOT").GetRawText();
  }

  private void InitExitMessage()
  {
    ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%ExitMessage")).Text = new LocString("credits", "EXIT_MESSAGE").GetRawText();
  }

  public override void _EnterTree()
  {
    NHotkeyManager.Instance.AddBlockingScreen((Node) this);
    NHotkeyManager.Instance.PushHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.CloseScreenDebug));
  }

  public override void _ExitTree()
  {
    NHotkeyManager.Instance.RemoveHotkeyReleasedBinding(StringName.op_Implicit(MegaInput.cancel), new Action(this.CloseScreenDebug));
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this);
  }

  public Control DefaultFocusedControl => (Control) this;

  private async Task EnableScreenExit()
  {
    await Task.Delay(2000);
    this._canClose = true;
  }

  private void CloseScreenDebug()
  {
    if (!this._canClose || this._exitingScreen)
      return;
    this._exitingScreen = true;
    TaskHelper.RunSafely(this.FadeAndExitScreen());
  }

  private async Task FadeAndExitScreen()
  {
    this._tween?.Kill();
    this._tween = ((Node) this).CreateTween().SetParallel(true);
    this._tween.TweenProperty((GodotObject) this._screenContents, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(0.0f), 1.0);
    if (!await this._tween.AwaitFinished((Node) this))
      return;
    NModalContainer.Instance.Clear();
  }

  public override void _GuiInput(InputEvent inputEvent)
  {
    if (inputEvent is InputEventMouseButton eventMouseButton && eventMouseButton.ButtonIndex == 1L && eventMouseButton.Pressed)
      this.CloseScreenDebug();
    this.ProcessScrollEvent(inputEvent);
  }

  public override void _Process(double delta)
  {
    float num = (float) delta;
    this._targetPosition -= 80f * num;
    Control screenContents = this._screenContents;
    Vector2 position = this._screenContents.Position;
    Vector2 vector2 = ((Vector2) ref position).Lerp(new Vector2(this._screenContents.Position.X, this._targetPosition), num * 20f);
    screenContents.Position = vector2;
  }

  private void ProcessScrollEvent(InputEvent inputEvent)
  {
    switch (inputEvent)
    {
      case InputEventMouseButton eventMouseButton:
        if (eventMouseButton.ButtonIndex == 4L)
        {
          this._targetPosition += 50f;
          break;
        }
        if (eventMouseButton.ButtonIndex != 5L)
          break;
        this._targetPosition -= 50f;
        break;
      case InputEventPanGesture inputEventPanGesture:
        this._targetPosition += (float) (-(double) inputEventPanGesture.Delta.Y * 20.0);
        break;
    }
  }

  private static string ShuffleOneColumn(string input)
  {
    if (string.IsNullOrWhiteSpace(input))
      return string.Empty;
    List<string> list = ((IEnumerable<string>) input.Split(new string[1]
    {
      "||"
    }, StringSplitOptions.None)).ToList<string>();
    for (int index1 = list.Count - 1; index1 > 0; --index1)
    {
      int index2 = Rng.Chaotic.NextInt(index1 + 1);
      List<string> stringList1 = list;
      int index3 = index1;
      List<string> stringList2 = list;
      int num = index2;
      string str1 = list[index2];
      string str2 = list[index1];
      stringList1[index3] = str1;
      int index4 = num;
      string str3 = str2;
      stringList2[index4] = str3;
    }
    return string.Join("\n", (IEnumerable<string>) list);
  }

  private (string Roles, string Names) SplitTwoColumn(string input)
  {
    List<string> values1 = new List<string>();
    List<string> values2 = new List<string>();
    string str1 = input;
    char[] separator1 = new char[1]{ '\n' };
    foreach (string str2 in str1.Split(separator1, StringSplitOptions.RemoveEmptyEntries))
    {
      string[] separator2 = new string[1]{ "||" };
      string[] array = ((IEnumerable<string>) str2.Split(separator2, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (p => p.Trim())).Where<string>((Func<string, bool>) (p => !string.IsNullOrWhiteSpace(p))).ToArray<string>();
      if (array.Length == 2)
      {
        values1.Add(array[0]);
        values2.Add(array[1]);
      }
    }
    return (string.Join("\n", (IEnumerable<string>) values1), string.Join("\n", (IEnumerable<string>) values2));
  }

  private (string Column1, string Column2, string Column3) SplitThreeColumnPlaytesters(string input)
  {
    string[] array = ((IEnumerable<string>) input.Split(new string[1]
    {
      "||"
    }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (p => p.Trim())).Where<string>((Func<string, bool>) (p => !string.IsNullOrWhiteSpace(p))).ToArray<string>();
    for (int index1 = array.Length - 1; index1 > 0; --index1)
    {
      int index2 = Rng.Chaotic.NextInt(index1 + 1);
      ref string local1 = ref array[index1];
      ref string local2 = ref array[index2];
      string str1 = array[index2];
      string str2 = array[index1];
      local1 = str1;
      string str3 = str2;
      local2 = str3;
    }
    List<string> values1 = new List<string>();
    List<string> values2 = new List<string>();
    List<string> values3 = new List<string>();
    for (int index = 0; index < array.Length; ++index)
    {
      switch (index % 3)
      {
        case 0:
          values1.Add(array[index]);
          break;
        case 1:
          values2.Add(array[index]);
          break;
        case 2:
          values3.Add(array[index]);
          break;
      }
    }
    return (string.Join("\n", (IEnumerable<string>) values1), string.Join("\n", (IEnumerable<string>) values2), string.Join("\n", (IEnumerable<string>) values3));
  }

  private static (string left, string right) SplitTwoColumnMultiRole(string input)
  {
    List<string> values1 = new List<string>();
    List<string> values2 = new List<string>();
    string[] strArray1 = input.Split(new char[1]{ '\n' }, StringSplitOptions.RemoveEmptyEntries);
    for (int index1 = 0; index1 < strArray1.Length; ++index1)
    {
      string[] strArray2 = strArray1[index1].Split(new string[1]
      {
        "||"
      }, StringSplitOptions.None);
      if (strArray2.Length == 2)
      {
        string str1 = strArray2[0].Trim();
        string str2 = strArray2[1].Trim();
        List<string> list = ((IEnumerable<string>) str1.Split(new char[1]
        {
          ','
        }, StringSplitOptions.RemoveEmptyEntries)).Select<string, string>((Func<string, string>) (r => r.Trim())).ToList<string>();
        for (int index2 = 0; index2 < list.Count; ++index2)
        {
          values1.Add(list[index2]);
          values2.Add(index2 == 0 ? str2 : "");
        }
        bool flag1 = list.Count > 1;
        bool flag2 = index1 == strArray1.Length - 1;
        if (flag1 && !flag2)
        {
          values1.Add("");
          values2.Add("");
        }
      }
    }
    return (string.Join("\n", (IEnumerable<string>) values1), string.Join("\n", (IEnumerable<string>) values2));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(25)
    {
      new MethodInfo(NCreditsScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitMegaCrit, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitComposer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitAdditionalProgramming, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitAdditionalVfx, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitMarketingSupport, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitConsultants, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitVoices, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitLocalization, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitTwitchExtension, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitModdingSupport, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitPlaytesters, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitTrailer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitFmod, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitSpine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitGodot, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.InitExitMessage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.CloseScreenDebug, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName._GuiInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName._Process, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 3L, StringName.op_Implicit("delta"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.ProcessScrollEvent, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NCreditsScreen.MethodName.ShuffleOneColumn, new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 33L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("input"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCreditsScreen ncreditsScreen = NCreditsScreen.Create();
      ret = VariantUtils.CreateFrom<NCreditsScreen>(ref ncreditsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitMegaCrit) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitMegaCrit();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitComposer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitComposer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitAdditionalProgramming) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitAdditionalProgramming();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitAdditionalVfx) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitAdditionalVfx();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitMarketingSupport) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitMarketingSupport();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitConsultants) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitConsultants();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitVoices) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitVoices();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitLocalization) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitLocalization();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitTwitchExtension) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitTwitchExtension();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitModdingSupport) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitModdingSupport();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitPlaytesters) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitPlaytesters();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitTrailer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitTrailer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitFmod) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitFmod();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitSpine) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitSpine();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitGodot) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitGodot();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitExitMessage) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.InitExitMessage();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.CloseScreenDebug) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.CloseScreenDebug();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName._GuiInput) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      base._GuiInput(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName._Process) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Process(VariantUtils.ConvertTo<double>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.ProcessScrollEvent) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ProcessScrollEvent(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NCreditsScreen.MethodName.ShuffleOneColumn) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    string str = NCreditsScreen.ShuffleOneColumn(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = VariantUtils.CreateFrom<string>(ref str);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NCreditsScreen ncreditsScreen = NCreditsScreen.Create();
      ret = VariantUtils.CreateFrom<NCreditsScreen>(ref ncreditsScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NCreditsScreen.MethodName.ShuffleOneColumn) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      string str = NCreditsScreen.ShuffleOneColumn(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = VariantUtils.CreateFrom<string>(ref str);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NCreditsScreen.MethodName.Create) || StringName.op_Equality(ref method, NCreditsScreen.MethodName._Ready) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitMegaCrit) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitComposer) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitAdditionalProgramming) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitAdditionalVfx) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitMarketingSupport) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitConsultants) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitVoices) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitLocalization) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitTwitchExtension) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitModdingSupport) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitPlaytesters) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitTrailer) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitFmod) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitSpine) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitGodot) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.InitExitMessage) || StringName.op_Equality(ref method, NCreditsScreen.MethodName._EnterTree) || StringName.op_Equality(ref method, NCreditsScreen.MethodName._ExitTree) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.CloseScreenDebug) || StringName.op_Equality(ref method, NCreditsScreen.MethodName._GuiInput) || StringName.op_Equality(ref method, NCreditsScreen.MethodName._Process) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.ProcessScrollEvent) || StringName.op_Equality(ref method, NCreditsScreen.MethodName.ShuffleOneColumn) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._canClose))
    {
      this._canClose = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._exitingScreen))
    {
      this._exitingScreen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._tween))
    {
      this._tween = VariantUtils.ConvertTo<Tween>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._screenContents))
    {
      this._screenContents = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreditsScreen.PropertyName._targetPosition))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._targetPosition = VariantUtils.ConvertTo<float>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._canClose))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._canClose);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._exitingScreen))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._exitingScreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._tween))
    {
      value = VariantUtils.CreateFrom<Tween>(ref this._tween);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._screenContents))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._screenContents);
      return true;
    }
    if (StringName.op_Equality(ref name, NCreditsScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (!StringName.op_Equality(ref name, NCreditsScreen.PropertyName._targetPosition))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<float>(ref this._targetPosition);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 1L, NCreditsScreen.PropertyName._canClose, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NCreditsScreen.PropertyName._exitingScreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreditsScreen.PropertyName._tween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreditsScreen.PropertyName._screenContents, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreditsScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 3L, NCreditsScreen.PropertyName._targetPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NCreditsScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NCreditsScreen.PropertyName._canClose, Variant.From<bool>(ref this._canClose));
    info.AddProperty(NCreditsScreen.PropertyName._exitingScreen, Variant.From<bool>(ref this._exitingScreen));
    info.AddProperty(NCreditsScreen.PropertyName._tween, Variant.From<Tween>(ref this._tween));
    info.AddProperty(NCreditsScreen.PropertyName._screenContents, Variant.From<Control>(ref this._screenContents));
    info.AddProperty(NCreditsScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NCreditsScreen.PropertyName._targetPosition, Variant.From<float>(ref this._targetPosition));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NCreditsScreen.PropertyName._canClose, ref variant1))
      this._canClose = ((Variant) ref variant1).As<bool>();
    Variant variant2;
    if (info.TryGetProperty(NCreditsScreen.PropertyName._exitingScreen, ref variant2))
      this._exitingScreen = ((Variant) ref variant2).As<bool>();
    Variant variant3;
    if (info.TryGetProperty(NCreditsScreen.PropertyName._tween, ref variant3))
      this._tween = ((Variant) ref variant3).As<Tween>();
    Variant variant4;
    if (info.TryGetProperty(NCreditsScreen.PropertyName._screenContents, ref variant4))
      this._screenContents = ((Variant) ref variant4).As<Control>();
    Variant variant5;
    if (info.TryGetProperty(NCreditsScreen.PropertyName._backButton, ref variant5))
      this._backButton = ((Variant) ref variant5).As<NBackButton>();
    Variant variant6;
    if (!info.TryGetProperty(NCreditsScreen.PropertyName._targetPosition, ref variant6))
      return;
    this._targetPosition = ((Variant) ref variant6).As<float>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName InitMegaCrit = StringName.op_Implicit(nameof (InitMegaCrit));
    public static readonly StringName InitComposer = StringName.op_Implicit(nameof (InitComposer));
    public static readonly StringName InitAdditionalProgramming = StringName.op_Implicit(nameof (InitAdditionalProgramming));
    public static readonly StringName InitAdditionalVfx = StringName.op_Implicit(nameof (InitAdditionalVfx));
    public static readonly StringName InitMarketingSupport = StringName.op_Implicit(nameof (InitMarketingSupport));
    public static readonly StringName InitConsultants = StringName.op_Implicit(nameof (InitConsultants));
    public static readonly StringName InitVoices = StringName.op_Implicit(nameof (InitVoices));
    public static readonly StringName InitLocalization = StringName.op_Implicit(nameof (InitLocalization));
    public static readonly StringName InitTwitchExtension = StringName.op_Implicit(nameof (InitTwitchExtension));
    public static readonly StringName InitModdingSupport = StringName.op_Implicit(nameof (InitModdingSupport));
    public static readonly StringName InitPlaytesters = StringName.op_Implicit(nameof (InitPlaytesters));
    public static readonly StringName InitTrailer = StringName.op_Implicit(nameof (InitTrailer));
    public static readonly StringName InitFmod = StringName.op_Implicit(nameof (InitFmod));
    public static readonly StringName InitSpine = StringName.op_Implicit(nameof (InitSpine));
    public static readonly StringName InitGodot = StringName.op_Implicit(nameof (InitGodot));
    public static readonly StringName InitExitMessage = StringName.op_Implicit(nameof (InitExitMessage));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName CloseScreenDebug = StringName.op_Implicit(nameof (CloseScreenDebug));
    public static readonly StringName _GuiInput = StringName.op_Implicit(nameof (_GuiInput));
    public static readonly StringName _Process = StringName.op_Implicit(nameof (_Process));
    public static readonly StringName ProcessScrollEvent = StringName.op_Implicit(nameof (ProcessScrollEvent));
    public static readonly StringName ShuffleOneColumn = StringName.op_Implicit(nameof (ShuffleOneColumn));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _canClose = StringName.op_Implicit(nameof (_canClose));
    public static readonly StringName _exitingScreen = StringName.op_Implicit(nameof (_exitingScreen));
    public static readonly StringName _tween = StringName.op_Implicit(nameof (_tween));
    public static readonly StringName _screenContents = StringName.op_Implicit(nameof (_screenContents));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _targetPosition = StringName.op_Implicit(nameof (_targetPosition));
  }

  public class SignalName : Control.SignalName
  {
  }
}
