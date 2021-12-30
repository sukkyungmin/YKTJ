using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;

namespace RealTimeRMT
{
    class ConstDefine
    {
        //
        // Tab Items
        //
        //public enum eMainTab { home=0, realTime, sapUpdate, tjAsset, library, admin};
        public enum eMainTab { home = 0, realTime, sapUpdate, tjAsset, library, admin };
        public enum eRtFilterTab { rawMaterialSum = 0, rawMaterialDetailSum, totalWaste };
        public enum eSapUpdateTab { update = 0, setting };
        public enum eTjAssetTab { oee = 0, productionPlan, productionCalender, productionPrediction, kpi, Cov };
        public enum eTaOeeTab { fl = 0, udOee, tjOee };
        public enum eLibraryTab { commonLibrary = 0, personalLibrary, noticeBoard, freeBoard, addressBook };
        public enum eAdminTab { userList = 0, security };


        //
        // ListView Items
        // 
        public enum eSapUpdateLogListView { no = 0, updateItem, updatedDate, updateUser, updateInfo };
        public enum eSapButtListview { processCode = 0, processName, butt };
        public enum eMachineStatusListView { machine = 0, status, prd, oee, yield, delay, running, prdVer, prdType };
        public enum eProductPlanListView
        {
            productCode, dateTime, productLine, amount, unit, prodGender, prodSize, prodDomestic, prodCountry,
            countPerBag, countPerCase, totalCount
        };
        public enum eUserListView
        {
            no = 0, profilePicture, userName, userId, password, securityValue, phoneNumber, mobileNumber, email, positionValue,
            teamTypeValue, tjTypeValue, planePassword, securityCode, positionCode, teamTypeCode, tjTypeCode
        };
        public enum eLyUserListView
        {
            no = 0, profilePicture, userName, userId, securityValue, phoneNumber, mobileNumber, email, positionValue,
            teamTypeValue, tjTypeValue, securityCode, positionCode, teamTypeCode, tjTypeCode
        };
        public enum eSecurityListView { securityCode = 0, securityValue, viewRealTime, viewSapUpdate, viewTjAsset, viewLibrary, viewAdmin };
        public enum eLibraryListView { no = 0, fileName, fileDescription, viewCount, downloadCount, writer, createdDate };
        public enum eBoardListView { no = 0, title, viewCount, writer, createdDate, fileName }
        public enum eHeBoardListView { createdDate = 0, title, viewCount, no };
        public enum eHeSearchLogListView { machine, startDate, startTime, dash, endDate, endTime, reportName, searchCondition, logNo };
        public enum eHeMachineStatusListView { machine, status };
        public enum eProductionPredictionMaterialListView { no = 0, name, materialCode, description, fps, unitQty, estimateRollCount, fpsValue };
        //
        // 작업 디렉토리
        // 
        static public string workDir = @"C:\RealTime";
        static public string macroDir = @"C:\RealTime\Macro";
        static public string sapSourceDir = @"C:\RealTime\Macro\SAPSourceFiles";
        static public string outputDir = @"C:\RealTime\Macro\Output";

        //
        // SAP File Type
        // 
        public enum eSapFileType { schedule = 0, product, bom, length, batch };

        //
        // Team Type, TJ Type
        // 
        public enum eTeamType { bc = 0, cc };
        public enum eTjType { tj01 = 0, tj02, tj03, tj04, tj05, tj21, tj22, tj23 };
        //public enum eBcTjType { tj01 = 0, tj02, tj03, tj04, tj05 };
        //public enum eCcTjType { tj21 = 0, tj22 }; 

        //
        // Library Type
        //
        public enum eLibraryType { common = 0, personal };

        // 
        // Color Type
        // 
        public static Color panelBackColor = Color.FromArgb(206, 206, 206);
        public static Color validStateColor = Color.FromArgb(0, 255, 0);
        public static Color normalStateColor = panelBackColor;


        //
        // MessageBox Title
        // 
        public static string loginTitle = "로그인";
        public static string updateTitle = "RealTime DataBase Update";
        public static string searchTitle = "RealTime";
        public static string adminTitle = "Admin";
        public static string libraryTitle = "Library";
        public static string noticeBoardTitle = "공지사항";
        public static string freeBoardTitle = "자유게시판";

        public static int formWidth = 1280;
        //public static int formHeight = 768; 
        public static int formHeight = 900;

        //
        // Security Id
        // 
        public static int securityAdmin = 1000;

        // 
        // StatusBar Height
        public static int mainStatusBarHeight = 21;

        // Main Tab 내 좌측 패널 크기
        public static int leftPanelSize = 225;

        public static int sidePanelSize = 84;

        public static int scrollSize = 0;

        public static int defaultGap = 20;
        public static int TOP_PANEL_HEIGHT = 70;
    }
}