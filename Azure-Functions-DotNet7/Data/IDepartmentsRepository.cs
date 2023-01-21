namespace Azure_Functions_DotNet7.Data
{
	public interface IDepartmentsRepository
	{
        int InsertDepartment(Department department);
        Department GetDepartment(int id);
        List<Department> GetDepartments();
        void UpdateDepartment(int id, Department department);
        void DeleteDepartment(int id);
    }
}