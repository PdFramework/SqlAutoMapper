namespace PeinearyDevelopment.Framework.Data
{
	internal interface IDbCommandData
	{
		void AddInputParameter(string parameterName, object value);
		void AddInputParameter<T>(string parameterName, T TValue);
		void AddOutputParameter<T>(string parameterName, T TValue);
	}
}
