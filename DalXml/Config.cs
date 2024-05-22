namespace Dal;

internal static class Config
{
    static string s_data_config_xml = "data-config";
    static DateTime? ProjectEndDate { get; set; } = XMLTools.LoadProjectDate("End");
    static DateTime? ProjectStartDate { get; set; } = XMLTools.LoadProjectDate("Start");
    internal static int NextTaskId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextTaskId"); }
    internal static int NextDependencyId { get => XMLTools.GetAndIncreaseNextId(s_data_config_xml, "NextDependencyId"); }
}
