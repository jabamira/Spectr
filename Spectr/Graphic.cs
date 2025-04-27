using OxyPlot.Series;
using OxyPlot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using Spectr.Data;

namespace Spectr
{
    public static class Graphic
    {
        public static PlotModel GenerateSchem(Contract contract)
        {
             
         
            if (contract == null)
                return null;

       

            var plotModel = new PlotModel { Title = "Схема Контракта" };

            // 1. Рисуем ВСЕ Areas
            foreach (var area in contract.Areas)
            {
                if (area.AreaCoordinates != null && area.AreaCoordinates.Count > 0)
                {
                    var areaSeries = new LineSeries
                    {
                        Title = $"Area {area.AreaName}",
                        Color = OxyColors.Green,
                        StrokeThickness = 2
                    };

                    foreach (var coord in area.AreaCoordinates)
                    {
                        areaSeries.Points.Add(new DataPoint(coord.X, coord.Y));
                    }

                    // Замыкаем контур
                    var first = area.AreaCoordinates.First();
                    areaSeries.Points.Add(new DataPoint(first.X, first.Y));

                    plotModel.Series.Add(areaSeries);
                }

                // 2. Рисуем ВСЕ Profiles внутри каждой Area
                if (area.Profiles != null)
                {
                    foreach (var profile in area.Profiles)
                    {
                        if (profile.ProfileCoordinates != null && profile.ProfileCoordinates.Count > 0)
                        {
                            var profileSeries = new LineSeries
                            {
                                Title = $"Profile {profile.ProfileName}",
                                Color = OxyColors.Orange,
                                LineStyle = LineStyle.Dash,
                                StrokeThickness = 1.5
                            };

                            foreach (var coord in profile.ProfileCoordinates)
                            {
                                profileSeries.Points.Add(new DataPoint(coord.X, coord.Y));
                            }

                            // Замыкаем профиль
                            var first = profile.ProfileCoordinates.First();
                            profileSeries.Points.Add(new DataPoint(first.X, first.Y));

                            plotModel.Series.Add(profileSeries);
                        }

                        // 3. Рисуем ВСЕ Pickets внутри каждого Profile
                        if (profile.Pickets != null)
                        {
                            var picketSeries = new ScatterSeries
                            {
                                Title = $"Pickets {profile.ProfileName}",
                                MarkerType = MarkerType.Circle,
                                MarkerFill = OxyColors.Blue,
                                MarkerSize = 3
                            };

                            foreach (var picket in profile.Pickets)
                            {
                                picketSeries.Points.Add(new ScatterPoint(picket.CoordinateX, picket.CoordinateY));
                            }

                            plotModel.Series.Add(picketSeries);

                        }
                       
                    }
                }
            }

            return plotModel;


        }
    }
    
   
}
