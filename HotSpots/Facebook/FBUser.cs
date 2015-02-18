using System.Runtime.Serialization;

namespace HotSpots.Facebook {
    [DataContract]
	public class FBUser {
		#region ID
		private string m_strID;
		[DataMember(Name = "id")]
		public string ID {
			get { return m_strID ?? ""; }
			set { m_strID = value; }
		}
		#endregion
		#region Name
		private string m_strName;
		[DataMember(Name = "name")]
		public string Name {
			get { return m_strName ?? ""; }
			set { m_strName = value; }
		}
		#endregion
		#region PictureLink
		private string m_strPictureLink;
		[DataMember(Name = "picture")]
		public string PictureLink {
			get { return m_strPictureLink ?? ""; }
			set { m_strPictureLink = value; }
		}
		#endregion

		#region Gender
		private string m_strGender;
		[DataMember(Name = "gender")]
		public string Gender {
			get { return m_strGender ?? ""; }
			set { m_strGender = value; }
		}
		#endregion
		#region Link
		private string m_strLink;
		[DataMember(Name = "link")]
		public string Link {
			get { return m_strLink ?? ""; }
			set { m_strLink = value; }
		}
		#endregion
		#region HomeTown
		private FBHomeTown m_fbjtHomeTown;
		[DataMember(Name = "hometown")]
		public FBHomeTown HomeTown {
			get { return m_fbjtHomeTown ?? new FBHomeTown { Name = "" }; }
			set { m_fbjtHomeTown = value; }
		}
		#endregion
		#region PicLink
		public string PicLink {
			//use a default image if user id is unknonw - should not occure
			get { return string.Format("http://graph.facebook.com/{0}/picture", m_strID ?? "100000886762882"); }
		}
		#endregion
		[DataContract]
		public class FBHomeTown {
			#region Name
			private string m_strName;
			[DataMember(Name = "name")]
			public string Name {
				get { return m_strName ?? ""; }
				set { m_strName = value; }
			}
			#endregion
		}
	}
}
