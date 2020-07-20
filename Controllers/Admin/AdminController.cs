using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using NewModelEx_S1.Context;
using NewModelEx_S1.Models;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace NewModelEx_S1.Controllers
{
    public class AdminController : Controller
    {
        OCTIIS_WEBAPPEntities3 db = new OCTIIS_WEBAPPEntities3();
        // GET: Admin
        public ActionResult Index()
        {

            ViewData["Department"] = QueryDepartment();
            ViewData["Position"] = QueryPosition();
            return View();
        }
        public ActionResult AddUser()
        {

            ViewData["Department"] = QueryDepartment();
            ViewData["Position"] = QueryPosition();

            return View();
        }

        public ActionResult EditUser()
        {

            return View();
        }
        public ActionResult SettingPer()
        {
            ViewData["PerUser"] = QueryPer();
            return View();
        }
        public ActionResult AddSetPermission()
        {

            ViewData["PerUser"] = QueryPer();
            ViewData["PerMenu"] = QueryPerMenu();
            return View();

        }
        public ActionResult AddDeptPos()
        {
            return View();
        }

        [HttpPost]
        public JsonResult AddDeptPos(string data)
        {

            var listDep = QueryDepartment();
            return Json(listDep, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPos(string data)
        {

            var listPos = QueryPosition();
            return Json(listPos, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DeleteDataPermis(int idPermis)
        {

            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var deleteData = (from del in dbAdd.Eng_PermissionSub where del.id_persub == idPermis select del).FirstOrDefault();
                if (deleteData != null)
                {
                    dbAdd.Eng_PermissionSub.Remove(deleteData);
                }

                dbAdd.SaveChanges();
            }
            string message = "Delete success.";
            return Json(String.Format(message));
        }

        [HttpGet]
        public ActionResult EditSearchSetPermission(int idPermis)
        {
            ViewData["PerUser"] = QueryPer();
            ViewData["PerMenu"] = QueryPerMenu();

            var dbPermission = (from dbsup in db.Eng_PermissionSub
                                join dbMain in db.Eng_PermissionMainmenu on dbsup.id_mainmenu equals dbMain.id_mainmenu
                                join dbUser in db.Eng_PermissionUser on dbsup.id_permissionuser equals dbUser.Id_permissionUser
                                where dbsup.id_persub == idPermis

                                select new ModelPermission
                                {
                                    idPer = dbsup.id_persub.ToString(),
                                    PerNameMainmanu = dbMain.id_mainmenu.ToString(),
                                    PermissionName = dbUser.Id_permissionUser.ToString(),
                                    PerMainAction = dbsup.name_action,
                                    PerNameControl = dbsup.name_controler,
                                    PerNameFun = dbsup.name_function,
                                    PerStatus = dbsup.name_statusfc
                                }).ToList();

            ViewData["PermissionSystem"] = dbPermission;
            return View();
        }

        [HttpPost]
        public ActionResult EditSetPermission(string jsonData)
        {
            JObject objdata = new JObject();
            objdata = JObject.Parse(jsonData);
            var idper = Convert.ToInt32(objdata["txtSubId"].ToString());
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var updateData = (from update in dbAdd.Eng_PermissionSub where update.id_persub == idper select update).FirstOrDefault();
                if (updateData != null)
                {
                    updateData.name_function = objdata["txtfunName"].ToString();
                    updateData.name_controler = objdata["txtControlName"].ToString();
                    updateData.name_action = objdata["txtMainAction"].ToString();
                    updateData.id_permissionuser = Convert.ToInt32(objdata["txtperUser"].ToString());
                    updateData.name_statusfc = objdata["txtstatus"].ToString();
                    updateData.id_mainmenu = Convert.ToInt32(objdata["txtManuGrp"].ToString());
                }

                dbAdd.SaveChanges();
            }
            return Json(String.Format("Complete"));
        }

        [HttpPost]
        public ActionResult AddPermission(string jsonData)
        {
            JObject objJson = new JObject();
            objJson = JObject.Parse(jsonData);
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var savePermissions = new Eng_PermissionSub()
                {
                    name_function = objJson["txtfunName"].ToString(),
                    name_controler = objJson["txtControlName"].ToString(),
                    name_action = objJson["txtMainAction"].ToString(),
                    id_permissionuser = Convert.ToInt32(objJson["txtperUser"].ToString()),
                    name_statusfc = objJson["txtstatus"].ToString(),
                    id_mainmenu = Convert.ToInt32(objJson["txtManuGrp"].ToString())

                };
                dbAdd.Eng_PermissionSub.Add(savePermissions);
                dbAdd.SaveChanges();

            }

            return Json(String.Format("Complete"));
        }

        [HttpGet]
        public ActionResult EditUser(string idEmps)
        {
            ViewData["Department"] = QueryDepartment();
            ViewData["Position"] = QueryPosition();

            var pullUser = (from user in db.Eng_AccountUser
                            where user.id_employee == idEmps
                            select user).FirstOrDefault();

            List<ModelUser> modelUr = new List<ModelUser>();
            modelUr.Add(new ModelUser
            {
                EmpId = pullUser.id_employee,
                EmpFName = pullUser.name,
                EmpLName = pullUser.lastname,
                EmpDep = pullUser.id_dept.ToString(),
                EmpPos = pullUser.id_position.ToString(),
                EmpPassWord = DecryptPassword(pullUser.password),
                EmpUserName = pullUser.username,
                EmpEmail = pullUser.email,
                EmpNickName = pullUser.nickname,
                EmpPermission = pullUser.permission_id.ToString()
            });

            ViewData["UserData"] = modelUr;
            return View();
        }

        public string DecryptPassword(string cipherPassText)
        {
            string EncryptionKey = "OCTENDCODEDATA";
            byte[] cipherBytes = Convert.FromBase64String(cipherPassText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes
                (EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms,

                        encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(cipherBytes, 0, cipherBytes.Length);
                        cs.Close();
                    }
                    cipherPassText = Encoding.Unicode.GetString(ms.ToArray());
                }
            }
            return cipherPassText;
        }

        [HttpPost]
        public ActionResult SaveData(string JsonData)
        {
            JObject objdata = new JObject();
            objdata = JObject.Parse(JsonData);
            var encriptPw = encryptPassword(objdata["password"].ToString());
            var geta = objdata["idEmployee"].ToString();
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var saveadduser = new Eng_AccountUser()
                {
                    id_employee = objdata["idEmployee"].ToString(),
                    username = objdata["userName"].ToString(),
                    password = encriptPw,
                    name = objdata["nameUser"].ToString(),
                    lastname = objdata["lastName"].ToString(),
                    nickname = objdata["nickname"].ToString(),
                    id_dept = Convert.ToInt32(objdata["department"].ToString()),
                    id_position = Convert.ToInt32(objdata["position"].ToString()),
                    permission_id = Convert.ToInt32(objdata["setpermissionUser"].ToString()),
                    email = objdata["email"].ToString() + "@ogura-thia.com"
                };
                dbAdd.Eng_AccountUser.Add(saveadduser);
                dbAdd.SaveChanges();

            }
            return Json(String.Format("Complete"));
        }

        [HttpPost]
        public ActionResult UpdateData(string JsonData)
        {
            JObject objdata = new JObject();
            objdata = JObject.Parse(JsonData);
            var encriptPw = encryptPassword(objdata["password"].ToString());

            var idEmp = objdata["idEmployee"].ToString();
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var updateData = (from update in dbAdd.Eng_AccountUser where update.id_employee == idEmp select update).FirstOrDefault();
                if (updateData != null)
                {
                    updateData.id_employee = objdata["idEmployee"].ToString();
                    updateData.username = objdata["userName"].ToString();
                    updateData.password = encriptPw;
                    updateData.name = objdata["nameUser"].ToString();
                    updateData.lastname = objdata["lastName"].ToString();
                    updateData.nickname = objdata["nickname"].ToString();
                    updateData.id_dept = Convert.ToInt32(objdata["department"].ToString());
                    updateData.id_position = Convert.ToInt32(objdata["position"].ToString());
                    updateData.permission_id = Convert.ToInt32(objdata["setpermissionUser"].ToString());
                    updateData.email = objdata["emails"].ToString();
                }

                dbAdd.SaveChanges();
            }
            return Json(String.Format("Complete"));
        }
        public string encryptPassword(string passText)
        {
            string EncryptionKey = "OCTENDCODEDATA";
            byte[] clearBytes = Encoding.Unicode.GetBytes(passText);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);
                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cs.Write(clearBytes, 0, clearBytes.Length);
                        cs.Close();
                    }
                    passText = Convert.ToBase64String(ms.ToArray());
                }
            }
            return passText;
        }

        [HttpPost]
        public ActionResult DeleteDataUser(string idEmp)
        {

            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var updateData = (from update in dbAdd.Eng_AccountUser where update.id_employee == idEmp select update).FirstOrDefault();
                if (updateData != null)
                {
                    dbAdd.Eng_AccountUser.Remove(updateData);
                }

                dbAdd.SaveChanges();
            }
            string message = "Delete success.";
            return Json(String.Format(message));
        }

        [HttpPost]
        public ActionResult SaveDataPosition(string jsonStr)
        {

            var dataPos = jsonStr.ToString();
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var savepos = new Position()
                {
                    position1 = dataPos,

                };
                dbAdd.Positions.Add(savepos);
                dbAdd.SaveChanges();

            }
            return Json(String.Format("Complete"));
        }

        [HttpPost]
        public JsonResult SearchDataUser(string JsonData)
        {
            JObject jsonObj = new JObject();
            jsonObj = JObject.Parse(JsonData);
            string whName = "", whlastname = "", whNickname = "";
            var whEmpid = jsonObj["getDataEmps"].ToString();
            var whDep = jsonObj["getDataDepartments"].ToString();
            var whPos = jsonObj["getDataPositions"].ToString();
            whName = jsonObj["getDataName"].ToString();
            whNickname = jsonObj["getNickname"].ToString();

            var userAcc = (from tbAcc in db.Eng_AccountUser
                           join tbDep in db.Departments on tbAcc.id_dept equals tbDep.dept_id
                           join tbPos in db.Positions on tbAcc.id_position equals tbPos.id_position
                           join tbPer in db.Eng_PermissionUser on tbAcc.permission_id equals tbPer.Id_permissionUser
                           where tbAcc.name.Contains(whName) && tbAcc.nickname.Contains(whNickname)
                           && tbDep.department1.Contains(whDep) && tbPos.position1.Contains(whPos)
                            && tbAcc.id_employee.Contains(whEmpid)
                           select new ModelUser
                           {
                               EmpId = tbAcc.id_employee,
                               EmpFName = tbAcc.name,
                               EmpLName = tbAcc.lastname,
                               EmpNickName = tbAcc.nickname,
                               EmpDep = tbDep.department1,
                               EmpPos = tbPos.position1,
                               EmpPermission = tbPer.level_admin,
                               EmpEmail = tbAcc.email
                           }).ToList();

            return Json(userAcc, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SearchDataPermission(string data)
        {

            var dbPermission = (from dbsup in db.Eng_PermissionSub
                                join dbMain in db.Eng_PermissionMainmenu on dbsup.id_mainmenu equals dbMain.id_mainmenu
                                join dbUser in db.Eng_PermissionUser on dbsup.id_permissionuser equals dbUser.Id_permissionUser
                                where dbsup.id_permissionuser.ToString().Contains(data)
                                select new ModelPermission
                                {
                                    idPer = dbsup.id_persub.ToString(),
                                    PerNameMainmanu = dbMain.mainmenu_name,
                                    PermissionName = dbUser.level_admin,
                                    PerMainAction = dbsup.name_action,
                                    PerNameControl = dbsup.name_controler,
                                    PerNameFun = dbsup.name_function,
                                    PerStatus = dbsup.name_statusfc
                                }).ToList();
            return Json(dbPermission, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public ActionResult EditDataPosition(string jsonStr)
        {
            JObject objJson = new JObject();
            objJson = JObject.Parse(jsonStr);
            var posid = Convert.ToInt32(objJson["posid"].ToString());
            using (var db = new OCTIIS_WEBAPPEntities3())
            {

                var posdb = (from d in db.Positions
                             where d.id_position == posid
                             select d).FirstOrDefault();
                if (posdb != null)
                {
                    posdb.position1 = objJson["posname"].ToString();
                }
                db.SaveChanges();
            }

            return Json("Update data success");
        }

        [HttpPost]
        public ActionResult DeleteDataPosition(string jsonStr)
        {
            var posid = Convert.ToInt32(jsonStr.ToString());
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var posdb = (from posdel in dbAdd.Positions where posdel.id_position == posid select posdel).FirstOrDefault();
                if (posdb != null)
                {
                    dbAdd.Positions.Remove(posdb);
                }

                dbAdd.SaveChanges();
            }
            return Json("Delete Complete");
        }

        [HttpPost]
        public ActionResult SaveDataDepartment(string jsonStr)
        {

            var dataDep = jsonStr.ToString();
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var savedep = new Department()
                {
                    department1 = dataDep,

                };
                dbAdd.Departments.Add(savedep);
                dbAdd.SaveChanges();

            }
            return Json(String.Format("Complete"));
        }

        [HttpPost]
        public ActionResult DeleteDataDepartment(string jsonStr)
        {
            var depid = Convert.ToInt32(jsonStr.ToString());
            using (var dbAdd = new OCTIIS_WEBAPPEntities3())
            {
                var deptdb = (from dele in dbAdd.Departments where dele.dept_id == depid select dele).FirstOrDefault();
                if (deptdb != null)
                {
                    dbAdd.Departments.Remove(deptdb);
                }

                dbAdd.SaveChanges();
            }
            return Json("Delete Complete");
        }

        [HttpPost]
        public ActionResult EditDataDepartment(string jsonStr)
        {
            JObject objJson = new JObject();
            objJson = JObject.Parse(jsonStr);
            var depid = Convert.ToInt32(objJson["depid"].ToString());
            using (var db = new OCTIIS_WEBAPPEntities3())
            {

                var deptdb = (from d in db.Departments
                              where d.dept_id == depid
                              select d).FirstOrDefault();
                if (deptdb != null)
                {
                    deptdb.department1 = objJson["depname"].ToString();
                }
                db.SaveChanges();
            }

            return Json("Update data success");
        }
        public List<ModelDepartment> QueryDepartment()
        {
            var deptdb = (from d in db.Departments
                          select new ModelDepartment
                          {
                              idDept = d.dept_id,
                              department = d.department1

                          }).ToList();
            return deptdb;
        }
        public List<ModelPosition> QueryPosition()
        {
            var posdb = (from p in db.Positions
                         select new ModelPosition
                         {
                             idPos = p.id_position,
                             Position = p.position1
                         }).ToList();
            return posdb;
        }

        public List<ModelSelPer> QueryPer()
        {
            var perUser = (from p in db.Eng_PermissionUser
                           select new ModelSelPer
                           {
                               idPermission = p.Id_permissionUser.ToString(),
                               namePermission = p.level_admin

                           }).ToList();
            return perUser;
        }
        public List<ModelSelPermenu> QueryPerMenu()
        {
            var perUsermenu = (from p in db.Eng_PermissionMainmenu
                               select new ModelSelPermenu
                               {
                                   idPermissionMenu = p.id_mainmenu.ToString(),
                                   namePermissionMenu = p.mainmenu_name

                               }).ToList();
            return perUsermenu;
        }


        [HttpPost]
        public ActionResult CheckUsersame(string idEemp)
        {
            string alertUser = "";
            var chkUser = (from user in db.Eng_AccountUser where user.id_employee == idEemp select user.id_employee).ToList();
            var countUser = chkUser.Count();
            if (countUser > 0)
            {
                alertUser = "You have this User in System aleardy";
            }
            return Json(String.Format(alertUser));
        }


    }
}