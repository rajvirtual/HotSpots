using System.Runtime.Serialization;

namespace HotSpots.Facebook {
	[DataContract]
	public class FBFriends {
		#region Friends
		private FBUser[] m_aFriends;
		[DataMember(Name = "data")]
		public FBUser[] Friends {
			get { return m_aFriends; }
			set { m_aFriends = value; }
		}
		#endregion
	}
}