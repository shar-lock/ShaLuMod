// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Entities.CardRewardAlternatives.CardRewardAlternative
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.ControllerInput;
using MegaCrit.Sts2.Core.Entities.Rewards;
using MegaCrit.Sts2.Core.Hooks;
using MegaCrit.Sts2.Core.Localization;
using MegaCrit.Sts2.Core.Rewards;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

#nullable enable
namespace MegaCrit.Sts2.Core.Entities.CardRewardAlternatives;

public class CardRewardAlternative
{
  public string OptionId { get; }

  public LocString Title
  {
    get => new LocString("card_reward_ui", $"OPTION_{this.OptionId.ToUpperInvariant()}.name");
  }

  public string Hotkey { get; }

  public Func<Task> OnSelect { get; private set; }

  public PostAlternateCardRewardAction AfterSelected { get; private set; }

  public CardRewardAlternative(string optionId, PostAlternateCardRewardAction afterSelected)
    : this(optionId, (Func<Task>) (() => Task.CompletedTask), afterSelected)
  {
  }

  public CardRewardAlternative(
    string optionId,
    Func<Task> onSelect,
    PostAlternateCardRewardAction afterSelected)
  {
    this.OptionId = optionId;
    this.OnSelect = onSelect;
    this.AfterSelected = afterSelected;
    this.Hotkey = StringName.op_Implicit(afterSelected == PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward ? MegaInput.cancel : MegaInput.viewExhaustPileAndTabRight);
  }

  public static IReadOnlyList<CardRewardAlternative> Generate(CardReward cardReward)
  {
    List<CardRewardAlternative> alternatives = new List<CardRewardAlternative>();
    if (cardReward.CanSkip)
      alternatives.Add(new CardRewardAlternative("Skip", PostAlternateCardRewardAction.EndSelectionAndDoNotCompleteReward));
    if (cardReward.CanReroll)
      alternatives.Add(new CardRewardAlternative("REROLL", (Func<Task>) (() =>
      {
        cardReward.Reroll();
        return Task.CompletedTask;
      }), PostAlternateCardRewardAction.DoNothing));
    Hook.ModifyCardRewardAlternatives(cardReward.Player.RunState, cardReward.Player, cardReward, alternatives);
    return alternatives.Count <= 2 ? (IReadOnlyList<CardRewardAlternative>) alternatives : throw new InvalidOperationException("More than 2 card reward alternatives are not supported.");
  }
}
