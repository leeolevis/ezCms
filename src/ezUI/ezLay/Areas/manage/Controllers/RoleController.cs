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

namespace ezLay.Areas.manage.Controllers
{
    [Area("manage")]
    public class roleController : baseController
    {
        private readonly IAuthorizationProvider _authorization;
        private readonly IDistributedCache _cache;
        private readonly IDatabase _database;

        public roleController(IDatabase database, IAuthorizationProvider authorization, IDistributedCache cache)
        {
            _cache = cache;
            _database = database;
            _authorization = authorization;
        }

        public IActionResult Index()
        {
            //List<roleModel> roles = new List<roleModel>();
            //for (int i = 0; i <=10; i++)
            //{
            //    roles.Add(new roleModel() { Title = $"LHS批量测试{i}" });
            //}
            //_database.Insert<roleModel>(roles);
            return View();
        }

        [HttpGet]
        public IActionResult AddRole()
        {
            var role = new roleModel();
            SeedPermissions(role);

            return View(role);
        }

        [HttpPost]
        public IActionResult AddRole(roleModel role)
        {
            if (!ModelState.IsValid)
            {
                SeedPermissions(role);
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(role);
            }
            //事务操作
            var selectIds = role.Permissions.SelectedIds;
            SeedPermissions(role);
            int roleId = 0;
            try
            {
                DapperExtensions.DapperExtensions.DefaultMapper = typeof(roleMapper);

                List<roleprivilege> roles = new List<roleprivilege>();
                roleId = _database.Insert<roleModel>(role);
                foreach (var item in selectIds)
                    roles.Add(new roleprivilege { roleid = roleId, privilegeid = item });
                _database.Insert<roleprivilege>(roles);

                ShowTipMessage(LayerIconType.Success);
                _authorization.Refresh(ez.Core.Helpers.Convert.ToString(roleId));
                _cache.Remove(_cacheRoleList);
            }
            catch (Exception e)
            {
                ShowTipMessage(LayerIconType.Erro, "", false);
            }
            return View(role);
        }

        [HttpGet]
        public IActionResult EditRole(string id)
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(roleMapper);
            var model = _database.Get<roleModel>(id);
            if (model != null)
            {
                //List<int> Permissions = new List<int>();
                //var PermissionsString = _database.GetListSQL<string>($"select privilegeid from roleprivilege where roleid={id}");
                //if (PermissionsString != null) Permissions = PermissionsString.Cast<int>().ToList();
                //model.Permissions.SelectedIds = Permissions;

                model.Permissions.SelectedIds = _database.GetList<roleprivilege>(Predicates.Field<roleprivilege>(f => f.roleid, Operator.Eq, id))
                                                .Select(x => x.privilegeid).ToList();
            }
            SeedPermissions(model);
            return View(model);
        }

        [HttpPost]
        public IActionResult EditRole(roleModel role)
        {
            var rolePer = role.Permissions;
            if (!ModelState.IsValid)
            {
                SeedPermissions(role);
                ShowTipMessage(LayerIconType.Sigh, "", false);
                return View(role);
            }

            var selectIds = role.Permissions.SelectedIds;
            SeedPermissions(role);
            try
            {
                List<roleprivilege> roles = new List<roleprivilege>();
                _database.RunInTransaction(() =>
                {
                    //更新角色信息
                    DapperExtensions.DapperExtensions.DefaultMapper = typeof(roleMapper);
                    var reUpdate = _database.Update(
                        new roleModel { name = role.name, defaulturl = role.defaulturl, remark = role.remark },
                        Predicates.Field<roleModel>(t => t.id, Operator.Eq, role.id));

                    //删除老权限
                    var result =
                    _database.Delete<roleprivilege>(Predicates.Field<roleprivilege>(t => t.roleid, Operator.Eq, role.id));

                    //重新添加权限点
                    foreach (var item in selectIds)
                        roles.Add(new roleprivilege { roleid = role.id, privilegeid = item });
                    _database.Insert<roleprivilege>(roles);
                });
                ShowTipMessage(LayerIconType.Success);
                _authorization.Refresh(role.id.ToString());
                _cache.Remove(_cacheRoleList);
            }
            catch (Exception e)
            {
                ShowTipMessage(LayerIconType.Erro, "", false);
            }

            return View(role);
        }

        [HttpGet]
        public IActionResult DetailRole(string id)
        {
            DapperExtensions.DapperExtensions.DefaultMapper = typeof(roleMapper);
            var model = _database.Get<roleModel>(id);
            if (model != null)
                model.Permissions.SelectedIds = _database.GetList<roleprivilege>(Predicates.Field<roleprivilege>(f => f.roleid, Operator.Eq, id))
                                                .Select(x => x.privilegeid).ToList();
            SeedPermissions(model);
            return View("EditRole", model);
        }


        [HttpPost]
        public JsonResult DeleteRole(string id)
        {
            var result = _database.Delete<role>(Predicates.Field<role>(p => p.id, Operator.Eq, id));
            return Json(new AJaxResponse(result));
        }

        private IEnumerable<privilege> GetAllPermissions()
        {
            var result = _database.GetList<privilege>();
            return result;
        }

        private void SeedPermissions(roleModel viewModel)
        {
            var root = new JsTreeNode();
            var allList = GetAllPermissions();
            var tree = new JsTree
            {
                SelectedIds = viewModel.Permissions.SelectedIds,
                Nodes = ToTree(allList.ToList())
            };
            viewModel.Permissions = tree;
        }


        //多级无限节点
        private List<JsTreeNode> ToTree(List<privilege> allList)
        {
            var dic = new Dictionary<int, JsTreeNode>(allList.Count);
            foreach (var chapter in allList)
                dic.Add(chapter.id, new JsTreeNode(chapter.id, chapter.name, chapter.pid.Value));
            foreach (var chapter in dic.Values)
                if (chapter.ParentId > 0)
                    if (dic.ContainsKey(chapter.ParentId))
                    {
                        if (dic[chapter.ParentId].Nodes == null)
                            dic[chapter.ParentId].Nodes = new List<JsTreeNode>();
                        dic[chapter.ParentId].Nodes.Add(chapter);
                    }
            //根节点条件
            return dic.Values.Where(t => t.ParentId == 0).ToList();
        }

        [HttpPost]
        [AllowUnauthorized]
        public JsonResult GetData(int page, int limit, string sortField, string sortType, string name, string index)
        {
            var pg = GetMainFilter(name, index);
            var result = _database.GetPages<role>(page - 1, limit, pg, new List<ISort> { Predicates.Sort<role>(sortField, sortType), Predicates.Sort<privilege>("modifyon", "desc") });
            return Json(new PageResponse(result.TotalItems, result.Items));
        }

        //获取过滤条件
        private PredicateGroup GetMainFilter(string name, string index)
        {
            var pgMain = new PredicateGroup { Operator = GroupOperator.And, Predicates = new List<IPredicate>() };

            var pgA = new PredicateGroup { Operator = GroupOperator.Or, Predicates = new List<IPredicate>() };
            if (!string.IsNullOrEmpty(name))
            {
                pgA.Predicates.Add(Predicates.Field<role>(f => f.name, Operator.Like, $"%{name}%"));
                pgA.Predicates.Add(Predicates.Field<role>(f => f.remark, Operator.Like, $"%{name}%"));
                pgMain.Predicates.Add(pgA);
            }
            return pgMain;
        }
    }
}