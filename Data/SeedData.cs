using MvdBackend.Models;
using BCrypt.Net;

namespace MvdBackend.Data
{
    public static class SeedData
    {
        public static void Initialize(AppDbContext context)
        {
            // Статусы 
            if (!context.RequestStatuses.Any())
            {
                context.RequestStatuses.AddRange(
                    new RequestStatus { Name = "Новое" },
                    new RequestStatus { Name = "В работе" },
                    new RequestStatus { Name = "На проверке" },
                    new RequestStatus { Name = "Выполнено" },
                    new RequestStatus { Name = "Отклонено" }
                );
                context.SaveChanges();
            }

            // Типы обращений 
            if (!context.RequestTypes.Any())
            {
                context.RequestTypes.AddRange(
                    new RequestType { Name = "Заявление о преступлении" },
                    new RequestType { Name = "Жалоба на действия сотрудников" },
                    new RequestType { Name = "Консультация по законодательству" },
                    new RequestType { Name = "Запрос информации" },
                    new RequestType { Name = "Обращение по ПДД" }
                );
                context.SaveChanges();
            }

            // Категории
            if (!context.Categories.Any())
            {
                context.Categories.AddRange(
                    new Category { Name = "Имущественные преступления", Description = "Кражи, грабежи, мошенничество, вымогательство, повреждение имущества" },
                    new Category { Name = "Транспорт и ПДД", Description = "ДТП, нарушение ПДД, опасное вождение, неправильная парковка" },
                    new Category { Name = "Общественный порядок", Description = "Хулиганство, драки, нарушение тишины, распитие алкоголя" },
                    new Category { Name = "Бытовые конфликты", Description = "Конфликты с соседями, шум, коммунальные проблемы" },
                    new Category { Name = "Угрозы и безопасность", Description = "Угрозы жизни, нападения, преследование, вымогательство" },
                    new Category { Name = "Киберпреступления", Description = "Интернет-мошенничество, взломы, кибербуллинг" },
                    new Category { Name = "Наркотики", Description = "Распространение, употребление наркотических веществ" },
                    new Category { Name = "Экология и животные", Description = "Свалки, загрязнение, жестокое обращение с животными" },
                    new Category { Name = "Пропавшие люди", Description = "Поиск пропавших без вести людей" },
                    new Category { Name = "Другое", Description = "Иные обращения, не вошедшие в категории" }
                );
                context.SaveChanges();
            }

            // Граждане
            if (!context.Citizens.Any())
            {
                context.Citizens.AddRange(
                    new Citizen { LastName = "Иванов", FirstName = "Иван", Patronymic = "Иванович", Phone = "+79131112233" },
                    new Citizen { LastName = "Петров", FirstName = "Петр", Patronymic = "Петрович", Phone = "+79132223344" },
                    new Citizen { LastName = "Сидорова", FirstName = "Анна", Patronymic = "Владимировна", Phone = "+79133334455" }
                );
                context.SaveChanges();
            }
            // Роли
            if (!context.Roles.Any())
            {
                context.Roles.AddRange(
                    new Role { Name = "Operator" },
                    new Role { Name = "Manager" },
                    new Role { Name = "Admin" }
                );
                context.SaveChanges();
            }
            // Сотрудники
            if (!context.Employees.Any())
            {
                context.Employees.AddRange(
                    new Employee { LastName = "Козлов", FirstName = "Александр", Patronymic = "Сергеевич" },
                    new Employee { LastName = "Никитина", FirstName = "Елена", Patronymic = "Дмитриевна" },
                    new Employee { LastName = "Федоров", FirstName = "Максим", Patronymic = "Игоревич" }
                );
                context.SaveChanges();
            }
            // Пользователи (учетные записи)
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
            // Районы
            if (!context.Districts.Any())
            {
                context.Districts.AddRange(
                    new District { Name = "Центральный", Description = "Центральный район" },
                    new District { Name = "Железнодорожный", Description = "Железнодорожный район" },
                    new District { Name = "Заельцовский", Description = "Заельцовский район" },
                    new District { Name = "Калининский", Description = "Калининский район" },
                    new District { Name = "Кировский", Description = "Кировский район" },
                    new District { Name = "Ленинский", Description = "Ленинский район" },
                    new District { Name = "Октябрьский", Description = "Октябрьский район" },
                    new District { Name = "Первомайский", Description = "Первомайский район" },
                    new District { Name = "Советский", Description = "Советский район" },
                    new District { Name = "Дзержинский", Description = "Дзержинский район" }
                );
                context.SaveChanges();
            }
           

        }
    }
}