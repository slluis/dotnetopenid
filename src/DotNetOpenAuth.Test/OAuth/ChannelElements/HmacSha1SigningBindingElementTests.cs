﻿//-----------------------------------------------------------------------
// <copyright file="HmacSha1SigningBindingElementTests.cs" company="Andrew Arnott">
//     Copyright (c) Andrew Arnott. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------

namespace DotNetOpenAuth.Test.OAuth.ChannelElements {
	using DotNetOpenAuth.OAuth.ChannelElements;
	using DotNetOpenAuth.OAuth.Messages;
	using DotNetOpenAuth.Test.Mocks;
	using NUnit.Framework;

	[TestFixture]
	public class HmacSha1SigningBindingElementTests : MessagingTestBase {
		[TestCase]
		public void SignatureTest() {
			UnauthorizedTokenRequest message = SigningBindingElementBaseTests.CreateTestRequestTokenMessage(this.MessageDescriptions, null);

			var hmac = new HmacSha1SigningBindingElement();
			hmac.Channel = new TestChannel(this.MessageDescriptions);
			Assert.AreEqual("kR0LhH8UqylaLfR/esXVVlP4sQI=", hmac.GetSignatureTestHook(message));
		}
	}
}
