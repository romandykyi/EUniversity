namespace EUniversity.Core.Policy;

public static class Policies
{
    public const string Default = "Default";
    public const string IsStudent = "IsStudent";
    public const string IsTeacher = "IsTeacher";
    public const string IsTeacherOrAdministrator = "IsTeacherOrAdmin";
    public const string HasAdministratorPermission = "HasAdministratorPermission";
    /// <summary>
    /// Policy that determines whether user can view students enrollments.
    /// Allows teachers and administrators to view all enrollments and other users to view
    /// only their own enrollments.
    /// </summary>
    public const string CanViewStudentEnrollments = "CanViewStudentEnrollments";
}
