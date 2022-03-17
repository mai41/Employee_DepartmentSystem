using EmployeeSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace EmployeeSystem.Controllers
{
    public class departmentController : ApiController
    {
        [HttpGet]
        [Route("api/department")]
        public HttpResponseMessage getAllDepartments()
        {
            using (DepartmentEntities entities = new DepartmentEntities())
            {
                return Request.CreateResponse(HttpStatusCode.OK, entities.Departments.ToList());

            }
        }

        [HttpGet]
        [Route("api/department")]
        public HttpResponseMessage getDepartment(int dep_id)
        {
            using (DepartmentEntities entities = new DepartmentEntities())
            {
                var entity = entities.Departments.FirstOrDefault(D => D.Department_id == dep_id);

                if (entity != null)
                {
                    return Request.CreateResponse(HttpStatusCode.OK, entity);
                }
                else
                {
                    return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Can not Find This Department");
                }
            }
        }

        [HttpPost]
        [Route("api/department")]
        public HttpResponseMessage addDepartment([FromBody] Department dep)
        {
            try
            {
                using (DepartmentEntities entities = new DepartmentEntities())
                {
                    entities.Departments.Add(dep);
                    entities.SaveChanges();

                    var message = Request.CreateResponse(HttpStatusCode.Created, dep);
                    message.Headers.Location = new Uri(Request.RequestUri + dep.Department_id.ToString());
                    return message;

                }
            }
            catch (Exception ex)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ex);
            }
        }

        [HttpDelete]
        [Route("api/department")]
        public HttpResponseMessage removeDepartment(int dep_id)
        {
            try
            {
                using (DepartmentEntities entities = new DepartmentEntities())
                {
                    var entity = entities.Departments.FirstOrDefault(D => D.Department_id == dep_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot find This Department");
                    }
                    else
                    {
                        entities.Departments.Remove(entity);
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
        [Route("api/department")]
        public HttpResponseMessage editDepartment([FromBody] Department dep)
        {
            try
            {
                using (DepartmentEntities entities = new DepartmentEntities())
                {
                    var entity = entities.Departments.FirstOrDefault(D => D.Department_id == dep.Department_id);
                    if (entity == null)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Cannot Find This Department");
                    }
                    else
                    {
                        entity.Department_Name = dep.Department_Name;
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
    }
}
