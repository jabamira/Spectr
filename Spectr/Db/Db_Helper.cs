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
        public void LoadContract()
        {
            contracts = new ObservableCollection<Contract>(
                context.Contracts
                    .Include(c => c.Customer)
                    
                    .Include(c => c.Administrator)
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.AreaCoordinates) // Включаем координаты зон
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.ProfileCoordinates) // Включаем координаты профилей
                    .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                                .ThenInclude(pk => pk.Operator)
                      .Include(c => c.Areas)
                        .ThenInclude(a => a.Profiles)
                            .ThenInclude(p => p.Pickets)
                            .ThenInclude(pk => pk.GammaSpectrometer)
                    .Include(c => c.ContractAnalysts) // Включаем связи контракт-аналитик
                        .ThenInclude(ca => ca.Analyst) // Включаем сами объекты аналитиков
                    .ToList() // ✅ Закрываем `ToList()` перед закрытием скобки
            );
        }
        public void SaveProject(object project) 
        {
            if (project == null) return;


            Debug.WriteLine(project.GetType());
            switch (project)
            {
                case Contract contract:
                    context.Contracts.Update(contract);
                    Debug.WriteLine( contract.StartDate);
                    context.SaveChanges();
 
                    break;

                case Area area:
                    context.Areas.Update(area);
                    context.SaveChanges();

                    break;

                case Profile profile:
                    context.Profiles.Update(profile);
                    context.SaveChanges();
   
                    break;

                case Picket picket:
                    context.Pickets.Update(picket);
                    context.GammaSpectrometers.Update(picket.GammaSpectrometer);
                    context.SaveChanges();
     
                    break;
                case AreaCoordinates areaCoordinate:
                    context.AreaCoordinates.Update(areaCoordinate);
                    context.SaveChanges();
                    break;
                case ProfileCoordinates profileCoordinate:
                    context.ProfileCoordinates.Update(profileCoordinate);
                    context.SaveChanges();
                    break;
                case Analyst analyst:
                    context.Analysts.Update(analyst);
                    context.SaveChanges();
                    break;
                case Operator _operator:
                    context.Operators.Update(_operator);
                    context.SaveChanges();
                    break;
               
            }
          


        }



    }

}
