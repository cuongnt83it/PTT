﻿using PTT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace PTT.Controllers
{
    [AuthorizeBusiness]
    public class ContentController : ApiController
    {
        // GET: api/Content
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Content/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Content
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Content/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Content/5
        public void Delete(int id)
        {
        }
    }
}
