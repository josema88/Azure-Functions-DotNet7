using Dapper;
using System.Data.SqlClient;

namespace Azure_Functions_DotNet7.Data
{
	public class DepartmentsRepository : IDepartmentsRepository
    {
        private readonly string _connectionString = string.Empty;

        public DepartmentsRepository(string connectionString)
        {
            _connectionString = connectionString;
        }

        public int InsertDepartment(Department department)
        {
            var db = new SqlConnection(_connectionString);
            var query = @"INSERT INTO [dbo].[Departments] ([name], [description])
                        OUTPUT INSERTED.Id
                        VALUES (@name, @description);";
            var id = db.QuerySingle<int>(query, new
            {
                name = department.Name,
                description = department.Description,
            });
            return id;
        }

        public Department GetDepartment(int id)
        {
            var db = new SqlConnection(_connectionString);
            var query = @"SELECT
                            [id],
                            [name],
                            [description],
                            [enabled]
                        FROM [dbo].[Departments]
                        WHERE
                            [id] = @id";
            var department = db.QueryFirstOrDefault<Department>(query, new { id } );
            return department;
        }

        public List<Department> GetDepartments()
        {
            var db = new SqlConnection(_connectionString);
            string query = @"SELECT
                                [id],
                                [name],
                                [description],
                                [enabled]
                            FROM [dbo].[Departments]";
            var departments = db.Query<Department>(query);
            return departments.ToList();
        }

        public void UpdateDepartment(int id, Department department)
        {
            using SqlConnection connection = new(_connectionString);
            var db = new SqlConnection(_connectionString);
            var query = @"UPDATE [dbo].[Departments]
                        SET
                            [name] = @name,
                            [description] = @description,
                            [enabled] = @enabled
                        WHERE
                            [id] = @id";
            db.Query(query, new
            {
                id,
                name = department.Name,
                description = department.Description,
                enabled = department.Enabled,
            });
        }

        public void DeleteDepartment(int id)
        {
            string query = @"DELETE FROM [dbo].[Departments]
                            WHERE
                                [id] = @id";
            var db = new SqlConnection(_connectionString);
            db.Query(query, new { id });
        }
    }
}

