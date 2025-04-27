using Microsoft.EntityFrameworkCore;
using Spectr.Data;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Windows;
using Contract = Spectr.Data.Contract;
namespace Spectr.Db
{
    public class Db_Helper
    {
        public ApplicationContext context;
        public ObservableCollection<Contract> contracts;
        public ObservableCollection<Profile> profiles;// для операторов
        public ObservableCollection<Operator> operators;
        public ObservableCollection<Analyst> analysts;
        public ObservableCollection<GammaSpectrometer> gammaSpectrometers;

        public Db_Helper()
        {

            context = new ApplicationContext();

        }

        public void DeleteProject(object project)
        {
            if (project == null) return;

            try
            {
                switch (project)
                {
                    case Contract contract:
                        context.Contracts.Remove(contract);
                        contracts.Remove(contract);
                        break;

                    case Area area:
                        context.Areas.Remove(area);
                        area.Contract?.Areas?.Remove(area);
                        break;

                    case Profile profile:
                        context.Profiles.Remove(profile);
                        profile.Area?.Profiles?.Remove(profile);
                        break;

                    case Picket picket:
                        context.Pickets.Remove(picket);
                        picket.Profile?.Pickets?.Remove(picket);
                        break;

                    case Operator @operator:
                        context.Operators.Remove(@operator);
                        break;

                    case Analyst analyst:
                        context.Analysts.Remove(analyst);
                        break;

                    case AreaCoordinates areaCoordinate:
                        context.AreaCoordinates.Remove(areaCoordinate);
                        break;

                    case ProfileCoordinates profileCoordinate:
                        context.ProfileCoordinates.Remove(profileCoordinate);
                        break;
                    case GammaSpectrometer gammaSpectrometer:
                        context.GammaSpectrometers.Remove(gammaSpectrometer);
                        break;

                    default:
                        MessageBox.Show("Неизвестный тип данных для удаления.", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Warning);
                        return;
                }

                context.SaveChanges();
            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(ex); // вывод полной трассировки в Output
                MessageBox.Show($"Произошла ошибка при удалении:\n{innerMessage}", "Ошибка удаления", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public void DeleteProfileOperator(int profileId, int operatorId)
        {
            var profileOperator = context.ProfileOperator
                .FirstOrDefault(po => po.ProfileID == profileId && po.OperatorID == operatorId);

            if (profileOperator != null)
            {
                context.Remove(profileOperator);
                context.SaveChanges();
            }
        }
        public void DeleteContractAnalyst(int contractId, int analystId)
        {
            var сontractAnalyst = context.ContractAnalyst
                .FirstOrDefault(po => po.ContractID == contractId && po.AnalystID == analystId);

            if (сontractAnalyst != null)
            {
                context.ContractAnalyst.Remove(сontractAnalyst);
                context.SaveChanges();
            }
        }
        public void AddProfileOperator(int profileId, int operatorId)
        {
            try
            {
                // Проверяем, существует ли такой оператор
                bool operatorExists = context.Operators.Any(op => op.OperatorID == operatorId);
                if (!operatorExists)
                {
                    throw new InvalidOperationException($"Оператор с ID = {operatorId} не существует.");
                }

                // Проверяем, существует ли уже связь
                var exists = context.ProfileOperator
                    .Any(po => po.ProfileID == profileId && po.OperatorID == operatorId);

                if (!exists)
                {
                    var profileOperator = new ProfileOperator
                    {
                        ProfileID = profileId,
                        OperatorID = operatorId
                    };

                    context.ProfileOperator.Add(profileOperator);
                    context.SaveChanges();
                }
            }
            catch (DbUpdateException ex)
            {
                // Например, логирование или сообщение пользователю
                MessageBox.Show("Ошибка при сохранении связи профиль-оператор: " + ex.InnerException?.Message);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message);
            }
        }

        public void AddContractAnalyst(int contractId, int analystId)
        {

            var exists = context.ContractAnalyst
                .Any(c => c.ContractID == contractId && c.AnalystID == analystId);

            if (!exists)
            {
                var contractAnalyst = new ContractAnalyst
                {
                    ContractID = contractId,
                    AnalystID = analystId
                };

                context.ContractAnalyst.Add(contractAnalyst);
                context.SaveChanges();
            }
        }



        public void LoadContract()
        {
            contracts = new ObservableCollection<Contract>(
                context.Contracts
                    .Include(c => c.Customer)
                    .Include(c => c.Administrator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.AreaCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.Operator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.GammaSpectrometer)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileOperators) // 👈 Добавили связь с промежуточной таблицей
                                .ThenInclude(po => po.Operator)    // 👈 И подгружаем сами объекты Operator
                    .Include(c => c.ContractAnalysts)
                        .ThenInclude(ca => ca.Analyst)
                    .ToList()
            );

        }
        public void LoadContractsForAnalyst(int analystId)
        {
            contracts = new ObservableCollection<Contract>(
                context.Contracts
                    .Include(c => c.Customer)
                    .Include(c => c.Administrator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.AreaCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.Operator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.GammaSpectrometer)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileOperators)
                                .ThenInclude(po => po.Operator)
                    .Include(c => c.ContractAnalysts)
                        .ThenInclude(ca => ca.Analyst)
                    .Where(c => c.ContractAnalysts.Any(ca => ca.AnalystID == analystId))
                    .ToList()
            );
        }
        public void LoadProfilesForOperator(int operatorId)
        {
            profiles = new ObservableCollection<Profile>(
                context.Profiles
                    .Include(p => p.ProfileCoordinates)
                    .Include(p => p.Pickets)
                        .ThenInclude(pk => pk.GammaSpectrometer)
                    .Include(p => p.Pickets)
                        .ThenInclude(pk => pk.Operator)
                    .Include(p => p.ProfileOperators)
                        .ThenInclude(po => po.Operator)
                    .Include(p => p.Area)
                        .ThenInclude(a => a.Contract)
                            .ThenInclude(c => c.Customer)
                    .Where(p => p.ProfileOperators.Any(po => po.OperatorID == operatorId))
                    .ToList()
            );


        }

        public void LoadContractsForCustomerById(int customerId)
        {
            contracts = new ObservableCollection<Contract>(
                context.Contracts
                    .Include(c => c.Customer)
                    .Include(c => c.Administrator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.AreaCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileCoordinates)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.Operator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.GammaSpectrometer)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileOperators)
                                .ThenInclude(po => po.Operator)
                    .Include(c => c.ContractAnalysts)
                        .ThenInclude(ca => ca.Analyst)
                    .Where(c => c.Customer.CustomerID == customerId)
                    .ToList()
            );
        }


        public void LoadOperators()
        {
            operators = new ObservableCollection<Operator>(
                context.Operators.ToList()
            );

        }
        public void LoadAnalyst()
        {
            analysts = new ObservableCollection<Analyst>(
                context.Analysts.ToList()
            );

        }
        public void LoadSpectrometrs()
        {
            gammaSpectrometers = new ObservableCollection<GammaSpectrometer>(
                context.GammaSpectrometers.ToList()
            );

        }

        public void SaveProject(object project)
        {
            if (project == null) return;

            try
            {
                switch (project)
                {
                    case Contract contract:
                        if (contract.ContractID == 0)
                            context.Contracts.Add(contract);
                        else
                            context.Contracts.Update(contract);
                        break;

                    case Area area:
                        if (area.AreaID == 0)
                            context.Areas.Add(area);
                        else
                            context.Areas.Update(area);
                        break;

                    case Profile profile:
                        if (profile.ProfileID == null)
                        {
                            context.Profiles.Add(profile);
                            Debug.WriteLine(profile.ProfileID);
                            Debug.WriteLine("null");
                        }

                        else
                        {
                            context.Profiles.Update(profile);
                            Debug.WriteLine(profile.ProfileID);
                        }

                        break;

                    case Picket picket:

                        context.Pickets.Update(picket);

                        break;

                    case AreaCoordinates areaCoordinate:
                        if (areaCoordinate.AreaCoordinatesID == 0)
                            context.AreaCoordinates.Add(areaCoordinate);
                        else
                            context.AreaCoordinates.Update(areaCoordinate);
                        break;

                    case ProfileCoordinates profileCoordinate:
                        if (profileCoordinate.ProfileCoordinatesID == 0)
                            context.ProfileCoordinates.Add(profileCoordinate);
                        else
                            context.ProfileCoordinates.Update(profileCoordinate);
                        break;

                    case Analyst analyst:
                        if (analyst.AnalystID == 0)
                            context.Analysts.Add(analyst);
                        else
                            context.Analysts.Update(analyst);
                        break;

                    case Operator _operator:
                        if (_operator.OperatorID == 0)
                        {

                            context.Operators.Add(_operator);
                        }

                        else
                        {

                            context.Operators.Update(_operator);
                        }

                        break;


                }

                context.SaveChanges();

            }
            catch (Exception ex)
            {
                var innerMessage = ex.InnerException?.Message ?? ex.Message;
                Debug.WriteLine(ex); // Показывает полную трассировку
                MessageBox.Show($"Произошла ошибка при сохранении данных:\n{innerMessage}", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }

        }
        Random random = new Random();


        private float GenerateRandomCoordinate(float min, float max)
        {
            Random rand = new Random();
            return (float)(rand.NextDouble() * (max - min) + min);
        }
        
        public async Task SeedDatabaseAsync(ApplicationContext context)
        {
            // Очищаем базу
            context.ProfileOperator.RemoveRange(context.ProfileOperator);
            context.Pickets.RemoveRange(context.Pickets);
            context.ProfileCoordinates.RemoveRange(context.ProfileCoordinates);
            context.Profiles.RemoveRange(context.Profiles);
            context.AreaCoordinates.RemoveRange(context.AreaCoordinates);
            context.Areas.RemoveRange(context.Areas);
            context.ContractAnalyst.RemoveRange(context.ContractAnalyst);
            context.GammaSpectrometers.RemoveRange(context.GammaSpectrometers);
            context.Contracts.RemoveRange(context.Contracts);
            context.Customers.RemoveRange(context.Customers);
            context.Operators.RemoveRange(context.Operators);
            context.Analysts.RemoveRange(context.Analysts);
            context.Administrators.RemoveRange(context.Administrators);

            await context.SaveChangesAsync();

            // Создаем операторов
            var op1 = new Operator { FullName = "Дмитрий Кузнецо Дмитриевич", PhoneNumber = "+79139828461", Email = "d.kuznetsov@work.ru", JobTitle = "Инженер-радиолог", OperatorLogin = "operator_dmitry", OperatorPassword = "op_dmitry123" };
            var op2 = new Operator { FullName = "Ольга Петрова Ивановна", PhoneNumber = "+79163334455", Email = "o.petrova@work.ru", JobTitle = "Эколог", OperatorLogin = "operator_olga", OperatorPassword = "op_olga123" };
            var op3 = new Operator { FullName = "Лесников Артем Алексеевич", PhoneNumber = "+891376523929", Email = "l.artem@work.ru", JobTitle = "Инженер-радиолог", OperatorLogin = "Artemka", OperatorPassword = "12" };
            var op4 = new Operator { FullName = "Всеволод Осипкин Чебупелькин", PhoneNumber = "+7916231212", Email = "o.osipkin@work.ru", JobTitle = "Эколог", OperatorLogin = "SEVA", OperatorPassword = "1" };

            context.Operators.AddRange(op1, op2, op3, op4);
            await context.SaveChangesAsync();
            var operators = new List<Operator> { op1, op2, op3, op4 };

            // Спектрометры
            var spectrometers = new List<GammaSpectrometer>
{
    new GammaSpectrometer { CommissioningDate = DateTime.Parse("2023-06-10"), MeasurementAccuracy = 0.98f, MeasurementTime = 120 },
    new GammaSpectrometer { CommissioningDate = DateTime.Parse("2024-01-12"), MeasurementAccuracy = 0.58f, MeasurementTime = 130 },
    new GammaSpectrometer { CommissioningDate = DateTime.Parse("2023-03-13"), MeasurementAccuracy = 2.98f, MeasurementTime = 110 },
    new GammaSpectrometer { CommissioningDate = DateTime.Parse("2023-02-14"), MeasurementAccuracy = 1.98f, MeasurementTime = 140 },
    new GammaSpectrometer { CommissioningDate = DateTime.Parse("2022-08-20"), MeasurementAccuracy = 0.95f, MeasurementTime = 150 }
};
            context.GammaSpectrometers.AddRange(spectrometers);
            await context.SaveChangesAsync();

            // Админы и Клиенты
            var admin1 = new Administrator { FullName = "Ivan Petrov", PhoneNumber = "+79991234567", Email = "ivan.petrov@admin.com", JobTitle = "Главный администратор", AdministratorLogin = "admin_ivan", AdministratorPassword = "pass_ivan123" };
            var admin2 = new Administrator { FullName = "Elena Sokolova", PhoneNumber = "+79997654321", Email = "elena.sokolova@admin.com", JobTitle = "Менеджер", AdministratorLogin = "admin_elena", AdministratorPassword = "pass_elena123" };
            context.Administrators.AddRange(admin1, admin2);


            var customer1 = new Customer { CompanyName = "ООО \"ТехноСервис\"", PhoneNumber = "+79261234567", ContactPerson = "Андрей Смирнов", Email = "andrey@technoservice.ru", Address = "Москва, ул. Ленина, 10", CustomerLogin = "techservice", CustomerPassword = "tech123" };
            var customer2 = new Customer { CompanyName = "АО \"ЭкоМониторинг\"", PhoneNumber = "+79371234568", ContactPerson = "Мария Иванова", Email = "maria@ecomonitor.ru", Address = "Санкт-Петербург, ул. Пушкина, 5", CustomerLogin = "ecomonitor", CustomerPassword = "eco123" };
            context.Customers.AddRange(customer1, customer2);
            await context.SaveChangesAsync();

            // Контракты
            var contract1 = new Contract { StartDate = DateTime.Parse("2024-05-11"), EndDate = DateTime.Parse("2025-01-21"), ServiceDescription = "Мониторинг радиационной безопасности", CustomerID = customer1.CustomerID, AdministratorID = admin1.AdministratorID };
            var contract2 = new Contract { StartDate = DateTime.Parse("2024-06-11"), EndDate = DateTime.Parse("2025-03-15"), ServiceDescription = "Мониторинг уровня радиации", CustomerID = customer1.CustomerID, AdministratorID = admin1.AdministratorID };
            var contract3 = new Contract { StartDate = DateTime.Parse("2024-03-11"), EndDate = DateTime.Parse("2025-02-01"), ServiceDescription = "Мониторинг чистоты воздуха", CustomerID = customer2.CustomerID, AdministratorID = admin1.AdministratorID };
            var contract4 = new Contract { StartDate = DateTime.Parse("2024-02-15"), EndDate = DateTime.Parse("2025-02-18"), ServiceDescription = "Экологический аудит", CustomerID = customer2.CustomerID, AdministratorID = admin1.AdministratorID };
            context.Contracts.AddRange(contract1, contract2, contract3, contract4);
            await context.SaveChangesAsync();

            // Зоны
            var areas = new List<Area>()
            {
                new Area { AreaName = "Зона Север", ContractID = contract1.ContractID },
                new Area { AreaName = "Зона Запад", ContractID = contract1.ContractID },
                new Area { AreaName = "Зона Северо-Восток", ContractID = contract1.ContractID },
                new Area { AreaName = "Зона Восток", ContractID = contract2.ContractID },
                new Area { AreaName = "Зона Юг", ContractID = contract2.ContractID },
                new Area { AreaName = "Зона НижнеВартовск", ContractID = contract2.ContractID },
                new Area { AreaName = "Зона Плоскогорья", ContractID = contract3.ContractID },
                new Area { AreaName = "Зона Отчуждения", ContractID = contract4.ContractID }
            };
            context.Areas.AddRange(areas);
            await context.SaveChangesAsync();

            Random rand = new Random();

            // Генерация координат для Зон
            foreach (var area in areas)
            {
                var minX = rand.Next(0, 100);
                var minY = rand.Next(0, 100);
                var width = rand.Next(20, 50);
                var height = rand.Next(20, 50);

                context.AreaCoordinates.AddRange(new List<AreaCoordinates>
            {
                new AreaCoordinates { X = minX,         Y = minY,          AreaID = area.AreaID },
                new AreaCoordinates { X = minX + width, Y = minY,          AreaID = area.AreaID },
                new AreaCoordinates { X = minX + width, Y = minY + height, AreaID = area.AreaID },
                new AreaCoordinates { X = minX,         Y = minY + height, AreaID = area.AreaID },
                new AreaCoordinates { X = minX,         Y = minY,          AreaID = area.AreaID } // замыкаем контур
            });
            }
            await context.SaveChangesAsync();


            // Профили внутри Зон
            var profiles = new List<Profile>();
            foreach (var area in areas)
            {
                int profileCount = rand.Next(1, 3);
                for (int i = 0; i < profileCount; i++)
                {
                    var profile = new Profile
                    {
                        ProfileName = (i % 2 == 0) ? "Радиационный контроль" : "Экологический анализ",
                        ProfileType = (i % 2 == 0) ? "Гамма-спектрометрия" : "Почвенные исследования",
                        AreaID = area.AreaID
                    };
                    profiles.Add(profile);
                }
            }
            await context.Profiles.AddRangeAsync(profiles);
            await context.SaveChangesAsync();

            var savedProfiles = await context.Profiles.ToListAsync();
            var savedAreas = await context.Areas.Include(a => a.AreaCoordinates).ToListAsync();

            // Генерация координат профилей внутри зоны
            foreach (var profile in savedProfiles)
            {
                var area = savedAreas.FirstOrDefault(a => a.AreaID == profile.AreaID);

                if (area != null && area.AreaCoordinates.Count >= 2)
                {
                    var minX = area.AreaCoordinates.Min(c => c.X);
                    var maxX = area.AreaCoordinates.Max(c => c.X);
                    var minY = area.AreaCoordinates.Min(c => c.Y);
                    var maxY = area.AreaCoordinates.Max(c => c.Y);

                    // Сгенерируем 5 случайных координат для профиля
                    var profileCoordinates = new List<ProfileCoordinates>();
                    for (int i = 0; i < 5; i++)
                    {
                        profileCoordinates.Add(new ProfileCoordinates
                        {
                            X = GenerateRandomCoordinate(minX, maxX),
                            Y = GenerateRandomCoordinate(minY, maxY),
                            ProfileID = profile.ProfileID
                        });
                    }

                    // Замкнем контур
                    profileCoordinates.Add(new ProfileCoordinates
                    {
                        X = profileCoordinates[0].X,  // Последняя координата будет совпадать с первой
                        Y = profileCoordinates[0].Y,
                        ProfileID = profile.ProfileID
                    });

                    // Добавим все координаты в контекст
                    context.ProfileCoordinates.AddRange(profileCoordinates);
                }
            }

            await context.SaveChangesAsync();



             bool IsPointInPolygon(float pointX, float pointY, List<ProfileCoordinates> polygon)
        {
            int i, j = polygon.Count - 1;
            bool inside = false;

            for (i = 0; i < polygon.Count; j = i++)
            {
                var xi = polygon[i].X;
                var yi = polygon[i].Y;
                var xj = polygon[j].X;
                var yj = polygon[j].Y;

                var intersect = ((yi > pointY) != (yj > pointY)) &&
                                (pointX < (xj - xi) * (pointY - yi) / (yj - yi) + xi);
                if (intersect)
                    inside = !inside;
            }

            return inside;
        }
            var profileCoords = await context.ProfileCoordinates.ToListAsync();
            var pickets = new List<Picket>();
            int operatorIndex = 0;

            // Изменим тип на float, чтобы соответствовать типу данных в базе
            float picketX, picketY;

            foreach (var profile in savedProfiles)
            {
                var coords = profileCoords.Where(p => p.ProfileID == profile.ProfileID).ToList();

                if (coords.Count >= 3)  // Убедимся, что у нас есть хотя бы 3 точки для формирования многоугольника
                {
                    var picketCount = rand.Next(3, 6);

                    for (int i = 0; i < picketCount; i++)
                    {
                        var currentOp = operators[operatorIndex % operators.Count];
                        operatorIndex++;

                        // Ищем подходящие координаты внутри многоугольника
                        do
                        {
                            picketX = GenerateRandomCoordinate(coords.Min(c => c.X), coords.Max(c => c.X));
                            picketY = GenerateRandomCoordinate(coords.Min(c => c.Y), coords.Max(c => c.Y));
                        }
                        while (!IsPointInPolygon(picketX, picketY, coords));

                        pickets.Add(new Picket
                        {
                            CoordinateX = picketX,
                            CoordinateY = picketY,
                            Channel1 = GenerateRandomCoordinate(0, 100),
                            Channel2 = GenerateRandomCoordinate(0, 100),
                            Channel3 = GenerateRandomCoordinate(0, 100),
                            ProfileID = profile.ProfileID,
                            OperatorID = currentOp.OperatorID,
                            SpectrometerID = spectrometers[0].GammaSpectrometerID
                        });
                    }
                }
            }




            await context.Pickets.AddRangeAsync(pickets);
        await context.SaveChangesAsync();


            // Генерация Аналитиков и привязки к контрактам
            var analysts = new List<Analyst>
{
    new Analyst { FullName = "Сергей Николаев", PhoneNumber = "+79112223344", Email = "sergey.nikolaev@analysis.ru", JobTitle = "Аналитик данных", AnalystLogin = "analyst_sergey", AnalystPassword = "analyst_pass1" },
    new Analyst { FullName = "Анна Козлова", PhoneNumber = "+79114445566", Email = "anna.kozlova@analysis.ru", JobTitle = "Ведущий аналитик", AnalystLogin = "analyst_anna", AnalystPassword = "analyst_pass2" },
    new Analyst { FullName = "Игорь Смирнов", PhoneNumber = "+79005556677", Email = "igor.smirnov@analysis.ru", JobTitle = "Младший аналитик", AnalystLogin = "analyst_igor", AnalystPassword = "analyst_pass3" }
};
            await context.Analysts.AddRangeAsync(analysts);
            await context.SaveChangesAsync();

            var contractAnalysts = new List<ContractAnalyst>
{
    new ContractAnalyst { ContractID = contract1.ContractID, AnalystID = analysts[0].AnalystID },
    new ContractAnalyst { ContractID = contract1.ContractID, AnalystID = analysts[1].AnalystID },
    new ContractAnalyst { ContractID = contract2.ContractID, AnalystID = analysts[1].AnalystID },
    new ContractAnalyst { ContractID = contract3.ContractID, AnalystID = analysts[0].AnalystID },
    new ContractAnalyst { ContractID = contract4.ContractID, AnalystID = analysts[1].AnalystID },
    new ContractAnalyst { ContractID = contract4.ContractID, AnalystID = analysts[0].AnalystID },
    new ContractAnalyst { ContractID = contract1.ContractID, AnalystID = analysts[2].AnalystID }
};
            await context.ContractAnalyst.AddRangeAsync(contractAnalysts);
            await context.SaveChangesAsync();

            // Привязка Профилей к Операторам через Пикеты
            var allPickets = await context.Pickets
                .Select(p => new { p.ProfileID, p.OperatorID })
                .ToListAsync();

            var profileOperatorLinks = allPickets
                .GroupBy(p => p.ProfileID)
                .SelectMany(g => g.Select(p => p.OperatorID).Distinct()
                    .Select(operatorId => new ProfileOperator
                    {
                        ProfileID = g.Key,
                        OperatorID = operatorId
                    }))
                .ToList();

            await context.ProfileOperator.AddRangeAsync(profileOperatorLinks);
            await context.SaveChangesAsync();
        }








    }

}
