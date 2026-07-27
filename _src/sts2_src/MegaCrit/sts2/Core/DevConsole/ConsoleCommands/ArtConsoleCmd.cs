// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.DevConsole.ConsoleCommands.ArtConsoleCmd
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Players;
using MegaCrit.Sts2.Core.Models;
using MegaCrit.Sts2.Core.Models.Cards;
using MegaCrit.Sts2.Core.Models.Enchantments;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.DevConsole.ConsoleCommands;

public class ArtConsoleCmd : AbstractConsoleCmd
{
  private static readonly string[] _types = new string[5]
  {
    "affliction",
    "card",
    "enchantment",
    "power",
    "relic"
  };

  public override string CmdName => "art";

  public override string Args => "<type:string>";

  public override string Description
  {
    get
    {
      return "Lists all the content of the specified type that is missing art. " + ArtConsoleCmd.TypesHint;
    }
  }

  public override bool IsNetworked => false;

  private static string TypesHint => $"Types: {string.Join(", ", ArtConsoleCmd._types)}.";

  public override CmdResult Process(Player? issuingPlayer, string[] args)
  {
    if (args.Length < 1)
      return new CmdResult(false, $"{this.CmdName} requires a type. {ArtConsoleCmd.TypesHint}");
    string str1 = args[0].ToLowerInvariant();
    if (str1.EndsWith('s'))
    {
      string str2 = str1;
      str1 = str2.Substring(0, str2.Length - 1);
    }
    List<ArtConsoleCmd.MissingArt> source = new List<ArtConsoleCmd.MissingArt>();
    switch (str1)
    {
      case "affliction":
        using (IEnumerator<AfflictionModel> enumerator = ModelDb.DebugAfflictions.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            AfflictionModel current = enumerator.Current;
            if (!current.HasOverlay)
              source.Add(new ArtConsoleCmd.MissingArt((AbstractModel) current, current.OverlayPath, current.DynamicDescription.GetRawText()));
          }
          break;
        }
      case "card":
        using (IEnumerator<CardModel> enumerator = ModelDb.AllCards.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            CardModel current = enumerator.Current;
            if (!(current is DeprecatedCard))
            {
              foreach (string allPortraitPath in current.AllPortraitPaths)
              {
                (string str3, string str4) = AtlasResourceLoader.ParsePath(allPortraitPath);
                if (str3 == null || !AtlasManager.HasSprite(str3, str4))
                  source.Add(new ArtConsoleCmd.MissingArt((AbstractModel) current, current.PortraitPath, current.Description.GetRawText()));
              }
            }
          }
          break;
        }
      case "enchantment":
        using (IEnumerator<EnchantmentModel> enumerator = ModelDb.DebugEnchantments.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            EnchantmentModel current = enumerator.Current;
            if (!(current is DeprecatedEnchantment) && !(current.IconPath == current.IntendedIconPath))
              source.Add(new ArtConsoleCmd.MissingArt((AbstractModel) current, current.IntendedIconPath, current.DynamicDescription.GetRawText()));
          }
          break;
        }
      case "power":
        using (IEnumerator<PowerModel> enumerator = ModelDb.AllPowers.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            PowerModel current = enumerator.Current;
            (string str5, string str6) = AtlasResourceLoader.ParsePath(current.PackedIconPath);
            if (str5 == null || !AtlasManager.HasSprite(str5, str6))
              source.Add(new ArtConsoleCmd.MissingArt((AbstractModel) current, current.PackedIconPath, current.Description.GetRawText()));
          }
          break;
        }
      case "relic":
        using (IEnumerator<RelicModel> enumerator = ModelDb.AllRelics.GetEnumerator())
        {
          while (enumerator.MoveNext())
          {
            RelicModel current = enumerator.Current;
            (string str7, string str8) = AtlasResourceLoader.ParsePath(current.PackedIconPath);
            if (str7 == null || !AtlasManager.HasSprite(str7, str8))
              source.Add(new ArtConsoleCmd.MissingArt((AbstractModel) current, current.PackedIconPath, current.DynamicDescription.GetRawText()));
          }
          break;
        }
      default:
        return new CmdResult(false, $"'{args[0]}' is not a valid type. {ArtConsoleCmd.TypesHint}");
    }
    string str9 = source.Count == 1 ? "" : "s";
    StringBuilder stringBuilder1 = new StringBuilder($"{source.Count} {str1}{str9} with missing art:\n");
    foreach (ArtConsoleCmd.MissingArt missingArt in (IEnumerable<ArtConsoleCmd.MissingArt>) source.OrderBy<ArtConsoleCmd.MissingArt, string>((Func<ArtConsoleCmd.MissingArt, string>) (m => m.Model.Id.Entry)))
    {
      StringBuilder stringBuilder2 = stringBuilder1;
      StringBuilder stringBuilder3 = stringBuilder2;
      StringBuilder.AppendInterpolatedStringHandler interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(0, 1, stringBuilder2);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(missingArt.Model.Id.Entry);
      ref StringBuilder.AppendInterpolatedStringHandler local1 = ref interpolatedStringHandler;
      stringBuilder3.AppendLine(ref local1);
      StringBuilder stringBuilder4 = stringBuilder1;
      StringBuilder stringBuilder5 = stringBuilder4;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder4);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("* Intended Path: ");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(missingArt.IntendedPath);
      ref StringBuilder.AppendInterpolatedStringHandler local2 = ref interpolatedStringHandler;
      stringBuilder5.AppendLine(ref local2);
      StringBuilder stringBuilder6 = stringBuilder1;
      StringBuilder stringBuilder7 = stringBuilder6;
      interpolatedStringHandler = new StringBuilder.AppendInterpolatedStringHandler(17, 1, stringBuilder6);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("* Description: \"");
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendFormatted(missingArt.Description);
      ((StringBuilder.AppendInterpolatedStringHandler) ref interpolatedStringHandler).AppendLiteral("\"");
      ref StringBuilder.AppendInterpolatedStringHandler local3 = ref interpolatedStringHandler;
      stringBuilder7.AppendLine(ref local3);
    }
    return new CmdResult(true, stringBuilder1.ToString());
  }

  public override CompletionResult GetArgumentCompletions(Player? player, string[] args)
  {
    if (args.Length <= 1)
      return this.CompleteArgument((IEnumerable<string>) ((IEnumerable<string>) ArtConsoleCmd._types).ToList<string>(), Array.Empty<string>(), ((IEnumerable<string>) args).FirstOrDefault<string>() ?? "");
    return new CompletionResult()
    {
      Type = CompletionType.Argument,
      ArgumentContext = this.CmdName
    };
  }

  private struct MissingArt(AbstractModel model, string intendedPath, string description)
  {
    public AbstractModel Model { get; } = model;

    public string IntendedPath { get; } = intendedPath;

    public string Description { get; } = description;
  }
}
