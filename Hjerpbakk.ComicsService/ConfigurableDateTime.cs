using System;
namespace Hjerpbakk.ComicsService
{
	public static class ConfigurableDateTime
	{
		public static DateTime? CurrentTime { private get; set; }

		public static DateTime UtcNow => CurrentTime ?? DateTime.UtcNow;
	}
}
