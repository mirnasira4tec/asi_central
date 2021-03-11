using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Routing;

namespace asi.asicentral.util.show
{
    public static class ShowHelper
    {
        public static ShowType CreateOrUpdateShowType(IObjectService objectService, ShowType objShowType)
        {
            if (objShowType == null) return null;
            ShowType showType = null;
            if (objShowType.Id == 0)
            {
                showType = new ShowType()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowType>(showType);
            }
            else
            {
                showType = objectService.GetAll<ShowType>().Where(ctxt => ctxt.Id == objShowType.Id).SingleOrDefault();
                objectService.Update<ShowType>(showType);
            }
            showType.Type = objShowType.Type;
            showType.UpdateDate = DateTime.UtcNow;
            showType.UpdateSource = objShowType.UpdateSource;
            return showType;
        }

        public static ShowASI CreateOrUpdateShow(IObjectService objectService, ShowASI objShow)
        {
            if (objShow == null) return null;
            ShowASI show = null;
            if (objShow.Id == 0)
            {
                show = new ShowASI()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowASI>(show);
            }
            else
            {
                show = objectService.GetAll<ShowASI>().Where(ctxt => ctxt.Id == objShow.Id).SingleOrDefault();
            }
            show.Name = objShow.Name;
            show.StartDate = objShow.StartDate;
            show.EndDate = objShow.EndDate;
            show.ShowTypeId = objShow.ShowTypeId;
            show.Address = objShow.Address;
            show.UpdateDate = DateTime.UtcNow;
            show.UpdateSource = objShow.UpdateSource;
            return show;
        }

        public static ShowAddress CreateOrUpdateAddress(IObjectService objectService, ShowAddress objAddress)
        {
            if (objAddress == null) return null;
            ShowAddress address = null;
            if (objAddress.Id == 0)
            {
                address = new ShowAddress()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowAddress>(address);
            }
            else
            {
                address = objectService.GetAll<ShowAddress>().Where(ctxt => ctxt.Id == objAddress.Id).SingleOrDefault();
            }
            address.Phone = objAddress.Phone;
            address.PhoneAreaCode = objAddress.PhoneAreaCode;
            address.FaxAreaCode = objAddress.FaxAreaCode;
            address.Fax = objAddress.Fax;
            address.Street1 = objAddress.Street1;
            address.Street2 = objAddress.Street2;
            address.Zip = objAddress.Zip;
            address.State = objAddress.State;
            address.Country = objAddress.Country;
            address.City = objAddress.City;
            address.UpdateDate = DateTime.UtcNow;
            address.UpdateSource = objAddress.UpdateSource;
            return address;
        }

        public static ShowEmployee CreateOrUpdateEmployee(IObjectService objectService, ShowEmployee objEmployee)
        {
            if (objEmployee == null) return null;
            ShowEmployee employee = null;
            if (objEmployee.Id == 0)
            {
                employee = new ShowEmployee()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowEmployee>(employee);
            }
            else
            {
                employee = objectService.GetAll<ShowEmployee>().Where(ctxt => ctxt.Id == objEmployee.Id).SingleOrDefault();
            }
            employee.FirstName = objEmployee.FirstName;
            employee.MiddleName = objEmployee.MiddleName;
            employee.LastName = objEmployee.LastName;
            employee.Email = objEmployee.Email;
            employee.LoginEmail = objEmployee.LoginEmail;
            employee.EPhoneAreaCode = objEmployee.EPhoneAreaCode;
            employee.EPhone = objEmployee.EPhone;
            employee.CompanyId = objEmployee.CompanyId;
            employee.Address = objEmployee.Address;
            employee.UpdateDate = DateTime.UtcNow;
            employee.UpdateSource = objEmployee.UpdateSource;
            return employee;
        }

        public static ShowCompany CreateOrUpdateCompany(IObjectService objectService, ShowCompany objCompany)
        {
            if (objCompany == null) return null;
            ShowCompany company = null;
            if (objCompany.Id == 0)
            {
                company = new ShowCompany()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowCompany>(company);
            }
            else
            {
                company = objectService.GetAll<ShowCompany>().Where(ctxt => ctxt.Id == objCompany.Id).SingleOrDefault();
            }
            company.Name = objCompany.Name;
            company.WebUrl = objCompany.WebUrl;
            company.MemberType = objCompany.MemberType;
            company.ASINumber = objCompany.ASINumber;
            company.SecondaryASINo = objCompany.SecondaryASINo;
            company.LogoUrl = objCompany.LogoUrl;
            company.UpdateDate = DateTime.UtcNow;
            company.UpdateSource = objCompany.UpdateSource;
            return company;
        }

        public static ShowCompanyAddress CreateOrUpdateCompanyAddress(IObjectService objectService, ShowCompanyAddress objCompanyAddress)
        {
            if (objCompanyAddress == null) return null;
            ShowCompanyAddress companyAddress = null;
            if (objCompanyAddress.Id == 0)
            {
                companyAddress = new ShowCompanyAddress()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowCompanyAddress>(companyAddress);
            }
            else
            {
                companyAddress = objectService.GetAll<ShowCompanyAddress>().Where(ctxt => ctxt.Id == objCompanyAddress.Id).SingleOrDefault();
            }
            companyAddress.CompanyId = objCompanyAddress.CompanyId;
            companyAddress.Address = objCompanyAddress.Address;
            companyAddress.UpdateDate = DateTime.UtcNow;
            companyAddress.UpdateSource = objCompanyAddress.UpdateSource;
            return companyAddress;
        }

        public static ShowAttendee CreateOrUpdateShowAttendee(IObjectService objectService, ShowAttendee objShowAttendee)
        {
            if (objShowAttendee == null) return null;
            ShowAttendee showAttendee = null;
            if (objShowAttendee.Id == 0)
            {
                showAttendee = new ShowAttendee()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowAttendee>(showAttendee);
            }
            else
            {
                showAttendee = objectService.GetAll<ShowAttendee>().Where(ctxt => ctxt.Id == objShowAttendee.Id).SingleOrDefault();
            }
            showAttendee.CompanyId = objShowAttendee.CompanyId;
            showAttendee.ShowId = objShowAttendee.ShowId;
            showAttendee.IsSponsor = objShowAttendee.IsSponsor;
            showAttendee.IsExhibitDay = objShowAttendee.IsExhibitDay;
            showAttendee.BoothNumber = objShowAttendee.BoothNumber;
            showAttendee.IsPresentation = objShowAttendee.IsPresentation;
            showAttendee.IsRoundTable = objShowAttendee.IsRoundTable;
            showAttendee.IsExisting = objShowAttendee.IsExisting;
            showAttendee.IsCatalog = objShowAttendee.IsCatalog;
            showAttendee.UpdateDate = DateTime.UtcNow;
            showAttendee.UpdateSource = objShowAttendee.UpdateSource;
            return showAttendee;
        }

        public static ShowEmployeeAttendee AddOrDeleteShowEmployeeAttendance(IObjectService objectService, ShowAttendee objShowAttendee, ShowEmployee objShowEmployee, bool isAdd, string updateSource)
        {
            ShowEmployeeAttendee showEmployeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(ctxt => ctxt.EmployeeId == objShowEmployee.Id && ctxt.AttendeeId == objShowAttendee.Id).SingleOrDefault(); ;
            if (showEmployeeAttendee == null && isAdd)
            {
                showEmployeeAttendee = new ShowEmployeeAttendee()
                {
                    CreateDate = DateTime.UtcNow,
                };
                showEmployeeAttendee.EmployeeId = objShowEmployee.Id;
                showEmployeeAttendee.AttendeeId = objShowAttendee.Id;
                showEmployeeAttendee.UpdateDate = DateTime.UtcNow;
                showEmployeeAttendee.UpdateSource = updateSource;
                objectService.Add<ShowEmployeeAttendee>(showEmployeeAttendee);
            }
            else if (showEmployeeAttendee != null && !isAdd)
            {
                DeleteShowEmployeeAttendee(objectService, showEmployeeAttendee, updateSource);
            }
            return showEmployeeAttendee;
        }

        public static ShowEmployeeAttendee CreateOrUpdateEmployeeAttendee(IObjectService objectService, ShowEmployeeAttendee objEmployeeAttendee)
        {
            if (objEmployeeAttendee == null) return null;
            ShowEmployeeAttendee employeeAttendee = null;
            if (objEmployeeAttendee.Id == 0)
            {
                employeeAttendee = new ShowEmployeeAttendee()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowEmployeeAttendee>(employeeAttendee);
            }
            else
            {
                employeeAttendee = objectService.GetAll<ShowEmployeeAttendee>().Where(ctxt => ctxt.Id == objEmployeeAttendee.Id).SingleOrDefault();
            }
            employeeAttendee.Employee = objEmployeeAttendee.Employee;
            employeeAttendee.AttendeeId = objEmployeeAttendee.AttendeeId;
            employeeAttendee.UpdateDate = DateTime.UtcNow;
            employeeAttendee.UpdateSource = objEmployeeAttendee.UpdateSource;
            return employeeAttendee;
        }

        public static ShowDistShowLogo CreateOrUpdateDistShowLogo(IObjectService objectService, ShowDistShowLogo objDistShowLogo)
        {
            if (objDistShowLogo == null) return null;
            ShowDistShowLogo distShowLogo = null;
            if (objDistShowLogo.Id == 0)
            {
                distShowLogo = new ShowDistShowLogo()
                {
                    CreateDate = DateTime.UtcNow,
                };
                objectService.Add<ShowDistShowLogo>(distShowLogo);
            }
            else
            {
                distShowLogo = objectService.GetAll<ShowDistShowLogo>().Where(ctxt => ctxt.Id == objDistShowLogo.Id).SingleOrDefault();
            }
            distShowLogo.AttendeeId = objDistShowLogo.AttendeeId;
            distShowLogo.LogoImageUrl = objDistShowLogo.LogoImageUrl;
            distShowLogo.UpdateDate = DateTime.UtcNow;
            distShowLogo.UpdateSource = objDistShowLogo.UpdateSource;
            return distShowLogo;
        }

        public static void DeleteShowAttendee(IObjectService objectService, ShowAttendee objShowAttendee, string updateSource)
        {
            if (objShowAttendee != null)
            {
                int employeeAttendeeCount = objShowAttendee.EmployeeAttendees.Count();
                for (int i = employeeAttendeeCount; i > 0; i--)
                {
                    var showEmployeeAttendee = objShowAttendee.EmployeeAttendees.ElementAt(i - 1);
                    // Deleting Reference of Employee Attendee from Travel Form
                    if (showEmployeeAttendee.TravelForms != null)
                    {
                        foreach (var frmInstance in showEmployeeAttendee.TravelForms)
                        {
                            frmInstance.EmployeeAttendeeId = null;
                            frmInstance.UpdateDate = DateTime.UtcNow;
                            frmInstance.UpdateSource = updateSource;
                        }
                    }
                    // Deleting Reference of Employee Attendee from Profile Requests
                    if (showEmployeeAttendee.ProfileRequests != null)
                    {
                        foreach (var profile in showEmployeeAttendee.ProfileRequests)
                        {
                            profile.EmployeeAttendeeId = null;
                            profile.UpdateDate = DateTime.UtcNow;
                            profile.UpdateSource = updateSource;
                        }
                    }
                    // Deleting Employee Attendee
                    ShowHelper.DeleteShowEmployeeAttendee(objectService, showEmployeeAttendee, updateSource);
                }
                // Deleting Reference of Attendee from Travel Form
                if (objShowAttendee.TravelForms != null)
                {
                    foreach (var frmInstance in objShowAttendee.TravelForms)
                    {
                        frmInstance.AttendeeId = null;
                        frmInstance.UpdateDate = DateTime.UtcNow;
                        frmInstance.UpdateSource = updateSource;
                    }
                }
                // Deleting Reference of Attendee from Profile Requests
                if (objShowAttendee.ProfileRequests != null)
                {
                    foreach (var profile in objShowAttendee.ProfileRequests)
                    {
                        profile.AttendeeId = null;
                        profile.UpdateDate = DateTime.UtcNow;
                        profile.UpdateSource = updateSource;
                    }
                }
                if (objShowAttendee.DistShowLogos != null)
                {
                    int attendeeLogoCount = objShowAttendee.DistShowLogos.Count();
                    for (int i = attendeeLogoCount; i > 0; i--)
                    {
                        var distShowLogo = objShowAttendee.DistShowLogos.ElementAt(i - 1);
                        objectService.Delete(distShowLogo);
                    }
                }
                // Deleting Attendee
                objectService.Delete<ShowAttendee>(objShowAttendee);
                objectService.SaveChanges();
            }
        }

        public static void DeleteShowEmployeeAttendee(IObjectService objectService, ShowEmployeeAttendee objShowEmployeeAttendee, string updateSource)
        {
            if (objShowEmployeeAttendee != null)
            {
                if (objShowEmployeeAttendee.TravelForms != null)
                {
                    int travelFormsCount = objShowEmployeeAttendee.TravelForms.Count();
                    for (int i = travelFormsCount; i > 0; i--)
                    {
                        var frmInstance = objShowEmployeeAttendee.TravelForms.ElementAt(i - 1);
                        frmInstance.EmployeeAttendeeId = null;
                        frmInstance.UpdateDate = DateTime.UtcNow;
                        frmInstance.UpdateSource = updateSource;
                    }
                }
                if (objShowEmployeeAttendee.ProfileRequests != null)
                {
                    int profileCount = objShowEmployeeAttendee.ProfileRequests.Count();
                    for (int i = profileCount; i > 0; i--)
                    {
                        var profile = objShowEmployeeAttendee.ProfileRequests.ElementAt(i - 1);
                        profile.EmployeeAttendeeId = null;
                        profile.UpdateDate = DateTime.UtcNow;
                        profile.UpdateSource = updateSource;
                    }
                }
                objectService.Delete<ShowEmployeeAttendee>(objShowEmployeeAttendee);
                objectService.SaveChanges();
            }
        }
    }
}
