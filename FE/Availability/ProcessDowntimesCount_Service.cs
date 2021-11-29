using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalReport_BL.DTO;
using TechnicalReport_BL.Models;
using TechnicalReport_BL.Models.FE;
using TechnicalReport_Data;
using TechnicalReport_Data.Models.FE;

namespace TechnicalReport_BL.FE.Availability
{
    public interface IProcessDowntimesCount_Service
    {
        List<FE_Context> GetProcessDown(List<FE_Context> fe, List<ProcessDowntimes> pdtv, Date data);
        List<int> GetEquipList(int eq);
    }
    public class ProcessDowntimesCount_Service : IProcessDowntimesCount_Service
    {

        /// <summary>
        /// This method compares, removes and renames all events in FE list which happen during Process Downtime
        /// </summary>
        /// <param name="fe"> This list consist of all events which happens during requested date in list Date</param>
        /// <param name="pdtv">This List consist all ProcessDowntime</param>
        /// <param name="data">This List consist Userdata </param>
        /// <returns></returns>
        public List<FE_Context> GetProcessDown(List<FE_Context> fe, List<ProcessDowntimes> pdtv, Date data)
        {
            
            DateTime wrdate = new DateTime(1900, 1, 1);

            for (int i = 0; i < fe.Count; i++)
            {
                if (fe[i].TS_END == wrdate)         //If event its happenning now. In database TS_END is has date "1/1/1900"
                {                                   // In this for cycle we just rename this date to currend DateTime.Now
                    fe[i].TS_END = DateTime.Now;
                }
            }

            for (int j = 0; j < pdtv.Count; j++)
            {
                List<int> s = GetEquipList(pdtv[j].Eq_id);  //Get EQ_Id list of all machine related to Item For example PVD+PVD Loader+PVD Unloader

                for (int k = 0; k < s.Count; k++)
                {
                    //Filter Ids which happens during ProcessDowntime 
                    pdtv[j].Eq_id = s[k];                              
                    var dat = fe.Where(x => x.EQIDENT == pdtv[j].Eq_id).Where(x => x.TS_START >= data.From && x.TS_END < data.To).ToList();
                    dat = dat.Where(x => x.EQIDENT == pdtv[j].Eq_id).Where(x => x.TS_START >= pdtv[j].from_dt && x.TS_END < pdtv[j].to_dt).ToList();


                    //Delete all objects from main List
                    for (int i = 0; i < dat.Count; i++)
                    {
                        int fe_index = fe.FindIndex(a => a.UNIQUEID == dat[i].UNIQUEID);
                        fe.RemoveAt(fe_index);
                    }

                    dat = fe.Where(x => x.EQIDENT == pdtv[j].Eq_id).Where(x => (x.TS_START >= pdtv[j].from_dt && x.TS_START < pdtv[j].to_dt) || (x.TS_END >= pdtv[j].from_dt && x.TS_END < pdtv[j].to_dt) || (x.TS_START < pdtv[j].from_dt && x.TS_END > pdtv[j].to_dt)).ToList();

                    for (int i = 0; i < dat.Count; i++)
                    {
                        if (dat[i].TS_START < data.From && dat[i].TS_END > data.To)     
                        {
                            //If event start before that requesting date and end after requesting date we rename TS_END and Create new TS-Start
                            DateTime tmp = dat[i].TS_END;
                            fe.Where(x => x.UNIQUEID == dat[i].UNIQUEID).ToList().ForEach(s => s.TS_END = data.From);
                            fe.Insert(0, new FE_Context() { UNIQUEID = dat[i].UNIQUEID, EQIDENT = dat[i].EQIDENT, STATEIDENT = dat[i].STATEIDENT, TS_START = data.To, TS_END = tmp });
                        }

                        if (dat[i].TS_START > pdtv[j].from_dt && dat[i].TS_END > pdtv[j].to_dt)
                        {
                            //If TS_END out of Process Downtime date we changing TS_Start event to new date.
                            fe.Where(x => x.UNIQUEID == dat[i].UNIQUEID).ToList().ForEach(s => s.TS_START = pdtv[j].to_dt);
                        }

                        if (dat[i].TS_END < pdtv[j].to_dt && dat[i].TS_START < pdtv[j].from_dt)     
                        {
                            //If Ts_Start out of Process Downtime Date we changing TS_END
                            fe.Where(x => x.UNIQUEID == dat[i].UNIQUEID).ToList().ForEach(s => s.TS_END = pdtv[j].from_dt);
                        }

                        if (dat[i].TS_END < pdtv[j].to_dt && dat[i].TS_START > pdtv[j].from_dt)
                        {
                            int fe_index = fe.FindIndex(a => a.UNIQUEID == dat[i].UNIQUEID);
                            fe.RemoveAt(fe_index);
                        }
                    }
                }
            }
            List<FE_Context> Result = fe;
            fe = null;
            return Result;
        }

        /// <summary>
        /// This method expand  Equipment lists because some machines consist from loader and unloader units 
        /// and downtime one machine Imply loader/unloader downtimes
        /// </summary>
        /// <param name="eq">Equipment List</param>
        /// <returns></returns>
        public List<int> GetEquipList(int eq)
        {
            List<int> Eqlist = new List<int>();
            if (eq == 5002 || eq == 5)
            {
                Eqlist.Add(5002);
                Eqlist.Add(5);
            }
            else if (eq == 6 || eq == 6002 || eq == 6003)
            {
                Eqlist.Add(6);
                Eqlist.Add(6002);
                Eqlist.Add(6003);
            }
            else if (eq == 7 || eq == 7002 || eq == 7003)
            {
                Eqlist.Add(7);
                Eqlist.Add(7002);
                Eqlist.Add(7003);
            }
            else if (eq == 20 || eq == 20002)
            {
                Eqlist.Add(20);
                Eqlist.Add(20002);
            }
            else
            {
                Eqlist.Add(eq);
            }

            return Eqlist;
        }
    }
}
