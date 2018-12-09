using PostOffice.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace PostOffice.Controllers
{
    public class UserController : Controller
    {
        public ActionResult IndexAddress()
        {
            if ((Session["IsAuthorizedUser"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            var items = PostOperations.GetIndexEf().OrderBy(p => p.INDEX1).Select(p => p.INDEX1.ToString() + ", " + p.ADDRES).ToList();
            items.Add("");
            SelectList selectList = new SelectList(items, "");

            ViewBag.addressList = selectList;
            return View();
        }

        public ActionResult StoryHistory(string search = "")
        {
            if ((Session["IsAuthorizedUser"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            try
            {
                var ReceivedPackages = PostOperations.GetReceivedPackageEf();
                ViewBag.ReceivedPackagesList = ReceivedPackages
                    .Where(p => ((p.SENDER == PostOperations.UserName || p.RECIPIENT == PostOperations.UserName) 
                                && (p.Package.PACKAGEDESCRIPTION.Contains(search) 
                                    || p.SENDER.Contains(search)
                                    || p.RECIPIENT.Contains(search)
                                    || p.FINALCOST.ToString().Contains(search))))
                    .Select(p => new
                    UserReceivedPackagesModel
                    {
                        PACKAGEDESCRIPTION = p.Package.PACKAGEDESCRIPTION,
                        SENDER = p.SENDER,
                        RECIPIENT = p.RECIPIENT,
                        FINALCOST = p.FINALCOST
                    }).ToList();
            }
            catch { }
            return View();
        }

        public ActionResult Subscriptions()
        {
            if ((Session["IsAuthorizedUser"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            try
            {
                var Subscriptions = PostOperations.GetSubscribeEf();
                ViewBag.ActiveSubscriptionsList = Subscriptions.Where(p => ((DateTime.Now - p.DATEACTIVATION.Value).Days < p.PERIOD * 30)
                                                                   && p.IDCLIENT == PostOperations.UserName)
                                                        .Select(p => new
                                                        SubscriptionsListModel
                                                        {
                                                            Edition        = p.Edition.EDITION1,
                                                            Client         = p.IDCLIENT,
                                                            ActivationDate = p.DATEACTIVATION,
                                                            FinalPrice     = p.PERIOD * p.Edition.COSTFORMONTH
                                                        }).ToList();

                ViewBag.NonActiveSubscriptionsList = Subscriptions.Where(p => ((DateTime.Now - p.DATEACTIVATION.Value).Days > p.PERIOD * 30)
                                                   && p.IDCLIENT == PostOperations.UserName)
                                        .Select(p => new
                                        SubscriptionsListModel
                                        {
                                            Edition        = p.Edition.EDITION1,
                                            Client         = p.IDCLIENT,
                                            ActivationDate = p.DATEACTIVATION,
                                            FinalPrice     = p.PERIOD * p.Edition.COSTFORMONTH
                                        }).ToList();
            }
            catch { }

            return View();
        }

        public ActionResult StoryStatusHistory(string search = "")
        {
            if ((Session["IsAuthorizedUser"] as string) != "yes")
            {
                return RedirectToAction("Login", "Main", new LoginModel());
            }
            try
            {
                var StatusPacks = PostOperations.GetStatusPacksEf();
                ViewBag.UserPackagesList = StatusPacks
                    .Where(p => (p.SentPackage.RECIPIENT == PostOperations.UserName
                                 || p.SentPackage.SENDER == PostOperations.UserName)
                                 && (p.SentPackage.RECIPIENT.Contains(search) || p.SentPackage.SENDER.Contains(search) || p.SentPackage.Package.PACKAGEDESCRIPTION.Contains(search)
                                     || p.ADDRES.Contains(search) || p.INDEX.ToString().Contains(search) || (p.SentPackage.Package.COST + p.SentPackage.PRICE).ToString().Contains(search)  ))
                    .Select(p => new
                    StoryStatusHistoryModel{
                        SENDER                  = p.SentPackage.SENDER,
                        PACKAGEDESCRIPTION      = p.SentPackage.Package.PACKAGEDESCRIPTION,
                        FIN_COST                = p.SentPackage.Package.COST + p.SentPackage.PRICE,
                        ADDRES                  = p.ADDRES,
                        INDEX                   = p.INDEX
                    }).ToList();
            }
            catch { }
            return View();
        }
    }
}