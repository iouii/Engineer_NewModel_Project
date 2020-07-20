using NewModelEx_S1.Context;
using NewModelEx_S1.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;


namespace NewModelEx_S1.Controllers.Engineer
{
    public class ActionController : Controller
    {
        OCTIIS_WEBAPPEntities3 dbObjx = new OCTIIS_WEBAPPEntities3();

        // GET: Engineer
        public  HomeController _AdminController;


        public ActionResult Index(HomeController AdminController)
        {



            showModel();
            
            

            return View();
        }

        public ActionResult ModelDetial(string id)
        {

            Array ModelPartArray;
            List<ModelPart> modelpartget = new List<ModelPart>();
            List<string> drawingnode = new List<string>();
            var getmodelpart = dbObjx.Eng_ModelSub
                .Select(ems => new { ems.model_drawingid, ems.drawing, ems.modelname, ems.partname, ems.revision, ems.other_commment, ems.model_subid })
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

        public ActionResult AddDepartAction(string masterkey, string nodekey, string lid)
        {

            Array ModeldepartlistArray;
            List<ModelShowDepartList> modelshowdepartlist = new List<ModelShowDepartList>();

            var getmodeldepartlist = dbObjx.Eng_AddDepartmentModel
                .Join(dbObjx.Departments, ea => ea.dept_id, d => d.dept_id, (ea, d) => new { ea, d })
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
            ViewBag.nodekey = lid;

            depart();

            return View();
        }
        public ActionResult ModelDepartAction(string masterkey, string nodekey, string idlist)
        {



            Array ModeldepartjoblistArray;
            List<ModelDepartJob> modelshowdepartjob = new List<ModelDepartJob>();

            var getmodeldepartlistjob = dbObjx.Eng_ModelAction
                .Join(dbObjx.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjx.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
                .Where(ee => ee.emej.em.id_depmodel.ToString() == idlist)
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

            ModeldepartjoblistArray = modelshowdepartjob.ToArray();

            ViewBag.ShowModelpartjoblist = ModeldepartjoblistArray;


            ViewBag.masterkey = masterkey;
            ViewBag.nodekey = nodekey;
            ViewBag.idlist = idlist;


            return View();

        }

        public ActionResult DepartActionModelDataJob(string idModelAction, string masterkey, string nodekey, string idlist)
        {
            /*level-1*/

            Array array;


            List<ModelNew> modelnew = new List<ModelNew>();


            var showmodelnew = dbObjx.Eng_newModel
                .Join(dbObjx.Eng_Customer, en => en.id_customer.ToString(), ec => ec.cus_id.ToString(), (en, ec) => new { en, ec })
                .Join(dbObjx.Eng_Clusifiation, enec => enec.en.status_modelid.ToString(), ecl => ecl.id.ToString(), (enec, ecl) => new { enec, ecl })
                .Where(e => e.enec.en.model_drawingid.ToString() == masterkey)
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
            var getmodelpart = dbObjx.Eng_ModelSub
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

            var getmodeldepartDatajob = dbObjx.Eng_ModelAction
                .Join(dbObjx.Eng_JobItemList, em => em.id_jobitem, ej => ej.job_itemid, (em, ej) => new { em, ej })
                .Join(dbObjx.Eng_AccountUser, emej => emej.em.empid, ea => ea.id_employee, (emej, ea) => new { emej, ea })
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

         
        public void depart()
        {

            Array arrayDepart;

            List<NewModelEx_S1.Models.ModelDetailDepart> modeldepart = new List<NewModelEx_S1.Models.ModelDetailDepart>();




            var depart = dbObjx.Departments.Select(d => new { d.dept_id, d.department1 }).ToList();


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



            var customer = dbObjx.Eng_Customer.Select(c => new { c.cus_id, c.cus_name }).ToList();


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


            var customer = dbObjx.Eng_Clusifiation.Select(c => new { c.id, c.name_clusftion }).ToList();


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


            var showmodelnew = dbObjx.Eng_newModel
                .Join(dbObjx.Eng_Customer, en => en.id_customer.ToString(), ec => ec.cus_id.ToString(), (en, ec) => new { en, ec })
                .Join(dbObjx.Eng_Clusifiation, enec => enec.en.status_modelid.ToString(), ecl => ecl.id.ToString(), (enec, ecl) => new { enec, ecl })
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

            var getjoblist = dbObjx.Eng_JobItemList.ToList();

            foreach (var joblist in getjoblist)
            {

                jobitemlist.Add(new ModelJobItemList
                {

                    SelectItemId = joblist.job_itemid.ToString(),
                    SelectItem = joblist.job_itemname




                });

            }

            JobListarray = jobitemlist.ToArray();


            ViewBag.JobListShow = JobListarray;


        }

        public void UserList()
        {
            Array UserListarray;

            List<ModelUser> useritemlist = new List<ModelUser>();

            var getuserlist = dbObjx.Eng_AccountUser.ToList();

            foreach (var userlist in getuserlist)
            {

                useritemlist.Add(new ModelUser
                {
                    EmpId = userlist.id_employee,
                    EmpUserName = userlist.username,
                    EmpFName = userlist.name,
                    EmpLName = userlist.lastname,
                    EmpDep = userlist.id_dept.ToString(),
                    EmpPos = userlist.id_position.ToString(),
                    EmpPassWord = userlist.password,
                    EmpNickName = userlist.nickname,
                    EmpEmail = userlist.email



                });



            }

            UserListarray = useritemlist.ToArray();


            ViewBag.UserListShow = UserListarray;


        }



        [HttpPost]
        public ActionResult UploadFiles(string test,string com)
        {


            if (Request.Files.Count > 0)
            {
                try
                {
                    string fname, fnameup, type, fnamedel;
                    //  Get all files from Request object  
                    HttpFileCollectionBase files = Request.Files;
                    for (int i = 0; i < files.Count; i++)
                    {
                        //string path = AppDomain.CurrentDomain.BaseDirectory + "Uploads/";  
                        //string filename = Path.GetFileName(Request.Files[i].FileName);  

                        HttpPostedFileBase file = files[i];
                       

                        // Checking for Internet Explorer  
                        if (Request.Browser.Browser.ToUpper() == "IE" || Request.Browser.Browser.ToUpper() == "INTERNETEXPLORER")
                        {
                            string[] testfiles = file.FileName.Split(new char[] { '\\' });
                         
                            
                            fname = testfiles[testfiles.Length - 1];
                            fnameup = file.FileName;
                            
                        }
                        else
                        {
                            fname = file.FileName;

                            fnameup = file.FileName;
                           
                        }

                        // Get the complete folder path and store the file inside it. 
                        string[] filetype = file.FileName.Split(new char[] { '.' });

                        type = filetype[1];
                       
                        switch (type){

                            case "pdf":

                                fname = Path.Combine(Server.MapPath("~/File/PDF/"), fname);
                                 file.SaveAs(fname);
                                 fnameup = file.FileName;

                                    using (var context = new OCTIIS_WEBAPPEntities3())
                                    {

                                       

                                        var uplist = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);

                                        foreach (var del in uplist)
                                        {

                                            var delname = del.name_docpdf;
                                          
                                            fnamedel = Server.MapPath("~/File/PDF/"+delname);
                                           
                                            try
                                            {

                                                System.IO.File.Delete(fnamedel);
                                            }
                                            catch(Exception e)
                                            {
                                                Console.WriteLine("error", e.Message);
                                            }
                                        
                                        }
                                        
                                        

                                        foreach (var up in uplist)
                                        {

                                            up.name_docpdf = fnameup ;
                                           
                                
                                        }

                                        UpdateModel(uplist);
                                        context.SaveChanges();

                                    }

                                break;

                              case "xls":                               
                              case "csv":
                              case "xml":
                              case "xlsm":
                              case "xlsx":

                                fname = Path.Combine(Server.MapPath("~/File/Excel/"), fname);
                                file.SaveAs(fname);
                                     fnameup = file.FileName;

                                    using (var context = new OCTIIS_WEBAPPEntities3())
                                    {


                                        var uplist = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);

                                        foreach (var del in uplist)
                                        {

                                            var delname = del.name_docexcel;

                                            fnamedel = Server.MapPath("~/File/Excel/"+delname);

                                            try
                                            {

                                                System.IO.File.Delete(fnamedel);
                                            }
                                            catch (Exception e)
                                            {
                                                Console.WriteLine("error", e.Message);
                                            }

                                        }
                                        foreach (var up in uplist)
                                        {

                                            up.name_docexcel = fnameup ;
                                           
                                
                                        }

                                        UpdateModel(uplist);
                                        context.SaveChanges();

                                    }
                                break;
                             
                              case "png":
                              case "jpg":
                              case "jpge":                              
                              case "gif":
                              

                                fname = Path.Combine(Server.MapPath("~/File/Picture/"), fname);
                                file.SaveAs(fname);
                                fnameup = file.FileName;

                                using (var context = new OCTIIS_WEBAPPEntities3())
                                {


                                    var uplist = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);

                                    foreach (var del in uplist)
                                    {

                                        var delname = del.name_image;

                                        fnamedel = Server.MapPath("~/File/Picture/"+delname);

                                        try
                                        {

                                            System.IO.File.Delete(fnamedel);
                                        }
                                        catch (Exception e)
                                        {
                                            Console.WriteLine("error", e.Message);
                                        }

                                    }
                                    foreach (var up in uplist)
                                    {

                                        up.name_image = fnameup;
                                        

                                    }

                                    UpdateModel(uplist);
                                    context.SaveChanges();

                                }

                                break;

                      
                        }


                        using (var context = new OCTIIS_WEBAPPEntities3())
                        {


                            var upother = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);
                           

                            foreach (var up in upother)
                            {
                                var data = up.date_useradd;

                                if(data != null ){

                                up.comment_user = com;
                                up.status_job = "4";
                                up.date_userupdate = DateTime.Now.ToString();
                                }
                                else
                                {

                                    up.comment_user = com;
                                    up.status_job = "4";
                                    up.date_useradd = DateTime.Now.ToString();

                                }

                               

                            }

                            UpdateModel(upother);
                            context.SaveChanges();

                        }
                        
                       
                    }

                    // Returns message that successfully uploaded  
                    return Json("File Uploaded Successfully!");
                }
                catch (Exception ex)
                {
                    return Json("Error occurred. Error details: " + ex.Message);
                }
            }
            else
            {
                return Json("No files selected.");
            }
        }


        [HttpPost]
        public ActionResult UpComment(string test, string com)
        {
            using (var context = new OCTIIS_WEBAPPEntities3())
            {


                var upother = context.Eng_ModelAction.Where(e => e.id_modelact.ToString() == test);

                foreach (var up in upother)
                {

                 var data = up.date_useradd;

                 if (data != null)
                 {

                     up.comment_user = com;
                     up.status_job = "4";
                     up.date_userupdate = DateTime.Now.ToString();
                 }
                 else
                 {

                     up.comment_user = com;
                     up.status_job = "4";
                     up.date_useradd = DateTime.Now.ToString();

                 }

                }

                UpdateModel(upother);
                context.SaveChanges();

            }


            return Json("Successfully!");
        }

        


    }



}