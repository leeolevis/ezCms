using DapperExtensions;
using ezModel.DbModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ezLay.Controllers
{
    public class VerifyController : Controller
    {
        private readonly IDatabase _database;

        public VerifyController(IDatabase database)
        {
            _database = database;
        }

        //字典Code唯一性验证
        public JsonResult ExistDictCode(string code, string ccode)
        {
            if (code == ccode) return Json(true);
            var result = _database.Get<dictionary>(Predicates.Field<dictionary>(f => f.code, Operator.Eq, code), true);
            return Json(result == null);
        }

        //判断角色唯一性验证
        public JsonResult ExistRoleName(string name, string cname)
        {
            if (name == cname) return Json(true);
            var result = _database.Get<role>(Predicates.Field<role>(f => f.name, Operator.Eq, name), true);
            return Json(result == null);
        }
    }
}
