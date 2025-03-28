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

namespace Spectr.Db
{
    public class Db_Helper
    {
        ApplicationContext context;
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
                    break;

                case Profile profile:
                    context.Profiles.Remove(profile);
                    break;

                case Picket picket:
                    context.Pickets.Remove(picket);
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
                            .ThenInclude(p => p.Pickets) // Включаем пикеты
                                .ThenInclude(pk => pk.Operator)
                    .Include(c => c.ContractAnalysts) // Включаем связи контракт-аналитик
                        .ThenInclude(ca => ca.Analyst) // Включаем сами объекты аналитиков
                    .ToList() // ✅ Закрываем `ToList()` перед закрытием скобки
            );
        }
        public void SaveProject(object project) 
        {
            if (project == null) return;



            switch (project)
            {
                case Contract contract:
                    context.Contracts.Update(contract);
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
                    context.SaveChanges();
                    break;
            }
          


        }



    }

}
