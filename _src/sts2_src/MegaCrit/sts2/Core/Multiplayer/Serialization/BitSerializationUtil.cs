// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.BitSerializationUtil
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

internal static class BitSerializationUtil
{
  public static byte GetByteMask(int bits, int startBit)
  {
    if (bits > 8 || startBit + bits > 8)
      throw new InvalidOperationException();
    return (byte) ((1 << bits) - 1 << startBit);
  }

  public static byte GetBitsAtPosition(byte[] bytes, int bitPosition, int bitsToObtain)
  {
    int index = bitPosition / 8;
    int num1 = bitPosition % 8;
    if (bitsToObtain > 8)
      throw new InvalidOperationException();
    if (num1 == 0)
      return (byte) ((uint) bytes[index] & (uint) BitSerializationUtil.GetByteMask(bitsToObtain, 0));
    int num2 = 8 - num1;
    return bitsToObtain <= num2 ? (byte) ((uint) bytes[index] >> num1 & (uint) BitSerializationUtil.GetByteMask(bitsToObtain, 0)) : (byte) ((uint) (byte) ((int) (byte) ((uint) bytes[index] >> num1) & (int) BitSerializationUtil.GetByteMask(num2, 0) | (int) (byte) ((uint) bytes[index + 1] << num2) & (int) BitSerializationUtil.GetByteMask(bitsToObtain - num2, num2)) & (uint) BitSerializationUtil.GetByteMask(bitsToObtain, 0));
  }

  public static void WriteBytes(
    byte[] originBuffer,
    byte[] destinationBuffer,
    int destinationBitPosition,
    int totalBitsToWrite)
  {
    int num1 = destinationBitPosition;
    int num2;
    for (int bitPosition = 0; bitPosition < totalBitsToWrite; bitPosition += num2)
    {
      int index = (num1 + bitPosition) / 8;
      int num3 = (num1 + bitPosition) % 8;
      num2 = Mathf.Min(totalBitsToWrite - bitPosition, 8 - num3);
      byte bitsAtPosition = BitSerializationUtil.GetBitsAtPosition(originBuffer, bitPosition, num2);
      byte num4 = destinationBuffer[index];
      byte byteMask1 = BitSerializationUtil.GetByteMask(num3, 0);
      byte byteMask2 = BitSerializationUtil.GetByteMask(num2, num3);
      byte num5 = (byte) ((uint) bitsAtPosition << num3);
      byte num6 = (byte) ((int) num4 & (int) byteMask1 | (int) num5 & (int) byteMask2);
      destinationBuffer[index] = num6;
    }
  }

  public static void ReadBits(
    byte[] originBuffer,
    int originBitPosition,
    byte[] destinationBuffer,
    int totalBitsToRead)
  {
    int num1 = originBitPosition;
    int num2;
    for (int index1 = 0; index1 < totalBitsToRead; index1 += num2)
    {
      int index2 = index1 / 8;
      int num3 = index1 % 8;
      num2 = Mathf.Min(totalBitsToRead - index1, 8 - num3);
      byte bitsAtPosition = BitSerializationUtil.GetBitsAtPosition(originBuffer, num1 + index1, num2);
      byte num4 = destinationBuffer[index2];
      byte byteMask1 = BitSerializationUtil.GetByteMask(num3, 0);
      byte byteMask2 = BitSerializationUtil.GetByteMask(num2, num3);
      byte num5 = (byte) ((uint) bitsAtPosition << num3);
      byte num6 = (byte) ((int) num4 & (int) byteMask1 | (int) num5 & (int) byteMask2);
      destinationBuffer[index2] = num6;
    }
  }
}
