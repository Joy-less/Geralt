using System;
using System.IO;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Geralt.Tests;

[TestClass]
public class BLAKE2bTests
{
    // https://github.com/BLAKE2/BLAKE2/blob/master/testvectors/blake2-kat.json
    public static IEnumerable<object[]> UnkeyedTestVectors()
    {
        yield return new object[]
        {
            "786a02f742015903c6c6fd852552d272912f4740e15847618a86e217f71f5419d25e1031afee585313896444934eb04b903a685b1448b755d56f701afe9be2ce",
            ""
        };
        yield return new object[]
        {
            "cbaa0ba7d482b1f301109ae41051991a3289bc1198005af226c5e4f103b66579f461361044c8ba3439ff12c515fb29c52161b7eb9c2837b76a5dc33f7cb2e2e8",
            "0001020304"
        };
        yield return new object[]
        {
            "9447d98aa5c9331352f43d3e56d0a9a9f9581865998e2885cc56dd0a0bd5a7b50595bd10f7529bcd31f37dc16a1465d594079667da2a3fcb70401498837cedeb",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f2021"
        };
        yield return new object[]
        {
            "2fc6e69fa26a89a5ed269092cb9b2a449a4409a7a44011eecad13d7c4b0456602d402fa5844f1a7a758136ce3d5d8d0e8b86921ffff4f692dd95bdc8e5ff0052",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
        yield return new object[]
        {
            "956da1c68d83a7b881e01b9a966c3c0bf27f68606a8b71d457bd016d4c41dd8a380c709a296cb4c6544792920fd788835771a07d4a16fb52ed48050331dc4c8b",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f40414243444546"
        };
    }
    
    // https://github.com/BLAKE2/BLAKE2/blob/master/testvectors/blake2b-kat.txt
    public static IEnumerable<object[]> KeyedTestVectors()
    {
        yield return new object[]
        {
            "10ebb67700b1868efb4417987acf4690ae9d972fb7a590c2f02871799aaa4786b5e996e8f0f4eb981fc214b005f42d2ff4233499391653df7aefcbc13fc51568",
            "",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
        yield return new object[]
        {
            "961f6dd1e4dd30f63901690c512e78e4b45e4742ed197c3c5e45c549fd25f2e4187b0bc9fe30492b16b0d0bc4ef9b0f34c7003fac09a5ef1532e69430234cebd",
            "00",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
        yield return new object[]
        {
            "86221f3ada52037b72224f105d7999231c5e5534d03da9d9c0a12acb68460cd375daf8e24386286f9668f72326dbf99ba094392437d398e95bb8161d717f8991",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
        yield return new object[]
        {
            "65676d800617972fbd87e4b9514e1c67402b7a331096d3bfac22f1abb95374abc942f16e9ab0ead33b87c91968a6e509e119ff07787b3ef483e1dcdccf6e3022",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
        yield return new object[]
        {
            "49d6a608c9bde4491870498572ac31aac3fa40938b38a7818f72383eb040ad39532bc06571e13d767e6945ab77c0bdc3b0284253343f9f6c1244ebf2ff0df866",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f4041424344",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f"
        };
    }
    
    // https://github.com/emilbayes/blake2b/blob/master/test-vectors.json
    public static IEnumerable<object[]> KeyDerivationTestVectors()
    {
        yield return new object[]
        {
            "9b273ebe335540b87be899abe169389ed61ed262c3a0a16e4998bbf752f0bee3",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f",
            "35313236666232613337343030643261",
            "35623662343165643962333433666530",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e"
        };
        yield return new object[]
        {
            "5fcdcc02be7714a0dbc77df498bf999ea9225d564adca1c121c9af03af92cac8177b9b4a86bcc47c79aa32aac58a3fef967b2132e9352d4613fe890beed2571b",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e3f",
            "35313236666232613337343030643261",
            "35623662343165643962333433666530",
            "000102030405060708090a0b0c0d0e0f101112131415161718191a1b1c1d1e1f202122232425262728292a2b2c2d2e2f303132333435363738393a3b3c3d3e"
        };
    }
    
    public static IEnumerable<object[]> TagInvalidParameterSizes()
    {
        yield return new object[] { BLAKE2b.MaxTagSize + 1, 1, BLAKE2b.KeySize };
        yield return new object[] { BLAKE2b.MinTagSize - 1, 1, BLAKE2b.KeySize };
        yield return new object[] { BLAKE2b.TagSize, 1, BLAKE2b.MaxKeySize + 1 };
        yield return new object[] { BLAKE2b.TagSize, 1, BLAKE2b.MinKeySize - 1 };
    }
    
    [TestMethod]
    [DynamicData(nameof(UnkeyedTestVectors), DynamicDataSourceType.Method)]
    public void ComputeHash_Valid(string hash, string message)
    {
        Span<byte> h = stackalloc byte[hash.Length / 2];
        Span<byte> m = Convert.FromHexString(message);
        
        BLAKE2b.ComputeHash(h, m);
        
        Assert.AreEqual(hash, Convert.ToHexString(h).ToLower());
    }
    
    [TestMethod]
    [DynamicData(nameof(UnkeyedTestVectors), DynamicDataSourceType.Method)]
    public void ComputeHashStream_Valid(string hash, string message)
    {
        Span<byte> h = stackalloc byte[hash.Length / 2];
        using var m = new MemoryStream(Convert.FromHexString(message), writable: false);
        
        BLAKE2b.ComputeHash(h, m);
        
        Assert.AreEqual(hash, Convert.ToHexString(h).ToLower());
    }
    
    [TestMethod]
    [DataRow(BLAKE2b.MaxHashSize + 1, 1)]
    [DataRow(BLAKE2b.MinHashSize - 1, 1)]
    public void ComputeHash_Invalid(int hashSize, int messageSize)
    {
        var h = new byte[hashSize];
        var m = new byte[messageSize];
        
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BLAKE2b.ComputeHash(h, m));
    }
    
    [TestMethod]
    [DataRow(BLAKE2b.MaxHashSize + 1, 1)]
    [DataRow(BLAKE2b.MinHashSize - 1, 1)]
    [DataRow(BLAKE2b.MaxHashSize, 0)]
    public void ComputeHashStream_Invalid(int hashSize, int messageSize)
    {
        var h = new byte[hashSize];
        
        if (messageSize > 0) {
            using var m = new MemoryStream(new byte[messageSize], writable: false);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => BLAKE2b.ComputeHash(h, m));
        }
        else {
            using MemoryStream? m = null;
            Assert.ThrowsException<ArgumentNullException>(() => BLAKE2b.ComputeHash(h, m));
        }
    }
    
    [TestMethod]
    [DynamicData(nameof(KeyedTestVectors), DynamicDataSourceType.Method)]
    public void ComputeTag_Valid(string tag, string message, string key)
    {
        Span<byte> t = stackalloc byte[tag.Length / 2];
        Span<byte> m = Convert.FromHexString(message);
        Span<byte> k = Convert.FromHexString(key);
        
        BLAKE2b.ComputeTag(t, m, k);
        
        Assert.AreEqual(tag, Convert.ToHexString(t).ToLower());
    }
    
    [TestMethod]
    [DynamicData(nameof(TagInvalidParameterSizes), DynamicDataSourceType.Method)]
    public void ComputeTag_Invalid(int tagSize, int messageSize, int keySize)
    {
        var t = new byte[tagSize];
        var m = new byte[messageSize];
        var k = new byte[keySize];
        
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BLAKE2b.ComputeTag(t, m, k));
    }
    
    [TestMethod]
    [DynamicData(nameof(KeyedTestVectors), DynamicDataSourceType.Method)]
    public void VerifyTag_Valid(string tag, string message, string key)
    {
        Span<byte> t = Convert.FromHexString(tag);
        Span<byte> m = Convert.FromHexString(message);
        Span<byte> k = Convert.FromHexString(key);
        
        bool valid = BLAKE2b.VerifyTag(t, m, k);
        
        Assert.IsTrue(valid);
    }
    
    [TestMethod]
    [DynamicData(nameof(KeyedTestVectors), DynamicDataSourceType.Method)]
    public void VerifyTag_Tampered(string tag, string message, string key)
    {
        var parameters = new List<byte[]>
        {
            Convert.FromHexString(tag),
            message.Length > 0 ? Convert.FromHexString(message) : new byte[1],
            Convert.FromHexString(key)
        };
        
        foreach (var param in parameters) {
            param[0]++;
            bool valid = BLAKE2b.VerifyTag(parameters[0], parameters[1], parameters[2]);
            param[0]--;
            Assert.IsFalse(valid);
        }
    }
    
    [TestMethod]
    [DynamicData(nameof(TagInvalidParameterSizes), DynamicDataSourceType.Method)]
    public void VerifyTag_Invalid(int tagSize, int messageSize, int keySize)
    {
        var t = new byte[tagSize];
        var m = new byte[messageSize];
        var k = new byte[keySize];
        
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BLAKE2b.VerifyTag(t, m, k));
    }
    
    [TestMethod]
    [DynamicData(nameof(KeyDerivationTestVectors), DynamicDataSourceType.Method)]
    public void DeriveKey_Valid(string outputKeyingMaterial, string inputKeyingMaterial, string personalisation, string salt, string info)
    {
        Span<byte> o = stackalloc byte[outputKeyingMaterial.Length / 2];
        Span<byte> k = Convert.FromHexString(inputKeyingMaterial);
        Span<byte> p = Convert.FromHexString(personalisation);
        Span<byte> s = Convert.FromHexString(salt);
        Span<byte> i = Convert.FromHexString(info);
        
        BLAKE2b.DeriveKey(o, k, p, s, i);
        
        Assert.AreEqual(outputKeyingMaterial, Convert.ToHexString(o).ToLower());
    }
    
    [TestMethod]
    [DataRow(BLAKE2b.MaxKeySize + 1, BLAKE2b.KeySize, BLAKE2b.PersonalSize, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.MinKeySize - 1, BLAKE2b.KeySize, BLAKE2b.PersonalSize, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.MaxKeySize + 1, BLAKE2b.PersonalSize, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.MinKeySize - 1, BLAKE2b.PersonalSize, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.KeySize, BLAKE2b.PersonalSize + 1, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.KeySize, BLAKE2b.PersonalSize - 1, BLAKE2b.SaltSize, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.KeySize, BLAKE2b.PersonalSize, BLAKE2b.SaltSize + 1, 1)]
    [DataRow(BLAKE2b.KeySize, BLAKE2b.KeySize, BLAKE2b.PersonalSize, BLAKE2b.SaltSize - 1, 1)]
    public void DeriveKey_Invalid(int outputKeyingMaterialSize, int inputKeyingMaterialSize, int personalisationSize, int saltSize, int infoSize)
    {
        var o = new byte[outputKeyingMaterialSize];
        var k = new byte[inputKeyingMaterialSize];
        var p = new byte[personalisationSize];
        var s = new byte[saltSize];
        var i = new byte[infoSize];
        
        Assert.ThrowsException<ArgumentOutOfRangeException>(() => BLAKE2b.DeriveKey(o, k, p, s, i));
    }
    
    [TestMethod]
    [DynamicData(nameof(UnkeyedTestVectors), DynamicDataSourceType.Method)]
    [DynamicData(nameof(KeyedTestVectors), DynamicDataSourceType.Method)]
    public void Incremental_Valid(string hash, string message, string? key = null)
    {
        Span<byte> h = stackalloc byte[hash.Length / 2];
        Span<byte> m = Convert.FromHexString(message);
        Span<byte> k = key != null ? Convert.FromHexString(key) : Span<byte>.Empty;
        
        using var blake2b = new IncrementalBLAKE2b(h.Length, k);
        blake2b.Update(m);
        blake2b.Finalize(h);
        
        Assert.AreEqual(hash, Convert.ToHexString(h).ToLower());
    }
    
    [TestMethod]
    [DynamicData(nameof(TagInvalidParameterSizes), DynamicDataSourceType.Method)]
    public void Incremental_Invalid(int hashSize, int messageSize, int keySize)
    {
        var h = new byte[hashSize];
        var m = new byte[messageSize];
        var k = new byte[keySize];
        
        if (keySize is < IncrementalBLAKE2b.MinKeySize or > IncrementalBLAKE2b.MaxKeySize) {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IncrementalBLAKE2b(hashSize, k));
        }
        else if (hashSize is < IncrementalBLAKE2b.MinHashSize or > IncrementalBLAKE2b.MaxHashSize) {
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => new IncrementalBLAKE2b(hashSize));
            using var blake2b = new IncrementalBLAKE2b(IncrementalBLAKE2b.MaxHashSize);
            blake2b.Update(m);
            Assert.ThrowsException<ArgumentOutOfRangeException>(() => blake2b.Finalize(h));
        }
    }
}