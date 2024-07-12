public struct StructHash
{
	private uint hash;

	private uint count;

	public void Combine<T>(T obj)
	{
		uint hashCode = (uint)obj.GetHashCode();
		hashCode *= 3432918353u;
		hashCode = (hashCode << 15) | (hashCode >> 17);
		hashCode *= 461845907;
		foldIn(hashCode);
	}

	private void foldIn(uint k)
	{
		hash ^= k;
		hash = (hash << 13) | (hash >> 19);
		hash = hash * 5 + 3864292196u;
		count++;
	}

	public void Combine<T>(T[] obj)
	{
		if (obj != null)
		{
			for (int i = 0; i < obj.Length; i++)
			{
				Combine(obj[i]);
			}
		}
		else
		{
			foldIn(0u);
		}
	}

	public static implicit operator int(StructHash sh)
	{
		uint num = sh.hash ^ sh.count;
		num ^= num >> 16;
		num *= 2246822507u;
		num ^= num >> 13;
		num *= 3266489909u;
		return (int)(num ^ (num >> 16));
	}
}
