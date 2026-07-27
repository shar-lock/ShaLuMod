// Decompiled with JetBrains decompiler
// Type: MegaCrit.Sts2.Core.Multiplayer.Serialization.PacketReader
// Assembly: sts2, Version=0.1.0.0, Culture=neutral, PublicKeyToken=null
// MVID: DB296A8D-D1DB-4F9A-B229-9E9A5BB4D47E
// Assembly location: D:\shaluMod\ShaLuMod\_src\sts2.dll

using Godot;
using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.Text;

#nullable enable
namespace MegaCrit.Sts2.Core.Multiplayer.Serialization;

public class PacketReader
{
  private readonly byte[] _tempBuffer = new byte[16 /*0x10*/];
  private byte[] _stringBuffer = new byte[64 /*0x40*/];

  public int BitPosition { get; private set; }

  public byte[] Buffer { get; private set; }

  public void Reset(byte[] buffer)
  {
    this.BitPosition = 0;
    this.Buffer = buffer;
  }

  public bool ReadBool()
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 1);
    ++this.BitPosition;
    return this._tempBuffer[0] > (byte) 0;
  }

  public byte ReadByte(int bits = 8)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return this._tempBuffer[0];
  }

  public void ReadBytes(byte[] destinationBuffer, int byteCount)
  {
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, destinationBuffer, byteCount * 8);
    this.BitPosition += byteCount * 8;
  }

  public short ReadShort(int bits = 16 /*0x10*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadInt16LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public ushort ReadUShort(int bits = 16 /*0x10*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadUInt16LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public T ReadEnum<T>() where T : struct, Enum
  {
    if (!typeof (int).IsAssignableFrom(Enum.GetUnderlyingType(typeof (T))))
      throw new InvalidOperationException($"Trying to write enum type {typeof (T)} that is not assignable to int!");
    return (T) Enum.ToObject(typeof (T), this.ReadInt(Mathf.CeilToInt(Math.Log2((double) MaxEnumValueCache.Get<T>()) + 1.0)));
  }

  public int ReadInt(int bits = 32 /*0x20*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadInt32LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public uint ReadUInt(int bits = 32 /*0x20*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadUInt32LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public float ReadFloat(QuantizeParams? quantizeParams = null)
  {
    Array.Clear((Array) this._tempBuffer);
    if (quantizeParams.HasValue)
      return PacketReader.Unquantize(this.ReadUInt(quantizeParams.Value.bits), quantizeParams.Value.min, quantizeParams.Value.max, quantizeParams.Value.bits);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 32 /*0x20*/);
    this.BitPosition += 32 /*0x20*/;
    return BinaryPrimitives.ReadSingleLittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public long ReadLong(int bits = 64 /*0x40*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadInt64LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public ulong ReadULong(int bits = 64 /*0x40*/)
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, bits);
    this.BitPosition += bits;
    return BinaryPrimitives.ReadUInt64LittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public double ReadDouble()
  {
    Array.Clear((Array) this._tempBuffer);
    BitSerializationUtil.ReadBits(this.Buffer, this.BitPosition, this._tempBuffer, 64 /*0x40*/);
    this.BitPosition += 64 /*0x40*/;
    return BinaryPrimitives.ReadDoubleLittleEndian(Span<byte>.op_Implicit(MemoryExtensions.AsSpan<byte>(this._tempBuffer)));
  }

  public Vector2 ReadVector2(QuantizeParams? quantizeParamsX = null, QuantizeParams? quantizeParamsY = null)
  {
    return new Vector2(this.ReadFloat(quantizeParamsX), this.ReadFloat(quantizeParamsY));
  }

  public List<T> ReadList<T>(int lengthBits = 32 /*0x20*/) where T : IPacketSerializable, new()
  {
    List<T> objList = new List<T>();
    int num = this.ReadInt(lengthBits);
    for (int index = 0; index < num; ++index)
      objList.Add(this.Read<T>());
    return objList;
  }

  public string ReadString()
  {
    int byteCount = this.ReadInt();
    if (this._stringBuffer.Length < byteCount)
      this._stringBuffer = new byte[(int) Math.Pow(2.0, Math.Ceiling(Math.Log2((double) byteCount)))];
    this.ReadBytes(this._stringBuffer, byteCount);
    return Encoding.UTF8.GetString(this._stringBuffer, 0, byteCount);
  }

  public T Read<T>() where T : IPacketSerializable, new()
  {
    T obj = new T();
    obj.Deserialize(this);
    return obj;
  }

  public static float Unquantize(uint value, float min, float max, int bitLength)
  {
    return (float) ((double) value / Math.Pow(2.0, (double) bitLength) * ((double) max - (double) min)) + min;
  }
}
