using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechnicalReport_BL.DTO;
using TechnicalReport_BL.Models;
using TechnicalReport_Data.Models.FE;

namespace TechnicalReport_BL.FE.Availability
{
    public interface IDiscrepancyDataSeparator
    {
        List<double> SeparateDataAccordingDiscrepancy(Date dateDTO, List<FE_Context> fE_Contexts);
    }
    public class DiscrepancyDataSeparator : IDiscrepancyDataSeparator
    {
        private readonly ICalculateClass _CalculateClass;
        public DiscrepancyDataSeparator(ICalculateClass CalculateClass)
        {
            _CalculateClass = CalculateClass;
        }

        /// <summary>
        /// This method separate discrepancy group depends on User choice
        /// </summary>
        /// <param name="data"></param>
        /// <param name="fE_Contexts"></param>
        /// <returns></returns>
        public List<double> SeparateDataAccordingDiscrepancy(Date data, List<FE_Context> fE_Contexts)
        {
            

            TimeSpan ts = data.To - data.From;
            TimeSpan Times = TimeSpan.Zero;
            var Result = new List<double>();
            Date ChangedDate = new Date();
            switch (data.DiscrId)
            {
                case 1://For each hour
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddHours(1);
                    for (int i = 0; i < ts.TotalHours; i++)
                    {
                        Times = GetSeparateAvailability(ChangedDate, fE_Contexts);
                        Result.Add(_CalculateClass.CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddHours(1);
                            ChangedDate.To = ChangedDate.To.AddHours(1);
                        }
                    }
                    break;

                case 2://For each shift
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddHours(12);
                    for (int i = 0; i < ts.TotalDays; i++)
                    {
                        Times = GetSeparateAvailability(ChangedDate, fE_Contexts);
                        Result.Add(_CalculateClass.CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddHours(12);
                            ChangedDate.To = ChangedDate.To.AddHours(12);
                        }
                    }
                    break;

                case 3://For each day
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddDays(1);
                    for (int i = 0; i < ts.TotalDays; i++)
                    {
                        if (ChangedDate.To > data.To)
                        {
                            ChangedDate.To = data.To;
                        }
                        Times = GetSeparateAvailability(ChangedDate, fE_Contexts);
                        Result.Add(_CalculateClass.CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddDays(1);
                            ChangedDate.To = ChangedDate.To.AddDays(1);
                        }
                    }
                    break;

                case 4: //for each week
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddDays(7);
                    for (int i = 0; i < ts.TotalDays/7; i++)
                    {
                        Times = GetSeparateAvailability(ChangedDate, fE_Contexts);
                        Result.Add(_CalculateClass.CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddDays(7);
                            ChangedDate.To = ChangedDate.To.AddDays(7);
                        }
                    }
                    break;

                case 5:
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddDays(1);
                    for (int i = 0; i < ts.TotalDays; i++)
                    {
                        //Times = GetSeparateAvailability(ChangedDate, ListFE);
                        //Result.Add(CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddMonths(1);
                            ChangedDate.To = ChangedDate.To.AddMonths(1);
                        }
                    }
                    break;

                case 6:
                    ChangedDate.From = data.From;
                    ChangedDate.To = data.From.AddYears(1);
                    for (int i = 0; i < ts.TotalDays / 7; i++)
                    {
                        if (ChangedDate.To > data.To)
                        {
                            ChangedDate.To = data.To;
                        }
                        //Times = GetSeparateAvailability(ChangedDate, ListFE);
                        //Result.Add(CalculateProcent(Times, ChangedDate));
                        if (ChangedDate.To < data.To)
                        {
                            ChangedDate.From = ChangedDate.From.AddYears(1);
                            ChangedDate.To = ChangedDate.To.AddYears(1);
                        }
                    }
                    break;
            }

            return Result;
        }

/// <summary>
/// This Method makes separating for each time frame. (day, hour, shift, etc...)
/// </summary>
/// <param name="ChangedDate"></param>
/// <param name="fE_Contexts"></param>
/// <returns></returns>
        private TimeSpan GetSeparateAvailability(Date ChangedDate, List<FE_Context> fE_Contexts)
        {
           
            TimeSpan[] Eq = new TimeSpan[26];
            TimeSpan Result = TimeSpan.Zero;

            var ShortListFe=fE_Contexts.Where(x => (x.TS_START < ChangedDate.From && x.TS_END > ChangedDate.To) || (x.TS_END > ChangedDate.From && x.TS_END < ChangedDate.To) || (x.TS_START < ChangedDate.To && x.TS_END > ChangedDate.To)).ToList();
            Eq = _CalculateClass.CalculatingUptime(ShortListFe, ChangedDate);

            for (int j = 0; j < Eq.Length; j++)
            {
                if (Eq[j] != TimeSpan.Zero)
                {
                    Result = Eq[j];
                    break;
                }
                if (j == Eq.Length - 1)
                {
                    Result = TimeSpan.Zero;
                }
            }

            return Result;
        }

    }
}
