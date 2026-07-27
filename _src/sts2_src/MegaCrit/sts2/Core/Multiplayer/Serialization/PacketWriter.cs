// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketWriter
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using MegaCrit.Sts2.Core.Logging;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public class PacketWriter
{
  private readonly byte[] _tempBuffer = new byte[16 /*0x10*/];
  private byte[] _stringBuffer = new byte[64 /*0x40*/];

  public int BitPosition { get; private set; }

  public int BytePosition => Mathf.CeilToInt((float) this.BitPosition / 8f);

  public byte[] Buffer { get; private set; } = new byte[1024 /*0x0400*/];

  public bool WarnOnGrow { get; set; } = true;

  public void Reset() => this.BitPosition = 0;

  public void WriteBool(bool val)
  {
    this._tempBuffer[0] = (byte) val;
    this.ResizeBufferIfNecessary(1);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 1);
    ++this.BitPosition;
  }

  public void WriteByte(byte val, int bits = 8)
  {
    this._tempBuffer[0] = val;
    this.ResizeBufferIfNecessary(bits);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteBytes(byte[] bytes, int byteCount)
  {
    this.ResizeBufferIfNecessary(byteCount * 8);
    BitSerializationUtil.WriteBytes(bytes, this.Buffer, this.BitPosition, byteCount * 8);
    this.BitPosition += byteCount * 8;
  }

  public void WriteShort(short val, int bits = 16 /*0x10*/)
  {
    this.ResizeBufferIfNecessary(bits);
    BinaryPrimitives.WriteInt16LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteUShort(ushort val, int bits = 16 /*0x10*/)
  {
    this.ResizeBufferIfNecessary(bits);
    BinaryPrimitives.WriteUInt16LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteEnum<T>(T val) where T : struct, Enum
  {
    if (!typeof (int).IsAssignableFrom(Enum.GetUnderlyingType(typeof (T))))
      throw new InvalidOperationException($"Trying to write enum type {typeof (T)} that is not assignable to int!");
    int bits = Mathf.CeilToInt(Math.Log2((double) MaxEnumValueCache.Get<T>()) + 1.0);
    this.WriteInt(Convert.ToInt32((object) val), bits);
  }

  public void WriteInt(int val, int bits = 32 /*0x20*/)
  {
    this.ResizeBufferIfNecessary(bits);
    BinaryPrimitives.WriteInt32LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteUInt(uint val, int bits = 32 /*0x20*/)
  {
    this.ResizeBufferIfNecessary(bits);
    BinaryPrimitives.WriteUInt32LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteFloat(float val, QuantizeParams? quantizeParams = null)
  {
    if (quantizeParams.HasValue)
    {
      this.ResizeBufferIfNecessary(quantizeParams.Value.bits);
      this.WriteUInt(PacketWriter.Quantize(val, quantizeParams.Value.min, quantizeParams.Value.max, quantizeParams.Value.bits), quantizeParams.Value.bits);
    }
    else
    {
      this.ResizeBufferIfNecessary(32 /*0x20*/);
      BinaryPrimitives.WriteSingleLittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
      BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 32 /*0x20*/);
      this.BitPosition += 32 /*0x20*/;
    }
  }

  public void WriteLong(long val, int bits = 64 /*0x40*/)
  {
    this.ResizeBufferIfNecessary(64 /*0x40*/);
    BinaryPrimitives.WriteInt64LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteULong(ulong val, int bits = 64 /*0x40*/)
  {
    this.ResizeBufferIfNecessary(64 /*0x40*/);
    BinaryPrimitives.WriteUInt64LittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, bits);
    this.BitPosition += bits;
  }

  public void WriteDouble(double val)
  {
    this.ResizeBufferIfNecessary(64 /*0x40*/);
    BinaryPrimitives.WriteDoubleLittleEndian(MemoryExtensions.AsSpan<byte>(this._tempBuffer), val);
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, 64 /*0x40*/);
    this.BitPosition += 64 /*0x40*/;
  }

  public void WriteVector2(
    Vector2 val,
    QuantizeParams? quantizeParamsX = null,
    QuantizeParams? quantizeParamsY = null)
  {
    this.WriteFloat(val.X, quantizeParamsX);
    this.WriteFloat(val.Y, quantizeParamsY);
  }

  public void WriteList<T>(IReadOnlyList<T> list, int lengthBits = 32 /*0x20*/) where T : IPacketSerializable, new()
  {
    if ((ulong) list.Count >= 1UL << lengthBits)
      throw new IndexOutOfRangeException($"List length {list.Count} is too large to fit in bit size {lengthBits}!");
    this.WriteInt(list.Count, lengthBits);
    foreach (T val in (IEnumerable<T>) list)
      this.Write<T>(val);
  }

  public void WriteString(string str)
  {
    int byteCount = Encoding.UTF8.GetByteCount(str);
    if (this._stringBuffer.Length < byteCount)
      this._stringBuffer = new byte[(int) Math.Pow(2.0, Math.Ceiling(Math.Log2((double) byteCount)))];
    int bytes = Encoding.UTF8.GetBytes(string.op_Implicit(str), Span<byte>.op_Implicit(this._stringBuffer));
    this.WriteInt(bytes);
    this.WriteBytes(this._stringBuffer, bytes);
  }

  public void Write<T>(T val) where T : IPacketSerializable => val.Serialize(this);

  public void ZeroByteRemainder()
  {
    int totalBitsToWrite = 8 - this.BitPosition % 8;
    if (totalBitsToWrite == 8)
      return;
    this._tempBuffer[0] = (byte) 0;
    BitSerializationUtil.WriteBytes(this._tempBuffer, this.Buffer, this.BitPosition, totalBitsToWrite);
  }

  private void ResizeBufferIfNecessary(int bitsBeingWritten)
  {
    int num = Mathf.CeilToInt((float) (this.BitPosition + bitsBeingWritten) / 8f);
    int length = this.Buffer.Length;
    while (num >= length)
      length *= 2;
    if (length == this.Buffer.Length)
      return;
    byte[] destinationArray = new byte[length];
    Array.Copy((Array) this.Buffer, (Array) destinationArray, this.Buffer.Length);
    if (this.WarnOnGrow)
      Log.Warn($"Warning: Packet writer is growing from {this.Buffer.Length} bytes to {destinationArray.Length} bytes!");
    this.Buffer = destinationArray;
  }

  public static uint Quantize(float value, float min, float max, int bitLength)
  {
    return (uint) (((double) value - (double) min) / ((double) max - (double) min) * Math.Pow(2.0, (double) bitLength));
  }
}
