// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.addons.mega_text.MegaLabelHelper
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Assets;
using MegaCrit.Sts2.Core.Entities.Text;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.addons.mega_text;

public static class MegaLabelHelper
{
  private const float _defaultLineSpacing = -3f;
  private static readonly List<BbcodeObject> _cachedBbcodeObjects = new List<BbcodeObject>();

  public static void AssertThemeFontOverride(Control control, StringName fontOverrideName)
  {
    if (!control.HasThemeFontOverride(fontOverrideName))
      throw new InvalidOperationException($"{control.GetType().Name} '{((Node) control).GetPath()}' has no theme font override. Please set one to avoid a Godot engine bug.");
  }

  public static List<BbcodeObject> ParseBbcode(string bbcode)
  {
    MegaLabelHelper._cachedBbcodeObjects.Clear();
    MegaLabelHelper.BbcodeParsingState bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.NotInTag;
    Stack<string> stringStack = new Stack<string>();
    StringBuilder stringBuilder = new StringBuilder();
    for (int index = 0; index < bbcode.Length; ++index)
    {
      if (bbcode[index] == '[' && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.NotInTag)
      {
        if (stringBuilder.Length > 0)
        {
          MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
          {
            text = stringBuilder.ToString(),
            type = BbcodeObjectType.Text
          });
          stringBuilder.Clear();
        }
        bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InTag;
      }
      else if ((bbcode[index] == ' ' || bbcode[index] == '=') && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag)
      {
        string str = stringBuilder.ToString();
        MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
        {
          tag = str,
          type = BbcodeObjectType.BeginTag
        });
        stringStack.Push(str);
        bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InTagEnvironment;
      }
      else if (bbcode[index] == '/' && bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag)
        bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.InEndTag;
      else if (bbcode[index] == ']' && (bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag || bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InEndTag || bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTagEnvironment))
      {
        if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.InTagEnvironment)
        {
          string str = stringBuilder.ToString();
          switch (str)
          {
            case "lb":
              MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
              {
                text = "[",
                type = BbcodeObjectType.Text
              });
              break;
            case "rb":
              MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
              {
                text = "]",
                type = BbcodeObjectType.Text
              });
              break;
            default:
              if (bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InEndTag)
              {
                if (stringStack.Count == 0)
                  throw new InvalidOperationException($"Found end tag {str} with no tag on the stack. ({bbcode})");
                if (stringStack.Peek() != str)
                  throw new InvalidOperationException($"Found end tag {str}, expected {stringStack.Peek()}. ({bbcode})");
                stringStack.Pop();
              }
              else
                stringStack.Push(str);
              MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
              {
                tag = str,
                type = bbcodeParsingState == MegaLabelHelper.BbcodeParsingState.InTag ? BbcodeObjectType.BeginTag : BbcodeObjectType.EndTag
              });
              break;
          }
        }
        bbcodeParsingState = MegaLabelHelper.BbcodeParsingState.NotInTag;
        stringBuilder.Clear();
      }
      else if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.InTagEnvironment)
        stringBuilder.Append(bbcode[index]);
    }
    if (bbcodeParsingState != MegaLabelHelper.BbcodeParsingState.NotInTag)
      throw new InvalidOperationException("In tag at end of string");
    if (stringBuilder.Length > 0)
      MegaLabelHelper._cachedBbcodeObjects.Add(new BbcodeObject()
      {
        text = stringBuilder.ToString(),
        type = BbcodeObjectType.Text
      });
    return MegaLabelHelper._cachedBbcodeObjects;
  }

  public static Vector2 EstimateTextSize(
    TextParagraph paragraph,
    List<BbcodeObject> objs,
    Font font,
    int fontSize,
    float maxWidth,
    float lineSpacing)
  {
    paragraph.Clear();
    paragraph.Direction = (TextServer.Direction) 0L;
    paragraph.Orientation = (TextServer.Orientation) 0L;
    Stack<string> stringStack = new Stack<string>();
    int num = 0;
    foreach (BbcodeObject bbcodeObject in objs)
    {
      if (bbcodeObject.type == BbcodeObjectType.BeginTag)
        stringStack.Push(bbcodeObject.tag);
      else if (bbcodeObject.type == BbcodeObjectType.EndTag)
        stringStack.Pop();
      else if (bbcodeObject.type == BbcodeObjectType.Text)
      {
        string str;
        if (stringStack.TryPeek(ref str) && str == "img")
        {
          Texture2D texture2D = PreloadManager.Cache.GetTexture2D(bbcodeObject.text);
          paragraph.AddObject(Variant.op_Implicit(num), texture2D.GetSize(), (InlineAlignment) 5L, 1, 0.0f);
          ++num;
        }
        else
          paragraph.AddString(bbcodeObject.text, font, fontSize, "", new Variant());
      }
    }
    paragraph.Width = maxWidth;
    paragraph.BreakFlags = (TextServer.LineBreakFlag) 3L;
    paragraph.JustificationFlags = (TextServer.JustificationFlag) 3L;
    paragraph.TextOverrunBehavior = (TextServer.OverrunBehavior) 1L;
    paragraph.Alignment = (HorizontalAlignment) 1L;
    paragraph.MaxLinesVisible = -1;
    int lineCount = paragraph.GetLineCount();
    return Vector2.op_Addition(paragraph.GetSize(), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, lineSpacing - -3f), (float) (lineCount - 1)));
  }

  public static bool IsTooBig(
    TextParagraph paragraph,
    List<BbcodeObject> objs,
    Font font,
    int fontSize,
    float lineSpacing,
    Vector2 rectSize,
    bool horizontallyBound,
    bool verticallyBound)
  {
    Vector2 vector2 = MegaLabelHelper.EstimateTextSize(paragraph, objs, font, fontSize, rectSize.X, lineSpacing);
    float x = rectSize.X;
    float y = rectSize.Y;
    bool flag1 = (double) vector2.X > (double) x;
    bool flag2 = (double) vector2.Y > (double) y;
    return flag1 & horizontallyBound || flag2 & verticallyBound;
  }

  public static Vector2 EstimateTextSize(
    TextParagraph paragraph,
    string text,
    Font font,
    int fontSize,
    float maxWidth,
    float lineSpacing,
    bool wrap = true)
  {
    paragraph.Clear();
    paragraph.Direction = (TextServer.Direction) 0L;
    paragraph.Orientation = (TextServer.Orientation) 0L;
    paragraph.AddString(text, font, fontSize, "", new Variant());
    paragraph.Width = maxWidth;
    paragraph.BreakFlags = wrap ? (TextServer.LineBreakFlag) 3L : (TextServer.LineBreakFlag) 0L;
    paragraph.JustificationFlags = (TextServer.JustificationFlag) 3L;
    paragraph.TextOverrunBehavior = (TextServer.OverrunBehavior) 0L;
    paragraph.Alignment = (HorizontalAlignment) 1L;
    paragraph.MaxLinesVisible = -1;
    int lineCount = paragraph.GetLineCount();
    return Vector2.op_Addition(paragraph.GetSize(), Vector2.op_Multiply(Vector2.op_Multiply(Vector2.Down, lineSpacing - -3f), (float) (lineCount - 1)));
  }

  public static bool IsTooBig(
    TextParagraph paragraph,
    string text,
    Font font,
    int fontSize,
    float lineSpacing,
    bool wrap,
    Vector2 rectSize)
  {
    Vector2 vector2 = MegaLabelHelper.EstimateTextSize(paragraph, text, font, fontSize, rectSize.X, lineSpacing, wrap);
    float x = rectSize.X;
    float y = rectSize.Y;
    return (double) vector2.X > (double) x | (double) vector2.Y > (double) y;
  }

  private enum BbcodeParsingState
  {
    NotInTag,
    InTag,
    InEndTag,
    InTagEnvironment,
  }
}
