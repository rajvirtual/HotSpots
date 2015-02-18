using System.Runtime.Serialization.Json;
using System.IO;
using System.Text;

namespace HotSpots.Facebook {
	public static class JsonStringSerializer {
		public static T Deserialize<T>(string strData) where T : class {
			//needs System.Servicemodel.Web
			DataContractJsonSerializer dcsJson = new DataContractJsonSerializer(typeof(T));
			byte[] byteArray = Encoding.UTF8.GetBytes(strData);
			MemoryStream mS = new MemoryStream(byteArray);
			T tRet = dcsJson.ReadObject(mS) as T;
			mS.Dispose();
			return (tRet);
		}
	}
}
