using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Abstractions;
using Spectr.Data;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System;
using System.Diagnostics;
using System.Diagnostics.Contracts;
using Contract = Spectr.Data.Contract;
using System.Windows;
namespace Spectr.Db
{
    public class Db_Helper
    {
        public ApplicationContext context;
        public ObservableCollection<Contract> contracts;
        public ObservableCollection<Operator> operators;
        public Db_Helper() 
        {
          
            context = new ApplicationContext();
           
        }
        public void DeleteProject(object project)
        {
            if (project == null) return;

     

            switch (project)
            {
                case Contract contract:
                    context.Contracts.Remove(contract);
                    contracts.Remove(contract);
                    break;

                case Area area:
                    context.Areas.Remove(area);

                    area.Contract.Areas.Remove(area);

                    break;
                    
                case Profile profile: 
                    context.Profiles.Remove(profile); 
                    profile.Area.Profiles.Remove(profile);
                    break;

                case Picket picket:
                    context.Pickets.Remove(picket);
                    picket.Profile.Pickets.Remove(picket);
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
                    MessageBox.Show("Неизвестный тип данных для удаления.");
                    break;

            }
            context.SaveChanges();

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
        public void AddProfileOperator(int profileId, int operatorId)
        {
    
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
        public void LoadOperators()
        {
            operators = new ObservableCollection<Operator>(
                context.Operators.ToList()
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
                        if (profile.ProfileID == 0)
                            context.Profiles.Add(profile);
                        else
                            context.Profiles.Update(profile);
                        break;

                    case Picket picket:
                        if (picket.PicketID == 0)
                            context.Pickets.Add(picket);
                        else
                            context.Pickets.Update(picket);

                        if (picket.GammaSpectrometer != null)
                        {
                            if (picket.GammaSpectrometer.GammaSpectrometerID == 0)
                                context.GammaSpectrometers.Add(picket.GammaSpectrometer);
                            else
                                context.GammaSpectrometers.Update(picket.GammaSpectrometer);
                        }
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
                    case ProfileOperator profileOperator:
                        context.ProfileOperator.Add(profileOperator);

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
