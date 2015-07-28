using asi.asicentral.database;
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
      public static ShowType CreateOrUpdateShowType(ObjectService objectService, ShowType objShowType)
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

      public static Show CreateOrUpdateShow(ObjectService objectService, Show objShow)
      {
          if (objShow == null) return null;
          Show show = null;
          if (objShow.Id == 0)
          {
              show = new Show()
              {
                  CreateDate = DateTime.UtcNow,
              };
              objectService.Add<Show>(show);
          }
          else
          {
              show = objectService.GetAll<Show>().Where(ctxt => ctxt.Id == objShow.Id).SingleOrDefault();
              objectService.Update<Show>(show);
          }
          show.Name = objShow.Name;
          show.StartDate = objShow.StartDate;
          show.EndDate = objShow.EndDate;
          show.ShowTypeId = Convert.ToInt32(objShow.ShowType);
          show.AddressId = Convert.ToInt32(objShow.Address);
          show.UpdateDate = DateTime.UtcNow;
          show.UpdateSource = objShow.UpdateSource;
          return show;
      }

      public static Address CreateOrUpdateAddress(ObjectService objectService, Address objAddress)
      {
          if (objAddress == null) return null;
          Address address = null;
          if (objAddress.Id == 0)
          {
              address = new Address()
              {
                  CreateDate = DateTime.UtcNow,
              };
              objectService.Add<Address>(address);
          }
          else
          {
              address = objectService.GetAll<Address>().Where(ctxt => ctxt.Id == objAddress.Id).SingleOrDefault();
              objectService.Update<Address>(address);
          }
          address.Phone = objAddress.Phone;
          address.PhoneAreaCode = objAddress.PhoneAreaCode;
          address.Cell = objAddress.Cell;
          address.FaxAreaCode = objAddress.FaxAreaCode;
          address.Fax = objAddress.Fax;
          address.Title = objAddress.Title;
          address.Street1 = objAddress.Street1;
          address.Street2 = objAddress.Street2;
          address.Zip = objAddress.Zip;
          address.State = objAddress.State;
          address.CountryCode = objAddress.CountryCode;
          address.Country = objAddress.Country;
          address.City = objAddress.City;
          address.UpdateDate = DateTime.UtcNow;
          address.UpdateSource = objAddress.UpdateSource;
          return address;
      }

      public static Employee CreateOrUpdateEmployee(ObjectService objectService, Employee objEmployee)
      {
          if (objEmployee == null) return null;
          Employee employee = null;
          if (objEmployee.Id == 0)
          {
              employee = new Employee()
              {
                  CreateDate = DateTime.UtcNow,
              };
              objectService.Add<Employee>(employee);
          }
          else
          {
              employee = objectService.GetAll<Employee>().Where(ctxt => ctxt.Id == objEmployee.Id).SingleOrDefault();
              objectService.Update<Employee>(employee);
          }
          employee.FirstName = objEmployee.FirstName;
          employee.MiddleName = objEmployee.MiddleName;
          employee.LastName = objEmployee.LastName;
          employee.Email = objEmployee.Email;
          employee.CompanyId = Convert.ToInt32(objEmployee.Company);
          employee.AddressId = Convert.ToInt32(objEmployee.Address);
          employee.UpdateDate = DateTime.UtcNow;
          employee.UpdateSource = objEmployee.UpdateSource;
          return employee;
      }

      public static ShowCompany CreateOrUpdateCompany(ObjectService objectService, ShowCompany objCompany)
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
          company.ASINumber = objCompany.ASINumber;
          company.UpdateDate = DateTime.UtcNow;
          company.UpdateSource = objCompany.UpdateSource;
          return company;
      }

      public static CompanyAddress CreateOrUpdateCompanyAddress(ObjectService objectService, CompanyAddress objCompanyAddress)
      {
          if (objCompanyAddress == null) return null;
          CompanyAddress companyAddress = null;
          if (objCompanyAddress.Id == 0)
          {
              companyAddress = new CompanyAddress()
              {
                  CreateDate = DateTime.UtcNow,
              };
              objectService.Add<CompanyAddress>(companyAddress);
          }
          else
          {
              companyAddress = objectService.GetAll<CompanyAddress>().Where(ctxt => ctxt.Id == objCompanyAddress.Id).SingleOrDefault();
              objectService.Update<CompanyAddress>(companyAddress);
          }
          companyAddress.CompanyId = objCompanyAddress.CompanyId;
          companyAddress.AddressId = objCompanyAddress.AddressId;
          companyAddress.UpdateDate = DateTime.UtcNow;
          companyAddress.UpdateSource = objCompanyAddress.UpdateSource;
          return companyAddress;
      }
    }
}
