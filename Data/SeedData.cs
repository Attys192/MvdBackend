using MvdBackend.Models;
using BCrypt.Net;

namespace MvdBackend.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // ������� 
            if (!context.RequestStatuses.Any())
            {
                context.RequestStatuses.AddRange(
                    new RequestStatus { Name = "�����" },
                    new RequestStatus { Name = "� ������" },
                    new RequestStatus { Name = "�� ��������" },
                    new RequestStatus { Name = "���������" },
                    new RequestStatus { Name = "���������" }
                );
                context.SaveChanges();
            }

            // ���� ��������� 
            if (!context.RequestTypes.Any())
            {
                context.RequestTypes.AddRange(
                    new RequestType { Name = "��������� � ������������" },
                    new RequestType { Name = "������ �� �������� �����������" },
                    new RequestType { Name = "������������ �� ����������������" },
                    new RequestType { Name = "������ ����������" },
                    new RequestType { Name = "��������� �� ���" }
                );
                context.SaveChanges();
            }

            // ���������
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "������������� ������������", Description = "�����, �������, �������������, ��������������, ����������� ���������" },
                    new Category { Name = "��������� � ���", Description = "���, ��������� ���, ������� ��������, ������������ ��������" },
                    new Category { Name = "������������ �������", Description = "�����������, �����, ��������� ������, �������� ��������" },
                    new Category { Name = "������� ���������", Description = "��������� � ��������, ���, ������������ ��������" },
                    new Category { Name = "������ � ������������", Description = "������ �����, ���������, �������������, ��������������" },
                    new Category { Name = "�����������������", Description = "��������-�������������, ������, ������������" },
                    new Category { Name = "���������", Description = "���������������, ������������ ������������� �������" },
                    new Category { Name = "�������� � ��������", Description = "������, �����������, �������� ��������� � ���������" },
                    new Category { Name = "��������� ����", Description = "����� ��������� ��� ����� �����" },
                    new Category { Name = "������", Description = "���� ���������, �� �������� � ���������" }
                );
                context.SaveChanges();
            }

            // ��������
            if (!context.Citizens.Any())
            {
                context.Citizens.AddRange(
                    new Citizen { LastName = "������", FirstName = "����", Patronymic = "��������", Phone = "+79131112233" },
                    new Citizen { LastName = "������", FirstName = "����", Patronymic = "��������", Phone = "+79132223344" },
                    new Citizen { LastName = "��������", FirstName = "����", Patronymic = "������������", Phone = "+79133334455" }
                );
                context.SaveChanges();
            }
            // ����
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Operator" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Admin" }
                );
                context.SaveChanges();
            }
            // ����������
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee { LastName = "������", FirstName = "���������", Patronymic = "���������" },
                    new Employee { LastName = "��������", FirstName = "�����", Patronymic = "����������" },
                    new Employee { LastName = "�������", FirstName = "������", Patronymic = "��������" }
                );
                context.SaveChanges();
            }
            // ������������ (������� ������)
            if (!context.Users.Any())
            {
                var users = new[]
                {
            new User { Username = "kozlova", PasswordHash = BCrypt.Net.BCrypt.HashPassword("operator123"), EmployeeId = 1, RoleId = 1 },
            new User { Username = "nikitina", PasswordHash = BCrypt.Net.BCrypt.HashPassword("manager123"), EmployeeId = 2, RoleId = 2 },
            new User { Username = "fedorov", PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"), EmployeeId = 3, RoleId = 3 }
        };
                context.Users.AddRange(users);
                context.SaveChanges();
            }
            // ������
            if (!context.Districts.Any())
            {
                context.Districts.AddRange(
                    new District { Name = "�����������", Description = "����������� �����" },
                    new District { Name = "���������������", Description = "��������������� �����" },
                    new District { Name = "������������", Description = "������������ �����" },
                    new District { Name = "�����������", Description = "����������� �����" },
                    new District { Name = "���������", Description = "��������� �����" },
                    new District { Name = "���������", Description = "��������� �����" },
                    new District { Name = "�����������", Description = "����������� �����" },
                    new District { Name = "������������", Description = "������������ �����" },
                    new District { Name = "���������", Description = "��������� �����" },
                    new District { Name = "�����������", Description = "����������� �����" }
                );
                context.SaveChanges();
            }
           

        }
    }
}