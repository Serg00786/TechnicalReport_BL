using AutoMapper;
using System.Collections.Generic;
using System.Threading.Tasks;
using TechnicalReport_BL.DTO;
using TechnicalReport_BL.Models;
using TechnicalReport_Data.DTO;
using TechnicalReport_Data.FE.Availability;
using TechnicalReport_Data.Models.FE;
using TechnicalReport_Data.ProcDown;

namespace TechnicalReport_BL.FE.Availability
{
    
    interface IMainFEAvailability
    {
        Task<List<double>> GetFeAvailabilityReportData(DateDTO dateDTO);
    }

    /// <summary>
    /// Organization class for Data distrubution for FE Availability report
    /// It gets data from UI and from DB, Send for calculation
    /// </summary>
   
    public class MainFEAvailability : IMainFEAvailability
    {
        private readonly CalculateClass CalculateClass= new CalculateClass();
        private readonly IFERepository _repositary;
        private readonly IProcDownRepository _procdownRepositary;
        public MainFEAvailability(IFERepository FeRepositary, IProcDownRepository procDownRepository)
        {
            _repositary = FeRepositary;
            _procdownRepositary = procDownRepository;
        }

        public async Task <List<double>> GetFeAvailabilityReportData(DateDTO dateDTO)
        {
            var dtDTO = Mapping.Mapping.Mapper.Map<DatebaseDTO>(dateDTO);
            var dataMapped = Mapping.Mapping.Mapper.Map<Date>(dateDTO);

            var fE_Contexts = await _repositary.GetFeAvailabilityData(dtDTO);
            var Fe_procdown = await _procdownRepositary.GetProcDownListByTimePeriod(dtDTO);

            IProcessDowntimesCount_Service processDowntimesCount = new ProcessDowntimesCount_Service();
            var FEListWithoutProcessDowntimes= processDowntimesCount.GetProcessDown(fE_Contexts, Fe_procdown, dataMapped);
            
            IDiscrepancyDataSeparator discrepancyDataSeparator = new DiscrepancyDataSeparator(CalculateClass);
            var FinalFeAvailability = discrepancyDataSeparator.SeparateDataAccordingDiscrepancy(dataMapped, FEListWithoutProcessDowntimes);
            return FinalFeAvailability;
        }

    }
}
