using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace WasteReport
{
    class MDBmanager
    {
        DB m_DB = new DB();

        DataSet configDset = new DataSet();
        DataTable dbTable;

        public MDBmanager(string db)
        {
            configDset.ReadXml(@"Setting\Config.xml");//설정값들이 들어있는 XML파일을 읽는다
            dbTable = configDset.Tables["MDB"];

            string catalog = "";
            //if (db == "유아1부")
            //{
            //    catalog = dbTable.Rows[0]["path_유아1부"].ToString();
            //}
            //else
            //{
            //    catalog = dbTable.Rows[0]["path_TJ02"].ToString();
            //}

            switch (db)
            {
                case "유아1부":
                    catalog = dbTable.Rows[0]["pathDb"].ToString();
                    break;
            }

            m_DB.InitializeDB(DB_TYPE.MDB ,"", "","", catalog);

            m_DB.ConnectDB();
        }

        public DataTable GetProd(string timeCondition, string machineCond, string shiftCond)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"select * from Prod WHERE {0} AND {1} AND {2}";

                query = string.Format(query, timeCondition, machineCond, shiftCond);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetProd");
            }

            return dataTable;
        }

        public DataTable GetWaste(string top, string timeCondition, string machineCond, string shiftCond, string wstGrpCond, string orderBy)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT  {0} Waste.Machine, Waste.DateStamp, Waste.Shift, W_CODE.WasteGroup, Waste.WasteCode, W_CODE.Description, 
                                    Waste.WasteOccr, Waste.WasteDeft
                                    FROM     (Waste INNER JOIN
                                                   W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                     WHERE {1} AND {2} AND {3} AND {4}
                                    {5}";

                query = string.Format(query, top, timeCondition, machineCond, shiftCond, wstGrpCond, orderBy);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetWaste");
            }

            return dataTable;
        }



        public DataTable GetDaily(string timeCondition, string machineCond, string shiftCond)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT  Waste.WasteCode, W_CODE.WasteGroup, W_CODE.Description, Waste.DateStamp, Waste.Machine,
                                                SUM(Waste.WasteOccr) AS WasteOccr, SUM(Waste.WasteDeft) AS WasteDeft
                                        FROM     (Waste LEFT OUTER JOIN
                                                       W_CODE ON W_CODE.WasteCode = Waste.WasteCode)
                                        WHERE  1 = 1 AND {0} AND {1} AND {2}
                                        GROUP BY Waste.WasteCode,  W_CODE.WasteGroup, W_CODE.Description, Waste.DateStamp, Waste.Machine
                                        Order by  SUM(Waste.WasteOccr) desc   ";

                query = string.Format(query, timeCondition, machineCond, shiftCond);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetDaily");
            }

            return dataTable;
        }


        public DataTable GetMonthly(string timeCondition, string machineCond, string shiftCond)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"TRANSFORM SUM(WasteOccr)
                                    Select WasteCode, Description From(
                                            SELECT  Waste.WasteCode,  W_CODE.WasteGroup, W_CODE.Description, LEFT(CStr(Waste.DateStamp), 10) AS DateStamp,
                                                               Waste.Machine, Waste.Shift, SUM(Waste.WasteOccr) AS WasteOccr
                                            FROM     ( Waste LEFT JOIN
                                                                W_CODE ON W_CODE.WasteCode = Waste.WasteCode)
                                            WHERE 1 = 1 AND {0}  AND {1} AND {2}
                                            GROUP BY Waste.WasteCode,  W_CODE.WasteGroup, W_CODE.Description, DateStamp, Waste.Machine, Waste.Shift
                                            Order by Waste.Machine, DateStamp)
                                    Group by WasteCode, Description
                                    PIVOT  DateStamp";

                query = string.Format(query, timeCondition, machineCond, shiftCond);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetMonthly");
            }

            return dataTable;
        }

        public DataTable GetGroup(string timeCondition, string machineCond, string shiftCond)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"TRANSFORM SUM(WasteOccr)
                                    Select WasteGroup From(   
                                            SELECT  Waste.WasteCode,  W_CODE.WasteGroup, W_CODE.Description, LEFT(CStr(Waste.DateStamp), 10) AS DateStamp,
                                                               Waste.Machine, Waste.Shift, SUM(Waste.WasteOccr) AS WasteOccr
                                            FROM     ( Waste LEFT JOIN
                                                                W_CODE ON W_CODE.WasteCode = Waste.WasteCode)
                                            WHERE 1 = 1 AND {0}  AND {1} AND {2}
                                            GROUP BY Waste.WasteCode,  W_CODE.WasteGroup, W_CODE.Description, DateStamp, Waste.Machine, Waste.Shift
                                            Order by Waste.Machine, DateStamp)
                                    Group by WasteGroup   
                                    PIVOT  DateStamp";

                query = string.Format(query, timeCondition, machineCond, shiftCond);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetGroup");
            }

            return dataTable;
        }


        public DataTable GetWstGrp()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT  WasteGroup
                                    FROM    W_CODE      
                                    GROUP BY WasteGroup";

                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetWstGrp");
            }

            return dataTable;
        }

        public DataTable GetWstGroups()
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT  IIF(ISNULL(WasteGroup), 'others', WasteGroup) AS WasteGroup
                                     FROM     W_CODE
                                     GROUP BY WasteGroup";

                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetWstGroups");
            }

            return dataTable;
        }

        public DataTable GetByWstCd(string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT TOP {0} subtbl.[Waste Code], SUM(subtbl.Occ) as Occ, SUM(subtbl.Def) as Def, 
                                                                      ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                                      ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                                      ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], 
                                                                      IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT  IIF(ISNULL(W_CODE.Description), Waste.WasteCode, W_CODE.Description) AS [Waste Code], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                        Waste.DateStamp, Waste.Shift,
                                                            (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                               FROM PROD
                                                               WHERE  1 = 1 AND  (PROD.DateStamp >= #{1}#) AND (PROD.DateStamp <= #{2}#)) AS cuts
                                                        FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                        WHERE  1 = 1  AND (Waste.DateStamp >= #{1}#) AND (Waste.DateStamp <= #{2}#) {3}) subtbl
                                    GROUP BY  subtbl.[Waste Code], subtbl.cuts
                                    order by SUM(subtbl.Def) desc";

                query = string.Format(query, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstCdWithShift");
            }

            return dataTable;
        }

        public DataTable GetByWstCdWithShift(string shift, string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT TOP {1} subtbl.[Waste Code], SUM(subtbl.Occ) as Occ, SUM(subtbl.Def) as Def, 
                                                                      ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                                      ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                                      ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], 
                                                                      IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT  IIF(ISNULL(W_CODE.Description), Waste.WasteCode, W_CODE.Description) AS [Waste Code], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                        Waste.DateStamp, Waste.Shift,
                                                            (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                               FROM PROD
                                                               WHERE  1 = 1 AND PROD.shift = '{0}' AND (PROD.DateStamp >= #{2}#) AND (PROD.DateStamp <= #{3}#)) AS cuts
                                                        FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                        WHERE  1 = 1 AND (Waste.Shift = '{0}') AND (Waste.DateStamp >= #{2}#) AND (Waste.DateStamp <= #{3}#) {4}) subtbl
                                    GROUP BY  subtbl.[Waste Code], subtbl.cuts
                                    order by SUM(subtbl.Def) desc";

                query = string.Format(query, shift, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstCdWithShift");
            }

            return dataTable;
        }

        public DataTable GetByWstCdOnlyShift(string shift, string dp, string ct ,string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"SELECT TOP {1} *
                                 FROM(
                                 SELECT  Waste.WasteCode AS [Code #], IIF(ISNULL (W_CODE.Description), 'Not Used', W_CODE.Description) AS [Description],
                                         SUM(Waste.WasteOccr) AS [OCCR], SUM(Waste.WasteDeft) AS [PROD],
                                         IIF([PROD] = 0, 0, ROUND([PROD]/[OCCR],1)) AS [PROD/OCCR]                         
                                 FROM (Waste INNER JOIN W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                 WHERE  1 = 1 AND (Waste.DateStamp >= #{3}#) AND (Waste.DateStamp <= #{4}#) AND (Waste.Shift = '{0}') {5} 
                                 GROUP BY Waste.WasteCode, W_CODE.Description) subtbl
                                 ORDER BY [PROD] DESC    
                                 ";

                query = string.Format(query, shift, dp, ct, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstCdOnlyShift");
            }

            return dataTable;
        }

        public DataTable GetByWstCdOnlySum(string shift, string dp, string ct, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"SELECT TOP {1} *
                                 FROM(
                                 SELECT  Waste.WasteCode AS [Code #], IIF(ISNULL (W_CODE.Description), 'Not Used', W_CODE.Description) AS [Description],
                                         SUM(Waste.WasteOccr) AS [OCCR], SUM(Waste.WasteDeft) AS [PROD],
                                         IIF([PROD] = 0, 0, ROUND([PROD]/[OCCR],1)) AS [PROD/OCCR]                         
                                 FROM (Waste INNER JOIN W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                 WHERE  1 = 1 AND (Waste.DateStamp >= #{3}#) AND (Waste.DateStamp <= #{4}#) {5} 
                                 GROUP BY Waste.WasteCode, W_CODE.Description) subtbl
                                 ORDER BY [PROD] DESC    
                                 ";

                query = string.Format(query, shift, dp, ct, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstCdOnlySum");
            }

            return dataTable;
        }


        public DataTable GetByWstGp(string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT TOP {0} IIF(ISNULL(subtbl.[Waste Group]), 'others', subtbl.[Waste Group]) AS [Waste Group], SUM(subtbl.Occ) as Occ, 
                                                                        SUM(subtbl.Def) as Def, ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                          ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                          ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                       Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                         FROM PROD
                                                                         WHERE  1 = 1  AND 
                                                                                          (PROD.DateStamp >= #{1}#) AND (PROD.DateStamp <= #{2}#)) AS cuts
                                                     FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  1 = 1 AND (Waste.DateStamp >= #{1}#) AND (Waste.DateStamp <= #{2}#) {3}) subtbl
                                    GROUP BY  subtbl.[Waste Group], subtbl.cuts
                                    order by SUM(subtbl.Def) desc";

                query = string.Format(query, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstGp");
            }

            return dataTable;
        }

        public DataTable GetByWstGpWithShift(string shift, string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT TOP {1} IIF(ISNULL(subtbl.[Waste Group]), 'others', subtbl.[Waste Group]) AS [Waste Group], SUM(subtbl.Occ) as Occ, 
                                                                        SUM(subtbl.Def) as Def, ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                          ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                          ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                       Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                         FROM PROD
                                                                         WHERE  1 = 1 AND PROD.shift = '{0}' AND 
                                                                                          (PROD.DateStamp >= #{2}#) AND (PROD.DateStamp <= #{3}#)) AS cuts
                                                     FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  1 = 1 AND (Waste.Shift = '{0}') AND (Waste.DateStamp >= #{2}#) AND (Waste.DateStamp <= #{3}#)  {4}) subtbl
                                    GROUP BY  subtbl.[Waste Group], subtbl.cuts
                                    order by SUM(subtbl.Def) desc";

                query = string.Format(query, shift, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetByWstGpWithShift");
            }

            return dataTable;
        }

        public DataTable GetAofWstGp(string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT subtbl.[Waste Group], subtbl.[DateStamp], SUM(subtbl.Occ) as Occ, 
                                                                        SUM(subtbl.Def) as Def, ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                          ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                          ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT   IIF(ISNULL(W_CODE.WasteGroup), 'others', TRIM(W_CODE.WasteGroup)) AS [Waste Group], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                       Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                         FROM PROD
                                                                         WHERE  1 = 1  AND 
                                                                                          (PROD.DateStamp >= #{0}#) AND (PROD.DateStamp <= #{1}#)) AS cuts
                                                     FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  1 = 1 AND (Waste.DateStamp >= #{0}#) AND (Waste.DateStamp <= #{1}#)  AND 1 = 1) subtbl
                                    where subtbl.[Waste Group]= '{2}'
                                    GROUP BY  subtbl.[Waste Group],  subtbl.[DateStamp], subtbl.cuts
                                    order by subtbl.[DateStamp]";

                query = string.Format(query, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetAofWstGp");
            }

            return dataTable;
        }

        public DataTable GetAofWstGpWithShift(string shift, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT subtbl.[Waste Group], subtbl.[DateStamp], SUM(subtbl.Occ) as Occ, 
                                                                        SUM(subtbl.Def) as Def, ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                          ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                          ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                    SELECT   IIF(ISNULL(W_CODE.WasteGroup), 'others', TRIM(W_CODE.WasteGroup)) AS [Waste Group], Waste.WasteOccr AS Occ, Waste.WasteDeft AS Def, 
                                                       Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                         FROM PROD
                                                                         WHERE  1 = 1  AND PROD.shift = '{0}' AND 
                                                                                          (PROD.DateStamp >= #{1}#) AND (PROD.DateStamp <= #{2}#)) AS cuts
                                                     FROM     (Waste LEFT JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  1 = 1 AND (Waste.Shift = '{0}') AND (Waste.DateStamp >= #{1}#) AND (Waste.DateStamp <= #{2}#)  AND 1 = 1) subtbl
                                    where subtbl.[Waste Group]= '{3}'
                                    GROUP BY  subtbl.[Waste Group],  subtbl.[DateStamp], subtbl.cuts
                                    order by subtbl.[DateStamp]";

                query = string.Format(query, shift, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetAofWstGpWithShift");
            }

            return dataTable;
        }

        #region (16.04.06 Made by MSC)
        public DataTable GetDailyWgDdown(string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT    
                                    Format(subtbl.[Date Stamp],'yyyy-MM-dd') As [Date Stamp], 
                                    subtbl.[Waste Code], 
                                    SUM(subtbl.Occ) AS Occ, 
                                    SUM(subtbl.Def) AS Def          
                        FROM(
                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], 
                                            W_CODE.Description AS [Waste Code], 
                                            Waste.WasteOccr AS Occ, 
                                            Waste.WasteDeft AS Def, 
                                            Waste.DateStamp AS [Date Stamp]
                                    FROM     (Waste INNER JOIN W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                    WHERE  1 = 1 AND (Waste.DateStamp >= #{1}#) AND (Waste.DateStamp <= #{2}#) 
                                                    {3}) subtbl
                        GROUP BY  subtbl.[Waste Group], subtbl.[Waste Code], subtbl.[Date Stamp]
                        ORDER BY  subtbl.[Date Stamp]";

                query = string.Format(query, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetDailyWgDdown");
            }

            return dataTable;
        }

        public DataTable GetDailyWgDownWithShift(string shift, string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT    
                                    Format(subtbl.[Date Stamp],'yyyy-MM-dd') As [Date Stamp],
                                    subtbl.[Waste Code], 
                                    SUM(subtbl.Occ) as Occ, 
                                    SUM(subtbl.Def) as Def
                        FROM(
                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], 
                                            W_CODE.Description AS [Waste Code], 
                                            Waste.WasteOccr AS Occ, 
                                            Waste.WasteDeft AS Def, 
                                            Waste.DateStamp AS [Date Stamp], 
                                            Waste.Shift,
                                            (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                            FROM PROD
                                            WHERE  PROD.shift = '{0}' AND (PROD.DateStamp >= #{2}#) AND (PROD.DateStamp <= #{3}#)) AS cuts
                                    FROM    (Waste INNER JOIN W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                    WHERE   (Waste.Shift = '{0}') AND (Waste.DateStamp >= #{2}#) AND (Waste.DateStamp <= #{3}#) 
                                                    {4}) subtbl
                        GROUP BY    subtbl.[Waste Group], subtbl.[Waste Code], subtbl.[Date Stamp], subtbl.cuts
                        ORDER BY    subtbl.[Date Stamp]";

                query = string.Format(query, shift, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetDailyWgDownWithShift");
            }

            return dataTable;
        }
        #endregion

        public DataTable GetWgDdown(string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT    IIF(ISNULL(subtbl.[Waste Group]), 'others', subtbl.[Waste Group]) AS [Waste Group], 
                                                            subtbl.[Waste Code], SUM(subtbl.Occ) as Occ, SUM(subtbl.Def) as Def, 
                                                            ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                            ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                            ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], 
                                                            IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], W_CODE.Description AS [Waste Code], 
                                                                        Waste.WasteOccr AS Occ, 
                                                                        Waste.WasteDeft AS Def, Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                            FROM PROD
                                                                            WHERE  1 = 1 AND (PROD.DateStamp >= #{1}#) AND 
                                                                            (PROD.DateStamp <= #{2}#)) AS cuts
                                                     FROM     (Waste INNER JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  1 = 1 AND (Waste.DateStamp >= #{1}#) AND (Waste.DateStamp <= #{2}#) 
                                                                      {3}) subtbl
                                    GROUP BY  subtbl.[Waste Group], subtbl.[Waste Code], subtbl.cuts
                                    order by subtbl.[Waste Group], subtbl.[Waste Code]";

                query = string.Format(query, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetWgDdown");
            }

            return dataTable;
        }

        public DataTable GetWgDdownWithShift(string shift, string dp, string strDate, string endDate, string group)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;

                query = @"SELECT   IIF(ISNULL(subtbl.[Waste Group]), 'others', subtbl.[Waste Group]) AS [Waste Group], 
                                                            subtbl.[Waste Code], SUM(subtbl.Occ) as Occ, SUM(subtbl.Def) as Def, 
                                                            ROUND((SUM(subtbl.Def) / subtbl.cuts) * 100, 2) AS [%Waste], 
                                                            ROUND((SUM(subtbl.Def) / SUM(subtbl.Occ)), 1) as [Def/Occ], 
                                                            ROUND((SUM(subtbl.Def) / subtbl.cuts) * 10000, 1) AS [Def/Cut], 
                                                            IIF([Def/Cut] = '0', 0, ROUND([Def/Occ] / [Def/Cut], 2)) AS [Cut/Occ]
                                    FROM(
                                                    SELECT  TRIM(W_CODE.WasteGroup) AS [Waste Group], W_CODE.Description AS [Waste Code], 
                                                                        Waste.WasteOccr AS Occ, 
                                                                        Waste.WasteDeft AS Def, Waste.DateStamp, Waste.Shift,
                                                                        (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                            FROM PROD
                                                                            WHERE  PROD.shift = '{0}' AND (PROD.DateStamp >= #{2}#) AND 
                                                                            (PROD.DateStamp <= #{3}#)) AS cuts
                                                     FROM     (Waste INNER JOIN
                                                                    W_CODE ON Waste.WasteCode = W_CODE.WasteCode)
                                                     WHERE  (Waste.Shift = '{0}') AND (Waste.DateStamp >= #{2}#) AND (Waste.DateStamp <= #{3}#) 
                                                                      {4}) subtbl
                                    GROUP BY  subtbl.[Waste Group], subtbl.[Waste Code], subtbl.cuts
                                    order by subtbl.[Waste Group], subtbl.[Waste Code]";

                query = string.Format(query, shift, dp, strDate, endDate, group);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetWgDdownWithShift");
            }

            return dataTable;
        }



        public string GetTotalCut(string shiftCond, string strDate, string endDate)
        {
            string tCut = "";

            try
            {
                string query = @"  (SELECT  SUM(PROD.TotalMCuts + PROD.TotalRCuts)
                                                                            FROM PROD
                                                                            WHERE  1 = 1 {0} AND (PROD.DateStamp >= #{1}#) AND 
                                                                            (PROD.DateStamp <= #{2}#))";

                query = string.Format(query, shiftCond, strDate, endDate);
                tCut = m_DB.Selectquery(query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetTotalCut");
            }

            return tCut;
        }

        public DataTable GetDailyTopInfo(string strDate, string endDate)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"SELECT Shift  AS [Shift] ,TotalMCuts AS [Cuts], TotalMCulls AS [Culls], 
                                 ROUND(TotalWst,1) AS [TW%], ROUND(RunnWst,1) AS [CW%], ROUND(PkgWst,1) AS [PW%], TotalCases AS [CaseCount], 
                                 CaseCountCorr AS [CCC], ROUND(AvgMD_HR,1) AS [Avg_MDPH], ROUND((MachDownTime / 43200) * 100, 1) AS [%Down], 
                                 MachStopsOcc AS [Stops], ROUND(AvgDPM,1) AS [AvgDPM], ProdPerCase AS [Prod/Case] 
                          FROM   PROD
                          WHERE  (DateStamp >= #{0}#) AND (DateStamp <= #{1}#) 
                          ORDER BY Shift ASC ";

                query = string.Format(query, strDate, endDate);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetDailyTopInfo");
            }

            return dataTable;
        }

        public DataTable GetIntervalTopInfo(string strDate, string endDate)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"SELECT Shift  AS [Shift], SUM(TotalMCuts) AS [Cuts], SUM(TotalMCulls) AS [Culls], 
                                 ROUND(AVG(TotalWst),1) AS [TW%], ROUND(AVG(RunnWst),1) AS [CW%], ROUND(AVG(PkgWst),1) AS [PW%], SUM(TotalCases) AS [CaseCount], 
                                 SUM(CaseCountCorr) AS [CCC], ROUND(AVG(AvgMD_HR),1) AS [Avg_MDPH], 
                                 IIF(SUM(MachDownTime) = 0, 0, ROUND((SUM(MachDownTime) / (86400 * (DateDiff(""d"", #{0}#, #{1}#) + 1))) * 100, 1)) AS [%Down], 
                                 SUM(MachStopsOcc) AS [Stops], ROUND(AVG(AvgDPM),1) AS [AvgDPM] 
                          FROM   PROD
                          WHERE  (DateStamp >= #{0}#) AND (DateStamp <= #{1}#)
                          GROUP BY Shift 
                          ORDER BY Shift ASC ";

                query = string.Format(query, strDate, endDate);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetIntervalTopInfo");
            }

            return dataTable;
        }

        public DataTable GetIntervalTopWithShift(string strDate, string endDate, string shift)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query = @"SELECT AVG(Shift) AS [Shift], SUM(TotalMCuts) AS [Cuts], SUM(TotalMCulls) AS [Culls], 
                                 ROUND(AVG(TotalWst),1) AS [TW%], ROUND(AVG(RunnWst),1) AS [CW%], ROUND(AVG(PkgWst),1) AS [PW%], SUM(TotalCases) AS [CaseCount], 
                                 SUM(CaseCountCorr) AS [CCC], ROUND(AVG(AvgMD_HR),1) AS [Avg_MDPH], 
                                 IIF(SUM(MachDownTime) = 0, 0, ROUND((SUM(MachDownTime) / (86400 * (DateDiff(""d"", #{0}#, #{1}#) + 1))) * 100, 1)) AS [%Down], 
                                 SUM(MachStopsOcc) AS [Stops], ROUND(AVG(AvgDPM),1) AS [AvgDPM] 
                          FROM   PROD
                          WHERE  (DateStamp >= #{0}#) AND (DateStamp <= #{1}#) AND (Shift = '{2}') ";

                query = string.Format(query, strDate, endDate, shift);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetIntervalTopWithShift");
            }

            return dataTable;
        }

        public DataTable GetProdList(string strDate, string endDate, string shifttype)
        {
            DataTable dataTable = new DataTable();

            try
            {
                string query;


                if (shifttype == "*")
                {
                    query = @"SELECT DateStamp AS [Date], Shift  AS [Shift] ,TotalMCuts AS [Cuts], TotalMCulls AS [Culls], 
                                 ROUND(TotalWst,1) AS [TW%], ROUND(RunnWst,1) AS [CW%], ROUND(PkgWst,1) AS [PW%], TotalCases AS [CaseCount], 
                                 CaseCountCorr AS [CCC], ROUND(AvgMD_HR,1) AS [Avg_MDPH], ROUND((MachDownTime / 43200) * 100, 1) AS [%Down], 
                                 MachStopsOcc AS [Stops], ROUND(AvgDPM,1) AS [AvgDPM], ProdPerCase AS [Prod/Case] 
                          FROM   PROD
                          WHERE  (DateStamp >= #{0}#) AND (DateStamp <= #{1}#) 
                          ORDER BY DateStamp DESC ";
                }
                else
                {
                    query = @"SELECT DateStamp AS [Date], Shift  AS [Shift] ,TotalMCuts AS [Cuts], TotalMCulls AS [Culls], 
                                 ROUND(TotalWst,1) AS [TW%], ROUND(RunnWst,1) AS [CW%], ROUND(PkgWst,1) AS [PW%], TotalCases AS [CaseCount], 
                                 CaseCountCorr AS [CCC], ROUND(AvgMD_HR,1) AS [Avg_MDPH], ROUND((MachDownTime / 43200) * 100, 1) AS [%Down], 
                                 MachStopsOcc AS [Stops], ROUND(AvgDPM,1) AS [AvgDPM], ProdPerCase AS [Prod/Case] 
                          FROM   PROD
                          WHERE  (DateStamp >= #{0}#) AND (DateStamp <= #{1}#) AND (Shift = '{2}') 
                          ORDER BY DateStamp DESC ";
                }

                query = string.Format(query, strDate, endDate, shifttype);
                m_DB.Runquery(ref dataTable, query);
            }
            catch (Exception ex)
            {
                new LogWriter().LOG(ex.Message, "GetProdList");
            }

            return dataTable;
        }

    }
}
