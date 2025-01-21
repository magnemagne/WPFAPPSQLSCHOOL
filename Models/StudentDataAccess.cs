using MySqlConnector;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
class StudentDataAccess
{

    public IEnumerable<Student> GetStudents(string cs)
    {
        List<Student> students = [];

        using IDbConnection connection = new MySqlConnection(cs);
        connection.Open();

        const string query = """
        SELECT
            studentid,
            firstname,
            lastname,
            class,
            age
        FROM students;
        """;

        using IDbCommand command = connection.CreateCommand();

        command.CommandText = query;

        using IDataReader reader = command.ExecuteReader();
        while (reader.Read())
        {
            Student student = new Student();
            students.Add(student);
            //specifichiamo nel metodo GetInt32 la colonna
            student.Id = reader.GetInt32(reader.GetOrdinal("studentid"));
            var ageFromTable = reader.GetValue(reader.GetOrdinal("age"));
            student.Age = (int?) (ageFromTable == DBNull.Value ? null : ageFromTable);
            student.Firstname = reader["firstname"] as string ?? "";
            student.Lastname = reader["lastname"] as string ?? "";
            student.Class = reader["class"] as string ?? "";

        }

        return students;
    }

    public Student? GetStudent(int id, string cs)
    {
        Student? student = null;

        using var connection = new MySqlConnection(cs);
        connection.Open();

        const string query = """
        SELECT
            studentid,
            firstname,
            lastname,
            class,
            age
        FROM students
        WHERE studentid = @id;
        """;

        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        using var reader = command.ExecuteReader();
        if (reader.Read())
        {
            student = new Student();
            student.Id = reader.GetInt32(reader.GetOrdinal("studentid"));
            student.Age = reader.GetInt32((reader.GetOrdinal("age")));
            student.Firstname = reader["firstname"] as string ?? "";
            student.Lastname = reader["lastname"] as string ?? "";
            student.Class = reader["class"] as string ?? "";

            return student;
        }
        else
        {
            return null;
        }
    }

    public void InsertStudent(Student student, string cs)
    {
        using var connection = new MySqlConnection(cs);
        connection.Open();

        const string query = """
        INSERT INTO students (firstname, lastname, class, age)
        VALUES (@firstname, @lastname, @class, @age)
        """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@firstname", student.Firstname);
        command.Parameters.AddWithValue("@lastname", student.Lastname);
        command.Parameters.AddWithValue("@class", student.Class);
        command.Parameters.AddWithValue("@age", student.Age);

        command.ExecuteNonQuery();
    }

    public void UpdateStudent(Student student, string cs)
    {
        using var connection = new MySqlConnection(cs);
        connection.Open();

        const string query = """
        UPDATE students
        SET
            firstname = @firstname,
            lastname = @lastname,
            class = @class,
            age = @age
        WHERE
            studentid = @id;
        """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@firstname", student.Firstname);
        command.Parameters.AddWithValue("@lastname", student.Lastname);
        command.Parameters.AddWithValue("@class", student.Class);
        command.Parameters.AddWithValue("@age", student.Age);
        command.Parameters.AddWithValue("@id", student.Id);

        command.ExecuteNonQuery();

        int affectedRows = command.ExecuteNonQuery();
        if (affectedRows == 0)
        {
            throw new Exception("Student not found.");
        }
    }

    public bool DeleteStudent(int id, string cs)
    {
        using var connection = new MySqlConnection(cs);
        connection.Open();

        const string query = """
        DELETE FROM students
        WHERE
            studentid = @id;
        """;
        using var command = new MySqlCommand(query, connection);
        command.Parameters.AddWithValue("@id", id);

        int affectedRows = command.ExecuteNonQuery();
        if (affectedRows == 1) { return true; }
        return false;
    }
}
