using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewModelEx_S1.Context;
using Newtonsoft.Json.Linq;
using NewModelEx_S1.Models;
using System.Data;
using System.Data.SqlClient;

namespace NewModelEx_S1.Controllers.SumaryReport
{
    public class SumaryReportController : Controller
    {
        ConnectDatabase conSql = new ConnectDatabase();
        OCTIIS_WEBAPPEntities3 db = new OCTIIS_WEBAPPEntities3();
        // GET: SumaryReport
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Chart() {
       
            return View();
        }

        [HttpPost]
        public ActionResult Chart(string date)
        {
            var dbChart = "SELECT Dep.department,count(EMA.status_job) As countStatus ,'Complete' As StatusDoc "+
            "FROM Eng_ModelAction EMA "+
            "INNER JOIN Eng_AddDepartmentModel  EAM ON EMA.id_depmodel = EAM.adddpmodel_id "+
            "INNER JOIN  Department Dep ON EAM.dept_id =  Dep.dept_id "+
            "WHERE EMA.status_job = 4 and FORMAT(CONVERT(date,EMA.dateadd_amin), 'MM-yyyy') = '" + date + "' " +
            "GROUP BY Dep.department "+

            "UNION ALL "+

            "SELECT Dep.department,count(EMA.status_job) As countStatus ,'Wait' As StatusDoc "+
            "FROM Eng_ModelAction EMA "+
            "INNER JOIN Eng_AddDepartmentModel  EAM ON EMA.id_depmodel = EAM.adddpmodel_id "+
            "INNER JOIN  Department Dep ON EAM.dept_id =  Dep.dept_id "+
            "WHERE EMA.status_job = 0 and FORMAT(CONVERT(date,EMA.dateadd_amin), 'MM-yyyy') = '" + date + "' " +
            "GROUP BY Dep.department ";

            DataTable dt = new DataTable();
            dt.TableName = "StatusPO";
            conSql.OpenConnectionSql();
            SqlDataAdapter da = new SqlDataAdapter(dbChart, conSql.con);
            da.Fill(dt);
            conSql.CloseConnectionSql();

            List<ModelStatus> model = new List<ModelStatus>();
            int countData = dt.Rows.Count;
            if (countData > 0)
            {
                for (int i = 0; i < countData; i++)
                {
                    model.Add(new ModelStatus()
                    {
                        department = dt.Rows[i]["department"].ToString(),
                        departmentDis = dt.Rows[i]["department"].ToString(),
                        countstatus = Convert.ToInt32(dt.Rows[i]["countStatus"].ToString()),
                        status = dt.Rows[i]["StatusDoc"].ToString(),
                    });
                }

           

            }
            return Json(model.ToList(),JsonRequestBehavior.AllowGet);
        }
    }
}