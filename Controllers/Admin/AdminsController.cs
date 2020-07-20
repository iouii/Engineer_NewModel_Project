using EASendMail;
using NewModelEx_S1.Context;
using NewModelEx_S1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace NewModelEx_S1.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home

        OCTIIS_WEBAPPEntities3 dbObjz = new OCTIIS_WEBAPPEntities3();

      
        public ActionResult Index()
        {
            showModel();
           

            return View();


        }

        public ActionResult testView()
        {
           // showModel();


            return View();


        }

        [HttpPost]
        public ActionResult Index(object sender, EventArgs e)
        {

            using (var context = new OCTIIS_WEBAPPEntities3())
            {

                var cusid = HttpContext.Request.Form["cuspost"];
                var modelname = HttpContext.Request.Form["modelnamepost"];
                var partname = HttpContext.Request.Form["partnamepost"];
                var drawing = HttpContext.Request.Form["drawingpost"].Trim();
                var programname = HttpContext.Request.Form["programpost"];
                var date = HttpContext.Request.Form["datepost"];
                var classification = HttpContext.Request.Form["classpost"];
                var rev = HttpContext.Request.Form["revpost"];
                var other = HttpContext.Request.Form["textpost"];


                var std = new Eng_newModel()
                {
                    id_customer = cusid,
                    model_name = modelname,
                    mode_partname = partname,
                    model_drawingid = drawing.Trim(),
                    customer_partname = programname,
                    model_datedoc = date,
                    status_modelid = classification,
                    version = rev,
                    otherdetail = other



                };
                context.Eng_newModel.Add(std);


                context.SaveChanges();



            }

            return RedirectToAction("Index", "Home", new { v = "1"});


        }
        [HttpPost]
        public ActionResult EditNewModel()
        {

            var cusid = HttpContext.Request.Form["cuspost"];
            var modelname = HttpContext.Request.Form["modelnamepost"];
            var partname = HttpContext.Request.Form["partnamepost"];
            var drawing = HttpContext.Request.Form["drawingpost"].Trim();
            var programname = HttpContext.Request.Form["programpost"];
            var date = HttpContext.Request.Form["datepost"];
            var classification = HttpContext.Request.Form["classpost"];
            var rev = HttpContext.Request.Form["revpost"];
            var other = HttpContext.Request.Form["textpost"];

            using (var context = new OCTIIS_WEBAPPEntities3())
            {


                var upother = context.Eng_newModel.Where(e => e.model_drawingid == drawing);

                foreach (var up in upother)
                {

                    
                    up.mode_partname = partname;
                    up.id_customer = cusid;
                    up.model_datedoc = date;
                    up.otherdetail = other;
                    up.status_modelid = classification;
                    up.customer_partname = programname;
                    up.version = rev;
                    up.model_drawingid = drawing;

                }

                UpdateModel(upother);
                context.SaveChanges();

            }

            return RedirectToAction("Index", "Home");

        }

        public ActionResult EditNewModel(string id)
        {


            Array EditArray;



            List<ModelNew> editmodelnew = new List<ModelNew>();

            var showeditmodelnew = dbObjz.Eng_newModel
                .Join(dbObjz.Eng_Customer, en => en.id_customer.ToString(), ec => ec.cus_id.ToString(), (en, ec) => new { en, ec })
                .Join(dbObjz.Eng_Clusifiation, enec => enec.en.status_modelid.ToString(), ecl => ecl.id.ToString(), (enec, ecl) => new { enec, ecl })
                .Where(c => c.enec.en.model_drawingid == id)
                .ToList();


            foreach (var editmodel in showeditmodelnew)
            {




                editmodelnew.Add(new ModelNew
                {

                    ProgramName = editmodel.enec.en.customer_partname,
                    ModelDrawing = editmodel.enec.en.model_drawingid.Trim(),
                    ModelName = editmodel.enec.en.model_name,
                    PartName = editmodel.enec.en.mode_partname,
                    ModelDate = editmodel.enec.en.model_datedoc,
                    CustomerName = editmodel.enec.ec.cus_name,
                    OtherInfo = editmodel.enec.en.otherdetail,
                    Classification = editmodel.ecl.name_clusftion,
                    version = editmodel.enec.en.version


                });



            }
            EditArray = editmodelnew.ToArray();
            ViewBag.EditNewModel = EditArray;







            Customer();
            Classification();
            
            return View();

        }

        public ActionResult ModelDetial(string id) 
        {
            
            Array ModelPartArray;
            List<ModelPart> modelpartget = new List<ModelPart>();
                List<string> drawingnode = new List<string>();
            var getmodelpart = dbObjz.Eng_ModelSub
                .Select(ems => new {ems.model_drawingid , ems.drawing,ems.modelname,ems.partname,ems.revision,ems.other_commment,ems.model_subid})
                .Where(ems => ems.model_drawingid == id).ToList();

            foreach (var modelpart in getmodelpart)
                    {
                        modelpartget.Add(new NewModelEx_S1.Models.ModelPart
                        {
                           MotherDrawing = modelpart.model_drawingid,
                           NodeDrawing = modelpart.drawing,
                           NodeModelName = modelpart.modelname,
                           NodePartName = modelpart.partname,
                           NodeRevision = modelpart.revision,
                           NodeOtherComment = modelpart.other_commment,
                           SubId = modelpart.model_subid.ToString()
                         

                        });

                
                        drawingnode.Add(modelpart.drawing);

                        
                       
                    }
                    
                    drawingnode.ToArray();

                    ModelPartArray = modelpartget.ToArray();

                    ViewBag.ShowModelpart = ModelPartArray;
                    ViewData["drawingNumber"] = id;




            //querydepartList(id, drawingnode);

    

            return View();
        }

        public ActionResult EditModelDetail(string id ,string nodekey,string lid)
        {
            Array ModelEditPartArray;
            List<ModelPart> modelpartget = new List<ModelPart>();
           
            List<string> drawingnode = new List<string>();
            var getmodelpart = dbObjz.Eng_ModelSub
                .Select(ems => new { ems.model_drawingid, ems.drawing, ems.modelname, ems.partname, ems.revision, ems.other_commment , ems.model_subid })
                .Where(ems => ems.model_drawingid == id && ems.drawing == nodekey).ToList();

            foreach (var modelpart in getmodelpart)
            {
                modelpartget.Add(new NewModelEx_S1.Models.ModelPart
                {
                    MotherDrawing = modelpart.model_drawingid,
                    NodeDrawing = modelpart.drawing,
                    NodeModelName = modelpart.modelname,
                    NodePartName = modelpart.partname,
                    NodeRevision = modelpart.revision,
                    NodeOtherComment = modelpart.other_commment,
                    SubId = modelpart.model_subid.ToString()


                });


                drawingnode.Add(modelpart.drawing);



            }

            drawingnode.ToArray();

            ModelEditPartArray = modelpartget.ToArray();

            ViewBag.ShowEditModelpart = ModelEditPartArray;
            ViewData["drawingNumber"] = id;





            return View();

        }
        [HttpPost]
        public ActionResult EditModelDetail()
        {

            var keydrawing = HttpContext.Request.Form["KeyDrawing"].Trim();
            var modelname = HttpContext.Request.Form["modelnamepost"];
            var partname = HttpContext.Request.Form["partnamepost"];
            var drawing = HttpContext.Request.Form["drawingpost"].Trim();
            var rev = HttpContext.Request.Form["revpost"];
            var other = HttpContext.Request.Form["textpost"];
            var subid = HttpContext.Request.Form["modelsubid"];
            using (var context = new OCTIIS_WEBAPPEntities3())
            {


                var uppart = context.Eng_ModelSub.Where(e => e.model_subid.ToString() == subid);

                foreach (var up in uppart)
                {


                    up.drawing = drawing;
                    up.date_update = DateTime.Now.ToString();
                    up.modelname = modelname;
                    up.partname = partname;
                    up.revision = rev;
                    up.other_commment = other;

                }

                UpdateModel(uppart);
                context.SaveChanges();

            }

            return RedirectToAction("ModelDetial", "Home", new { id = keydrawing });
        }
        public ActionResult ModelPart(string KeyPart)
        {


            ViewBag.keyDraw = KeyPart;
            return View();
        }
        
        [HttpPost]
        public ActionResult ModelPart()
        {
                var keydrawing = HttpContext.Request.Form["KeyDrawing"].Trim();
                var modelname = HttpContext.Request.Form["modelnamepost"];
                var partname = HttpContext.Request.Form["partnamepost"];
                var drawing = HttpContext.Request.Form["drawingpost"].Trim();
                var rev = HttpContext.Request.Form["revpost"];
                var other = HttpContext.Request.Form["textpost"];

            using (var context = new OCTIIS_WEBAPPEntities3())
            {
              


                var std = new Eng_ModelSub()
                {

                 model_drawingid =keydrawing ,
                 drawing = drawing,
                 modelname = modelname,
                 partname = partname,
                 revision = rev,
                 other_commment = other
                 




                };
                
                context.Eng_ModelSub.Add(std);

                context.SaveChanges();

            }

            return RedirectToAction("ModelDetial", "Home", new { id =keydrawing });
         
        }


       
        public ActionResult ModelDepartAction(string masterkey, string nodekey, string idlist)
        {



            Array ModeldepartjoblistArray;
            List<ModelDepartJob> modelshowdepartjob = new List<ModelDepartJob>();

            var getmodeldepartlistjob = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjz.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
                .Where(ee => ee.emej.em.id_depmodel.ToString() == idlist)
                .ToList();

            getmodeldepartlistjob.Count();
            foreach (var modelpartjob in getmodeldepartlistjob)
            {
                modelshowdepartjob.Add(new ModelDepartJob
                {
                   ModelId = modelpartjob.emej.em.id_modelact.ToString(),
                     DepartCode = modelpartjob.emej.em.id_depmodel.ToString(),
                     DepartJobId =modelpartjob.emej.em.id_jobitem.ToString(),
                     DepartJobName =modelpartjob.emej.ej.job_itemname,
                     DepartJobStatus=modelpartjob.emej.em.status_job, 
                     DepartJobDueDate =modelpartjob.emej.em.plandate,
                     DepartJobActionName =modelpartjob.emej.em.name_responsible,
                     DepartJobActionEmail =modelpartjob.emej.em.emailsend,
                     DepartJobActionComment =modelpartjob.emej.em.comment_admin

                });



            }

            ModeldepartjoblistArray = modelshowdepartjob.ToArray();

            ViewBag.ShowModelpartjoblist = ModeldepartjoblistArray;


                ViewBag.masterkey =masterkey;
                ViewBag.nodekey =nodekey;
                ViewBag.idlist =idlist;
       

            return View();

        }
         
        public ActionResult AddNewModel()
        {
            Customer();
            Classification();


            return View();
        }

        
        public ActionResult AddActionJob(string masterkey, string nodekey, string idlist)
        {

            ViewBag.masterkey = masterkey;

            ViewBag.nodekey = nodekey;
            ViewBag.idlist = idlist;

            JobList();
            UserList();

            return View();
        }

       
        [HttpPost]
        public ActionResult ActionJob(string myJSONs)
        {
            JObject postalCode = new JObject();
           
             postalCode = JObject.Parse(myJSONs);
             var a = postalCode["JobName"].Count();
             var iddepmodle = Convert.ToInt32(postalCode["IdListDepart"]);
            
             var get1 = dbObjz.Eng_AddDepartmentModel
                 .Join(dbObjz.Departments, ad => ad.dept_id, d => d.dept_id, (ad, d) => new { ad, d })
                 .Join(dbObjz.Eng_newModel, add => add.ad.model_masterdrawingid, nm => nm.model_drawingid, (add, nm) => new { add, nm })
                 .Join(dbObjz.Eng_Customer, addnm => addnm.nm.id_customer, c => c.cus_id.ToString(), (addnm, c) => new { addnm, c })
                 .Join(dbObjz.Eng_ModelSub, ad1 => ad1.addnm.add.ad.model_subnew, ms => ms.model_subid.ToString(), (ad1, ms) => new { ad1,ms})
                 .Where(xa => xa.ad1.addnm.add.ad.adddpmodel_id == iddepmodle)
                 .ToList();

            

             for (var i = 0; i < a; i++ )
             {

                 using (var context = new OCTIIS_WEBAPPEntities3())
                 {


                     var std = new Eng_ModelAction()
                     {
                         id_jobitem = Convert.ToInt32(postalCode["JobName"][i]),
                         id_depmodel = Convert.ToInt32(postalCode["IdListDepart"]),
                         empid = postalCode["JobInName"][i].ToString(),
                         name_responsible = postalCode["JobNameIn"][i].ToString(),
                         emailsend = postalCode["JobEmail"][i].ToString(),
                         plandate = postalCode["JobDate"][i].ToString(),
                         comment_admin = postalCode["JobCom"][i].ToString(),
                         dateadd_amin = DateTime.Now.ToString(),
                         status_job = "0"



                     };

                     context.Eng_ModelAction.Add(std);



                     context.SaveChanges();
                 }
                 foreach (var getI in get1)
                 {


                     var cusname = getI.ad1.c.cus_name;
                     var programname = getI.ad1.addnm.nm.customer_partname;
                     var masterdrawing = getI.ms.model_drawingid;
                     var modelmastername = getI.ad1.addnm.nm.model_name;
                     var partmastername = getI.ad1.addnm.nm.mode_partname;
                     var othermaster = getI.ad1.addnm.nm.otherdetail;


                     var subdrawing = getI.ms.drawing;
                     var submodelname = getI.ms.modelname;
                     var subpartname = getI.ms.partname;
                     var subinfo = getI.ms.other_commment;

                     var namejob = postalCode["NameJob"][i];
                     var namecom = postalCode["JobCom"][i].ToString();
                    
                         try
                         {
                             EASendMail.SmtpMail oMail = new EASendMail.SmtpMail("TryIt");

                             // Set sender email address, please change it to yours
                             oMail.From = "newmodel@ogura-thai.com";
                             // Set recipient email address, please change it to yours
                             oMail.To = postalCode["JobEmail"][i].ToString();
                             oMail.Cc = "p-boonhor@ogura-thai.com";

                             // Set email subject
                             oMail.Subject = "Engineer New Model || Drawing :" + masterdrawing;
                             // Set email body
                             oMail.HtmlBody = "<h4>Dear '" + postalCode["JobNameIn"][i].ToString() + "</h4>" +
                               "Please update information according to the assignment in new model drawing number  : " + modelmastername +
                                "<br/>"+
                                 "<br/>" +
                               "<table  style='font-size:14px; margin-top:5px; border: 1px solid black; border-collapse: collapse ;width:100%'>" +
                                    "<thead style='background-color:#1E1E1E; color:#fff; '>" +
                                    "<tr>"+
                                    "<th colspan='5'>MAGNET CLUCTH DETAILS</th>" +
                                    "</tr>" +
                                    "</thead>" +
                                    "<tr style='background-color:#3E3E42;color:#fff;'>" +
                                    "<td align='center' style=' border: 1px solid black; '>Program Name</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Customer Name</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Drawing#</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Model Name</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Part Name</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + programname + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + cusname + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + masterdrawing + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + modelmastername + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + partmastername + "</td>" +
                                    "</tr>" +
                                "</table>" +
                                "<br/>" +
                               
                                "<hr>"+
                                "<br/>" +
                              
                                "<table style='font-size:14px; margin-top:5px; border: 1px solid black; border-collapse: collapse ;width:100%'>" +
                                    "<thead style='background-color:#0078D4; color:#fff; '>" +
                                    "<tr>" +
                                    "<th colspan='5'>ASSIGNMENT DISCRIPTION</th>" +
                                    "</tr>" +
                                    "</thead> " +
                                    "<tr style='background-color:#50D9FF; color:#fff;'>" +
                                    "<td align='center' style=' border: 1px solid black; '>Drawing#</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Model Name</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Part Name</td>" +
                                    
                                    "<td align='center' style=' border: 1px solid black; '>Job</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>Comment</td>" +
                                    "</tr>" +
                                    "<tr>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + subdrawing + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + submodelname + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + subpartname + "</td>" +
                                    
                                    "<td align='center' style=' border: 1px solid black; '>" + namejob + "</td>" +
                                    "<td align='center' style=' border: 1px solid black; '>" + namecom + "</td>" +
                                    "</tr>" +
                                "</table>" +
                               "<br/>" +
                               "<br/>" +
                               "<br/>" +
                               "<a href='192.168.20.244/Action' >Click here to go to the website.!!</a>" +
                               "<br/>" +
                               "<br/>" + "*If you have any questions, please contact the engineer department." +
                               "<br/>" + "*This Email Send Automatic You can't reply this e-mail."


                               ;




                             // Set SMTP server address to "".
                             SmtpServer oServer = new SmtpServer("");

                             // Do not set user authentication
                             // Do not set SSL connection

                             Console.WriteLine("start to send email directly ...");

                             EASendMail.SmtpClient oSmtp = new EASendMail.SmtpClient();
                             oSmtp.SendMail(oServer, oMail);

                             Console.WriteLine("email was sent successfully!");
                         }
                         catch (Exception ep)
                         {
                             Console.WriteLine("failed to send email with the following error:");
                             Console.WriteLine(ep.Message);
                         }

                     }

                 

             }



             return Json(String.Format("E-mail was sent successfully!"));
          

        }

        public ActionResult AddDepartAction(string masterkey, string nodekey,string lid)
        {

            Array ModeldepartlistArray;
            List<ModelShowDepartList> modelshowdepartlist = new List<ModelShowDepartList>();

            var getmodeldepartlist = dbObjz.Eng_AddDepartmentModel
                .Join(dbObjz.Departments, ea => ea.dept_id, d => d.dept_id, (ea, d) => new { ea, d })
                .Where(e => e.ea.model_masterdrawingid == masterkey && e.ea.model_subnew == lid)
                .ToList();


            foreach (var modelpart in getmodeldepartlist)
            {
                modelshowdepartlist.Add(new NewModelEx_S1.Models.ModelShowDepartList
                {
                    DepartListId = modelpart.ea.adddpmodel_id.ToString(),
                    DepartCode = modelpart.ea.dept_id.ToString(),
                    DepartName = modelpart.d.department1,
                    DepartStatus = modelpart.ea.status_appord,
                    DepartDate = modelpart.ea.date_add

                });



            }

            ModeldepartlistArray = modelshowdepartlist.ToArray();


            ViewBag.ShowModeldepartList = ModeldepartlistArray;

            ViewBag.masterkey = masterkey;
            ViewBag.nodekey = lid ;

            depart();


            return View();
        }
        
        [HttpPost]
        public ActionResult AddDepartAction()
        {

            var keydrawing = HttpContext.Request.Form["masterdrawingpost"].Trim();
            var drawing = HttpContext.Request.Form["subdrawingpost"].Trim();
            var depart = HttpContext.Request.Form["departpost"];



            using (var context = new OCTIIS_WEBAPPEntities3())
            {

                var std = new Eng_AddDepartmentModel()
                {

                    model_masterdrawingid = keydrawing,
                    model_subnew = drawing,
                    dept_id = Convert.ToInt32(depart),
                    status_appord = "0",
                    date_add = DateTime.Now.ToString()




                };

                context.Eng_AddDepartmentModel.Add(std);



                context.SaveChanges();

            }


            return RedirectToAction("AddDepartAction", "Home", new { masterkey = keydrawing, lid = drawing });
        }


        public ActionResult DepartActionModelDataJob(string idModelAction, string masterkey, string nodekey, string idlist)
        {   
            /*level-1*/

            Array array;


            List<ModelNew> modelnew = new List<ModelNew>();


            var showmodelnew = dbObjz.Eng_newModel
                .Join(dbObjz.Eng_Customer, en => en.id_customer.ToString(), ec => ec.cus_id.ToString(), (en, ec) => new { en, ec })
                .Join(dbObjz.Eng_Clusifiation, enec => enec.en.status_modelid.ToString(), ecl => ecl.id.ToString(), (enec, ecl) => new { enec, ecl })
                .Where(e => e.enec.en.model_drawingid.ToString() == masterkey )
                .ToList();

            foreach (var showmodel in showmodelnew)
            {




                modelnew.Add(new ModelNew
                {

                    ProgramName = showmodel.enec.en.customer_partname,
                    ModelDrawing = showmodel.enec.en.model_drawingid.Trim(),
                    ModelName = showmodel.enec.en.model_name,
                    PartName = showmodel.enec.en.mode_partname,
                    ModelDate = showmodel.enec.en.model_datedoc,
                    CustomerName = showmodel.enec.ec.cus_name,
                    OtherInfo = showmodel.enec.en.otherdetail,
                    Classification = showmodel.ecl.name_clusftion


                });



            }
            array = modelnew.ToArray();


            ViewBag.DetailMasterLevelI = array;
           

            /*level-2*/
            Array ModelPartArray;
            List<ModelPart> modelpartget = new List<ModelPart>();
            List<string> drawingnode = new List<string>();
            var getmodelpart = dbObjz.Eng_ModelSub
                .Select(ems => new { ems.model_drawingid, ems.drawing, ems.modelname, ems.partname, ems.revision, ems.other_commment,ems.model_subid })
                .Where(ems => ems.model_drawingid == masterkey && ems.model_subid.ToString() == nodekey)
                .ToList();

            foreach (var modelpart in getmodelpart)
            {
                modelpartget.Add(new NewModelEx_S1.Models.ModelPart
                {
                    MotherDrawing = modelpart.model_drawingid,
                    NodeDrawing = modelpart.drawing,
                    NodeModelName = modelpart.modelname,
                    NodePartName = modelpart.partname,
                    NodeRevision = modelpart.revision,
                    NodeOtherComment = modelpart.other_commment


                });


                drawingnode.Add(modelpart.drawing);



            }

            drawingnode.ToArray();

            ModelPartArray = modelpartget.ToArray();

            ViewBag.DetailMasterLevelII = ModelPartArray;


            /*level-3*/
            Array ModeldepartActionDataArray;
            List<ModelDepartJob> modelshowdepartjob = new List<ModelDepartJob>();

            var getmodeldepartDatajob = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjz.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
                .Where(ee => ee.emej.em.id_depmodel.ToString() == idlist && ee.emej.em.id_modelact.ToString() == idModelAction)
                .ToList();

            getmodeldepartDatajob.Count();
            foreach (var modelpartjob in getmodeldepartDatajob)
            {
                modelshowdepartjob.Add(new ModelDepartJob
                {
                    ModelId = modelpartjob.emej.em.id_modelact.ToString(),
                    DepartCode = modelpartjob.emej.em.id_depmodel.ToString(),
                    DepartJobId = modelpartjob.emej.em.id_jobitem.ToString(),
                    DepartJobName = modelpartjob.emej.ej.job_itemname,
                    DepartJobStatus = modelpartjob.emej.em.status_job,
                    DepartJobDueDate = modelpartjob.emej.em.plandate,
                    DepartJobActionName = modelpartjob.emej.em.name_responsible,
                    DepartJobActionEmail = modelpartjob.emej.em.emailsend,
                    DepartJobActionComment = modelpartjob.emej.em.comment_admin,
                    DepartJobActionEmpCode = modelpartjob.emej.em.empid,
                    DepartJobActionDocumentPDF = modelpartjob.emej.em.name_docpdf,
                    DepartJobActionDocumentExcel = modelpartjob.emej.em.name_docexcel,
                    DepartJobActionImage = modelpartjob.emej.em.name_image,
                    DepartJobActionCommentUser = modelpartjob.emej.em.comment_user,
                    DepartJobActionCommentReject = modelpartjob.emej.em.comment_admin_reject
                });

                ViewBag.statusshow = modelpartjob.emej.em.status_job;
                ViewBag.modelId = modelpartjob.emej.em.id_modelact.ToString();

            }

            ModeldepartActionDataArray = modelshowdepartjob.ToArray();

            ViewBag.ShowModelpartDataJob = ModeldepartActionDataArray;



            ViewBag.masterkey = masterkey;
            ViewBag.nodekey = nodekey;
            ViewBag.idlist = idlist;



            return View();
        }

        [HttpPost]
        public ActionResult UpComment(string test, string com, string status)
        {
            using (var context = new OCTIIS_WEBAPPEntities3())
            {


                var upother = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);

                foreach (var up in upother)
                {

                    up.comment_admin_reject = com;
                    up.status_job = status;
                    

                }

                UpdateModel(upother);
                context.SaveChanges();

            }


            return Json("Successfully!");
        }

        public void depart()
        {

            Array arrayDepart;

            List<NewModelEx_S1.Models.ModelDetailDepart> modeldepart = new List<NewModelEx_S1.Models.ModelDetailDepart>();


            

                var depart = dbObjz.Departments.Select(d => new { d.dept_id, d.department1 }).ToList();
                    
                
                foreach (var departItem in depart)
                    {
                        modeldepart.Add(new NewModelEx_S1.Models.ModelDetailDepart
                        {

                            DepartCode = departItem.dept_id.ToString(),
                            DepartName = departItem.department1


                        });

                    }

            
        
            arrayDepart = modeldepart.ToArray();

            ViewBag.ShowModelDepart = arrayDepart;
           


        }

        public void Customer()
        {
            Array arrayCustomer;


            List<NewModelEx_S1.Models.ModelCustomer> modelcustomer = new List<NewModelEx_S1.Models.ModelCustomer>();

           

                var customer = dbObjz.Eng_Customer.Select(c => new {c.cus_id,c.cus_name }).ToList();


                foreach (var customerItem in customer)
                {

                    modelcustomer.Add(new NewModelEx_S1.Models.ModelCustomer
                    {
                        CustomerCode = customerItem.cus_id.ToString(),
                        CustomerName = customerItem.cus_name



                    });

                }

            

            arrayCustomer = modelcustomer.ToArray();

            ViewBag.ShowModelCustomer = arrayCustomer;
        }

        public void Classification()
        {
            Array arrayClassification;

            List<NewModelEx_S1.Models.ModelClassification> modelclassification = new List<NewModelEx_S1.Models.ModelClassification>();

            
                var customer = dbObjz.Eng_Clusifiation.Select(c => new { c.id, c.name_clusftion }).ToList();


                foreach (var classificationItem in customer)
                {

                    modelclassification.Add(new NewModelEx_S1.Models.ModelClassification
                    {
                        ClassiCode = classificationItem.id.ToString(),
                        ClassiName = classificationItem.name_clusftion



                    });

                }

            

            arrayClassification = modelclassification.ToArray();

            ViewBag.ShowModelClassification = arrayClassification;

        }

        public void showModel()
        {


            Array array;


            List<ModelNew> modelnew = new List<ModelNew>();

            
                var showmodelnew = dbObjz.Eng_newModel
                    .Join(dbObjz.Eng_Customer, en => en.id_customer.ToString(), ec => ec.cus_id.ToString(), (en, ec) => new { en, ec })
                    .Join(dbObjz.Eng_Clusifiation, enec => enec.en.status_modelid.ToString(), ecl => ecl.id.ToString(), (enec, ecl) => new { enec, ecl })
                    .ToList();

                foreach (var showmodel in showmodelnew)
                {


                  

                        modelnew.Add(new ModelNew
                        {

                            ProgramName = showmodel.enec.en.customer_partname,
                            ModelDrawing = showmodel.enec.en.model_drawingid.Trim(),
                            ModelName = showmodel.enec.en.model_name,
                            PartName = showmodel.enec.en.mode_partname,
                            ModelDate = showmodel.enec.en.model_datedoc,
                            CustomerName = showmodel.enec.ec.cus_name,
                            OtherInfo = showmodel.enec.en.otherdetail,
                            Classification = showmodel.ecl.name_clusftion
                            

                        });

                    

                }
                array = modelnew.ToArray();


                ViewBag.ShowModelNew = array;
                ViewData["showmodelnew"] = array;

            

        }

      

       

        public void JobList()
        {
            Array JobListarray;

            List<ModelJobItemList> jobitemlist = new List<ModelJobItemList>();

            var getjoblist = dbObjz.Eng_JobItemList.ToList();

            foreach(var joblist in getjoblist) {

                        jobitemlist.Add(new ModelJobItemList{
            
                    SelectItemId = joblist.job_itemid.ToString(),
                    SelectItem = joblist.job_itemname
            
            
            
            
                    });
  
            }

            JobListarray = jobitemlist.ToArray();


            ViewBag.JobListShow = JobListarray;
            

        }

        [HttpPost]
        public void ActionModelData(string idModelAction,string masterkey, string nodekey, string idlist)
        {

            Array ModeldepartActionDataArray;
            List<ModelDepartJob> modelshowdepartjob = new List<ModelDepartJob>();

            var getmodeldepartDatajob = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjz.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
                .Where(ee => ee.emej.em.id_depmodel.ToString() == idlist && ee.emej.em.id_modelact.ToString() == idModelAction)
                .ToList();

            getmodeldepartDatajob.Count();
            foreach (var modelpartjob in getmodeldepartDatajob)
            {
                modelshowdepartjob.Add(new ModelDepartJob
                {
                    ModelId = modelpartjob.emej.em.id_modelact.ToString(),
                    DepartCode = modelpartjob.emej.em.id_depmodel.ToString(),
                    DepartJobId = modelpartjob.emej.em.id_jobitem.ToString(),
                    DepartJobName = modelpartjob.emej.ej.job_itemname,
                    DepartJobStatus = modelpartjob.emej.em.status_job,
                    DepartJobDueDate = modelpartjob.emej.em.plandate,
                    DepartJobActionName = modelpartjob.emej.em.name_responsible,
                    DepartJobActionEmail = modelpartjob.emej.em.emailsend,
                    DepartJobActionComment = modelpartjob.emej.em.comment_admin

                });



            }

            ModeldepartActionDataArray = modelshowdepartjob.ToArray();

            ViewBag.ShowModelpartDataJob = ModeldepartActionDataArray;



            ViewBag.masterkey = masterkey;
            ViewBag.nodekey = nodekey;
            ViewBag.idlist = idlist;
        }


        public void UserList()
        {
            Array UserListarray;

            List<ModelUser> useritemlist = new List<ModelUser>();

            var getuserlist = dbObjz.Eng_AccountUser.ToList();

            foreach (var userlist in getuserlist)
            {

                useritemlist.Add(new ModelUser
                {
                    EmpId = userlist.id_employee,
                    EmpUserName = userlist.username,
                    EmpFName = userlist.name,
                    EmpLName = userlist.lastname,
                    EmpDep = userlist.id_dept.ToString(),
                    EmpPos =userlist.id_position.ToString(),
                    EmpPassWord = userlist.password,
                    EmpNickName = userlist.nickname,
                    EmpEmail = userlist.email



                });



            }

            UserListarray = useritemlist.ToArray();


            ViewBag.UserListShow = UserListarray;


        }

        public ActionResult DeleteModelAction(string masterkey, string nodekey, string idlist, string idDeleteModel)
        {



            var del = dbObjz.Eng_ModelAction.FirstOrDefault(s => s.id_modelact.ToString() == idDeleteModel);


            if (del != null)
            {

                dbObjz.Eng_ModelAction.Remove(del);
                dbObjz.SaveChanges();
            }
               
            

                       


            return RedirectToAction("ModelDepartAction", "Home", new { masterkey = masterkey, nodekey = nodekey, idlist = idlist ,v="1"});
        }

        public ActionResult DeleteDepartAction(string myJSONs)
        {
            JObject postalCode = new JObject();

            postalCode = JObject.Parse(myJSONs);

            var idlist = postalCode["Idlist"].ToString();

            var delModelAct = dbObjz.Eng_ModelAction.Where(s => s.id_depmodel.ToString() == idlist);
               

            if (delModelAct != null)
            {

                dbObjz.Eng_ModelAction.RemoveRange(delModelAct);
                dbObjz.SaveChanges();
            }


            var delDepartAct = dbObjz.Eng_AddDepartmentModel.FirstOrDefault(s => s.adddpmodel_id.ToString() == idlist);


            if (delDepartAct != null)
            {

                dbObjz.Eng_AddDepartmentModel.Remove(delDepartAct);
                dbObjz.SaveChanges();
            }




            return Json(String.Format("Delete Successfully"));

        }

        public ActionResult DeletePartAction(string myJSONs)
        {
            JObject postalCode = new JObject();

            postalCode = JObject.Parse(myJSONs);

            var NodekeyDel = postalCode["Nodekey"].ToString();


                var delle = dbObjz.Eng_AddDepartmentModel.Where(s => s.model_subnew == NodekeyDel).ToList();

            foreach (var id in delle)
            {

                var idd = id.adddpmodel_id;

                var delModelAct = dbObjz.Eng_ModelAction.Where(s => s.id_depmodel.ToString() == idd.ToString());


                if (delModelAct != null)
                {

                    dbObjz.Eng_ModelAction.RemoveRange(delModelAct);
                    dbObjz.SaveChanges();
                }

            }



            var delDepartAct = dbObjz.Eng_AddDepartmentModel.Where(s => s.model_subnew == NodekeyDel);


            if (delDepartAct != null)
            {


                dbObjz.Eng_AddDepartmentModel.RemoveRange(delDepartAct);
                dbObjz.SaveChanges();
            }

         

        


         var delpartAct = dbObjz.Eng_ModelSub.FirstOrDefault(s => s.model_subid.ToString() == NodekeyDel);

         if (delpartAct != null)
         {

             dbObjz.Eng_ModelSub.Remove(delpartAct);
             dbObjz.SaveChanges();


         }



         return Json(String.Format("Delete Successfully"));
        }


        public ActionResult NewModelDelete(string myJSONs)
        {

            JObject postalCode = new JObject();

            postalCode = JObject.Parse(myJSONs);

            var MasterKeydel = postalCode["Masterkey"].ToString();



            var subid = dbObjz.Eng_ModelSub.Where(m => m.model_drawingid == MasterKeydel).ToList();


            foreach (var mas in subid)
            {

                var NodekeyDel = mas.model_subid;


                var delle = dbObjz.Eng_AddDepartmentModel.Where(s => s.model_subnew == NodekeyDel.ToString()).ToList();

                foreach (var id in delle)
                {

                    var idd = id.adddpmodel_id;

                    var delModelAct = dbObjz.Eng_ModelAction.Where(s => s.id_depmodel.ToString() == idd.ToString());


                    

                        dbObjz.Eng_ModelAction.RemoveRange(delModelAct);
                        dbObjz.SaveChanges();
                    

                }



                var delDepartAct = dbObjz.Eng_AddDepartmentModel.Where(s => s.model_subnew == NodekeyDel.ToString());


               

                    dbObjz.Eng_AddDepartmentModel.RemoveRange(delDepartAct);
                    dbObjz.SaveChanges();
                






                var delpartAct = dbObjz.Eng_ModelSub.FirstOrDefault(s => s.model_subid.ToString() == NodekeyDel.ToString());

               

                    dbObjz.Eng_ModelSub.Remove(delpartAct);
                    dbObjz.SaveChanges();


            }
            var delmaster = dbObjz.Eng_newModel.FirstOrDefault(ma => ma.model_drawingid.Trim() == MasterKeydel);



            dbObjz.Eng_newModel.Remove(delmaster);
            dbObjz.SaveChanges();

            return Json(String.Format("Delete Successfully"));
        }


        /*
        public ActionResult UpdateModelAction(string masterkey, string nodekey, string idlist, string idUpdateModel)
        {
            JobList();
            UserList();
            Array ModeldepartjoblistEditArray;
            List<ModelDepartJob> modelshowdepartjob = new List<ModelDepartJob>();

            var getmodeldepartlistjob = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjz.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
                .Where(ee => ee.emej.em.id_modelact.ToString() == idUpdateModel)
                .ToList();

            getmodeldepartlistjob.Count();
            foreach (var modelpartjob in getmodeldepartlistjob)
            {
                modelshowdepartjob.Add(new ModelDepartJob
                {
                    ModelId = modelpartjob.emej.em.id_modelact.ToString(),
                    DepartCode = modelpartjob.emej.em.id_depmodel.ToString(),
                    DepartJobId = modelpartjob.emej.em.id_jobitem.ToString(),
                    DepartJobName = modelpartjob.emej.ej.job_itemname,
                    DepartJobStatus = modelpartjob.emej.em.status_job,
                    DepartJobDueDate = modelpartjob.emej.em.plandate,
                    DepartJobActionName = modelpartjob.emej.em.name_responsible,
                    DepartJobActionEmail = modelpartjob.emej.em.emailsend,
                    DepartJobActionComment = modelpartjob.emej.em.comment_admin

                });



            }

            ModeldepartjoblistEditArray = modelshowdepartjob.ToArray();

            ViewBag.ShowModelpartjoblist = ModeldepartjoblistEditArray;


            //var del = dbObjz.Eng_ModelAction.Single(s => s.id_modelact.ToString() == idUpdateModel);

            //if (del != null)
            //{

            //    del.id_jobitem =  3 ;
            //    dbObjz.SaveChanges();
            //}





            return View();
           // return RedirectToAction("ModelDepartAction", "Home", new { masterkey = masterkey, nodekey = nodekey, idlist = idlist });
        }

*/

        //public void querydepartList(string masterid,List<string> drawingnode)
        //{



        //    Array ModeldepartlistArray;
        //    List<ModelShowDepartList> modelshowdepartlist = new List<ModelShowDepartList>();

        //    var getmodeldepartlist = dbObjz.Eng_AddDepartmentModel
        //        .Join(dbObjz.Departments, ea => ea.dept_id, d => d.dept_id, (ea, d) => new { ea, d })
        //        .Where(e => e.ea.model_masterdrawingid == masterid && e.ea.model_subnew.Contains(drawingnode.ToString()))
        //        .ToList();


        //    foreach (var modelpart in getmodeldepartlist)
        //    {
        //        modelshowdepartlist.Add(new NewModelEx_S1.Models.ModelShowDepartList
        //        {

        //            DepartCode = modelpart.ea.dept_id.ToString(),
        //            DepartName = modelpart.d.department1,
        //            DepartStatus = modelpart.ea.status_appord,
        //            DepartDate = modelpart.ea.date_add

        //        });



        //    }

        //    ModeldepartlistArray = modelshowdepartlist.ToArray();


        //    ViewBag.ShowModeldepartList = ModeldepartlistArray;

            
        //}

        public string Encrypt()
        {

            try
            {
                string textToEncrypt = "";
                string ToReturn = "";
                string publickey = "santhosh";
                string secretkey = "engineer";
                byte[] secretkeyByte = { };
                secretkeyByte = System.Text.Encoding.UTF8.GetBytes(secretkey);
                byte[] publickeybyte = { };
                publickeybyte = System.Text.Encoding.UTF8.GetBytes(publickey);
                MemoryStream ms = null;
                CryptoStream cs = null;
                byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(textToEncrypt);
                using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
                {
                    ms = new MemoryStream();
                    cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                    cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                    cs.FlushFinalBlock();
                    ToReturn = Convert.ToBase64String(ms.ToArray());
                }
                return ToReturn;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message, ex.InnerException);
            }
        }



        public ActionResult ReportEngineer(string lid)
        {
            Array ReportEngineer,Part;

            List<ModelDepartJob> report = new List<ModelDepartJob>();
            List<ModelPart> modelpart = new List<ModelPart>();

            var queryNum0 = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_AddDepartmentModel, ez => ez.id_depmodel, ex => ex.adddpmodel_id, (ez, ex) => new { ez, ex })
                .Join(dbObjz.Departments, aa => aa.ex.dept_id, ee => ee.dept_id, (aa, ee) => new { aa, ee })
                .Where(eq => eq.aa.ex.model_subnew == lid)
                .Select(a => a.ee.department1)
                .Distinct()
                .ToList();
               
               
           

            var queryReport = dbObjz.Eng_ModelAction
                .Join(dbObjz.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjz.Eng_AddDepartmentModel, emej => emej.em.id_depmodel, ea => ea.adddpmodel_id, (emej, ea) => new { emej, ea })
                .Join(dbObjz.Departments, emc => emc.ea.dept_id, d => d.dept_id, (emc, d) => new { emc, d })
                .Where(va => va.emc.ea.model_subnew == lid)
                .OrderBy(v => v.d.dept_id)
                .ToList();
            foreach (var reportjob in queryReport)
            {

                report.Add(new ModelDepartJob
                {
                    ModelId = reportjob.emc.emej.em.id_modelact.ToString(),
                    DepartCode = reportjob.d.department1,
                    DepartJobDueDate = reportjob.emc.emej.em.plandate,
                    DepartJobName =reportjob.emc.emej.ej.job_itemname,
                    DepartJobActionName = reportjob.emc.emej.em.name_responsible,
                    DepartJobStatus = reportjob.emc.emej.em.status_job

                    

                });
               
            
            }

            var que = dbObjz.Eng_ModelSub.Where(es => es.model_subid.ToString() == lid).ToList();

            foreach (var quer in que)
            { 
            modelpart.Add(new ModelPart{
            
            
                NodeDrawing = quer.drawing,
                NodeModelName = quer.modelname,
                NodePartName = quer.partname


            
            
            });
            

            }

            Part = modelpart.ToArray();
            ViewBag.reportP = Part;




            ReportEngineer = report.ToArray();
            ViewBag.reportE = ReportEngineer;






            return View();
        }

      
    }
}