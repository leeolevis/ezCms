using DapperExtensions;
using ez.Core;
using ez.Core.Authorization;
using ezModel.BaseModel;
using ezModel.DbModel;
using ezModel.Mapper;
using ezModel.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ezLay.Areas.manage.Controllers
{
    [Area("manage")]
    public class dictionaryController : baseController
    {
        private readonly IDatabase _database;
        private readonly IDistributedCache _cache;

        public dictionaryController(IDatabase database, IDistributedCache cache)
        {
            _database = database;
            _cache = cache;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowUnauthorized]
        public JsonResult GetData(int page, int limit, string sortField, string sortType, string name, string index)
        {
            var pg = GetMainFilter(name, index);
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var result = _database.GetPages<dictionaryModel>(page - 1, limit, pg,
                new List<ISort> { Predicates.Sort<dictionaryModel>(sortField, sortType), Predicates.Sort<dictionaryModel>("modifyon", "desc") });
            return Json(new PageResponse(result.TotalItems, result.Items));
        }

        public IActionResult AddDictionary(int pid, string pname)
        {
            return View(new dictionaryModel { pid = pid, pname = pname });
        }

        [HttpPost]
        public IActionResult AddDictionary(dictionaryModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(viewModel);
            }
            if (string.IsNullOrEmpty(viewModel.type)) viewModel.pid = 0;
            var result = _database.Insert<dictionaryModel>(viewModel);
            if (result > 0)
            {
                ShowTipMessage(LayerIconType.Success, "", true);
                _cache.Remove(_cacheDictList);
            }
            else
            {
                ShowTipMessage(LayerIconType.Erro, "", false);
            }
            return View(viewModel);
        }

        public IActionResult EditDictionary(string id)
        {
            if (string.IsNullOrEmpty(id)) return View(new dictionaryModel());
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var model = _database.Get<dictionaryModel>(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditDictionary(dictionaryModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(viewModel);
            }

            try
            {
                //事务操作
                _database.RunInTransaction(() =>
                {
                    //更新信息,不更新pid和type
                    _database.UpdateSet<dictionaryModel>(
                        new
                        {
                            viewModel.id,
                            viewModel.name,
                            viewModel.val,
                            viewModel.code,
                            viewModel.remark,
                            viewModel.isystem,
                            viewModel.modifyon
                        });

                    //更新子类类型
                    _database.UpdateSet<dictionaryModel>(new { type = viewModel.name },
                        Predicates.Field<dictionaryModel>(f => f.pid, Operator.Eq, viewModel.id));
                });
                ShowTipMessage(LayerIconType.Success);
                _cache.Remove(_cacheDictList);
            }
            catch
            {
                ShowTipMessage(LayerIconType.Erro, "", false);
            }
            return View(viewModel);
        }

        public IActionResult DetailDictionary(string id)
        {
            if (string.IsNullOrEmpty(id)) return View(new dictionaryModel());
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var model = _database.Get<dictionaryModel>(id);
            return View("EditDictionary", model);
        }

        [HttpPost]
        public JsonResult DeleteDictionary(string id)
        {
            int count = _database.GetSQLField<int>($"SELECT count(*) FROM dictionary where pid={id}");
            if (count > 0)
                return Json(new AJaxResponse(false, "请先删除子集数据"));
            var result = _database.Delete<dictionary>(Predicates.Field<dictionary>(p => p.id, Operator.Eq, id));
            return Json(new AJaxResponse(result));
        }

        [HttpGet]
        [AllowUnauthorized]
        public JsonResult GetTreeData()
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(dictionaryMapper);
            var allData = _database.GetList<dictionaryModel>().ToList();
            var treeData = ToTree(allData);
            return Json(treeData);
        }

        //多级无限节点
        private List<dictionaryModel> ToTree(List<dictionaryModel> allList)
        {
            var dic = new Dictionary<int, dictionaryModel>(allList.Count);
            foreach (var chapter in allList)
                dic.Add(chapter.id, chapter);
            foreach (var chapter in dic.Values)
                if (chapter.pid != 0)
                    if (dic.ContainsKey(chapter.pid))
                    {
                        if (dic[chapter.pid].children == null)
                            dic[chapter.pid].children = new List<dictionaryModel>();
                        dic[chapter.pid].children.Add(chapter);
                    }
            //根节点条件
            return dic.Values.Where(t => t.pid == 0).ToList();
        }

        //获取过滤条件
        private PredicateGroup GetMainFilter(string name, string index)
        {
            var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            var pgA = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            if (!string.IsNullOrEmpty(name))
            {
                pgA.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.name, Operator.Like, $"%{name}%"));
                pgA.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.val, Operator.Like, $"%{name}%"));
                pgA.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.type, Operator.Like, $"%{name}%"));
            }
            pgMain.Predicates.Add(pgA);

            var pgB = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            if (!string.IsNullOrEmpty(index))
            {
                //判断是否还有子节点，有：显示子节点，没有：显示自己
                var res = _database.GetSQLField<int>($"select count(1) from dictionary where pid={index} ");
                if (res == 0)
                    pgB.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.id, Operator.Eq, $"{index}"));
                else
                    pgB.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.pid, Operator.Eq, $"{index}"));
            }
            else
                pgB.Predicates.Add(Predicates.Field<dictionaryModel>(f => f.code, Operator.Eq, $"", true));

            pgMain.Predicates.Add(pgB);

            return pgMain;
        }
    }
}
