using DapperExtensions;
using ez.Core;
using ez.Core.Authorization;
using ezModel.BaseModel;
using ezModel.DbModel;
using ezModel.Mapper;
using ezModel.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;

namespace ezLay.Areas.manage.Controllers
{
    [Area("manage")]
    public class privilegeController : baseController
    {
        private readonly IDatabase _database;

        public privilegeController(IDatabase database)
        {
            _database = database;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult AddPrivilege(int pid, string pname)
        {
            return View(new privilegeModel { pid = pid, pname = pname });
        }

        [HttpPost]
        public IActionResult AddPrivilege(privilegeModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(viewModel);
            }
            var result = _database.Insert<privilegeModel>(viewModel);
            ShowTipMessage(result > 0 ? LayerIconType.Success : LayerIconType.Erro, "", result > 0);
            return View(viewModel);
        }

        public IActionResult EditPrivilege(string id)
        {
            if (string.IsNullOrEmpty(id)) return View();
            var model = _database.Get<privilegeModel>(id);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditPrivilege(privilegeModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(viewModel);
            }
            var result = _database.UpdateSet<privilegeModel>(
                                                new
                                                {
                                                    viewModel.id,
                                                    viewModel.name,
                                                    viewModel.type,
                                                    viewModel.resource,
                                                    viewModel.remark,
                                                    viewModel.modifyon
                                                });

            ShowTipMessage(result ? LayerIconType.Success : LayerIconType.Erro, "", result);
            return View(viewModel);
        }

        public IActionResult DetailPrivilege(string id)
        {
            if (string.IsNullOrEmpty(id)) return View();
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(privilegeMapper);
            var model = _database.Get<privilegeModel>(id);
            return View("EditPrivilege", model);
        }

        [HttpPost]
        public JsonResult DeletePrivilege(string id)
        {
            int count = _database.GetSQLField<int>($"SELECT count(*) FROM privilege where pid={id}");
            if (count > 0)
                return Json(new AJaxResponse(false, "请先删除子集数据"));
            var result = _database.Delete<privilege>(Predicates.Field<privilege>(p => p.id, Operator.Eq, id));
            return Json(new AJaxResponse(result));
        }

        [HttpPost]
        [AllowUnauthorized]
        public JsonResult GetData(int page, int limit, string sortField, string sortType, string name, string index)
        {
            var pg = GetMainFilter(name, index);
            var result = _database.GetPages<privilege>(page - 1, limit, pg, new List<ISort> { Predicates.Sort<privilege>(sortField, sortType), Predicates.Sort<privilege>("modifyon", "desc") });
            return Json(new PageResponse(result.TotalItems, result.Items));
        }

        //获取过滤条件
        private PredicateGroup GetMainFilter(string name, string index)
        {
            var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            var pgA = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            if (!string.IsNullOrEmpty(name))
            {
                pgA.Predicates.Add(Predicates.Field<privilege>(f => f.name, Operator.Like, $"%{name}%"));
                pgA.Predicates.Add(Predicates.Field<privilege>(f => f.resource, Operator.Like, $"%{name}%"));
                pgA.Predicates.Add(Predicates.Field<privilege>(f => f.remark, Operator.Like, $"%{name}%"));
                pgMain.Predicates.Add(pgA);
            }

            var pgB = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            if (!string.IsNullOrEmpty(index))
            {
                pgB.Predicates.Add(Predicates.Field<privilege>(f => f.id, Operator.Eq, $"{index}"));
                pgB.Predicates.Add(Predicates.Field<privilege>(f => f.pid, Operator.Eq, $"{index}"));
            }
            pgMain.Predicates.Add(pgB);

            return pgMain;
        }

        [HttpGet]
        [AllowUnauthorized]
        public JsonResult GetTreeData()
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(privilegeMapper);
            var allData = _database.GetList<privilegeModel>().ToList();
            var treeData = ToTree(allData);
            return Json(treeData);
        }

        //多级无限节点
        private List<privilegeModel> ToTree(List<privilegeModel> allList)
        {
            var dic = new Dictionary<int, privilegeModel>(allList.Count);
            foreach (var chapter in allList)
                dic.Add(chapter.id, chapter);
            foreach (var chapter in dic.Values)
                if (chapter.pid > 0)
                    if (dic.ContainsKey(chapter.pid))
                    {
                        if (dic[chapter.pid].children == null)
                            dic[chapter.pid].children = new List<privilegeModel>();
                        dic[chapter.pid].children.Add(chapter);
                    }
            //根节点条件
            return dic.Values.Where(t => t.pid == 0).ToList();
        }
    }
}