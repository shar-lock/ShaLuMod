// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Debug.NDevConsole
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.DevConsole;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.ExceptionServices;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Debug;

[ScriptPath("res://src/Core/Nodes/Debug/NDevConsole.cs")]
public class NDevConsole : Panel
{
  public static readonly string assetPath = SceneHelper.GetScenePath("debug/dev_console");
  private static NDevConsole? _instance;
  private RichTextLabel _outputBuffer;
  private RichTextLabel _tabBuffer;
  private Control _inputContainer;
  private LineEdit _inputBuffer;
  private Label _promptLabel;
  private Label _ghostTextLabel;
  private bool _isFullscreen;
  private MegaCrit.Sts2.Core.DevConsole.DevConsole _devConsole;
  private const float _inputBufferSizeY = 40f;
  private readonly TabCompletionState _tabCompletion = new TabCompletionState();
  private string _yankBuffer = string.Empty;
  private string _symbolPrompt = ">";
  private string _symbolWarning = "[!]";
  private string _symbolUp = "^";
  private string _symbolDown = "v";

  public static NDevConsole Instance
  {
    get
    {
      return NDevConsole._instance ?? throw new InvalidOperationException("Dev console used before being created.");
    }
  }

  public static bool IsConsoleVisible
  {
    get
    {
      NDevConsole instance = NDevConsole._instance;
      return instance != null && ((CanvasItem) instance).Visible;
    }
  }

  public static CanvasLayer? Create()
  {
    return TestMode.IsOn ? (CanvasLayer) null : PreloadManager.Cache.GetScene(NDevConsole.assetPath).Instantiate<CanvasLayer>((PackedScene.GenEditState) 0L);
  }

  public override void _EnterTree()
  {
    if (TestMode.IsOn)
      ((Node) this).QueueFreeSafely();
    else if (NDevConsole._instance != null)
      ((Node) this).QueueFreeSafely();
    else
      NDevConsole._instance = this;
  }

  public override void _Ready()
  {
    this._outputBuffer = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("OutputContainer/OutputBuffer"));
    this._tabBuffer = ((Node) this).GetNode<RichTextLabel>(NodePath.op_Implicit("OutputContainer/TabBuffer"));
    this._inputContainer = ((Node) this).GetNode<Control>(NodePath.op_Implicit("InputContainer"));
    this._inputBuffer = ((Node) this).GetNode<LineEdit>(NodePath.op_Implicit("InputContainer/InputBufferContainer/InputBuffer"));
    this._promptLabel = ((Node) this).GetNode<Label>(NodePath.op_Implicit("InputContainer/PromptLabel"));
    this._ghostTextLabel = ((Node) this).GetNode<Label>(NodePath.op_Implicit("InputContainer/InputBufferContainer/GhostText"));
    this.HideConsole();
    this.MakeHalfScreen();
    this.DisableTabBuffer();
    this.HideGhostText();
    this._inputBuffer.CaretBlink = true;
    Font themeFont = ((Control) this._promptLabel).GetThemeFont(ThemeConstants.Label.Font, (StringName) null);
    if (themeFont.HasChar(10140L))
      this._symbolPrompt = "➜";
    if (themeFont.HasChar(9888L))
      this._symbolWarning = "⚠";
    if (themeFont.HasChar(8593L))
      this._symbolUp = "↑";
    if (themeFont.HasChar(8595L))
      this._symbolDown = "↓";
    this.UpdatePromptStyle();
    this._inputBuffer.TextChanged += new LineEdit.TextChangedEventHandler(this.OnInputTextChanged);
    this.PrintUsage();
    this._devConsole = new MegaCrit.Sts2.Core.DevConsole.DevConsole(OS.HasFeature("editor") || TestMode.IsOn || ModManager.IsRunningModded() || SaveManager.Instance.SettingsSave.FullConsole);
  }

  public override void _ExitTree()
  {
    if (TestMode.IsOff)
      this._inputBuffer.TextChanged -= new LineEdit.TextChangedEventHandler(this.OnInputTextChanged);
    ((Node) this)._ExitTree();
  }

  private void PrintUsage()
  {
    this._outputBuffer.Text += "[color=#888888]Use 'F11' to toggle console fullscreen. Press 'up arrow' to use the last command. You can autocomplete commands with 'tab'.[/color]";
    this._outputBuffer.Text += "\n\n";
  }

  public override void _Input(InputEvent inputEvent)
  {
    if (!(inputEvent is InputEventKey keyEvent) || !keyEvent.Pressed)
      return;
    Key keycode = keyEvent.Keycode;
    if (keycode <= 42L)
    {
      if (keycode != 39L && keycode != 42L)
        goto label_7;
    }
    else if (keycode != 94L && keycode != 96L /*0x60*/)
      goto label_7;
    bool flag1 = true;
    goto label_8;
label_7:
    flag1 = false;
label_8:
    if (flag1 || ((InputEventWithModifiers) keyEvent).IsShiftPressed() && keyEvent.Keycode == 56L)
    {
      if (((CanvasItem) this).Visible)
      {
        this.HideConsole();
      }
      else
      {
        bool flag2;
        switch (((Node) this).GetViewport().GuiGetFocusOwner())
        {
          case TextEdit _:
          case LineEdit _:
            flag2 = true;
            break;
          default:
            flag2 = false;
            break;
        }
        if (!flag2)
          this.ShowConsole();
      }
    }
    if (!((CanvasItem) this).Visible)
      return;
    if (keyEvent.Keycode == 4194305L /*0x400001*/)
    {
      if (this._tabCompletion.InSelectionMode)
        this.ExitSelectionMode();
      else
        this.HideConsole();
    }
    else if (keyEvent.Keycode == 4194342L)
      this.OnToggleMaximizeButtonPressed();
    else if (keyEvent.Keycode == 4194306L /*0x400002*/)
    {
      if (this._tabCompletion.InSelectionMode)
        this.NavigateSelection(1);
      else
        this.AutocompleteCommand();
    }
    else if (keyEvent.Keycode == 4194320L /*0x400010*/)
    {
      if (this._tabCompletion.InSelectionMode)
        this.NavigateSelection(-1);
      else if (this._devConsole.historyIndex < this._devConsole.history.Count)
      {
        this._tabCompletion.ProgrammaticTextChange = true;
        string str = this._devConsole.history[this._devConsole.historyIndex];
        this._inputBuffer.Text = str;
        if (this._devConsole.historyIndex < this._devConsole.history.Count - 1)
        {
          ++this._devConsole.historyIndex;
          while (this._devConsole.historyIndex < this._devConsole.history.Count - 1 && this._devConsole.history[this._devConsole.historyIndex] == str)
            ++this._devConsole.historyIndex;
        }
        this.MoveInputCursorToEndOfLine();
      }
      ((Node) this).GetViewport().SetInputAsHandled();
    }
    else if (keyEvent.Keycode == 4194322L)
    {
      if (this._tabCompletion.InSelectionMode)
        this.NavigateSelection(1);
      else if (this._devConsole.historyIndex < this._devConsole.history.Count)
      {
        this._tabCompletion.ProgrammaticTextChange = true;
        string str = this._devConsole.history[this._devConsole.historyIndex];
        this._inputBuffer.Text = str;
        if (this._devConsole.historyIndex > 0)
        {
          --this._devConsole.historyIndex;
          while (this._devConsole.historyIndex > 0 && this._devConsole.history[this._devConsole.historyIndex] == str)
            --this._devConsole.historyIndex;
        }
        this.MoveInputCursorToEndOfLine();
      }
      ((Node) this).GetViewport().SetInputAsHandled();
    }
    else if (keyEvent.Keycode == 4194309L /*0x400005*/)
    {
      if (this._tabCompletion.InSelectionMode)
        this.AcceptSelection();
      else
        this.ProcessCommand();
    }
    else
    {
      if (!((InputEventWithModifiers) keyEvent).IsCtrlPressed())
        return;
      this.HandleReadlineKeybinding(keyEvent);
    }
  }

  private void HandleReadlineKeybinding(InputEventKey keyEvent)
  {
    Key keycode = keyEvent.Keycode;
    if (keycode <= 75L)
    {
      long num = keycode - 65L;
      if (num <= 4L)
      {
        switch ((uint) num)
        {
          case 0:
            ((Node) this).GetViewport().SetInputAsHandled();
            this._inputBuffer.CaretColumn = 0;
            ((GodotObject) this._inputBuffer).CallDeferred(LineEdit.MethodName.Deselect, Array.Empty<Variant>());
            return;
          case 1:
            return;
          case 2:
            ((Node) this).GetViewport().SetInputAsHandled();
            this._inputBuffer.Text = string.Empty;
            this.ExitSelectionMode();
            return;
          case 3:
            ((Node) this).GetViewport().SetInputAsHandled();
            this.HideConsole();
            return;
          case 4:
            ((Node) this).GetViewport().SetInputAsHandled();
            this.MoveInputCursorToEndOfLine();
            return;
        }
      }
      if (keycode != 75L)
        return;
      ((Node) this).GetViewport().SetInputAsHandled();
      this.KillToEndOfLine();
    }
    else if (keycode != 76L)
    {
      long num = keycode - 85L;
      if (num > 4L)
        return;
      switch ((uint) num)
      {
        case 0:
          ((Node) this).GetViewport().SetInputAsHandled();
          this._yankBuffer = this._inputBuffer.Text;
          this._inputBuffer.Text = string.Empty;
          this.ExitSelectionMode();
          break;
        case 2:
          ((Node) this).GetViewport().SetInputAsHandled();
          this.DeleteWordBackward();
          break;
        case 4:
          ((Node) this).GetViewport().SetInputAsHandled();
          this.Yank();
          break;
      }
    }
    else
    {
      ((Node) this).GetViewport().SetInputAsHandled();
      this._outputBuffer.Text = string.Empty;
    }
  }

  private void DeleteWordBackward()
  {
    string text = this._inputBuffer.Text;
    int caretColumn = this._inputBuffer.CaretColumn;
    if (caretColumn == 0 || string.IsNullOrEmpty(text))
      return;
    int index = caretColumn - 1;
    while (index >= 0 && char.IsWhiteSpace(text[index]))
      --index;
    while (index >= 0 && !char.IsWhiteSpace(text[index]))
      --index;
    int num = index + 1;
    this._yankBuffer = text.Substring(num, caretColumn - num);
    string str = text.Substring(0, num) + text.Substring(caretColumn);
    this._tabCompletion.ProgrammaticTextChange = true;
    this._inputBuffer.Text = str;
    this._inputBuffer.CaretColumn = num;
  }

  private void KillToEndOfLine()
  {
    string text = this._inputBuffer.Text;
    int caretColumn = this._inputBuffer.CaretColumn;
    if (caretColumn >= text.Length)
      return;
    this._yankBuffer = text.Substring(caretColumn);
    string str = text.Substring(0, caretColumn);
    this._tabCompletion.ProgrammaticTextChange = true;
    this._inputBuffer.Text = str;
    this._inputBuffer.CaretColumn = caretColumn;
  }

  private void Yank()
  {
    if (string.IsNullOrEmpty(this._yankBuffer))
      return;
    string text = this._inputBuffer.Text;
    int caretColumn = this._inputBuffer.CaretColumn;
    string str = text.Insert(caretColumn, this._yankBuffer);
    this._tabCompletion.ProgrammaticTextChange = true;
    this._inputBuffer.Text = str;
    this._inputBuffer.CaretColumn = caretColumn + this._yankBuffer.Length;
  }

  private void EnableTabBuffer()
  {
    ((CanvasItem) this._outputBuffer).Visible = false;
    ((CanvasItem) this._tabBuffer).Visible = true;
  }

  private void DisableTabBuffer()
  {
    ((CanvasItem) this._outputBuffer).Visible = true;
    ((CanvasItem) this._tabBuffer).Visible = false;
  }

  public void SetBackgroundColor(Color color) => ((CanvasItem) this).Modulate = color;

  private void HideGhostText()
  {
    ((CanvasItem) this._ghostTextLabel).Visible = false;
    this._ghostTextLabel.Text = string.Empty;
  }

  private void ShowGhostText(string ghostText)
  {
    this._ghostTextLabel.Text = ghostText;
    ((CanvasItem) this._ghostTextLabel).Visible = true;
  }

  private void UpdateGhostText()
  {
    if (this._tabCompletion.InSelectionMode)
    {
      this.HideGhostText();
    }
    else
    {
      string text = this._inputBuffer.Text;
      if (string.IsNullOrWhiteSpace(text))
      {
        this.HideGhostText();
      }
      else
      {
        CompletionResult completionResults = this._devConsole.GetCompletionResults(text);
        string ghostText = MegaCrit.Sts2.Core.DevConsole.DevConsole.CalculateGhostText(text, completionResults);
        if (ghostText != null)
          this.ShowGhostText(ghostText);
        else
          this.HideGhostText();
      }
    }
  }

  private void AutocompleteCommand()
  {
    string text = this._inputBuffer.Text;
    CompletionResult completionResults = this._devConsole.GetCompletionResults(text);
    this._tabCompletion.LastCompletionResult = completionResults;
    if (string.IsNullOrWhiteSpace(text) && completionResults.Candidates.Count == 0)
    {
      completionResults = this._devConsole.GetCompletionResults("");
      this._tabCompletion.LastCompletionResult = completionResults;
    }
    if (completionResults.Candidates.Count == 0)
      this.ExitSelectionMode();
    else if (completionResults.Candidates.Count == 1)
    {
      this._tabCompletion.ProgrammaticTextChange = true;
      this._inputBuffer.Text = completionResults.CommonPrefix;
      this.MoveInputCursorToEndOfLine();
      this.ExitSelectionMode();
    }
    else
    {
      this._tabCompletion.CompletionCandidates.Clear();
      this._tabCompletion.CompletionCandidates.AddRange((IEnumerable<string>) completionResults.Candidates);
      this._tabCompletion.InSelectionMode = true;
      this._tabCompletion.SelectionIndex = 0;
      this.HideGhostText();
      this.RenderSelectionMenu();
      this.MoveInputCursorToEndOfLine();
    }
  }

  private void RenderSelectionMenu()
  {
    if (!this._tabCompletion.InSelectionMode || this._tabCompletion.CompletionCandidates.Count == 0)
      return;
    List<string> stringList = new List<string>();
    CompletionType? type = this._tabCompletion.LastCompletionResult?.Type;
    string str1;
    if (type.HasValue)
    {
      switch (type.GetValueOrDefault())
      {
        case CompletionType.Command:
          str1 = "Select command:";
          goto label_7;
        case CompletionType.Subcommand:
          str1 = $"Select {this._tabCompletion.LastCompletionResult.ArgumentContext} action:";
          goto label_7;
        case CompletionType.Argument:
          str1 = $"Select {this._tabCompletion.LastCompletionResult.ArgumentContext} argument:";
          goto label_7;
      }
    }
    str1 = "Select option:";
label_7:
    string str2 = str1;
    stringList.Add(str2);
    stringList.Add($"[color=gray]Tab/{this._symbolUp}{this._symbolDown}: navigate, Enter: accept, Esc: cancel, Type to filter[/color]");
    stringList.Add("");
    int count = this._tabCompletion.CompletionCandidates.Count;
    if (count <= 12)
    {
      for (int index = 0; index < count; ++index)
        this.AddCandidateToDisplay(stringList, index);
    }
    else
    {
      int num1 = Math.Max(0, this._tabCompletion.SelectionIndex - 6);
      int num2 = Math.Min(count, num1 + 12);
      if (num2 - num1 < 12)
        num1 = Math.Max(0, num2 - 12);
      if (num1 > 0)
        stringList.Add($"[color=gray]{this._symbolUp} {num1} more above {this._symbolUp}[/color]");
      for (int index = num1; index < num2; ++index)
        this.AddCandidateToDisplay(stringList, index);
      if (num2 < count)
      {
        int num3 = count - num2;
        stringList.Add($"[color=gray]{this._symbolDown} {num3} more below {this._symbolDown}[/color]");
      }
    }
    stringList.Add("");
    stringList.Add($"[color=gray]({this._tabCompletion.CompletionCandidates.Count} matches)[/color]");
    this._tabBuffer.Text = string.Join("\n", (IEnumerable<string>) stringList);
    this.EnableTabBuffer();
  }

  private void AddCandidateToDisplay(List<string> displayLines, int index)
  {
    if (index < 0 || index >= this._tabCompletion.CompletionCandidates.Count)
      return;
    string completionCandidate = this._tabCompletion.CompletionCandidates[index];
    string str1;
    if (index != this._tabCompletion.SelectionIndex)
      str1 = "  " + completionCandidate;
    else
      str1 = $"[color=yellow]{this._symbolPrompt} {completionCandidate}[/color]";
    string str2 = str1;
    displayLines.Add(str2);
  }

  private void OnInputTextChanged(string newText)
  {
    bool programmaticTextChange = this._tabCompletion.ProgrammaticTextChange;
    this._tabCompletion.ProgrammaticTextChange = false;
    if (this._tabCompletion.InSelectionMode && !programmaticTextChange)
    {
      CompletionResult completionResults = this._devConsole.GetCompletionResults(newText);
      this._tabCompletion.LastCompletionResult = completionResults;
      if (completionResults.Candidates.Count == 0)
        this.ExitSelectionMode();
      else if (completionResults.Candidates.Count == 1)
      {
        this._tabCompletion.ProgrammaticTextChange = true;
        this._inputBuffer.Text = completionResults.CommonPrefix;
        this.MoveInputCursorToEndOfLine();
        this.ExitSelectionMode();
      }
      else
      {
        this._tabCompletion.CompletionCandidates.Clear();
        this._tabCompletion.CompletionCandidates.AddRange((IEnumerable<string>) completionResults.Candidates);
        this._tabCompletion.SelectionIndex = 0;
        this.RenderSelectionMenu();
      }
    }
    if (programmaticTextChange)
      return;
    this.UpdateGhostText();
  }

  private void ExitSelectionMode()
  {
    this._tabCompletion.Reset();
    this._tabBuffer.Text = string.Empty;
    this.DisableTabBuffer();
    this.UpdateGhostText();
  }

  private void NavigateSelection(int direction)
  {
    if (!this._tabCompletion.InSelectionMode || this._tabCompletion.CompletionCandidates.Count == 0)
      return;
    this._tabCompletion.SelectionIndex = (this._tabCompletion.SelectionIndex + direction + this._tabCompletion.CompletionCandidates.Count) % this._tabCompletion.CompletionCandidates.Count;
    this.RenderSelectionMenu();
  }

  private void AcceptSelection()
  {
    if (!this._tabCompletion.InSelectionMode || this._tabCompletion.SelectionIndex < 0 || this._tabCompletion.SelectionIndex >= this._tabCompletion.CompletionCandidates.Count)
      return;
    string completionCandidate = this._tabCompletion.CompletionCandidates[this._tabCompletion.SelectionIndex];
    this._tabCompletion.ProgrammaticTextChange = true;
    string text = this._inputBuffer.Text;
    CompletionResult completionResult = this._tabCompletion.LastCompletionResult;
    if (completionResult != null)
    {
      switch (completionResult.Type)
      {
        case CompletionType.Command:
          this._inputBuffer.Text = completionCandidate + " ";
          break;
        case CompletionType.Subcommand:
        case CompletionType.Argument:
          string str = completionCandidate.Contains(' ') ? $"\"{completionCandidate}\"" : completionCandidate;
          this._inputBuffer.Text = !text.EndsWith(' ') ? $"{completionResult.CommandPrefix}{str} " : $"{completionResult.CommandPrefix}{str} ";
          break;
      }
    }
    else
    {
      string[] source = text.Trim().Split(' ', StringSplitOptions.None);
      string[] strArray = source.Length > 1 ? ((IEnumerable<string>) source).Take<string>(source.Length - 1).ToArray<string>() : Array.Empty<string>();
      this._inputBuffer.Text = $"{(strArray.Length != 0 ? string.Join(" ", strArray) + " " : string.Empty)}{completionCandidate} ";
    }
    this.MoveInputCursorToEndOfLine();
    this.ExitSelectionMode();
  }

  private void ProcessCommand()
  {
    if (string.IsNullOrWhiteSpace(this._inputBuffer.Text))
      return;
    this._outputBuffer.Text += $"[color=#00ff00]{this._symbolPrompt}[/color] {this._inputBuffer.Text}\n";
    if (this._inputBuffer.Text.Trim().Equals("clear"))
    {
      this._outputBuffer.Text = string.Empty;
      this._inputBuffer.Text = string.Empty;
    }
    else if (this._inputBuffer.Text.Trim().Equals("exit"))
    {
      this.HideConsole();
    }
    else
    {
      Exception source = (Exception) null;
      CmdResult cmdResult;
      try
      {
        cmdResult = this._devConsole.ProcessCommand(this._inputBuffer.Text);
      }
      catch (Exception ex)
      {
        // ISSUE: explicit reference operation
        ^ref cmdResult = new CmdResult(false, $"An exception occurred: {ex}");
        source = ex;
      }
      if (cmdResult.success)
      {
        RichTextLabel outputBuffer = this._outputBuffer;
        outputBuffer.Text = $"{outputBuffer.Text}{cmdResult.msg}\n";
      }
      else
        this._outputBuffer.Text += $"[color=#ff5555]{this._symbolWarning} {cmdResult.msg}[/color]\n";
      this._inputBuffer.Text = string.Empty;
      this._tabBuffer.Text = string.Empty;
      this.DisableTabBuffer();
      this.HideGhostText();
      if (source == null)
        return;
      ExceptionDispatchInfo.Capture(source).Throw();
    }
  }

  public Task ProcessNetCommand(Player? player, string netCommand)
  {
    Exception source = (Exception) null;
    CmdResult cmdResult;
    try
    {
      cmdResult = this._devConsole.ProcessNetCommand(player, netCommand);
    }
    catch (Exception ex)
    {
      // ISSUE: explicit reference operation
      ^ref cmdResult = new CmdResult(false, $"An exception occurred: {ex}");
      source = ex;
    }
    if (cmdResult.success)
    {
      RichTextLabel outputBuffer = this._outputBuffer;
      outputBuffer.Text = $"{outputBuffer.Text}{cmdResult.msg}\n";
    }
    else
      this._outputBuffer.Text += $"[color=#ff5555]{this._symbolWarning} {cmdResult.msg}[/color]\n";
    if (source != null)
      ExceptionDispatchInfo.Capture(source).Throw();
    return cmdResult.task ?? Task.CompletedTask;
  }

  public void ShowConsole()
  {
    ((CanvasItem) this).Visible = true;
    ((GodotObject) this._inputBuffer).CallDeferred(Control.MethodName.GrabFocus, Array.Empty<Variant>());
  }

  public void HideConsole()
  {
    ((CanvasItem) this).Visible = false;
    ((Node) this).GetViewport()?.GuiReleaseFocus();
  }

  public void MakeHalfScreen()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect).Size;
    float num = size.Y * 0.5f;
    this._inputContainer.SetSize(new Vector2(size.X, 40f), false);
    this._inputContainer.Position = new Vector2(0.0f, num - 40f);
    ((Control) this._outputBuffer).SetSize(new Vector2(size.X, num), false);
    ((Control) this._tabBuffer).SetSize(new Vector2(size.X, num), false);
    ((Control) this).SetSize(new Vector2(size.X, size.Y / 2f), false);
    this._isFullscreen = false;
  }

  public void MakeFullScreen()
  {
    Rect2 viewportRect = ((CanvasItem) this).GetViewportRect();
    Vector2 size = ((Rect2) ref viewportRect).Size;
    float y = size.Y;
    this._inputContainer.SetSize(new Vector2(size.X, 40f), false);
    this._inputContainer.Position = new Vector2(0.0f, y - 40f);
    ((Control) this._outputBuffer).SetSize(new Vector2(((Control) this._outputBuffer).Size.X, y - 40f), false);
    ((Control) this._tabBuffer).SetSize(new Vector2(((Control) this._outputBuffer).Size.X, y - 40f), false);
    ((Control) this).SetSize(size, false);
    this._isFullscreen = true;
  }

  private void OnToggleMaximizeButtonPressed()
  {
    if (!this._isFullscreen)
      this.MakeFullScreen();
    else
      this.MakeHalfScreen();
  }

  public void MoveInputCursorToEndOfLine()
  {
    this._inputBuffer.CaretColumn = this._inputBuffer.Text.Length;
  }

  private void UpdatePromptStyle()
  {
    this._promptLabel.Text = this._symbolPrompt;
    ((Control) this._promptLabel).AddThemeColorOverride(ThemeConstants.Label.FontColor, new Color(0.0f, 0.831f, 1f, 1f));
    ((Control) this._promptLabel).AddThemeFontSizeOverride(ThemeConstants.Label.FontSize, 18);
  }

  public void AddChildToTree(Node node) => ((Node) this).AddChildSafely(node);

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(31 /*0x1F*/)
    {
      new MethodInfo(NDevConsole.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("CanvasLayer"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName._EnterTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName._ExitTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.PrintUsage, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName._Input, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("inputEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEvent"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.HandleReadlineKeybinding, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("keyEvent"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("InputEventKey"), false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.DeleteWordBackward, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.KillToEndOfLine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.Yank, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.EnableTabBuffer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.DisableTabBuffer, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.SetBackgroundColor, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 20L, StringName.op_Implicit("color"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.HideGhostText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.ShowGhostText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("ghostText"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.UpdateGhostText, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.AutocompleteCommand, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.RenderSelectionMenu, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.OnInputTextChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 4L, StringName.op_Implicit("newText"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.ExitSelectionMode, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.NavigateSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 2L, StringName.op_Implicit("direction"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false)
      }, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.AcceptSelection, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.ProcessCommand, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.ShowConsole, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.HideConsole, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.MakeHalfScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.MakeFullScreen, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.OnToggleMaximizeButtonPressed, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.MoveInputCursorToEndOfLine, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.UpdatePromptStyle, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NDevConsole.MethodName.AddChildToTree, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("node"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Node"), false)
      }, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      CanvasLayer canvasLayer = NDevConsole.Create();
      ret = VariantUtils.CreateFrom<CanvasLayer>(ref canvasLayer);
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName._EnterTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._EnterTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName._ExitTree) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._ExitTree();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.PrintUsage) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.PrintUsage();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName._Input) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      ((Node) this)._Input(VariantUtils.ConvertTo<InputEvent>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.HandleReadlineKeybinding) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.HandleReadlineKeybinding(VariantUtils.ConvertTo<InputEventKey>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.DeleteWordBackward) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DeleteWordBackward();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.KillToEndOfLine) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.KillToEndOfLine();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.Yank) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Yank();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.EnableTabBuffer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.EnableTabBuffer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.DisableTabBuffer) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.DisableTabBuffer();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.SetBackgroundColor) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetBackgroundColor(VariantUtils.ConvertTo<Color>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.HideGhostText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideGhostText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.ShowGhostText) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ShowGhostText(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.UpdateGhostText) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdateGhostText();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.AutocompleteCommand) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AutocompleteCommand();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.RenderSelectionMenu) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.RenderSelectionMenu();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.OnInputTextChanged) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.OnInputTextChanged(VariantUtils.ConvertTo<string>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.ExitSelectionMode) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ExitSelectionMode();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.NavigateSelection) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.NavigateSelection(VariantUtils.ConvertTo<int>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.AcceptSelection) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.AcceptSelection();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.ProcessCommand) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ProcessCommand();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.ShowConsole) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ShowConsole();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.HideConsole) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.HideConsole();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.MakeHalfScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MakeHalfScreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.MakeFullScreen) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MakeFullScreen();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.OnToggleMaximizeButtonPressed) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnToggleMaximizeButtonPressed();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.MoveInputCursorToEndOfLine) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.MoveInputCursorToEndOfLine();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.UpdatePromptStyle) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.UpdatePromptStyle();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NDevConsole.MethodName.AddChildToTree) || ((NativeVariantPtrArgs) ref args).Count != 1)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.AddChildToTree(VariantUtils.ConvertTo<Node>(ref ((NativeVariantPtrArgs) ref args)[0]));
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NDevConsole.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      CanvasLayer canvasLayer = NDevConsole.Create();
      ret = VariantUtils.CreateFrom<CanvasLayer>(ref canvasLayer);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NDevConsole.MethodName.Create) || StringName.op_Equality(ref method, NDevConsole.MethodName._EnterTree) || StringName.op_Equality(ref method, NDevConsole.MethodName._Ready) || StringName.op_Equality(ref method, NDevConsole.MethodName._ExitTree) || StringName.op_Equality(ref method, NDevConsole.MethodName.PrintUsage) || StringName.op_Equality(ref method, NDevConsole.MethodName._Input) || StringName.op_Equality(ref method, NDevConsole.MethodName.HandleReadlineKeybinding) || StringName.op_Equality(ref method, NDevConsole.MethodName.DeleteWordBackward) || StringName.op_Equality(ref method, NDevConsole.MethodName.KillToEndOfLine) || StringName.op_Equality(ref method, NDevConsole.MethodName.Yank) || StringName.op_Equality(ref method, NDevConsole.MethodName.EnableTabBuffer) || StringName.op_Equality(ref method, NDevConsole.MethodName.DisableTabBuffer) || StringName.op_Equality(ref method, NDevConsole.MethodName.SetBackgroundColor) || StringName.op_Equality(ref method, NDevConsole.MethodName.HideGhostText) || StringName.op_Equality(ref method, NDevConsole.MethodName.ShowGhostText) || StringName.op_Equality(ref method, NDevConsole.MethodName.UpdateGhostText) || StringName.op_Equality(ref method, NDevConsole.MethodName.AutocompleteCommand) || StringName.op_Equality(ref method, NDevConsole.MethodName.RenderSelectionMenu) || StringName.op_Equality(ref method, NDevConsole.MethodName.OnInputTextChanged) || StringName.op_Equality(ref method, NDevConsole.MethodName.ExitSelectionMode) || StringName.op_Equality(ref method, NDevConsole.MethodName.NavigateSelection) || StringName.op_Equality(ref method, NDevConsole.MethodName.AcceptSelection) || StringName.op_Equality(ref method, NDevConsole.MethodName.ProcessCommand) || StringName.op_Equality(ref method, NDevConsole.MethodName.ShowConsole) || StringName.op_Equality(ref method, NDevConsole.MethodName.HideConsole) || StringName.op_Equality(ref method, NDevConsole.MethodName.MakeHalfScreen) || StringName.op_Equality(ref method, NDevConsole.MethodName.MakeFullScreen) || StringName.op_Equality(ref method, NDevConsole.MethodName.OnToggleMaximizeButtonPressed) || StringName.op_Equality(ref method, NDevConsole.MethodName.MoveInputCursorToEndOfLine) || StringName.op_Equality(ref method, NDevConsole.MethodName.UpdatePromptStyle) || StringName.op_Equality(ref method, NDevConsole.MethodName.AddChildToTree) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._outputBuffer))
    {
      this._outputBuffer = VariantUtils.ConvertTo<RichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._tabBuffer))
    {
      this._tabBuffer = VariantUtils.ConvertTo<RichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._inputContainer))
    {
      this._inputContainer = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._inputBuffer))
    {
      this._inputBuffer = VariantUtils.ConvertTo<LineEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._promptLabel))
    {
      this._promptLabel = VariantUtils.ConvertTo<Label>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._ghostTextLabel))
    {
      this._ghostTextLabel = VariantUtils.ConvertTo<Label>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._isFullscreen))
    {
      this._isFullscreen = VariantUtils.ConvertTo<bool>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._yankBuffer))
    {
      this._yankBuffer = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolPrompt))
    {
      this._symbolPrompt = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolWarning))
    {
      this._symbolWarning = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolUp))
    {
      this._symbolUp = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolDown))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._symbolDown = VariantUtils.ConvertTo<string>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._outputBuffer))
    {
      value = VariantUtils.CreateFrom<RichTextLabel>(ref this._outputBuffer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._tabBuffer))
    {
      value = VariantUtils.CreateFrom<RichTextLabel>(ref this._tabBuffer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._inputContainer))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._inputContainer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._inputBuffer))
    {
      value = VariantUtils.CreateFrom<LineEdit>(ref this._inputBuffer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._promptLabel))
    {
      value = VariantUtils.CreateFrom<Label>(ref this._promptLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._ghostTextLabel))
    {
      value = VariantUtils.CreateFrom<Label>(ref this._ghostTextLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._isFullscreen))
    {
      value = VariantUtils.CreateFrom<bool>(ref this._isFullscreen);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._yankBuffer))
    {
      value = VariantUtils.CreateFrom<string>(ref this._yankBuffer);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolPrompt))
    {
      value = VariantUtils.CreateFrom<string>(ref this._symbolPrompt);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolWarning))
    {
      value = VariantUtils.CreateFrom<string>(ref this._symbolWarning);
      return true;
    }
    if (StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolUp))
    {
      value = VariantUtils.CreateFrom<string>(ref this._symbolUp);
      return true;
    }
    if (!StringName.op_Equality(ref name, NDevConsole.PropertyName._symbolDown))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<string>(ref this._symbolDown);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._outputBuffer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._tabBuffer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._inputContainer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._inputBuffer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._promptLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NDevConsole.PropertyName._ghostTextLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 1L, NDevConsole.PropertyName._isFullscreen, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDevConsole.PropertyName._yankBuffer, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDevConsole.PropertyName._symbolPrompt, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDevConsole.PropertyName._symbolWarning, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDevConsole.PropertyName._symbolUp, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NDevConsole.PropertyName._symbolDown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NDevConsole.PropertyName._outputBuffer, Variant.From<RichTextLabel>(ref this._outputBuffer));
    info.AddProperty(NDevConsole.PropertyName._tabBuffer, Variant.From<RichTextLabel>(ref this._tabBuffer));
    info.AddProperty(NDevConsole.PropertyName._inputContainer, Variant.From<Control>(ref this._inputContainer));
    info.AddProperty(NDevConsole.PropertyName._inputBuffer, Variant.From<LineEdit>(ref this._inputBuffer));
    info.AddProperty(NDevConsole.PropertyName._promptLabel, Variant.From<Label>(ref this._promptLabel));
    info.AddProperty(NDevConsole.PropertyName._ghostTextLabel, Variant.From<Label>(ref this._ghostTextLabel));
    info.AddProperty(NDevConsole.PropertyName._isFullscreen, Variant.From<bool>(ref this._isFullscreen));
    info.AddProperty(NDevConsole.PropertyName._yankBuffer, Variant.From<string>(ref this._yankBuffer));
    info.AddProperty(NDevConsole.PropertyName._symbolPrompt, Variant.From<string>(ref this._symbolPrompt));
    info.AddProperty(NDevConsole.PropertyName._symbolWarning, Variant.From<string>(ref this._symbolWarning));
    info.AddProperty(NDevConsole.PropertyName._symbolUp, Variant.From<string>(ref this._symbolUp));
    info.AddProperty(NDevConsole.PropertyName._symbolDown, Variant.From<string>(ref this._symbolDown));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NDevConsole.PropertyName._outputBuffer, ref variant1))
      this._outputBuffer = ((Variant) ref variant1).As<RichTextLabel>();
    Variant variant2;
    if (info.TryGetProperty(NDevConsole.PropertyName._tabBuffer, ref variant2))
      this._tabBuffer = ((Variant) ref variant2).As<RichTextLabel>();
    Variant variant3;
    if (info.TryGetProperty(NDevConsole.PropertyName._inputContainer, ref variant3))
      this._inputContainer = ((Variant) ref variant3).As<Control>();
    Variant variant4;
    if (info.TryGetProperty(NDevConsole.PropertyName._inputBuffer, ref variant4))
      this._inputBuffer = ((Variant) ref variant4).As<LineEdit>();
    Variant variant5;
    if (info.TryGetProperty(NDevConsole.PropertyName._promptLabel, ref variant5))
      this._promptLabel = ((Variant) ref variant5).As<Label>();
    Variant variant6;
    if (info.TryGetProperty(NDevConsole.PropertyName._ghostTextLabel, ref variant6))
      this._ghostTextLabel = ((Variant) ref variant6).As<Label>();
    Variant variant7;
    if (info.TryGetProperty(NDevConsole.PropertyName._isFullscreen, ref variant7))
      this._isFullscreen = ((Variant) ref variant7).As<bool>();
    Variant variant8;
    if (info.TryGetProperty(NDevConsole.PropertyName._yankBuffer, ref variant8))
      this._yankBuffer = ((Variant) ref variant8).As<string>();
    Variant variant9;
    if (info.TryGetProperty(NDevConsole.PropertyName._symbolPrompt, ref variant9))
      this._symbolPrompt = ((Variant) ref variant9).As<string>();
    Variant variant10;
    if (info.TryGetProperty(NDevConsole.PropertyName._symbolWarning, ref variant10))
      this._symbolWarning = ((Variant) ref variant10).As<string>();
    Variant variant11;
    if (info.TryGetProperty(NDevConsole.PropertyName._symbolUp, ref variant11))
      this._symbolUp = ((Variant) ref variant11).As<string>();
    Variant variant12;
    if (!info.TryGetProperty(NDevConsole.PropertyName._symbolDown, ref variant12))
      return;
    this._symbolDown = ((Variant) ref variant12).As<string>();
  }

  public class MethodName : Panel.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _EnterTree = StringName.op_Implicit(nameof (_EnterTree));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName _ExitTree = StringName.op_Implicit(nameof (_ExitTree));
    public static readonly StringName PrintUsage = StringName.op_Implicit(nameof (PrintUsage));
    public static readonly StringName _Input = StringName.op_Implicit(nameof (_Input));
    public static readonly StringName HandleReadlineKeybinding = StringName.op_Implicit(nameof (HandleReadlineKeybinding));
    public static readonly StringName DeleteWordBackward = StringName.op_Implicit(nameof (DeleteWordBackward));
    public static readonly StringName KillToEndOfLine = StringName.op_Implicit(nameof (KillToEndOfLine));
    public static readonly StringName Yank = StringName.op_Implicit(nameof (Yank));
    public static readonly StringName EnableTabBuffer = StringName.op_Implicit(nameof (EnableTabBuffer));
    public static readonly StringName DisableTabBuffer = StringName.op_Implicit(nameof (DisableTabBuffer));
    public static readonly StringName SetBackgroundColor = StringName.op_Implicit(nameof (SetBackgroundColor));
    public static readonly StringName HideGhostText = StringName.op_Implicit(nameof (HideGhostText));
    public static readonly StringName ShowGhostText = StringName.op_Implicit(nameof (ShowGhostText));
    public static readonly StringName UpdateGhostText = StringName.op_Implicit(nameof (UpdateGhostText));
    public static readonly StringName AutocompleteCommand = StringName.op_Implicit(nameof (AutocompleteCommand));
    public static readonly StringName RenderSelectionMenu = StringName.op_Implicit(nameof (RenderSelectionMenu));
    public static readonly StringName OnInputTextChanged = StringName.op_Implicit(nameof (OnInputTextChanged));
    public static readonly StringName ExitSelectionMode = StringName.op_Implicit(nameof (ExitSelectionMode));
    public static readonly StringName NavigateSelection = StringName.op_Implicit(nameof (NavigateSelection));
    public static readonly StringName AcceptSelection = StringName.op_Implicit(nameof (AcceptSelection));
    public static readonly StringName ProcessCommand = StringName.op_Implicit(nameof (ProcessCommand));
    public static readonly StringName ShowConsole = StringName.op_Implicit(nameof (ShowConsole));
    public static readonly StringName HideConsole = StringName.op_Implicit(nameof (HideConsole));
    public static readonly StringName MakeHalfScreen = StringName.op_Implicit(nameof (MakeHalfScreen));
    public static readonly StringName MakeFullScreen = StringName.op_Implicit(nameof (MakeFullScreen));
    public static readonly StringName OnToggleMaximizeButtonPressed = StringName.op_Implicit(nameof (OnToggleMaximizeButtonPressed));
    public static readonly StringName MoveInputCursorToEndOfLine = StringName.op_Implicit(nameof (MoveInputCursorToEndOfLine));
    public static readonly StringName UpdatePromptStyle = StringName.op_Implicit(nameof (UpdatePromptStyle));
    public static readonly StringName AddChildToTree = StringName.op_Implicit(nameof (AddChildToTree));
  }

  public class PropertyName : Panel.PropertyName
  {
    public static readonly StringName _outputBuffer = StringName.op_Implicit(nameof (_outputBuffer));
    public static readonly StringName _tabBuffer = StringName.op_Implicit(nameof (_tabBuffer));
    public static readonly StringName _inputContainer = StringName.op_Implicit(nameof (_inputContainer));
    public static readonly StringName _inputBuffer = StringName.op_Implicit(nameof (_inputBuffer));
    public static readonly StringName _promptLabel = StringName.op_Implicit(nameof (_promptLabel));
    public static readonly StringName _ghostTextLabel = StringName.op_Implicit(nameof (_ghostTextLabel));
    public static readonly StringName _isFullscreen = StringName.op_Implicit(nameof (_isFullscreen));
    public static readonly StringName _yankBuffer = StringName.op_Implicit(nameof (_yankBuffer));
    public static readonly StringName _symbolPrompt = StringName.op_Implicit(nameof (_symbolPrompt));
    public static readonly StringName _symbolWarning = StringName.op_Implicit(nameof (_symbolWarning));
    public static readonly StringName _symbolUp = StringName.op_Implicit(nameof (_symbolUp));
    public static readonly StringName _symbolDown = StringName.op_Implicit(nameof (_symbolDown));
  }

  public class SignalName : Panel.SignalName
  {
  }
}
