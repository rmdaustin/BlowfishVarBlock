﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BlowfishVarBlock;

namespace BlowfishVarBlock.Tests
{
    [TestClass]
    public class Variable36bitBlockTests
    {
        byte[] key = new byte[] { 0x20, 0x36, 0xac, 0x45, 0xd0 };
        [TestMethod]
        public void Var_UInt64_36Bit_Min()
        {
            const ulong sourceValue = ulong.MinValue;
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;

            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            //Assert.AreEqual((ulong)158134701838, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void Var_UInt64_36Bit_Max()
        {
            const ulong sourceValue = (((ulong)1) << 36) - 1; // 36-bit int MaxValue
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;
            

            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            //Assert.AreEqual((ulong)130283766274, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void Var_UInt64_36Bit_Mixed()
        {
            const ulong sourceValue = (((ulong)1) << 36) - 10; // 36-bit int MaxValue
            ulong encryptedValue = ulong.MinValue;
            ulong targetValue = ulong.MaxValue;


            byte[] payload = new byte[5];
            byte[] buffer = new byte[8];

            Buffer.BlockCopy(BitConverter.GetBytes(sourceValue), 0, payload, 0, 5);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            encryptedValue = BitConverter.ToUInt64(buffer, 0);

            // one failure mode would be if the encryption didn't happen at all
            Assert.AreNotEqual(sourceValue, encryptedValue);

            // this is the expected value
            //Assert.AreEqual((ulong)130283766274, encryptedValue);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 36);

            payload.CopyTo(buffer, 0);
            targetValue = BitConverter.ToUInt64(buffer, 0);

            // the output should convert back tot he original uint
            Assert.AreEqual(sourceValue, targetValue);
        }
        [TestMethod]
        public void Var_UInt64_36Bit_Multiple_Blocks()
        {
            ulong[] data = new ulong[] { ulong.MinValue, ulong.MinValue+1, ulong.MinValue+2, (((ulong)1) << 36) - 3, (((ulong)1) << 36) - 2, (((ulong)1) << 36) - 1 };
            // Blowfish will calculate that a chunksize of 5 is required for 36-bit data, so provide that
            const int chunkSize = 5;

            byte[] original = new byte[data.Length * chunkSize];
            //byte[] encypheredExpected = new byte[] { 52, 202, 24, 208, 6, 73, 45, 38, 128, 114, 73, 45, 38, 128, 114 };

            for (int i = 0; i < data.Length; i++)
                Buffer.BlockCopy(BitConverter.GetBytes(data[i]), 0, original, i * chunkSize, chunkSize);
 
            byte[] payload = new byte[original.Length];

            Buffer.BlockCopy(original, 0, payload, 0, original.Length);

            var encryptor = new Blowfish(key);

            encryptor.Encipher(payload, payload.Length, 36);

            // one failure mode would be if the encryption didn't happen at all
            CollectionAssert.AreNotEqual(original, payload);

            // this is the expected value
            //CollectionAssert.AreEqual(encypheredExpected, payload);

            // use a separate decryptor instance to ensure no dependency on 
            // encryptor stored state
            var decryptor = new Blowfish(key);
            decryptor.Decipher(payload, payload.Length, 36);

            // the output should convert back to the original array
            CollectionAssert.AreEqual(original, payload);
        }
    }
}
