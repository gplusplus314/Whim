namespace Whim;

public static class IntExtensions
{
	public static int Mod(this int value, int mod) => ((value % mod) + mod) % mod;
}
