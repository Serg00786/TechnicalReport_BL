using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TechnicalReport_BL.Models;
using TechnicalReport_Data.Models.FE;

namespace TechnicalReport_BL.FE.Availability
{
    public interface ICalculateClass
    {
        double CalculateProcent(TimeSpan downtime, Date tmp);
        TimeSpan[] CalculatingUptime(List<FE_Context> Model, Date tms);
    }
    public class CalculateClass : ICalculateClass
    {
        /// <summary>
        /// This method makes final calculation of How equipment was available.
        /// </summary>
        /// <param name="downtime"></param>
        /// <param name="tmp"></param>
        /// <returns></returns>
        public double CalculateProcent(TimeSpan downtime, Date tmp)
        {
            TimeSpan TimeRequest = tmp.To - tmp.From;
            double procent;

            procent = 100 - (downtime.TotalSeconds * 100 / TimeRequest.TotalSeconds);
            procent = Math.Round(procent, 2);
            if (procent < 0)
            {
                procent = 0;
            }


            return procent;
        }

        /// <summary>
        /// This Method calculate how much downtime takes for each equipment
        /// </summary>
        /// <param name="Model"></param>
        /// <param name="tms"></param>
        /// <returns></returns>
        public TimeSpan[] CalculatingUptime(List<FE_Context> Model, Date tms)
        {
            TimeSpan[] Eq = new TimeSpan[26];
            TimeSpan[] PVD1 = new TimeSpan[3];
            TimeSpan[] PVD2 = new TimeSpan[3];
            TimeSpan[] PVD3 = new TimeSpan[3];
            TimeSpan[] PLU12 = new TimeSpan[2];
            List<double> proc = new List<double>();
            List<TimeSpan> test = new List<TimeSpan>();


            for (int i = 0; i < Model.Count; i++)
            {

                if (Model[i].TS_START < tms.From)
                {
                    Model[i].TS_START = tms.From;
                }
                if (Model[i].TS_END > tms.To)
                {
                    Model[i].TS_END = tms.To;
                }
                if (Model[i].TS_START > tms.To)         //Added 12.01.2020 Shoyld be checked with other reports
                {
                    Model[i].TS_START = tms.From;
                }


                switch (Model[i].EQIDENT)
                {
                    case 2:
                        if (Model[i].STATEIDENT > 4)
                            Eq[0] = Eq[0] + (Model[i].TS_END - Model[i].TS_START);  //WIS1
                        break;
                    case 3:
                        if (Model[i].STATEIDENT > 4)
                            Eq[1] = Eq[1] + (Model[i].TS_END - Model[i].TS_START);//WIS2
                        break;
                    case 22:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            Eq[2] = Eq[2] + (Model[i].TS_END - Model[i].TS_START);//WHQ
                        break;
                    case 4:
                        if (Model[i].STATEIDENT > 5)
                            Eq[3] = Eq[3] + (Model[i].TS_END - Model[i].TS_START);//Silex
                        break;
                    case 23:
                        if (Model[i].STATEIDENT == 2)
                            Eq[4] = Eq[4] + (Model[i].TS_END - Model[i].TS_START);//RENA
                        break;
                    case 5:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            PLU12[0] = PLU12[0] + (Model[i].TS_END - Model[i].TS_START);//PLU1 - 2-Unsc DownTime
                        break;
                    case 5002:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            PLU12[1] = PLU12[1] + (Model[i].TS_END - Model[i].TS_START);//PLU2
                        break;
                    case 5010:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[6] = Eq[6] + (Model[i].TS_END - Model[i].TS_START);//KAI1
                        break;
                    case 5011:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[7] = Eq[7] + (Model[i].TS_END - Model[i].TS_START);//KAI2
                        break;
                    case 5012:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[8] = Eq[8] + (Model[i].TS_END - Model[i].TS_START);//KAI3
                        break;
                    case 5018:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[9] = Eq[9] + (Model[i].TS_END - Model[i].TS_START);//KAI4

                        break;
                    case 5019:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[10] = Eq[10] + (Model[i].TS_END - Model[i].TS_START);//KAI5
                        break;
                    case 5016:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[11] = Eq[11] + (Model[i].TS_END - Model[i].TS_START);//KAI6
                        break;
                    case 5015:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[12] = Eq[12] + (Model[i].TS_END - Model[i].TS_START);//KAI7
                        break;
                    case 5013:
                        if (Model[i].STATEIDENT == 20 || Model[i].STATEIDENT == 30)
                            Eq[13] = Eq[13] + (Model[i].TS_END - Model[i].TS_START);//KAI8
                        break;
                    case 26:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            Eq[14] = Eq[14] + (Model[i].TS_END - Model[i].TS_START);//KAI9
                        break;
                    case 27:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            Eq[15] = Eq[15] + (Model[i].TS_END - Model[i].TS_START);//KAI10
                        break;
                    case 24:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3) { 
                            Eq[16] = Eq[16] + (Model[i].TS_END - Model[i].TS_START);//PLU3
                            test.Add(Model[i].TS_END - Model[i].TS_START);
                        }
                        break;
                    case 25:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            Eq[17] = Eq[17] + (Model[i].TS_END - Model[i].TS_START);//PLU4
                        break;
                    case 6:
                        if (Model[i].STATEIDENT == 6 || Model[i].STATEIDENT == 5)
                        {
                            PVD1[0] = PVD1[0] + (Model[i].TS_END - Model[i].TS_START);//PVD1
                        }
                        break;
                    case 6002:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                        {
                            PVD1[1] = PVD1[1] + (Model[i].TS_END - Model[i].TS_START);//PVD Loader1                                                                                     
                        }
                        break;
                    case 6003:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                        {
                            PVD1[2] = PVD1[2] + (Model[i].TS_END - Model[i].TS_START);//PVD Unloader1
                            //test.Add(Model[i]);
                        }
                        break;
                    case 7:
                        if (Model[i].STATEIDENT == 6 || Model[i].STATEIDENT == 5)
                            PVD2[0] = PVD2[0] + (Model[i].TS_END - Model[i].TS_START);//PVD2
                        break;
                    case 7002:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            PVD2[1] = PVD2[1] + (Model[i].TS_END - Model[i].TS_START);//PVD Loader2
                        break;
                    case 7003:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            PVD2[2] = PVD2[2] + (Model[i].TS_END - Model[i].TS_START);//PVD Unloader2
                        break;
                    case 20:
                        if (Model[i].STATEIDENT == 500 || Model[i].STATEIDENT == 400)
                            PVD3[0] = PVD3[0] + (Model[i].TS_END - Model[i].TS_START);//PVD3
                        break;
                    case 20002:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                            PVD3[1] = PVD3[1] + (Model[i].TS_END - Model[i].TS_START);//PVD Handler3
                        break;
                    case 9:
                        if (Model[i].STATEIDENT > 5)
                            Eq[21] = Eq[21] + (Model[i].TS_END - Model[i].TS_START);//PRN1
                        break;
                    case 21:
                        if (Model[i].STATEIDENT > 4)
                            Eq[22] = Eq[22] + (Model[i].TS_END - Model[i].TS_START);//PRN2
                        break;
                    case 10:
                        if (Model[i].STATEIDENT > 4 && Model[i].STATEIDENT < 28)
                            Eq[23] = Eq[23] + (Model[i].TS_END - Model[i].TS_START);//CIS1
                        break;
                    case 11:
                        if (Model[i].STATEIDENT > 4 && Model[i].STATEIDENT < 28)
                        {
                            Eq[24] = Eq[24] + (Model[i].TS_END - Model[i].TS_START);//CIS2
                        }
                        break;
                    case 13030:
                        if (Model[i].STATEIDENT == 2 || Model[i].STATEIDENT == 3)
                        {
                            Eq[25] = Eq[25] + (Model[i].TS_END - Model[i].TS_START);//CTS

                        }
                        break;
                }
            }
            Eq[5] = PLU12.Max();
            Eq[18] = PVD1.Max();
            Eq[19] = PVD2.Max();
            Eq[20] = PVD3.Max();

            return Eq;
        }
    }
}
