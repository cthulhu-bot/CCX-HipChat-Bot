using System;
using System.Collections.Generic;

namespace HipChatBots
{
	public class HipChatMessage
	{
		private sealed class DateEqualityComparer : IEqualityComparer<HipChatMessage>
		{
			public bool Equals(HipChatMessage x, HipChatMessage y)
			{
				if (ReferenceEquals(x, y))
				{
					return true;
				}
				if (ReferenceEquals(x, null))
				{
					return false;
				}
				if (ReferenceEquals(y, null))
				{
					return false;
				}
				if (x.GetType() != y.GetType())
				{
					return false;
				}
				return x.Date.Equals(y.Date);
			}

			public int GetHashCode(HipChatMessage obj)
			{
				return obj.Date.GetHashCode();
			}
		}

		private static readonly IEqualityComparer<HipChatMessage> DateComparerInstance = new DateEqualityComparer();

		public static IEqualityComparer<HipChatMessage> DateComparer
		{
			get { return DateComparerInstance; }
		}

		public HipChatMessage(DateTime date, string user, string contents)
		{
			Date = date;
			From = user;
			Message = contents;
		}

		public DateTime Date { get; private set; }
		public string From { get; private set; }
		public string Message { get; private set; }

		public static HipChatMessage NilMessage = new HipChatMessage(DateTime.MinValue, "none", string.Empty);
	}
}