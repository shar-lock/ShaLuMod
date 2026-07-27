// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Debug.FileDropHandler
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using MegaCrit.Sts2.Core.Multiplayer;
using MegaCrit.Sts2.Core.Multiplayer.Game;
using MegaCrit.Sts2.Core.Nodes;
using MegaCrit.Sts2.Core.Rooms;
using MegaCrit.Sts2.Core.Runs;
using MegaCrit.Sts2.Core.Saves;
using MegaCrit.Sts2.Core.Saves.Runs;
using System;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Debug;

public static class FileDropHandler
{
  public static void OnFilesDropped(string[] files)
  {
    if (!OS.HasFeature("editor") && !ModManager.IsRunningModded() && !SaveManager.Instance.SettingsSave.FullConsole || files.Length > 1)
      return;
    string file = files[0];
    if (!file.EndsWith(".run"))
      Log.Error($"We only support dropping .run files. You dropped {file}.");
    else
      TaskHelper.RunSafely(FileDropHandler.OnRunHistoryDropped(file));
  }

  private static async Task OnRunHistoryDropped(string file)
  {
    FileAccess fileAccess;
    SerializableRun savedRun;
    RunState loadedState;
    if (!RunManager.Instance.IsInProgress)
    {
      Log.Error("Can only load run history while run is in progress.");
      fileAccess = (FileAccess) null;
      savedRun = (SerializableRun) null;
      loadedState = (RunState) null;
    }
    else if (RunManager.Instance.DebugOnlyGetState().Players.Count > 1)
    {
      Log.Error("Only singleplayer supported for now.");
      fileAccess = (FileAccess) null;
      savedRun = (SerializableRun) null;
      loadedState = (RunState) null;
    }
    else
    {
      fileAccess = FileAccess.Open(file, (FileAccess.ModeFlags) 1L);
      try
      {
        if (fileAccess == null)
        {
          Log.Error($"Couldn't open file {file}: {FileAccess.GetOpenError()}");
          fileAccess = (FileAccess) null;
          savedRun = (SerializableRun) null;
          loadedState = (RunState) null;
        }
        else
        {
          ReadSaveResult<RunHistory> readSaveResult = JsonSerializationUtility.FromJson<RunHistory>(fileAccess.GetAsText(false));
          if (!readSaveResult.Success)
          {
            Log.Error($"Couldn't read {file}: {readSaveResult.ErrorMessage} ({readSaveResult.Status})");
            fileAccess = (FileAccess) null;
            savedRun = (SerializableRun) null;
            loadedState = (RunState) null;
          }
          else
          {
            RunHistory saveData = readSaveResult.SaveData;
            if (saveData.Players.Count > 1)
            {
              Log.Error("Only singleplayer supported for now.");
              fileAccess = (FileAccess) null;
              savedRun = (SerializableRun) null;
              loadedState = (RunState) null;
            }
            else
            {
              RunHistoryPlayer player1 = saveData.Players[0];
              Log.Info("Successfully loaded file " + file);
              savedRun = RunManager.Instance.ToSave((AbstractRoom) null);
              SerializablePlayer player2 = savedRun.Players[0];
              player2.Deck.Clear();
              player2.Deck.AddRange(player1.Deck);
              player2.Relics.Clear();
              player2.Relics.AddRange(player1.Relics);
              player2.Potions.Clear();
              player2.Potions.AddRange(player1.Potions);
              RunManager.Instance.CleanUp();
              loadedState = RunState.FromSerializable(savedRun);
              await RunManager.Instance.SetUpSavedSingleplayer(loadedState, savedRun);
              NGame.Instance.ReactionContainer.InitializeNetworking((INetGameService) new NetSingleplayerGameService());
              await NGame.Instance.LoadRun(loadedState, savedRun.PreFinishedRoom);
              fileAccess = (FileAccess) null;
              savedRun = (SerializableRun) null;
              loadedState = (RunState) null;
            }
          }
        }
      }
      finally
      {
        ((IDisposable) fileAccess)?.Dispose();
      }
    }
  }
}
