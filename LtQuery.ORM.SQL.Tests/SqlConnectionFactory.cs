using System.Collections.Generic;
using System.Data.SQLite;

namespace LtQuery.ORM.SQL.Tests
{
    class SqlConnectionFactory
    {
        public IEnumerable<NonRelationEntity> GetEntities()
        {
            var rand = new RandomEx(0);
            var id = 1;
            yield return new NonRelationEntity() { Id = id++, Code = rand.Next(), Name = rand.NextString() };
            yield return new NonRelationEntity() { Id = id++, Code = rand.Next(), Name = rand.NextString() };
            yield return new NonRelationEntity() { Id = id++, Code = rand.Next(), Name = rand.NextString() };
        }

        public SQLiteConnection Create()
        {
            var connection = new SQLiteConnection("Data Source=:memory:");
            connection.Open();

            // Create Table
            using (var command = new SQLiteCommand(connection))
            {
                command.CommandText = @"
CREATE TABLE [NonRelationEntity] (
	[Id] INTEGER PRIMARY KEY AUTOINCREMENT,
	[Code] INT NOT NULL,
	[Name] NVARCHAR(10) NULL
)
";
                command.ExecuteNonQuery();
            }

            // Insert Data
            {
                foreach (var entity in GetEntities())
                {
                    using (var command = new SQLiteCommand(connection))
                    {
                        command.CommandText = @"INSERT INTO [NonRelationEntity] ([Code], [Name]) VALUES(@Code, @Name)";
                        command.Parameters.AddWithValue("@Code", entity.Code);
                        command.Parameters.AddWithValue("@Name", entity.Name);
                        command.ExecuteNonQuery();
                    }
                }
            }

            return connection;
        }
    }
}
