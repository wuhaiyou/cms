﻿using System;
using System.Web.Http;
using SiteServer.API.Common;
using SiteServer.CMS.Core;
using SiteServer.CMS.DataCache;

namespace SiteServer.API.Controllers.Pages.Settings
{
    [RoutePrefix("pages/settings/adminPassword")]
    public class PagesAdminPasswordController : ControllerBase
    {
        private const string Route = "";

        [HttpGet, Route(Route)]
        public IHttpActionResult Get()
        {
            try
            {
                var request = GetRequest();
                var userId = request.GetQueryInt("userId");
                if (!request.IsAdminLoggin) return Unauthorized();
                var adminInfo = AdminManager.GetAdminInfoByUserId(userId);
                if (adminInfo == null) return NotFound();
                if (request.AdminId != userId &&
                    !request.AdminPermissionsImpl.HasSystemPermissions(ConfigManager.SettingsPermissions.Admin))
                {
                    return Unauthorized();
                }

                return Ok(new
                {
                    Value = adminInfo
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }

        [HttpPost, Route(Route)]
        public IHttpActionResult Submit()
        {
            try
            {
                var request = GetRequest();
                var userId = request.GetQueryInt("userId");
                if (!request.IsAdminLoggin) return Unauthorized();
                var adminInfo = AdminManager.GetAdminInfoByUserId(userId);
                if (adminInfo == null) return NotFound();
                if (request.AdminId != userId &&
                    !request.AdminPermissionsImpl.HasSystemPermissions(ConfigManager.SettingsPermissions.Admin))
                {
                    return Unauthorized();
                }

                var password = request.GetPostString("password");

                if (!DataProvider.AdministratorDao.ChangePassword(adminInfo, password, out var errorMessage))
                {
                    return BadRequest($"更改密码失败：{errorMessage}");
                }

                request.AddAdminLog("重设管理员密码", $"管理员:{adminInfo.UserName}");

                return Ok(new
                {
                    Value = true
                });
            }
            catch (Exception ex)
            {
                return InternalServerError(ex);
            }
        }
    }
}
