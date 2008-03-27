using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using DotNetOpenId.Provider;
using System.Runtime.Serialization;

namespace DotNetOpenId {
	/// <summary>
	/// A message did not conform to the OpenID protocol, or 
	/// some other processing error occurred.
	/// </summary>
	[Serializable]
	public class OpenIdException : Exception, IEncodable {
		NameValueCollection query;
		internal Identifier IdentityUrl { get; private set; }

		internal OpenIdException(string message, Identifier identityUrl, NameValueCollection query, Exception innerException)
			: base(message, innerException) {
			this.query = query;
			IdentityUrl = identityUrl;
		}
		internal OpenIdException(string message, Identifier identityUrl, NameValueCollection query)
			: this(message, identityUrl, query, null) {
		}
		internal OpenIdException(string message, Identifier identityUrl, Exception innerException)
			: this(message, identityUrl, null, innerException) {
		}
		internal OpenIdException(string message, Identifier identityUrl)
			: this(message, identityUrl, null, null) {
		}
		internal OpenIdException(string message, NameValueCollection query)
			: this(message, null, query, null) {
		}
		internal OpenIdException(string message, Exception innerException)
			: this(message, null, null, innerException) {
		}
		internal OpenIdException(string message)
			: this(message, null, null, null) {
		}
		protected OpenIdException(SerializationInfo info, StreamingContext context)
			: base(info, context) { }
		internal OpenIdException() { }

		internal bool HasReturnTo {
			get {
				return query == null ? false : (query[QueryStringArgs.openid.return_to] != null);
			}
		}

		#region IEncodable Members

		EncodingType IEncodable.EncodingType {
			get {
				if (HasReturnTo)
					return EncodingType.RedirectBrowserUrl;

				if (query != null) {
					string mode = query.Get(QueryStringArgs.openid.mode);
					if (mode != null)
						if (mode != QueryStringArgs.Modes.checkid_setup &&
							mode != QueryStringArgs.Modes.checkid_immediate)
							return EncodingType.ResponseBody;
				}

				// Notes from the original port
				//# According to the OpenID spec as of this writing, we are
				//# probably supposed to switch on request type here (GET
				//# versus POST) to figure out if we're supposed to print
				//# machine-readable or human-readable content at this
				//# point.  GET/POST seems like a pretty lousy way of making
				//# the distinction though, as it's just as possible that
				//# the user agent could have mistakenly been directed to
				//# post to the server URL.

				//# Basically, if your request was so broken that you didn't
				//# manage to include an openid.mode, I'm not going to worry
				//# too much about returning you something you can't parse.
				return EncodingType.None;
			}
		}

		public IDictionary<string, string> EncodedFields {
			get {
				var q = new Dictionary<string, string>();
				q.Add(QueryStringArgs.openid.mode, QueryStringArgs.Modes.error);
				q.Add(QueryStringArgs.openid.error, Message);
				return q;
			}
		}
		public Uri RedirectUrl {
			get {
				if (query == null)
					return null;
				string return_to = query.Get(QueryStringArgs.openid.return_to);
				if (return_to == null)
					throw new InvalidOperationException("return_to URL has not been set.");
				return new Uri(return_to);
			}
		}

		#endregion

	}
}
