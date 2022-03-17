using EmployeeSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace EmployeeSystem.Controllers
{
    public class employeeController : ApiController
    {
        [HttpGet]
        [Route("api/employee")]
        public HttpResponseMessage getAllEmployees()
        {
            using (EmployeeEntities entities = new EmployeeEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Employees.ToList());

            }
        }

        [HttpGet]
        [Route("api/employee")]
        public HttpResponseMessage getEmployee(int emp_id)
        {
            using (EmployeeEntities entities = new EmployeeEntities())
            {
                var entity = entities.Employees.FirstOrDefault(E => E.Employee_id == emp_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Can not Find This Employee");
                }
            }
        }

        [HttpPost]
        [Route("api/employee")]
        public HttpResponseMessage addEmployee([FromBody] Employee emp)
        {
            try
            {
                using (EmployeeEntities entities = new EmployeeEntities())
                {
                    entities.Employees.Add(emp);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, emp);
                    message.Headers.Location = new Uri(Request.RequestUri + emp.Employee_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/employee")]
        public HttpResponseMessage removeEmployee(int emp_id)
        {
            try
            {
                using (EmployeeEntities entities = new EmployeeEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(E => E.Employee_id == emp_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find This Employee");
                    }
                    else
                    {
                        entities.Employees.Remove(entity);
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, "Deleted Successfully");
                    }

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpPut]
        [Route("api/employee")]
        public HttpResponseMessage editEmployee([FromBody] Employee emp)
        {
            try
            {
                using (EmployeeEntities entities = new EmployeeEntities())
                {
                    var entity = entities.Employees.FirstOrDefault(E => E.Employee_id == emp.Employee_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot Find This Employee");
                    }
                    else
                    {
                        entity.Employee_Name = emp.Employee_Name;
                        entity.Department = emp.Department;
                        entity.DateOfJoining = emp.DateOfJoining;
                        entity.ProfilePhoto = emp.ProfilePhoto;
                        entities.SaveChanges();
                        return Request.CreateResponse(HttpStatusCode.OK, entity);
                    }
                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [Route("api/employee/getAllDepartmentsNames")]
        [HttpGet]
        public HttpResponseMessage getAllDepartmentsNames()
        {
            using (DepartmentEntities entities = new DepartmentEntities())
            {
                List<string> depNames = new List<string>();
                var entity = entities.Departments.ToList();
                if (entity != null)
                {
                    foreach(Department x in entity)
                    {
                        depNames.Add(x.Department_Name);
                    }
                }
                return Request.CreateResponse(HttpStatusCode.OK, depNames);

            }
        }

        [Route("api/employee/SaveFile")]
        public string SaveFile()
        {
            try
            {
                var httpRequest = HttpContext.Current.Request;
                var postedFile = httpRequest.Files[0];
                string filename = postedFile.FileName;
                var physicalPath = HttpContext.Current.Server.MapPath("~/Photos/" + filename);

                postedFile.SaveAs(physicalPath);

                return filename;
            }
            catch
            {
                return "anonymous.png";
            }
        }
    }
}
