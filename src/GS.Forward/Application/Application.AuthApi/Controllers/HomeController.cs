using IdentityServer4.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Application.AuthApi.Controllers
{
    [ApiController]
    [Route("")]
    public class HomeController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public object Index()
        {

            Microsoft.AspNetCore.Http.ConnectionInfo connection = HttpContext.Connection;

            return User.Claims.Select(u => new { u.Type, u.Value }).ToArray();
        }

        [HttpPost]
        public Info Tets([FromBody] Info data)
        {
            return data;
        }



    }
    [ApiController]
    [Route("Test")]
    [CusFilter]
    [OtherFilter]
    public class TestController : ControllerBase
    {

        [Authorize]
        [HttpGet]
        public object Index()
        {

            Microsoft.AspNetCore.Http.ConnectionInfo connection = HttpContext.Connection;

            return User.Claims.Select(u => new { u.Type, u.Value }).ToArray();
        }

        [HttpPost]
        public Info Tets([FromBody] Info data)
        {
            return data;
        }



    }

    public class OtherFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class CusFilterAttribute : Attribute, IActionFilter
    {
        public void OnActionExecuted(ActionExecutedContext context)
        {
            throw new NotImplementedException();
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            throw new NotImplementedException();
        }
    }

    public class LogFlagAttribute : ServiceFilterAttribute
    {
        public LogFlagAttribute() : base(typeof(CusFilterAttribute))
        {
        }
    }

    public class CollectionRequiredAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return false;
            }
            if (value is ICollection collection)
            {
                if (collection.Count == 0) return false;
            }
            return true;
        }

        public override string FormatErrorMessage(string name)
        {
            return base.FormatErrorMessage(name);
        }
    }

    public class MustDefinedWithPrevAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证的前提条件列-bool
        /// </summary>
        public string PrevCol { get; set; }

        public Type EnumType { get; set; }

        public MustDefinedWithPrevAttribute(string prevCol, string errorMessage, Type enumType)
        {
            PrevCol = prevCol;
            ErrorMessage = errorMessage;
            EnumType = enumType;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var type = validationContext.ObjectInstance.GetType();

            if (type.GetProperty(PrevCol).GetValue(validationContext.ObjectInstance) is bool flag && flag)
            {
                if(value == null)
                    return new ValidationResult(ErrorMessage);
            }
            return null;
        }
    }

    /// <summary>
    /// 带前提条件的必须定义
    /// </summary>
    public class MustDefinedWithPrevConditionAttribute : ValidationAttribute
    {
        /// <summary>
        /// 验证的前提条件列-bool
        /// </summary>
        public string PrevCol { get; set; }

        public Type EnumType { get; set; }

        public object DefaultValue { get; set; }

        public MustDefinedWithPrevConditionAttribute(string prevCol, string errorMessage, Type enumType, object defaultValue)
        {
            PrevCol = prevCol;
            ErrorMessage = errorMessage;
            EnumType = enumType;
            DefaultValue = defaultValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {

            var type = validationContext.ObjectInstance.GetType();

            if (type.GetProperty(PrevCol).GetValue(validationContext.ObjectInstance) is bool flag && flag)
            {
                if (value != null && value is int num && Enum.IsDefined(EnumType, num))
                {
                    return null;
                }
                return new ValidationResult(ErrorMessage);
            }
            else
            {
                validationContext.ObjectInstance.GetType().GetProperty(validationContext.MemberName).SetValue(validationContext.ObjectInstance, DefaultValue);
            }

            return null;
        }
    }

    public enum Source
    {
        PC = 1,
        Mobile = 2
    }

    public class Info
    {
        [MustDefinedWithPrevCondition(nameof(Record), "来源类型异常", typeof(Source), 1)]
        public Source Source { get; set; }
        public bool Record { get; set; }
        [Required]
        public string Data { get; set; }

        [CollectionRequired]
        public string[] Arr { get; set; }

    }

    public class Data
    {
        public int Id { get; set; }
    }

}
