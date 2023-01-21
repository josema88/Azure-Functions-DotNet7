using System;
namespace Azure_Functions_DotNet7.Data
{
	public class Department
	{
		public int Id { get; set; }
        public string Name { get; set; }
		public string? Description { get; set; }
		public bool Enabled { get; set; } 
    }
}