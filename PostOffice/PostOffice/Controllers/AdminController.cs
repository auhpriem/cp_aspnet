using OfficeOpenXml;
using PostOffice.App_Data;
using PostOffice.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Net.Mail;
using System.Text;

namespace PostOffice.Controllers
{
    public class AdminController : Controller
    {
        public ActionResult SendPackage()
        {
            if ( (Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            SetDropDowns();
            return View(new SendPackageModel());
        }
        [HttpPost]
        public ActionResult SendPackage(SendPackageModel sendPackage)
        {
            SetDropDowns();
            if (ModelState.IsValid)
            {
                try
                {
                    Clients client1 = new Clients(sendPackage.s_Surname,
                                                  sendPackage.s_Name,
                                                  sendPackage.s_MiddleName,
                                                  sendPackage.s_Address.Substring(0, 6),
                                                  sendPackage.s_Address.Substring(8));
                    try
                    {
                        var context = new POSTContext();
                        context.Clients.Add(client1);
                        context.SaveChanges();
                    }
                    catch { }
                    Clients client2 = new Clients(sendPackage.r_Surname,
                                                  sendPackage.r_Name,
                                                  sendPackage.r_MiddleName,
                                                  sendPackage.r_Address.Substring(0, 6),
                                                  sendPackage.r_Address.Substring(8));
                    try
                    {
                        var context = new POSTContext();
                        context.Clients.Add(client2);
                        context.SaveChanges();
                    }
                    catch { }
                    decimal pack_cost = (decimal)PostOperations.GetPackageEf()
                        .Where(p => p.IDPACKAGE == sendPackage.SendingType)
                        .Select(p => p.COST).First();
                    SentPackage SentPack = new SentPackage
                    {
                        SENDER = client1.IDCLIENT,
                        RECIPIENT = client2.IDCLIENT,
                        SADDRES = sendPackage.DeliveryAddress.Substring(8),
                        SINDEX = Convert.ToInt32(sendPackage.DeliveryAddress.Substring(0, 6)),
                        IDPACKAGE = sendPackage.SendingType,
                        PRICE = Convert.ToDecimal(sendPackage.SendingCost) + pack_cost
                    };
                    try
                    {
                        var context = new POSTContext();
                        context.SentPackage.Add(SentPack);
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (NotSupportedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    StatusPack StatP = new StatusPack
                    {
                        PACK_KEY = (PostOperations.GetSentPackageEf()
                                            .FindLast(p => p.SENDER == SentPack.SENDER
                                                        && p.RECIPIENT == SentPack.RECIPIENT)).KEY,
                        ADDRES = client1.ADDRES,
                        INDEX = client1.INDEX
                    };
                    try
                    {
                        var context = new POSTContext();
                        context.StatusPack.Add(StatP);
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (NotSupportedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
            }
            return View(new SendPackageModel());
        }

        public ActionResult PackageStatus(string sentSearch = "", string receivedSearch = "", string packStatusSearch = "")
        {
            if ((Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            ViewBag.addressList = addressList();
            ViewBag.packageKeysList = packageKeysList();
            SetPackStatusLists(sentSearch, receivedSearch, packStatusSearch);
            return View(new PackageStatusModel());
        }
        [HttpPost]
        public ActionResult PackageStatus(PackageStatusModel sendPackage, string sentSearch = "", string receivedSearch = "", string packStatusSearch = "")
        {
            if (ModelState.IsValid)
            {
                int pack_key;
                try
                {
                    //for mail
                    var MailData = PostOperations.GetSentPackageEf().Where(p => p.KEY == sendPackage.PackageKey).Select(p => p).First();
                    var MailDataOldStatus = PostOperations.GetStatusPacksEf().Where(p => p.PACK_KEY == MailData.KEY).Select(p => p).First();
                    //

                    pack_key = sendPackage.PackageKey;

                    StatusPack stat_p = new StatusPack()
                    {
                        ADDRES = sendPackage.Address.Substring(8),
                        PACK_KEY = pack_key,
                        INDEX = Convert.ToInt32(sendPackage.Address.Substring(0, 6))
                    };

                    var context = new POSTContext();
                    SentPackage sent_p = PostOperations.GetSentPackageEf()
                        .Where(p => p.KEY == stat_p.PACK_KEY)
                        .Select(p => p).First();
                    if (sent_p.SADDRES == stat_p.ADDRES)
                    {
                        ReceivedPackage rec_p = new ReceivedPackage()
                        {
                            SENDER = sent_p.SENDER,
                            RECIPIENT = sent_p.RECIPIENT,
                            SADDRES = sent_p.SADDRES,
                            SINDEX = sent_p.SINDEX,
                            IDPACKAGE = sent_p.IDPACKAGE,
                            FINALCOST = (Decimal)sent_p.PRICE + Convert.ToDecimal(Convert.ToDouble(sent_p.PRICE) * 0.03)
                        };
                        context.ReceivedPackage.Add(rec_p);
                        context.SentPackage.Remove(context.SentPackage.Where(p => p.KEY == sent_p.KEY).First());
                        context.StatusPack.Remove(context.StatusPack.Where(p => p.PACK_KEY == stat_p.PACK_KEY).First());
                        context.SaveChanges();
                    }
                    else
                    {
                        context.StatusPack.Remove(context.StatusPack.Where(p => p.PACK_KEY == stat_p.PACK_KEY).First());
                        context.StatusPack.Add(stat_p);
                        context.SaveChanges();
                    }

                    string Message = "Status of package with key \"" + MailData.KEY + "\" was updated. \n"
                           + "Old data of sent package: \n"
                           + "Sender: " + MailData.SENDER + "\n"
                           + "Recipient: " + MailData.RECIPIENT + "\n"
                           + "Package ID: " + MailData.IDPACKAGE + "\n"
                           + "Package Description: " + MailData.Package.PACKAGEDESCRIPTION + "\n"
                           + "Price: " + MailData.PRICE + "\n"
                           + "Old Address: " + MailDataOldStatus.ADDRES + "\n"
                           + "Old Index: " + MailDataOldStatus.INDEX.ToString() + "\n"
                           + "New Address: " + sendPackage.Address.Substring(8) + "\n"
                           + "New Index: " + sendPackage.Address.Substring(0, 6) + "\n";

                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("cacha51484@gmail.com", "66bogoni");
                        client.EnableSsl = true;
                        client.Host = "smtp.gmail.com";

                        MailMessage mm = new MailMessage("cacha51484@gmail.com", MailData.SENDER, "STATUS UPDATE - POST APPLICATION", Message);
                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        client.Send(mm);
                    }
                    catch { }
                    try
                    {
                        MailMessage mail = new MailMessage();
                        SmtpClient client = new SmtpClient();
                        client.Port = 587;
                        client.DeliveryMethod = SmtpDeliveryMethod.Network;
                        client.UseDefaultCredentials = false;
                        client.Credentials = new System.Net.NetworkCredential("cacha51484@gmail.com", "66bogoni");
                        client.EnableSsl = true;
                        client.Host = "smtp.gmail.com";

                        MailMessage mm = new MailMessage("cacha51484@gmail.com", MailData.RECIPIENT, "STATUS UPDATE - POST APPLICATION", Message);
                        mm.BodyEncoding = UTF8Encoding.UTF8;
                        mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;
                        client.Send(mm);
                    }
                    catch { }
                }
                catch (FormatException ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
                catch (OverflowException ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
            }
            ViewBag.addressList = addressList();
            ViewBag.packageKeysList = packageKeysList();
            SetPackStatusLists(sentSearch, receivedSearch, packStatusSearch);
            return View(new PackageStatusModel());
        }

        public ActionResult UserOperation(string userSearch = "", int CurrentOperation = 0)
        {
            if ((Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            SetInfoForClientOperations(CurrentOperation);
            if (CurrentOperation == 0)
                ViewBag.sidebarIsOpen = false;
            return View(new UserOperationModel());
        }
        [HttpPost]
        public ActionResult UserOperation(UserOperationModel operationModel, string userSearch = "", int CurrentOperation = 0)
        {
            SetInfoForClientOperations(CurrentOperation);
            return View(new UserOperationModel());
        }
        [HttpPost]
        public ActionResult SendToClientAction(SendToClientModel sendToClient, int CurrentOperation = 0)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        Clients client2 = new Clients(sendToClient.r_Surname,
                                  sendToClient.r_Name,
                                  sendToClient.r_MiddleName,
                                  sendToClient.r_Address.Substring(0, 6),
                                  sendToClient.r_Address.Substring(8));
                        try
                        {
                            var context1 = new POSTContext();
                            context1.Clients.Add(client2);
                            context1.SaveChanges();
                        }
                        catch { }

                        decimal pack_cost = (decimal)PostOperations.GetPackageEf()
                                                .Where(p => p.IDPACKAGE == sendToClient.SendingType)
                                                .Select(p => p.COST).First();
                        SentPackage SentPack = new SentPackage
                        {
                            SENDER = sendToClient.s_iduser,
                            RECIPIENT = client2.IDCLIENT,
                            SADDRES = sendToClient.DeliveryAddress.Substring(8),
                            SINDEX = Convert.ToInt32(sendToClient.DeliveryAddress.Substring(0, 6)),
                            IDPACKAGE = sendToClient.SendingType,
                            PRICE = Convert.ToDecimal(sendToClient.SendingCost) + pack_cost
                        };
                        var context = new POSTContext();
                        context.SentPackage.Add(SentPack);
                        context.SaveChanges();

                        Clients client = PostOperations.GetClientsEf()
                            .Where(p => p.IDCLIENT == sendToClient.s_iduser)
                            .Select(p => p)
                            .First();

                        StatusPack StatP = new StatusPack
                        {
                            PACK_KEY = (PostOperations.GetSentPackageEf()
                            .FindLast(p => p.SENDER == SentPack.SENDER
                                        && p.RECIPIENT == SentPack.RECIPIENT)).KEY,
                            ADDRES = client.ADDRES,
                            INDEX = client.INDEX
                        };
                        context = new POSTContext();
                        context.StatusPack.Add(StatP);
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (NotSupportedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
            }
            SetInfoForClientOperations(CurrentOperation);
            return View("UserOperation", new UserOperationModel());
        }
        [HttpPost]
        public ActionResult SendToUserAction(SendToUserModel sendToUser, int CurrentOperation = 0)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    try
                    {
                        decimal pack_cost = (decimal)PostOperations.GetPackageEf()
                                                .Where(p => p.IDPACKAGE == sendToUser.SendingType)
                                                .Select(p => p.COST).First();
                        SentPackage SentPack = new SentPackage
                        {
                            SENDER = sendToUser.s_iduser,
                            RECIPIENT = sendToUser.r_iduser,
                            SADDRES = sendToUser.DeliveryAddress.Substring(8),
                            SINDEX = Convert.ToInt32(sendToUser.DeliveryAddress.Substring(0, 6)),
                            IDPACKAGE = sendToUser.SendingType,
                            PRICE = Convert.ToDecimal(sendToUser.SendingCost) + pack_cost
                        };
                        var context = new POSTContext();
                        context.SentPackage.Add(SentPack);
                        context.SaveChanges();

                        Clients client = PostOperations.GetClientsEf()
                            .Where(p => p.IDCLIENT == sendToUser.s_iduser)
                            .Select(p => p)
                            .First();

                        StatusPack StatP = new StatusPack
                        {
                            PACK_KEY = (PostOperations.GetSentPackageEf()
                            .FindLast(p => p.SENDER == SentPack.SENDER
                                        && p.RECIPIENT == SentPack.RECIPIENT)).KEY,
                            ADDRES = client.ADDRES,
                            INDEX = client.INDEX
                        };
                        context = new POSTContext();
                        context.StatusPack.Add(StatP);
                        context.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateConcurrencyException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (System.Data.Entity.Validation.DbEntityValidationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (NotSupportedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (ObjectDisposedException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (InvalidOperationException ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ValidationMessage", ex.Message);
                        return View(new SendPackageModel());
                    }
                }
                catch (ArgumentNullException ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
            }
            SetInfoForClientOperations(CurrentOperation);
            return View("UserOperation", new UserOperationModel());
        }
        [HttpPost]
        public ActionResult SubscriptionAction(SubscriptionModel subscription, int CurrentOperation = 0)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Subscribe sub = new Subscribe
                    {
                        IDEDITION = PostOperations.GetEditionEf().Where(p => p.EDITION1 == subscription.Edition).Select(p => p.IDEDITION).First(),
                        IDCLIENT = subscription.s_iduser,
                        PERIOD = Convert.ToInt32(subscription.Period),
                        DATEACTIVATION = DateTime.Now
                    };
                    var context = new POSTContext();
                    context.Subscribe.Add(sub);
                    context.SaveChanges();
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage", ex.Message);
                    return View(new SendPackageModel());
                }
            }
            SetInfoForClientOperations(CurrentOperation);
            return View("UserOperation", new UserOperationModel());
        }

        public ActionResult PackageSearch(string search = "", string idsearch = "")
        {
            if ((Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            List<PackageListModel> packageList = new List<PackageListModel>();

            var sentPackages = PostOperations.GetSentPackageEf().Where(p => p.KEY.ToString().Contains(search)
                                  || p.SENDER.Contains(search)
                                  || p.RECIPIENT.Contains(search)
                                  || p.IDPACKAGE.Contains(search)
                                  || p.SINDEX.ToString().Contains(search)
                                  || p.SADDRES.Contains(search)
                                  || p.PRICE.ToString().Contains(search)
                                   ).Select(p => p);
            if (idsearch != "")
                sentPackages = sentPackages.Where(p => p.KEY.ToString().Equals(idsearch)).Select(p => p);
            foreach (var p in sentPackages)
            {
                packageList.Add(new PackageListModel
                {
                    KEY = p.KEY,
                    SENDER = p.SENDER,
                    RECIPIENT = p.RECIPIENT,
                    IDPACKAGE = p.IDPACKAGE,
                    SINDEX = p.SINDEX,
                    SADDRES = p.SADDRES,
                    PRICE = p.PRICE,
                    ADDRES = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.ADDRES).First(),
                    INDEX = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.INDEX).First()
                });
            }
            ViewBag.packageList = packageList;
            return View();
        }

        public ActionResult EditionPackageType()
        {
            if ((Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            SetEditionPackageTypes();
            return View(new EditionPackageTypeModel());
        }
        [HttpPost]
        public ActionResult AddPackageType(PackageTypeModel packageType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Package pack = new Package()
                    {
                        IDPACKAGE = packageType.PackageID,
                        PACKAGEDESCRIPTION = packageType.PackageDescription,
                        COST = Convert.ToDecimal(packageType.Cost)
                    };
                    var context = new POSTContext();
                    context.Package.Add(pack);
                    context.SaveChanges();
                }
                catch (ArgumentNullException aex)
                {
                    ModelState.AddModelError("ValidationMessage_Package", aex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage_Package", ex.Message);
                }
            }
            SetEditionPackageTypes();
            return View("EditionPackageType", new EditionPackageTypeModel());
        }
        [HttpPost]
        public ActionResult AddEditionType(EditionTypeModel editionType)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Edition edit = new Edition()
                    {
                        IDEDITION = editionType.EditionID,
                        EDITION1 = editionType.Edition,
                        COSTFORMONTH = Convert.ToDecimal(editionType.CostForMonth)
                    };
                    var context = new POSTContext();
                    context.Edition.Add(edit);
                    context.SaveChanges();
                }
                catch (ArgumentNullException aex)
                {
                    ModelState.AddModelError("ValidationMessage_Edition", "Проверьте наличие дубликатов. " + aex.Message);
                }
                catch (Exception ex)
                {
                    ModelState.AddModelError("ValidationMessage_Edition", "Проверьте наличие дубликатов. " + ex.Message);
                }
            }
            SetEditionPackageTypes();
            return View("EditionPackageType", new EditionPackageTypeModel());
        }
        [HttpPost]
        public ActionResult DeleteSelectedEditions(string[] edition_ids)
        {
            if(edition_ids != null)
            {
                foreach(string editionId in edition_ids)
                {
                    var context = new POSTContext();
                    context.Edition.Remove(context.Edition
                        .Where(p => (p.IDEDITION == editionId)).Select(p => p).First());
                    context.SaveChanges();
                }
            }

            SetEditionPackageTypes();
            return View("EditionPackageType", new EditionPackageTypeModel());
        }
        [HttpPost]
        public ActionResult DeleteSelectedPackages(string[] package_ids)
        {

            if (package_ids != null)
            {
                foreach (string packageId in package_ids)
                {
                    var context = new POSTContext();
                    context.Package.Remove(context.Package
                        .Where(p => (p.IDPACKAGE == packageId)).Select(p => p).First());
                    context.SaveChanges();
                }
            }

            SetEditionPackageTypes();
            return View("EditionPackageType", new EditionPackageTypeModel());
        }
        
        public ActionResult ExportData()
        {

            if ((Session["IsAuthorized"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }

            List<PackageListModel> packageList = new List<PackageListModel>();

            var sentPackages = PostOperations.GetSentPackageEf().Select(p => p);
            foreach (var p in sentPackages)
            {
                packageList.Add(new PackageListModel
                {
                    KEY = p.KEY,
                    SENDER = p.SENDER,
                    RECIPIENT = p.RECIPIENT,
                    IDPACKAGE = p.IDPACKAGE,
                    SINDEX = p.SINDEX,
                    SADDRES = p.SADDRES,
                    PRICE = p.PRICE,
                    ADDRES = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.ADDRES).First(),
                    INDEX = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.INDEX).First()
                });
            }
            List<SubscriptionsListModel> subscriptionList = PostOperations.GetSubscribeEf().Where(p => ((DateTime.Now - p.DATEACTIVATION.Value).Days < p.PERIOD * 30))
                                                        .Select(p => new SubscriptionsListModel
                                                        {
                                                            Edition = p.Edition.EDITION1,
                                                            Client =  p.IDCLIENT,
                                                            ActivationDate = p.DATEACTIVATION,
                                                            FinalPrice = p.PERIOD * p.Edition.COSTFORMONTH
                                                        }).ToList();

            ViewBag.packageList = packageList;
            ViewBag.subscriptionList = subscriptionList;
            return View();
        }
        public ActionResult PackagesExportToExcel()
        {
            List<PackageListModel> packageList = new List<PackageListModel>();

            var sentPackages = PostOperations.GetSentPackageEf().Select(p => p);
            foreach (var p in sentPackages)
            {
                packageList.Add(new PackageListModel
                {
                    KEY = p.KEY,
                    SENDER = p.SENDER,
                    RECIPIENT = p.RECIPIENT,
                    IDPACKAGE = p.IDPACKAGE,
                    SINDEX = p.SINDEX,
                    SADDRES = p.SADDRES,
                    PRICE = p.PRICE,
                    ADDRES = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.ADDRES).First(),
                    INDEX = PostOperations.GetStatusPacksEf().Where(t => t.PACK_KEY == p.KEY
                                   ).Select(t => t.INDEX).First()
                });
            }

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Key";
            ws.Cells["B6"].Value = "Sender";
            ws.Cells["C6"].Value = "Recipient";
            ws.Cells["D6"].Value = "Package ID";
            ws.Cells["E6"].Value = "Sender (Index)";
            ws.Cells["F6"].Value = "Sender (Address)";
            ws.Cells["G6"].Value = "Price";
            ws.Cells["H6"].Value = "Current Address";
            ws.Cells["I6"].Value = "Current Index";

            ws.Row(6).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Row(6).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("aliceblue")));

            int rowStart = 7;
            foreach(var item in packageList)
            {
                if (rowStart % 2 == 0)
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("aliceblue")));
                }
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.KEY;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.SENDER;
                ws.Cells[string.Format("C{0}", rowStart)].Value = item.RECIPIENT;
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.IDPACKAGE;
                ws.Cells[string.Format("E{0}", rowStart)].Value = item.SINDEX;
                ws.Cells[string.Format("F{0}", rowStart)].Value = item.SADDRES;
                ws.Cells[string.Format("G{0}", rowStart)].Value = item.PRICE;
                ws.Cells[string.Format("H{0}", rowStart)].Value = item.INDEX;
                ws.Cells[string.Format("I{0}", rowStart)].Value = item.ADDRES;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return RedirectToAction("ExportData");
        }
        public ActionResult SubscriptionsExportToExcel()
        {
            List<SubscriptionsListModel> subscriptionList = PostOperations.GetSubscribeEf().Where(p => ((DateTime.Now - p.DATEACTIVATION.Value).Days < p.PERIOD * 30))
                                                        .Select(p => new SubscriptionsListModel
                                                        {
                                                            Edition = p.Edition.EDITION1,
                                                            Client = p.IDCLIENT,
                                                            ActivationDate = p.DATEACTIVATION,
                                                            FinalPrice = p.PERIOD * p.Edition.COSTFORMONTH
                                                        }).ToList();

            ExcelPackage pck = new ExcelPackage();
            ExcelWorksheet ws = pck.Workbook.Worksheets.Add("Report");

            ws.Cells["A3"].Value = "Date";
            ws.Cells["B3"].Value = string.Format("{0:dd MMMM yyyy} at {0:H: mm tt}", DateTimeOffset.Now);

            ws.Cells["A6"].Value = "Client";
            ws.Cells["B6"].Value = "Edition";
            ws.Cells["C6"].Value = "Activation Date";
            ws.Cells["D6"].Value = "Final Price";

            ws.Row(6).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
            ws.Row(6).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("aliceblue")));

            int rowStart = 7;
            foreach (var item in subscriptionList)
            {
                if (rowStart % 2 == 0)
                {
                    ws.Row(rowStart).Style.Fill.PatternType = OfficeOpenXml.Style.ExcelFillStyle.Solid;
                    ws.Row(rowStart).Style.Fill.BackgroundColor.SetColor(ColorTranslator.FromHtml(string.Format("aliceblue")));
                }
                ws.Cells[string.Format("A{0}", rowStart)].Value = item.Client;
                ws.Cells[string.Format("B{0}", rowStart)].Value = item.Edition;
                ws.Cells[string.Format("C{0}", rowStart)].Value = string.Format("{0:dd MMMM yyyy}",  item.ActivationDate);
                ws.Cells[string.Format("D{0}", rowStart)].Value = item.FinalPrice;
                rowStart++;
            }

            ws.Cells["A:AZ"].AutoFitColumns();

            Response.Clear();
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.BinaryWrite(pck.GetAsByteArray());
            Response.End();

            return RedirectToAction("ExportData");
        }
        

        protected void SetEditionPackageTypes()
        {
            ViewBag.editionList = PostOperations.GetEditionEf().Select(p => p).ToList();
            ViewBag.packageList = PostOperations.GetPackageEf().Select(p => p).ToList();
        }
        protected void SetInfoForClientOperations(int CurrentOperation)
        {
            ViewBag.sidebarIsOpen = true;
            SetDropDowns();
            SetCurrentOperation(CurrentOperation);
            ViewBag.userLoginList = userLoginList();
            ViewBag.editionList = editionList();
        }
        protected void SetPackStatusLists(string sentSearch, string receivedSearch, string packStatusSearch)
        {

            ViewBag.sentPackageList = PostOperations.GetSentPackageEf().Where(p => p.KEY.ToString().Contains(sentSearch)
                                                                                || p.SENDER.Contains(sentSearch)
                                                                                || p.RECIPIENT.Contains(sentSearch)
                                                                                || p.IDPACKAGE.Contains(sentSearch)
                                                                                || p.SINDEX.ToString().Contains(sentSearch)
                                                                                || p.SADDRES.Contains(sentSearch)
                                                                                || p.PRICE.ToString().Contains(sentSearch)
                                                                                ).Select(p => p).ToList();

            ViewBag.receivedPackageList = PostOperations.GetReceivedPackageEf().Where(p => p.KEY.ToString().Contains(receivedSearch)
                                                                                || p.SENDER.Contains(receivedSearch)
                                                                                || p.RECIPIENT.Contains(receivedSearch)
                                                                                || p.IDPACKAGE.Contains(receivedSearch)
                                                                                || p.SINDEX.ToString().Contains(receivedSearch)
                                                                                || p.SADDRES.Contains(receivedSearch)
                                                                                || p.FINALCOST.ToString().Contains(receivedSearch)
                                                                                ).Select(p => p).ToList();

            ViewBag.packageStatusList = PostOperations.GetStatusPacksEf().Where(p => p.PACK_KEY.ToString().Contains(packStatusSearch)
                                                                                || p.ADDRES.Contains(packStatusSearch)
                                                                                || p.INDEX.ToString().Contains(packStatusSearch)
                                                                                ).Select(p => p).ToList();
        }
        protected void SetDropDowns()
        {
            ViewBag.addressList = addressList();
            ViewBag.packageList = packageList();
        }
        protected void SetCurrentOperation(int CurrentOperation)
        {
            switch (CurrentOperation)
            {
                case 1:
                    ViewBag.CurrentOperation = "_SendToUser";
                    break;
                case 2:
                    ViewBag.CurrentOperation = "_SendToClient";
                    break;
                case 3:
                    ViewBag.CurrentOperation = "_Subscription";
                    break;
                default:
                    ViewBag.CurrentOperation = "_SendToUser";
                    break;
            }
        }

        //DropDowns
        public SelectList addressList()
        {
            var items = PostOperations.GetIndexEf().OrderBy(p => p.INDEX1).Select(p => p.INDEX1.ToString() + ", " + p.ADDRES).ToList();
            items.Add("");
            SelectList selectList = new SelectList(items, "");
            return selectList;
        }
        public SelectList indexList()
        {
            List<int> items = PostOperations.GetIndexEf().OrderBy(p => p.INDEX1).Select(p => p.INDEX1).ToList();
            items.Add(0);
            SelectList selectList = new SelectList(items, 0);
            return selectList;
        }
        public SelectList packageList()
        {
            var items = PostOperations.GetPackageEf().Select(p => p.IDPACKAGE).ToList();
            items.Add("");
            SelectList selectList = new SelectList(items, "");
            return selectList;
        }
        public SelectList packageKeysList()
        {
            var items = PostOperations.GetStatusPacksEf().Select(p => p.PACK_KEY).ToList();
            //items.Add(0);
            SelectList selectList = new SelectList(items);
            return selectList;
        }
        public SelectList userLoginList()
        {
            var items = PostOperations.GetUsersEf().Where(p => p.ACCESSRIGHTS == "user").Select(p => p.IDUSER).ToList();
            items.Add("");
            SelectList selectList = new SelectList(items, "");
            return selectList;
        }
        public SelectList editionList()
        {
            var items = PostOperations.GetEditionEf().Select(p => p.EDITION1).ToList();
            items.Add("");
            SelectList selectList = new SelectList(items, "");
            return selectList;
        }
    }
}
