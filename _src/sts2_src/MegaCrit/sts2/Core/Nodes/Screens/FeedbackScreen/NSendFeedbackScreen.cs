// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen.NSendFeedbackScreen
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using Godot.Bridge;
using Godot.NativeInterop;
using MegaCrit.Sts2.addons.mega_text;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Nodes.CommonUi;
using MegaCrit.Sts2.Core.Nodes.GodotExtensions;
using MegaCrit.Sts2.Core.Nodes.Screens.ScreenContext;
using MegaCrit.Sts2.Core.Platform;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.TestSupport;
using Sentry;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Runtime.InteropServices;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Nodes.Screens.FeedbackScreen;

[ScriptPath("res://src/Core/Nodes/Screens/FeedbackScreen/NSendFeedbackScreen.cs")]
public class NSendFeedbackScreen : Control, IScreenContext
{
  private static readonly string _scenePath = SceneHelper.GetScenePath("screens/feedback_screen/feedback_screen");
  private const float _superWiggleTime = 0.25f;
  private const string _defaultUrl = "https://feedback.sts2.megacrit.com/feedback";
  private static readonly string _url = Environment.GetEnvironmentVariable("STS2_FEEDBACK_URL") ?? "https://feedback.sts2.megacrit.com/feedback";
  private static readonly HttpClient _httpClient = new HttpClient()
  {
    Timeout = TimeSpan.FromSeconds(10L)
  };
  private const int _maxDescriptionChars = 8000;
  private NBackButton _backButton;
  private Control _mainPanel;
  private NMegaTextEdit _descriptionInput;
  private MegaLabel _emojiLabel;
  private NButton _sendButton;
  private MegaLabel _sendLabel;
  private MegaLabel _categoryLabel;
  private NButton _returnToGameButton;
  private MegaLabel _returnToGameLabel;
  private MegaLabel _returnToGameHoverLabel;
  private NFeedbackCategoryDropdown _categoryDropdown;
  private Control _sendBackstop;
  private Control _sendPanel;
  private MegaLabel _successLabel;
  private MegaLabel _failedLabel;
  private MegaRichTextLabel _sendingLabel;
  private List<NSendFeedbackCartoon> _cartoons = new List<NSendFeedbackCartoon>();
  private NSendFeedbackFlower _flower;
  private CancellationTokenSource? _screenClosedCancelToken;
  private TaskCompletionSource? _runInBackgroundTaskSource;
  private readonly List<NSendFeedbackEmojiButton> _emojiButtons = new List<NSendFeedbackEmojiButton>();
  private NSendFeedbackEmojiButton? _selectedEmoteButton;
  private byte[]? _screenshotBytes;
  private Vector2 _originalSuccessPosition;
  private ulong _lastClosedMsec;
  private string _descriptionText = string.Empty;
  private int _descriptionCaretLine;
  private int _descriptionCaretColumn;
  private Tween? _wiggleTween;

  public static NSendFeedbackScreen? Create()
  {
    return TestMode.IsOn ? (NSendFeedbackScreen) null : PreloadManager.Cache.GetScene(NSendFeedbackScreen._scenePath).Instantiate<NSendFeedbackScreen>((PackedScene.GenEditState) 0L);
  }

  public override void _Ready()
  {
    this._mainPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%MainPanel"));
    this._descriptionInput = ((Node) this).GetNode<NMegaTextEdit>(NodePath.op_Implicit("%DescriptionInput"));
    this._emojiLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%EmojiLabel"));
    this._sendButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%SendButton"));
    this._returnToGameButton = ((Node) this).GetNode<NButton>(NodePath.op_Implicit("%ReturnToGameButton"));
    this._returnToGameLabel = ((Node) this._returnToGameButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._returnToGameHoverLabel = ((Node) this._returnToGameButton).GetNode<MegaLabel>(NodePath.op_Implicit("%ReturnToGameHoverLabel"));
    this._sendLabel = ((Node) this._sendButton).GetNode<MegaLabel>(NodePath.op_Implicit("Label"));
    this._categoryLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%CategoryLabel"));
    this._categoryDropdown = ((Node) this).GetNode<NFeedbackCategoryDropdown>(NodePath.op_Implicit("%CategoryDropdown"));
    this._sendBackstop = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SendBackstop"));
    this._sendPanel = ((Node) this).GetNode<Control>(NodePath.op_Implicit("%SendPanel"));
    this._successLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%SuccessLabel"));
    this._failedLabel = ((Node) this).GetNode<MegaLabel>(NodePath.op_Implicit("%FailedLabel"));
    this._sendingLabel = ((Node) this).GetNode<MegaRichTextLabel>(NodePath.op_Implicit("%SendingLabel"));
    this._backButton = ((Node) this).GetNode<NBackButton>(NodePath.op_Implicit("BackButton"));
    this._successLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_SUCCESS_LABEL").GetFormattedText());
    this._failedLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_FAILED_LABEL").GetFormattedText());
    this._sendingLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SENDING_LABEL").GetFormattedText());
    this._returnToGameLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_RUN_IN_BACKGROUND_LABEL").GetFormattedText());
    this._returnToGameHoverLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_RUN_IN_BACKGROUND_HOVER_LABEL").GetFormattedText());
    this._originalSuccessPosition = this._sendPanel.Position;
    int capacity = 3;
    List<NSendFeedbackCartoon> nsendFeedbackCartoonList = new List<NSendFeedbackCartoon>(capacity);
    CollectionsMarshal.SetCount<NSendFeedbackCartoon>(nsendFeedbackCartoonList, capacity);
    Span<NSendFeedbackCartoon> span = CollectionsMarshal.AsSpan<NSendFeedbackCartoon>(nsendFeedbackCartoonList);
    int num1 = 0;
    span[num1] = ((Node) this).GetNode<NSendFeedbackCartoon>(NodePath.op_Implicit("Sun"));
    int num2 = num1 + 1;
    span[num2] = ((Node) this).GetNode<NSendFeedbackCartoon>(NodePath.op_Implicit("Cupcake"));
    int num3 = num2 + 1;
    span[num3] = ((Node) this).GetNode<NSendFeedbackCartoon>(NodePath.op_Implicit("FlowerContainer/Flower"));
    this._cartoons = nsendFeedbackCartoonList;
    this._flower = ((Node) this).GetNode<NSendFeedbackFlower>(NodePath.op_Implicit("FlowerContainer"));
    foreach (Node child in ((Node) this).GetNode(NodePath.op_Implicit("%EmojiButtonContainer")).GetChildren(false))
    {
      if (child is NSendFeedbackEmojiButton feedbackEmojiButton)
      {
        this._emojiButtons.Add(feedbackEmojiButton);
        feedbackEmojiButton.PivotOffset = Vector2.op_Multiply(feedbackEmojiButton.Size, 0.5f);
        ((GodotObject) feedbackEmojiButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.EmojiButtonSelected)), 0U);
      }
    }
    ((GodotObject) this._sendButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.SendButtonSelected)), 0U);
    ((GodotObject) this._sendButton).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.SendButtonFocused)), 0U);
    ((GodotObject) this._sendButton).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.SendButtonUnfocused)), 0U);
    ((GodotObject) this._returnToGameButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>(new Action<NButton>(this.ReturnToGameSelected)), 0U);
    ((GodotObject) this._returnToGameButton).Connect(NClickableControl.SignalName.Focused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.ReturnToGameFocused)), 0U);
    ((GodotObject) this._returnToGameButton).Connect(NClickableControl.SignalName.Unfocused, Callable.From<NClickableControl>(new Action<NClickableControl>(this.ReturnToGameUnfocused)), 0U);
    ((GodotObject) this._descriptionInput).Connect(TextEdit.SignalName.TextChanged, Callable.From(new Action(this.OnDescriptionChanged)), 0U);
    ((CanvasItem) this._returnToGameHoverLabel).Visible = false;
    this._sendButton.FocusNeighborTop = ((Node) this._categoryDropdown).GetPath();
    this._sendButton.FocusNeighborLeft = ((Node) this._emojiButtons.Last<NSendFeedbackEmojiButton>()).GetPath();
    this._sendButton.FocusNeighborBottom = ((Node) this._sendButton).GetPath();
    this._sendButton.FocusNeighborRight = ((Node) this._sendButton).GetPath();
    this._emojiButtons.Last<NSendFeedbackEmojiButton>().FocusNeighborRight = ((Node) this._sendButton).GetPath();
    foreach (NSendFeedbackEmojiButton emojiButton in this._emojiButtons)
    {
      emojiButton.FocusNeighborTop = ((Node) this._categoryDropdown).GetPath();
      emojiButton.FocusNeighborBottom = ((Node) emojiButton).GetPath();
    }
    this._categoryDropdown.FocusNeighborRight = ((Node) this._sendButton).GetPath();
    this._categoryDropdown.FocusNeighborBottom = ((Node) this._emojiButtons.First<NSendFeedbackEmojiButton>()).GetPath();
    this._categoryDropdown.FocusNeighborTop = ((Node) this._descriptionInput).GetPath();
    ((Control) this._descriptionInput).FocusNeighborTop = ((Node) this._descriptionInput).GetPath();
    ((Control) this._descriptionInput).FocusNeighborLeft = ((Node) this._descriptionInput).GetPath();
    ((Control) this._descriptionInput).FocusNeighborRight = ((Node) this._descriptionInput).GetPath();
    ((Control) this._descriptionInput).FocusNeighborBottom = ((Node) this._categoryDropdown).GetPath();
    ((GodotObject) this._backButton).Connect(NClickableControl.SignalName.Released, Callable.From<NButton>((Action<NButton>) (_ => this.Close())), 0U);
    ((CanvasItem) this).Visible = false;
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backButton.Disable();
  }

  public void Relocalize()
  {
    this._descriptionInput.PlaceholderText = new LocString("settings_ui", "FEEDBACK_DESCRIPTION_PLACEHOLDER").GetFormattedText();
    this._categoryLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_CATEGORY_LABEL").GetFormattedText());
    this._emojiLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_EMOJI_LABEL").GetFormattedText());
    this._sendLabel.SetTextAutoSize(new LocString("settings_ui", "FEEDBACK_SEND_BUTTON_LABEL").GetFormattedText());
    this._descriptionInput.RefreshFont();
    this._categoryLabel.RefreshFont();
    this._emojiLabel.RefreshFont();
    this._sendLabel.RefreshFont();
  }

  private void OnDescriptionChanged()
  {
    if (this._descriptionInput.Text.Length > 8000)
    {
      this._descriptionInput.Text = this._descriptionText;
      this._descriptionInput.SetCaretLine(this._descriptionCaretLine, true, true, 0, 0);
      this._descriptionInput.SetCaretColumn(this._descriptionCaretColumn, true, 0);
    }
    else
    {
      this._descriptionText = this._descriptionInput.Text;
      this._descriptionCaretLine = this._descriptionInput.GetCaretLine(0);
      this._descriptionCaretColumn = this._descriptionInput.GetCaretColumn(0);
    }
  }

  public void SetScreenshot(Image screenshot)
  {
    int width = screenshot.GetWidth();
    int height = screenshot.GetHeight();
    float num = (float) width / (float) height;
    if (width > 1280 /*0x0500*/)
      screenshot.Resize(1280 /*0x0500*/, Mathf.RoundToInt(1280f / num), (Image.Interpolation) 1L);
    if (height > 720)
      screenshot.Resize(Mathf.RoundToInt(720f * num), 720, (Image.Interpolation) 1L);
    this._screenshotBytes = screenshot.SavePngToBuffer();
  }

  private void EmojiButtonSelected(NButton button)
  {
    this.SetSelectedEmoji((NSendFeedbackEmojiButton) button);
  }

  private void SendButtonFocused(NClickableControl _)
  {
    this._flower.SetState(NSendFeedbackFlower.State.Anticipation);
  }

  private void SendButtonUnfocused(NClickableControl _)
  {
    if (this._flower.MyState != NSendFeedbackFlower.State.Anticipation || ((CanvasItem) this._sendBackstop).Visible)
      return;
    this._flower.SetState(NSendFeedbackFlower.State.None);
  }

  public void Open()
  {
    if (((CanvasItem) this).Visible)
      return;
    Log.Info("Feedback screen opened");
    if (Time.GetTicksMsec() - this._lastClosedMsec > 60000UL)
      this.ClearInput();
    this._screenClosedCancelToken = new CancellationTokenSource();
    ((CanvasItem) this).Visible = true;
    this._flower.SetState(NSendFeedbackFlower.State.None);
    ((CanvasItem) this._sendBackstop).Visible = false;
    this._sendButton.Enable();
    this.MouseFilter = (Control.MouseFilterEnum) 0L;
    NHotkeyManager.Instance.AddBlockingScreen((Node) this);
    ActiveScreenContext.Instance.Update();
    this._backButton.Enable();
  }

  private void Close()
  {
    Log.Info("Feedback screen closed");
    this._flower.SetState(NSendFeedbackFlower.State.None);
    ((CanvasItem) this._sendBackstop).Visible = false;
    ((CanvasItem) this._mainPanel).Modulate = Colors.White;
    this._wiggleTween?.Kill();
    ((CanvasItem) this).Visible = false;
    this._lastClosedMsec = Time.GetTicksMsec();
    this._screenClosedCancelToken?.Cancel();
    this.MouseFilter = (Control.MouseFilterEnum) 2L;
    this._backButton.Disable();
    NHotkeyManager.Instance.RemoveBlockingScreen((Node) this);
    ActiveScreenContext.Instance.Update();
  }

  private void ClearInput()
  {
    this._descriptionInput.Text = string.Empty;
    this._descriptionText = string.Empty;
    this.SetSelectedEmoji((NSendFeedbackEmojiButton) null);
  }

  private void SetSelectedEmoji(NSendFeedbackEmojiButton? button)
  {
    NSendFeedbackEmojiButton selectedEmoteButton = this._selectedEmoteButton;
    this._selectedEmoteButton?.SetSelected(false);
    if (selectedEmoteButton == button)
      return;
    this._selectedEmoteButton = button;
    this._selectedEmoteButton?.SetSelected(true);
  }

  private void SendButtonSelected(NButton _)
  {
    TaskHelper.RunSafely(this.SendFeedbackWrapper());
    this._sendButton.Disable();
  }

  private void ReturnToGameSelected(NButton _) => this._runInBackgroundTaskSource?.TrySetResult();

  private void ReturnToGameFocused(NClickableControl _)
  {
    ((CanvasItem) this._returnToGameHoverLabel).Visible = true;
  }

  private void ReturnToGameUnfocused(NClickableControl _)
  {
    ((CanvasItem) this._returnToGameHoverLabel).Visible = false;
  }

  private async Task SendFeedbackWrapper()
  {
    Task<bool> sendTask;
    if (string.IsNullOrEmpty(this._descriptionText))
    {
      sendTask = (Task<bool>) null;
    }
    else
    {
      Log.Info($"Beginning asynchronous feedback send at {Log.Timestamp}: {this._descriptionText}");
      ReleaseInfo releaseInfo = ReleaseInfoManager.Instance.ReleaseInfo;
      string str = releaseInfo?.Commit ?? GitHelper.ShortCommitId;
      FeedbackData data = new FeedbackData()
      {
        description = this._descriptionText,
        category = this._categoryDropdown.CurrentCategory,
        gameVersion = releaseInfo?.Version ?? "v0.0.1",
        uniqueId = SaveManager.Instance.Progress.UniqueId,
        commit = str ?? "unknown",
        platformBranch = PlatformUtil.GetPlatformBranch().ToName(),
        sessionId = SentryService.SessionId,
        isModded = ModManager.IsRunningModded() || ModManager.HasHarmonyPatches(),
        isFullConsole = SaveManager.Instance.SettingsSave.FullConsole,
        lang = LocManager.Instance.Language
      };
      MemoryStream screenshotStream = new MemoryStream(this._screenshotBytes);
      int currentProfileId = SaveManager.Instance.CurrentProfileId;
      MemoryStream memoryStream = new MemoryStream();
      GetLogsConsoleCmd.ZipFeedbackLogs((Stream) memoryStream, currentProfileId);
      ((Stream) memoryStream).Seek(0L, (SeekOrigin) 0);
      this._runInBackgroundTaskSource = new TaskCompletionSource();
      sendTask = NSendFeedbackScreen.SendFeedback(data, (Stream) screenshotStream, (Stream) memoryStream);
      ((CanvasItem) this._sendBackstop).Visible = true;
      ((CanvasItem) this._sendingLabel).Visible = true;
      ((CanvasItem) this._failedLabel).Visible = false;
      ((CanvasItem) this._successLabel).Visible = false;
      ((CanvasItem) this._sendPanel).Modulate = Colors.Transparent;
      Control sendPanel = this._sendPanel;
      Vector2 position = this._sendPanel.Position;
      position.Y = this._originalSuccessPosition.Y + 20f;
      Vector2 vector2 = position;
      sendPanel.Position = vector2;
      Tween tween = ((Node) this).GetTree().CreateTween().Parallel();
      tween.TweenProperty((GodotObject) this._mainPanel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(new Color(0.1f, 0.1f, 0.1f, 1f)), 0.15000000596046448);
      tween.TweenProperty((GodotObject) this._sendPanel, NodePath.op_Implicit("modulate"), Variant.op_Implicit(Colors.White), 0.15000000596046448);
      tween.TweenProperty((GodotObject) this._sendPanel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(this._originalSuccessPosition.Y), 0.15000000596046448).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
      tween.Chain().TweenProperty((GodotObject) this._successLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._successLabel).Position.Y - 10f), 0.10000000149011612).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
      await TaskHelper.WhenAny((Task) sendTask, this._runInBackgroundTaskSource.Task);
      if (!((Node) this).IsValid())
        sendTask = (Task<bool>) null;
      else if (this._runInBackgroundTaskSource.Task.IsCompleted)
      {
        this.Close();
        sendTask = (Task<bool>) null;
      }
      else if (await sendTask)
      {
        await this.OnFeedbackSuccess();
        sendTask = (Task<bool>) null;
      }
      else
      {
        await this.OnFeedbackFailed();
        sendTask = (Task<bool>) null;
      }
    }
  }

  private static async Task<bool> SendFeedback(
    FeedbackData data,
    Stream screenshotStream,
    Stream logsMemoryStream)
  {
    try
    {
      using (MultipartFormDataContent formContent = NSendFeedbackScreen.BuildMultipartContent(data, screenshotStream, logsMemoryStream))
      {
        int[] delaysMs = new int[3]{ 500, 1000, 2000 };
        string sentryMessage = (string) null;
        for (int attempt = 0; attempt < 3; ++attempt)
        {
          try
          {
            using (HttpResponseMessage response = await NSendFeedbackScreen._httpClient.PutAsync(NSendFeedbackScreen._url, (HttpContent) formContent))
            {
              if (response.IsSuccessStatusCode)
              {
                Log.Info("Feedback successfully posted!");
                return true;
              }
              int statusCode = (int) response.StatusCode;
              if (statusCode >= 400 && statusCode < 500 && statusCode != 429)
              {
                string str = await response.Content.ReadAsStringAsync();
                Log.Warn($"Feedback rejected ({response.StatusCode}): {str}");
                SentrySdk.CaptureMessage($"Feedback rejected: Response status code {response.StatusCode}", (SentryLevel) 1);
                return false;
              }
              sentryMessage = $"Response status code {response.StatusCode}";
              Log.Warn($"Feedback attempt {attempt + 1}/{3} failed: {response.StatusCode}");
            }
          }
          catch (Exception ex) when (
          {
            // ISSUE: unable to correctly present filter
            bool flag;
            switch (ex)
            {
              case HttpRequestException _:
              case TaskCanceledException _:
                flag = true;
                break;
              default:
                flag = false;
                break;
            }
            if (flag)
            {
              SuccessfulFiltering;
            }
            else
              throw;
          }
          )
          {
            string text = $"Feedback attempt {attempt + 1}/{3} network error: {ex}\n Inner messages: {NSendFeedbackScreen.ExceptionMessageWithInner(ex)}";
            if (ex is HttpRequestException requestException && requestException.HttpRequestError != 1)
              sentryMessage = $"{ex.GetType().Name}: {NSendFeedbackScreen.ExceptionMessageWithInner(ex)}";
            Log.Warn(text);
          }
          if (attempt < 2)
          {
            await Task.Delay(delaysMs[attempt]);
            screenshotStream.Seek(0L, (SeekOrigin) 0);
            logsMemoryStream.Seek(0L, (SeekOrigin) 0);
          }
        }
        Log.Warn("Feedback send failed after all retry attempts");
        if (sentryMessage != null)
          SentrySdk.CaptureMessage("Feedback failed to send: " + sentryMessage, (SentryLevel) 1);
        return false;
      }
    }
    finally
    {
      screenshotStream.Close();
      logsMemoryStream.Close();
    }
  }

  private static string ExceptionMessageWithInner(Exception ex)
  {
    return ex.InnerException == null ? ex.Message : $"{ex.Message} | {NSendFeedbackScreen.ExceptionMessageWithInner(ex.InnerException)}";
  }

  private static MultipartFormDataContent BuildMultipartContent(
    FeedbackData data,
    Stream screenshotStream,
    Stream logsStream)
  {
    string content1 = JsonSerializer.Serialize<FeedbackData>(data, JsonSerializationUtility.GetTypeInfo<FeedbackData>());
    MultipartFormDataContent multipartFormDataContent = new MultipartFormDataContent();
    StringContent content2 = new StringContent(content1);
    content2.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    content2.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
    {
      Name = "payload_json"
    };
    multipartFormDataContent.Add((HttpContent) content2);
    StreamContent content3 = new StreamContent(logsStream);
    content3.Headers.ContentType = new MediaTypeHeaderValue("application/zip");
    content3.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
    {
      Name = "logs"
    };
    multipartFormDataContent.Add((HttpContent) content3);
    StreamContent content4 = new StreamContent(screenshotStream);
    content4.Headers.ContentType = new MediaTypeHeaderValue("image/png");
    content4.Headers.ContentDisposition = new ContentDispositionHeaderValue("form-data")
    {
      Name = "screenshot"
    };
    multipartFormDataContent.Add((HttpContent) content4);
    return multipartFormDataContent;
  }

  private async Task OnFeedbackFailed()
  {
    ((CanvasItem) this._sendingLabel).Visible = false;
    ((CanvasItem) this._failedLabel).Visible = true;
    ((CanvasItem) this._successLabel).Visible = false;
    Tween tween = ((Node) this).GetTree().CreateTween().Chain();
    tween.TweenProperty((GodotObject) this._failedLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._successLabel).Position.X - 10f), 0.02500000037252903).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._failedLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._successLabel).Position.X + 10f), 0.05000000074505806).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._failedLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._successLabel).Position.X - 10f), 0.05000000074505806).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._failedLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._successLabel).Position.X + 10f), 0.05000000074505806).SetEase((Tween.EaseType) 2L).SetTrans((Tween.TransitionType) 4L);
    tween.TweenProperty((GodotObject) this._failedLabel, NodePath.op_Implicit("position:x"), Variant.op_Implicit(((Control) this._successLabel).Position.X), 0.02500000037252903).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    this._wiggleTween?.Kill();
    this._flower.SetState(NSendFeedbackFlower.State.None);
    CancellationTokenSource closedCancelToken = this._screenClosedCancelToken;
    await Task.Delay(2000, closedCancelToken != null ? closedCancelToken.Token : new CancellationToken());
    ((CanvasItem) this._sendBackstop).Visible = false;
    ((CanvasItem) this._mainPanel).Modulate = Colors.White;
  }

  private async Task OnFeedbackSuccess()
  {
    this.ClearInput();
    this._screenshotBytes = (byte[]) null;
    ((CanvasItem) this._sendingLabel).Visible = false;
    ((CanvasItem) this._failedLabel).Visible = false;
    ((CanvasItem) this._successLabel).Visible = true;
    MegaLabel successLabel = this._successLabel;
    Color modulate = ((CanvasItem) this._successLabel).Modulate;
    modulate.A = 0.0f;
    Color color = modulate;
    ((CanvasItem) successLabel).Modulate = color;
    Tween tween = ((Node) this).GetTree().CreateTween().Parallel();
    tween.TweenProperty((GodotObject) this._successLabel, NodePath.op_Implicit("modulate:a"), Variant.op_Implicit(1f), 0.15000000596046448);
    tween.TweenProperty((GodotObject) this._successLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._successLabel).Position.Y - 10f), 0.10000000149011612).SetEase((Tween.EaseType) 1L).SetTrans((Tween.TransitionType) 4L);
    tween.Chain().TweenProperty((GodotObject) this._successLabel, NodePath.op_Implicit("position:y"), Variant.op_Implicit(((Control) this._successLabel).Position.Y), 0.10000000149011612).SetEase((Tween.EaseType) 0L).SetTrans((Tween.TransitionType) 4L);
    this._wiggleTween?.Kill();
    this._wiggleTween = ((Node) this).CreateTween();
    this._wiggleTween.TweenCallback(Callable.From(new Action(this.WiggleCartoons1)));
    this._wiggleTween.TweenInterval(0.25);
    this._wiggleTween.TweenCallback(Callable.From(new Action(this.WiggleCartoons2)));
    this._wiggleTween.TweenInterval(0.25);
    this._wiggleTween.SetLoops(0);
    Node2D child = PreloadManager.Cache.GetScene(SceneHelper.GetScenePath("vfx/vfx_dramatic_entrance_fullscreen")).Instantiate<Node2D>((PackedScene.GenEditState) 0L);
    ((Node) this).AddChildSafely((Node) child);
    ((Node) this).MoveChildSafely((Node) child, 1);
    Node2D node2D = child;
    Rect2 viewportRect = ((CanvasItem) NGame.Instance).GetViewportRect();
    Vector2 vector2 = Vector2.op_Multiply(((Rect2) ref viewportRect).Size, 0.5f);
    node2D.GlobalPosition = vector2;
    this._flower.SetState(NSendFeedbackFlower.State.NoddingFast);
    CancellationTokenSource closedCancelToken = this._screenClosedCancelToken;
    await Task.Delay(2000, closedCancelToken != null ? closedCancelToken.Token : new CancellationToken());
    this.Close();
  }

  private void WiggleCartoons1()
  {
    foreach (NSendFeedbackCartoon cartoon in this._cartoons)
    {
      if (this._flower.MyState == NSendFeedbackFlower.State.None || cartoon != this._flower.Cartoon)
        cartoon.SetRotation1();
    }
  }

  private void WiggleCartoons2()
  {
    foreach (NSendFeedbackCartoon cartoon in this._cartoons)
    {
      if (this._flower.MyState == NSendFeedbackFlower.State.None || cartoon != this._flower.Cartoon)
        cartoon.SetRotation2();
    }
  }

  public Control DefaultFocusedControl => (Control) this._descriptionInput;

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static 
  #nullable disable
  List<MethodInfo> GetGodotMethodList()
  {
    return new List<MethodInfo>(18)
    {
      new MethodInfo(NSendFeedbackScreen.MethodName.Create, new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false), (MethodFlags) 33L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName._Ready, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.Relocalize, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.OnDescriptionChanged, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.SetScreenshot, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("screenshot"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Image"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.EmojiButtonSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.Open, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.Close, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.ClearInput, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.SetSelectedEmoji, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("button"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.SendButtonSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameSelected, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameFocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.ReturnToGameUnfocused, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, new List<PropertyInfo>()
      {
        new PropertyInfo((Variant.Type) 24L, StringName.op_Implicit("_"), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, new StringName("Control"), false)
      }, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.WiggleCartoons1, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null),
      new MethodInfo(NSendFeedbackScreen.MethodName.WiggleCartoons2, new PropertyInfo((Variant.Type) 0L, StringName.op_Implicit(""), (PropertyHint) 0L, "", (PropertyUsageFlags) 6L, false), (MethodFlags) 1L, (List<PropertyInfo>) null, (List<Variant>) null)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool InvokeGodotClassMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSendFeedbackScreen nsendFeedbackScreen = NSendFeedbackScreen.Create();
      ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref nsendFeedbackScreen);
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName._Ready) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      ((Node) this)._Ready();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Relocalize) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Relocalize();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.OnDescriptionChanged) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.OnDescriptionChanged();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SetScreenshot) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetScreenshot(VariantUtils.ConvertTo<Image>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.EmojiButtonSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.EmojiButtonSelected(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SendButtonFocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SendButtonUnfocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Open) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Open();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Close) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.Close();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ClearInput) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.ClearInput();
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SetSelectedEmoji) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SetSelectedEmoji(VariantUtils.ConvertTo<NSendFeedbackEmojiButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.SendButtonSelected(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameSelected) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ReturnToGameSelected(VariantUtils.ConvertTo<NButton>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameFocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ReturnToGameFocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameUnfocused) && ((NativeVariantPtrArgs) ref args).Count == 1)
    {
      this.ReturnToGameUnfocused(VariantUtils.ConvertTo<NClickableControl>(ref ((NativeVariantPtrArgs) ref args)[0]));
      ret = new godot_variant();
      return true;
    }
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.WiggleCartoons1) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      this.WiggleCartoons1();
      ret = new godot_variant();
      return true;
    }
    if (!StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.WiggleCartoons2) || ((NativeVariantPtrArgs) ref args).Count != 0)
      return base.InvokeGodotClassMethod(ref method, args, ref ret);
    this.WiggleCartoons2();
    ret = new godot_variant();
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static bool InvokeGodotClassStaticMethod(
    in godot_string_name method,
    NativeVariantPtrArgs args,
    out godot_variant ret)
  {
    if (StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Create) && ((NativeVariantPtrArgs) ref args).Count == 0)
    {
      NSendFeedbackScreen nsendFeedbackScreen = NSendFeedbackScreen.Create();
      ret = VariantUtils.CreateFrom<NSendFeedbackScreen>(ref nsendFeedbackScreen);
      return true;
    }
    ret = new godot_variant();
    return false;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool HasGodotClassMethod(in godot_string_name method)
  {
    return StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Create) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName._Ready) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Relocalize) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.OnDescriptionChanged) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SetScreenshot) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.EmojiButtonSelected) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonFocused) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonUnfocused) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Open) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.Close) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ClearInput) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SetSelectedEmoji) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.SendButtonSelected) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameSelected) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameFocused) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.ReturnToGameUnfocused) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.WiggleCartoons1) || StringName.op_Equality(ref method, NSendFeedbackScreen.MethodName.WiggleCartoons2) || base.HasGodotClassMethod(ref method);
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool SetGodotClassPropertyValue(
    in godot_string_name name,
    in godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._backButton))
    {
      this._backButton = VariantUtils.ConvertTo<NBackButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._mainPanel))
    {
      this._mainPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionInput))
    {
      this._descriptionInput = VariantUtils.ConvertTo<NMegaTextEdit>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._emojiLabel))
    {
      this._emojiLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendButton))
    {
      this._sendButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendLabel))
    {
      this._sendLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._categoryLabel))
    {
      this._categoryLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameButton))
    {
      this._returnToGameButton = VariantUtils.ConvertTo<NButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameLabel))
    {
      this._returnToGameLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameHoverLabel))
    {
      this._returnToGameHoverLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._categoryDropdown))
    {
      this._categoryDropdown = VariantUtils.ConvertTo<NFeedbackCategoryDropdown>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendBackstop))
    {
      this._sendBackstop = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendPanel))
    {
      this._sendPanel = VariantUtils.ConvertTo<Control>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._successLabel))
    {
      this._successLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._failedLabel))
    {
      this._failedLabel = VariantUtils.ConvertTo<MegaLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendingLabel))
    {
      this._sendingLabel = VariantUtils.ConvertTo<MegaRichTextLabel>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._flower))
    {
      this._flower = VariantUtils.ConvertTo<NSendFeedbackFlower>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._selectedEmoteButton))
    {
      this._selectedEmoteButton = VariantUtils.ConvertTo<NSendFeedbackEmojiButton>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._screenshotBytes))
    {
      this._screenshotBytes = VariantUtils.ConvertTo<byte[]>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._originalSuccessPosition))
    {
      this._originalSuccessPosition = VariantUtils.ConvertTo<Vector2>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._lastClosedMsec))
    {
      this._lastClosedMsec = VariantUtils.ConvertTo<ulong>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionText))
    {
      this._descriptionText = VariantUtils.ConvertTo<string>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionCaretLine))
    {
      this._descriptionCaretLine = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionCaretColumn))
    {
      this._descriptionCaretColumn = VariantUtils.ConvertTo<int>(ref value);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._wiggleTween))
      return ((GodotObject) this).SetGodotClassPropertyValue(ref name, ref value);
    this._wiggleTween = VariantUtils.ConvertTo<Tween>(ref value);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override bool GetGodotClassPropertyValue(
    in godot_string_name name,
    out godot_variant value)
  {
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName.DefaultFocusedControl))
    {
      ref godot_variant local = ref value;
      Control defaultFocusedControl = this.DefaultFocusedControl;
      godot_variant from = VariantUtils.CreateFrom<Control>(ref defaultFocusedControl);
      local = from;
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._backButton))
    {
      value = VariantUtils.CreateFrom<NBackButton>(ref this._backButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._mainPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._mainPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionInput))
    {
      value = VariantUtils.CreateFrom<NMegaTextEdit>(ref this._descriptionInput);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._emojiLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._emojiLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._sendButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._sendLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._categoryLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._categoryLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameButton))
    {
      value = VariantUtils.CreateFrom<NButton>(ref this._returnToGameButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._returnToGameLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._returnToGameHoverLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._returnToGameHoverLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._categoryDropdown))
    {
      value = VariantUtils.CreateFrom<NFeedbackCategoryDropdown>(ref this._categoryDropdown);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendBackstop))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._sendBackstop);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendPanel))
    {
      value = VariantUtils.CreateFrom<Control>(ref this._sendPanel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._successLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._successLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._failedLabel))
    {
      value = VariantUtils.CreateFrom<MegaLabel>(ref this._failedLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._sendingLabel))
    {
      value = VariantUtils.CreateFrom<MegaRichTextLabel>(ref this._sendingLabel);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._flower))
    {
      value = VariantUtils.CreateFrom<NSendFeedbackFlower>(ref this._flower);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._selectedEmoteButton))
    {
      value = VariantUtils.CreateFrom<NSendFeedbackEmojiButton>(ref this._selectedEmoteButton);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._screenshotBytes))
    {
      value = VariantUtils.CreateFrom<byte[]>(ref this._screenshotBytes);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._originalSuccessPosition))
    {
      value = VariantUtils.CreateFrom<Vector2>(ref this._originalSuccessPosition);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._lastClosedMsec))
    {
      value = VariantUtils.CreateFrom<ulong>(ref this._lastClosedMsec);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionText))
    {
      value = VariantUtils.CreateFrom<string>(ref this._descriptionText);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionCaretLine))
    {
      value = VariantUtils.CreateFrom<int>(ref this._descriptionCaretLine);
      return true;
    }
    if (StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._descriptionCaretColumn))
    {
      value = VariantUtils.CreateFrom<int>(ref this._descriptionCaretColumn);
      return true;
    }
    if (!StringName.op_Equality(ref name, NSendFeedbackScreen.PropertyName._wiggleTween))
      return ((GodotObject) this).GetGodotClassPropertyValue(ref name, ref value);
    value = VariantUtils.CreateFrom<Tween>(ref this._wiggleTween);
    return true;
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  internal static List<PropertyInfo> GetGodotPropertyList()
  {
    return new List<PropertyInfo>()
    {
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._backButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._mainPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._descriptionInput, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._emojiLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._sendButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._sendLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._categoryLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._returnToGameButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._returnToGameLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._categoryDropdown, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._sendBackstop, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._sendPanel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._successLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._failedLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._sendingLabel, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._flower, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._selectedEmoteButton, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 29L, NSendFeedbackScreen.PropertyName._screenshotBytes, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 5L, NSendFeedbackScreen.PropertyName._originalSuccessPosition, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSendFeedbackScreen.PropertyName._lastClosedMsec, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 4L, NSendFeedbackScreen.PropertyName._descriptionText, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSendFeedbackScreen.PropertyName._descriptionCaretLine, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 2L, NSendFeedbackScreen.PropertyName._descriptionCaretColumn, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName._wiggleTween, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false),
      new PropertyInfo((Variant.Type) 24L, NSendFeedbackScreen.PropertyName.DefaultFocusedControl, (PropertyHint) 0L, "", (PropertyUsageFlags) 4096L /*0x1000*/, false)
    };
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void SaveGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).SaveGodotObjectData(info);
    info.AddProperty(NSendFeedbackScreen.PropertyName._backButton, Variant.From<NBackButton>(ref this._backButton));
    info.AddProperty(NSendFeedbackScreen.PropertyName._mainPanel, Variant.From<Control>(ref this._mainPanel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionInput, Variant.From<NMegaTextEdit>(ref this._descriptionInput));
    info.AddProperty(NSendFeedbackScreen.PropertyName._emojiLabel, Variant.From<MegaLabel>(ref this._emojiLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._sendButton, Variant.From<NButton>(ref this._sendButton));
    info.AddProperty(NSendFeedbackScreen.PropertyName._sendLabel, Variant.From<MegaLabel>(ref this._sendLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._categoryLabel, Variant.From<MegaLabel>(ref this._categoryLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameButton, Variant.From<NButton>(ref this._returnToGameButton));
    info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameLabel, Variant.From<MegaLabel>(ref this._returnToGameLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, Variant.From<MegaLabel>(ref this._returnToGameHoverLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._categoryDropdown, Variant.From<NFeedbackCategoryDropdown>(ref this._categoryDropdown));
    info.AddProperty(NSendFeedbackScreen.PropertyName._sendBackstop, Variant.From<Control>(ref this._sendBackstop));
    info.AddProperty(NSendFeedbackScreen.PropertyName._sendPanel, Variant.From<Control>(ref this._sendPanel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._successLabel, Variant.From<MegaLabel>(ref this._successLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._failedLabel, Variant.From<MegaLabel>(ref this._failedLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._sendingLabel, Variant.From<MegaRichTextLabel>(ref this._sendingLabel));
    info.AddProperty(NSendFeedbackScreen.PropertyName._flower, Variant.From<NSendFeedbackFlower>(ref this._flower));
    info.AddProperty(NSendFeedbackScreen.PropertyName._selectedEmoteButton, Variant.From<NSendFeedbackEmojiButton>(ref this._selectedEmoteButton));
    info.AddProperty(NSendFeedbackScreen.PropertyName._screenshotBytes, Variant.From<byte[]>(ref this._screenshotBytes));
    info.AddProperty(NSendFeedbackScreen.PropertyName._originalSuccessPosition, Variant.From<Vector2>(ref this._originalSuccessPosition));
    info.AddProperty(NSendFeedbackScreen.PropertyName._lastClosedMsec, Variant.From<ulong>(ref this._lastClosedMsec));
    info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionText, Variant.From<string>(ref this._descriptionText));
    info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionCaretLine, Variant.From<int>(ref this._descriptionCaretLine));
    info.AddProperty(NSendFeedbackScreen.PropertyName._descriptionCaretColumn, Variant.From<int>(ref this._descriptionCaretColumn));
    info.AddProperty(NSendFeedbackScreen.PropertyName._wiggleTween, Variant.From<Tween>(ref this._wiggleTween));
  }

  [EditorBrowsable(EditorBrowsableState.Never)]
  protected override void RestoreGodotObjectData(GodotSerializationInfo info)
  {
    ((GodotObject) this).RestoreGodotObjectData(info);
    Variant variant1;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._backButton, ref variant1))
      this._backButton = ((Variant) ref variant1).As<NBackButton>();
    Variant variant2;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._mainPanel, ref variant2))
      this._mainPanel = ((Variant) ref variant2).As<Control>();
    Variant variant3;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionInput, ref variant3))
      this._descriptionInput = ((Variant) ref variant3).As<NMegaTextEdit>();
    Variant variant4;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._emojiLabel, ref variant4))
      this._emojiLabel = ((Variant) ref variant4).As<MegaLabel>();
    Variant variant5;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendButton, ref variant5))
      this._sendButton = ((Variant) ref variant5).As<NButton>();
    Variant variant6;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendLabel, ref variant6))
      this._sendLabel = ((Variant) ref variant6).As<MegaLabel>();
    Variant variant7;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._categoryLabel, ref variant7))
      this._categoryLabel = ((Variant) ref variant7).As<MegaLabel>();
    Variant variant8;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameButton, ref variant8))
      this._returnToGameButton = ((Variant) ref variant8).As<NButton>();
    Variant variant9;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameLabel, ref variant9))
      this._returnToGameLabel = ((Variant) ref variant9).As<MegaLabel>();
    Variant variant10;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._returnToGameHoverLabel, ref variant10))
      this._returnToGameHoverLabel = ((Variant) ref variant10).As<MegaLabel>();
    Variant variant11;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._categoryDropdown, ref variant11))
      this._categoryDropdown = ((Variant) ref variant11).As<NFeedbackCategoryDropdown>();
    Variant variant12;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendBackstop, ref variant12))
      this._sendBackstop = ((Variant) ref variant12).As<Control>();
    Variant variant13;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendPanel, ref variant13))
      this._sendPanel = ((Variant) ref variant13).As<Control>();
    Variant variant14;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._successLabel, ref variant14))
      this._successLabel = ((Variant) ref variant14).As<MegaLabel>();
    Variant variant15;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._failedLabel, ref variant15))
      this._failedLabel = ((Variant) ref variant15).As<MegaLabel>();
    Variant variant16;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._sendingLabel, ref variant16))
      this._sendingLabel = ((Variant) ref variant16).As<MegaRichTextLabel>();
    Variant variant17;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._flower, ref variant17))
      this._flower = ((Variant) ref variant17).As<NSendFeedbackFlower>();
    Variant variant18;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._selectedEmoteButton, ref variant18))
      this._selectedEmoteButton = ((Variant) ref variant18).As<NSendFeedbackEmojiButton>();
    Variant variant19;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._screenshotBytes, ref variant19))
      this._screenshotBytes = ((Variant) ref variant19).As<byte[]>();
    Variant variant20;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._originalSuccessPosition, ref variant20))
      this._originalSuccessPosition = ((Variant) ref variant20).As<Vector2>();
    Variant variant21;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._lastClosedMsec, ref variant21))
      this._lastClosedMsec = ((Variant) ref variant21).As<ulong>();
    Variant variant22;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionText, ref variant22))
      this._descriptionText = ((Variant) ref variant22).As<string>();
    Variant variant23;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionCaretLine, ref variant23))
      this._descriptionCaretLine = ((Variant) ref variant23).As<int>();
    Variant variant24;
    if (info.TryGetProperty(NSendFeedbackScreen.PropertyName._descriptionCaretColumn, ref variant24))
      this._descriptionCaretColumn = ((Variant) ref variant24).As<int>();
    Variant variant25;
    if (!info.TryGetProperty(NSendFeedbackScreen.PropertyName._wiggleTween, ref variant25))
      return;
    this._wiggleTween = ((Variant) ref variant25).As<Tween>();
  }

  public class MethodName : Control.MethodName
  {
    public static readonly StringName Create = StringName.op_Implicit(nameof (Create));
    public static readonly StringName _Ready = StringName.op_Implicit(nameof (_Ready));
    public static readonly StringName Relocalize = StringName.op_Implicit(nameof (Relocalize));
    public static readonly StringName OnDescriptionChanged = StringName.op_Implicit(nameof (OnDescriptionChanged));
    public static readonly StringName SetScreenshot = StringName.op_Implicit(nameof (SetScreenshot));
    public static readonly StringName EmojiButtonSelected = StringName.op_Implicit(nameof (EmojiButtonSelected));
    public static readonly StringName SendButtonFocused = StringName.op_Implicit(nameof (SendButtonFocused));
    public static readonly StringName SendButtonUnfocused = StringName.op_Implicit(nameof (SendButtonUnfocused));
    public static readonly StringName Open = StringName.op_Implicit(nameof (Open));
    public static readonly StringName Close = StringName.op_Implicit(nameof (Close));
    public static readonly StringName ClearInput = StringName.op_Implicit(nameof (ClearInput));
    public static readonly StringName SetSelectedEmoji = StringName.op_Implicit(nameof (SetSelectedEmoji));
    public static readonly StringName SendButtonSelected = StringName.op_Implicit(nameof (SendButtonSelected));
    public static readonly StringName ReturnToGameSelected = StringName.op_Implicit(nameof (ReturnToGameSelected));
    public static readonly StringName ReturnToGameFocused = StringName.op_Implicit(nameof (ReturnToGameFocused));
    public static readonly StringName ReturnToGameUnfocused = StringName.op_Implicit(nameof (ReturnToGameUnfocused));
    public static readonly StringName WiggleCartoons1 = StringName.op_Implicit(nameof (WiggleCartoons1));
    public static readonly StringName WiggleCartoons2 = StringName.op_Implicit(nameof (WiggleCartoons2));
  }

  public class PropertyName : Control.PropertyName
  {
    public static readonly StringName DefaultFocusedControl = StringName.op_Implicit(nameof (DefaultFocusedControl));
    public static readonly StringName _backButton = StringName.op_Implicit(nameof (_backButton));
    public static readonly StringName _mainPanel = StringName.op_Implicit(nameof (_mainPanel));
    public static readonly StringName _descriptionInput = StringName.op_Implicit(nameof (_descriptionInput));
    public static readonly StringName _emojiLabel = StringName.op_Implicit(nameof (_emojiLabel));
    public static readonly StringName _sendButton = StringName.op_Implicit(nameof (_sendButton));
    public static readonly StringName _sendLabel = StringName.op_Implicit(nameof (_sendLabel));
    public static readonly StringName _categoryLabel = StringName.op_Implicit(nameof (_categoryLabel));
    public static readonly StringName _returnToGameButton = StringName.op_Implicit(nameof (_returnToGameButton));
    public static readonly StringName _returnToGameLabel = StringName.op_Implicit(nameof (_returnToGameLabel));
    public static readonly StringName _returnToGameHoverLabel = StringName.op_Implicit(nameof (_returnToGameHoverLabel));
    public static readonly StringName _categoryDropdown = StringName.op_Implicit(nameof (_categoryDropdown));
    public static readonly StringName _sendBackstop = StringName.op_Implicit(nameof (_sendBackstop));
    public static readonly StringName _sendPanel = StringName.op_Implicit(nameof (_sendPanel));
    public static readonly StringName _successLabel = StringName.op_Implicit(nameof (_successLabel));
    public static readonly StringName _failedLabel = StringName.op_Implicit(nameof (_failedLabel));
    public static readonly StringName _sendingLabel = StringName.op_Implicit(nameof (_sendingLabel));
    public static readonly StringName _flower = StringName.op_Implicit(nameof (_flower));
    public static readonly StringName _selectedEmoteButton = StringName.op_Implicit(nameof (_selectedEmoteButton));
    public static readonly StringName _screenshotBytes = StringName.op_Implicit(nameof (_screenshotBytes));
    public static readonly StringName _originalSuccessPosition = StringName.op_Implicit(nameof (_originalSuccessPosition));
    public static readonly StringName _lastClosedMsec = StringName.op_Implicit(nameof (_lastClosedMsec));
    public static readonly StringName _descriptionText = StringName.op_Implicit(nameof (_descriptionText));
    public static readonly StringName _descriptionCaretLine = StringName.op_Implicit(nameof (_descriptionCaretLine));
    public static readonly StringName _descriptionCaretColumn = StringName.op_Implicit(nameof (_descriptionCaretColumn));
    public static readonly StringName _wiggleTween = StringName.op_Implicit(nameof (_wiggleTween));
  }

  public class SignalName : Control.SignalName
  {
  }
}
