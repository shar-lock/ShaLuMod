// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.ControllerInput.MegaInput
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;

#nullable enable
namespace MegaCrit.Sts2.Core.ControllerInput;

public static class MegaInput
{
  public static readonly StringName up = StringName.op_Implicit("ui_up");
  public static readonly StringName down = StringName.op_Implicit("ui_down");
  public static readonly StringName left = StringName.op_Implicit("ui_left");
  public static readonly StringName right = StringName.op_Implicit("ui_right");
  public static readonly StringName altUp = StringName.op_Implicit("ui_alt_up");
  public static readonly StringName altDown = StringName.op_Implicit("ui_alt_down");
  public static readonly StringName altLeft = StringName.op_Implicit("ui_alt_left");
  public static readonly StringName altRight = StringName.op_Implicit("ui_alt_right");
  public static readonly StringName accept = StringName.op_Implicit("ui_accept");
  public static readonly StringName select = StringName.op_Implicit("ui_select");
  public static readonly StringName cancel = StringName.op_Implicit("ui_cancel");
  public static readonly StringName selectCard1 = StringName.op_Implicit("mega_select_card_1");
  public static readonly StringName selectCard2 = StringName.op_Implicit("mega_select_card_2");
  public static readonly StringName selectCard3 = StringName.op_Implicit("mega_select_card_3");
  public static readonly StringName selectCard4 = StringName.op_Implicit("mega_select_card_4");
  public static readonly StringName selectCard5 = StringName.op_Implicit("mega_select_card_5");
  public static readonly StringName selectCard6 = StringName.op_Implicit("mega_select_card_6");
  public static readonly StringName selectCard7 = StringName.op_Implicit("mega_select_card_7");
  public static readonly StringName selectCard8 = StringName.op_Implicit("mega_select_card_8");
  public static readonly StringName selectCard9 = StringName.op_Implicit("mega_select_card_9");
  public static readonly StringName selectCard10 = StringName.op_Implicit("mega_select_card_10");
  public static readonly StringName releaseCard = StringName.op_Implicit("mega_release_card");
  public static readonly StringName topPanel = StringName.op_Implicit("mega_top_panel");
  public static readonly StringName viewDrawPile = StringName.op_Implicit("mega_view_draw_pile");
  public static readonly StringName viewDiscardPile = StringName.op_Implicit("mega_view_discard_pile");
  public static readonly StringName viewDeckAndTabLeft = StringName.op_Implicit("mega_view_deck_and_tab_left");
  public static readonly StringName viewExhaustPileAndTabRight = StringName.op_Implicit("mega_view_exhaust_pile_and_tab_right");
  public static readonly StringName viewMap = StringName.op_Implicit("mega_view_map");
  public static readonly StringName pauseAndBack = StringName.op_Implicit("mega_pause_and_back");
  public static readonly StringName back = StringName.op_Implicit("mega_back");
  public static readonly StringName peek = StringName.op_Implicit("mega_peek");

  public static string[] AllInputs
  {
    get
    {
      return new string[19]
      {
        StringName.op_Implicit(MegaInput.up),
        StringName.op_Implicit(MegaInput.down),
        StringName.op_Implicit(MegaInput.left),
        StringName.op_Implicit(MegaInput.right),
        StringName.op_Implicit(MegaInput.altUp),
        StringName.op_Implicit(MegaInput.altDown),
        StringName.op_Implicit(MegaInput.altLeft),
        StringName.op_Implicit(MegaInput.altRight),
        StringName.op_Implicit(MegaInput.select),
        StringName.op_Implicit(MegaInput.cancel),
        StringName.op_Implicit(MegaInput.accept),
        StringName.op_Implicit(MegaInput.topPanel),
        StringName.op_Implicit(MegaInput.viewDeckAndTabLeft),
        StringName.op_Implicit(MegaInput.viewExhaustPileAndTabRight),
        StringName.op_Implicit(MegaInput.viewDiscardPile),
        StringName.op_Implicit(MegaInput.viewDrawPile),
        StringName.op_Implicit(MegaInput.pauseAndBack),
        StringName.op_Implicit(MegaInput.viewMap),
        StringName.op_Implicit(MegaInput.peek)
      };
    }
  }
}
