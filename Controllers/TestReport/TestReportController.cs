using NewModelEx_S1.Controllers.Engineer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewModelEx_S1.Context;
using NewModelEx_S1.Models;
using System.Diagnostics;

namespace NewModelEx_S1.Controllers.TestReport
{
    public class TestReportController : Controller
    {

        ReportController report = new ReportController();

       
        // GET: TestReport
        public ActionResult Index()
        {
            tee ty = new tee();

            report.showModel();

            int b = 8;
            int c = b + ty.a;

            ViewBag.ddd = c;
            return View();
        }

        
    }

    public class tee
    {
        public int a = 0;

    }
}