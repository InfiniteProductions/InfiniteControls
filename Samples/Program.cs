using System;

namespace InfiniteControlsSamples
{
#if WINDOWS || LINUX
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new ButtonsTest1())  // CircleCollisionTest())
				game.Run();
		}
	}
#endif
}
