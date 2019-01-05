using DapperExtensions;
using ezModel.Mapper;
using ezModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ezLay.Controllers
{
    public class SelectController : Controller
    {
        private readonly IDatabase _database;

        public SelectController(IDatabase database)
        {
            _database = database;
        }

        #region 字典查询

        //获取扩展字典
        [HttpPost]
        public JsonResult GetExtDictionary(string code)
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var dictResult = _database.Get<dictionaryModel>(Predicates.Field<dictionaryModel>(f => f.code, Operator.Eq, code), true);
            var list = _database.GetList<dictionaryModel>(Predicates.Field<dictionaryModel>(f => f.pid, Operator.Eq, dictResult.id),
                                                        new List<ISort> { Predicates.Sort<dictionaryModel>("modifyon", "desc") });
            foreach (var item in list)
                item.exts = _database.GetList<dictionaryModel>(Predicates.Field<dictionaryModel>(f => f.id, Operator.Eq, item.id)).ToList();
            return Json(list);
        }

        //获取普通字典
        [HttpPost]
        public JsonResult GetDictionary(string Code)
        {
            var result = SelectDictionary(Code);
            return Json(result);
        }


        public IEnumerable<dictionaryModel> SelectDictionary(string Code)
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var dictResult = _database.Get<dictionaryModel>(Predicates.Field<dictionaryModel>(f => f.code, Operator.Eq, Code), true);
            var list = _database.GetList<dictionaryModel>(Predicates.Field<dictionaryModel>(f => f.pid, Operator.Eq, dictResult.id),
                                                    new List<ISort> { Predicates.Sort<dictionaryModel>("modifyon", "desc") });
            return list;
        }

        #endregion
    }
}
