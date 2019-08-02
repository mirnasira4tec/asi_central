using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using asi.asicentral.services;
using asi.asicentral.interfaces;

namespace Core.Tests
{
	[TestClass]
	public class StringEncryptionTest
	{
		[TestMethod]
		public void TestEncrypt()
		{
			EncryptionService encryptionService = new EncryptionService();
			string key = "ASIP@ssWord34567";
			Assert.AreEqual("YMvnb94hIPcHSG+YEadrvIbw8EDkpG8SeZCeCcDeLik=", encryptionService.ECBEncrypt(key, "Upside Learning !"));
			Console.WriteLine("Encrypt \"This is a test\" gives \"" + encryptionService.ECBEncrypt(key, "This is a test") + "\"");
			Console.WriteLine("Encrypt \"This is another test\" gives \"" + encryptionService.ECBEncrypt(key, "This is another test") + "\"");
		}
	}
}
