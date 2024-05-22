
namespace Dal;
internal static class DataSource
{
    internal static class Config//A class that configure running variable numbers
    {
        public static DateTime? ProjectStartDate { get; set; } = new DateTime(2024 , 1 , 1);
        public static DateTime? ProjectEndDate { get; set; } = new DateTime(2025, 1, 1);
        internal const int startTaskId = 1000;
        private static int nextTaskId = startTaskId;
        internal static int NextTaskId { get => nextTaskId++; }
        internal const int startDependencyId = 1000;
        private static int nextDependencyId = startDependencyId;
        internal static int NextDependencyId { get => nextDependencyId++; }
    }
    #region Lists Of Entities
    internal static List<DO.Task>? Tasks { get; set; } = new();
    internal static List<DO.Engineer>? Engineers { get; set; } = new();
    internal static List<DO.Dependency>? Dependencies { get; set; } = new();
    #endregion

}
