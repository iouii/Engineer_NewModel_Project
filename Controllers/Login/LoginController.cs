using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using NewModelEx_S1.Context;
using NewModelEx_S1.Models;
using System.Text;
namespace NewModelEx_S1.Controllers
{
    public class LoginController : Controller
    {
        OCTIIS_WEBAPPEntities3 dbLogin = new OCTIIS_WEBAPPEntities3();
        // GET: Login

        public ActionResult Index()
        {
            return View();
        }


        [HttpPost]
        public ActionResult QueryDataUser(string JsonUser)
        {
            var countUser = 0; var countUsers = 0; string message = "";
            JObject loginData = new JObject();
            loginData = JObject.Parse(JsonUser);
            var user = loginData["username"].ToString();
            var pass = loginData["password"].ToString();
            var endCodePw = encryptPassword(pass);



            var Mainmenu = dbLogin.Eng_AccountUser.Join(dbLogin.Eng_PermissionUser, ea => ea.permission_id, ep => ep.Id_permissionUser, (ea, ep) => new { ea, ep })
               .Join(dbLogin.Eng_PermissionSub, eaep => eaep.ep.Id_permissionUser, eps => eps.id_permissionuser, (eaep, eps) => new { eaep, eps })
               .Join(dbLogin.Eng_PermissionMainmenu, epm => epm.eps.id_mainmenu, epmu => epmu.id_mainmenu, (epm, epmu) => new { epm, epmu })
               .Select(col => new { col.epm.eaep.ea.username, col.epm.eaep.ea.password, col.epmu.id_mainmenu, col.epmu.mainmenu_name })
         .Where(tbAccs => tbAccs.username == user.ToString() && tbAccs.password == endCodePw.ToString()).Distinct().ToList();
            List<ModelMainMenu> models = new List<ModelMainMenu>();
            countUsers = Mainmenu.Count();
            for (var j = 0; j < countUsers; j++)
            {
                models.Add(new ModelMainMenu()
                {
                    idMainMenu = Mainmenu[j].id_mainmenu.ToString(),
                    MainMenuname = Mainmenu[j].mainmenu_name.ToString()

                });
            }
            Session["menu"] = models;


            var tableAccquery = dbLogin.Eng_AccountUser.Join(dbLogin.Eng_PermissionUser, ea => ea.permission_id, ep => ep.Id_permissionUser, (ea, ep) => new { ea, ep })
                .Join(dbLogin.Eng_PermissionSub, eaep => eaep.ep.Id_permissionUser, eps => eps.id_permissionuser, (eaep, eps) => new { eaep, eps })
                .Select(col => new { col.eaep.ea.username, col.eaep.ea.password, col.eaep.ea.name, col.eaep.ea.lastname, col.eaep.ea.permission_id, col.eps.name_function, col.eps.name_controler, col.eps.name_action, col.eaep.ep.level_admin, col.eps.name_statusfc, col.eps.id_mainmenu })
          .Where(tbAcc => tbAcc.username == user.ToString() && tbAcc.password == endCodePw.ToString());
            countUser = tableAccquery.Count();
            var tableAcc = tableAccquery.ToList();
            List<ModelLogin> model = new List<ModelLogin>();
            for (var i = 0; i < countUser; i++)
            {
                model.Add(new ModelLogin()
                {
                    NameFunction = tableAcc[i].name_function.ToString(),
                    ControllerName = tableAcc[i].name_controler.ToString(),
                    ActionName = tableAcc[i].name_action.ToString(),
                    idPermission = tableAcc[i].permission_id.ToString(),
                    UserName = tableAcc[i].username.ToString(),
                    Password = tableAcc[i].password.ToString(),
                    Name = tableAcc[i].name.ToString(),
                    LastName = tableAcc[i].lastname.ToString(),
                    statusMenu = tableAcc[i].name_statusfc.ToString(),
                    idmainMenu = tableAcc[i].id_mainmenu.ToString()
                });
            }
            Session["SubMenu"] = model;

            var dbSetRep = tableAccquery.Select(ser => new { ser.permission_id, ser.name_statusfc, ser.id_mainmenu }).Distinct().ToList();
            var countdbSet = dbSetRep.Count();
            List<ModelChkSetRep> modelck = new List<ModelChkSetRep>();
            for (var i = 0; i < countdbSet; i++)
            {
                modelck.Add(new ModelChkSetRep()
                {
                    idMain = dbSetRep[i].id_mainmenu.ToString(),
                    idPermis = dbSetRep[i].permission_id.ToString(),
                    idSetRep = dbSetRep[i].name_statusfc.ToString()
                });
            }
            Session["SetRep"] = modelck;

            if (countUser > 0)
            {
                message = "1";
            }
            else
            {
                message = "0";
            }

            return Json(String.Format(message));
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
    }
}