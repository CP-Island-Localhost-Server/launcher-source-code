using System;
using System.Globalization;
using Newtonsoft.Json.Utilities;

namespace Newtonsoft.Json.Serialization
{
	internal class DefaultReferenceResolver : IReferenceResolver
	{
		private int _referenceCount;

		private BidirectionalDictionary<string, object> GetMappings(object context)
		{
			JsonSerializerInternalBase jsonSerializerInternalBase;
			if (context is JsonSerializerInternalBase)
			{
				jsonSerializerInternalBase = (JsonSerializerInternalBase)context;
			}
			else
			{
				if (!(context is JsonSerializerProxy))
				{
					throw new Exception("The DefaultReferenceResolver can only be used internally.");
				}
				jsonSerializerInternalBase = ((JsonSerializerProxy)context).GetInternalSerializer();
			}
			return jsonSerializerInternalBase.DefaultReferenceMappings;
		}

		public object ResolveReference(object context, string reference)
		{
			object second;
			GetMappings(context).TryGetByFirst(reference, out second);
			return second;
		}

		public string GetReference(object context, object value)
		{
			BidirectionalDictionary<string, object> mappings = GetMappings(context);
			string first;
			if (!mappings.TryGetBySecond(value, out first))
			{
				_referenceCount++;
				first = _referenceCount.ToString(CultureInfo.InvariantCulture);
				mappings.Add(first, value);
			}
			return first;
		}

		public void AddReference(object context, string reference, object value)
		{
			GetMappings(context).Add(reference, value);
		}

		public bool IsReferenced(object context, object value)
		{
			string first;
			return GetMappings(context).TryGetBySecond(value, out first);
		}
	}
}
