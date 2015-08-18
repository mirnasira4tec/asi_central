using asi.asicentral.database;
using asi.asicentral.interfaces;
using asi.asicentral.model.show;
using asi.asicentral.services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
              objectService.Update<ShowEmployee>(employee);
          }
          employee.FirstName = objEmployee.FirstName;
          employee.MiddleName = objEmployee.MiddleName;
          employee.LastName = objEmployee.LastName;
          employee.Email = objEmployee.Email;
          employee.Company = objEmployee.Company;
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
              objectService.Update<ShowCompany>(company);
          }
          company.Name = objCompany.Name;
          company.WebUrl = objCompany.WebUrl;
          company.MemberType = objCompany.MemberType;
          company.ASINumber = objCompany.ASINumber;
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


    }
}
