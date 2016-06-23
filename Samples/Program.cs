using System;

namespace InfiniteControlsSamples
{
#if WINDOWS || LINUX
	public static class Program
	{
		[STAThread]
		static void Main()
		{
			using (var game = new CircleCollisionTest())  // ButtonsTest1())
				game.Run();
		}
	}
#endif
}
