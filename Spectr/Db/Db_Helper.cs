using Azure.Identity;
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




    }

}
