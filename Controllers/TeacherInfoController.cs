﻿    using Grpc.Core;
using Microsoft.AspNetCore.Mvc;
using Suai.Bot.TeacherInfo.Proto;
using suai_api.Domain.Timetable.Exceptions;
using suai_api.Models.TeacherInfo;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;
using System.Web;

namespace suai_api.Controllers;

[ApiController()]
[Route("[controller].[action]")]
public class TeacherInfoController : ControllerBase 
{
    private readonly ILogger<TeacherInfoController> _logger;
    private readonly ITeacherInfoProvider _teacherInfoProvider;

    public TeacherInfoController(ILogger<TeacherInfoController> logger, ITeacherInfoProvider teacherInfoProvider)
    {
        _logger = logger;
        _teacherInfoProvider = teacherInfoProvider;
    }

    [HttpGet]
    [ActionName("get")]
    public IActionResult GetTeacherInfo(string lastName) 
    {
        TeacherInfoReply result;
        _logger.Log(LogLevel.Warning, lastName);
        try
        {
            result = _teacherInfoProvider.GetTeacherInfo(lastName);
        }
        // Если сервис недоступен
        catch (ServiceUnavailableException)
        {
            _logger.Log(LogLevel.Warning, "TeacherInfo service is unavailable");
            return StatusCode(503);
        }
        // Ошибка процедуры (но сервис доступен)
        catch (RpcException ex)
        {
            _logger.LogError(ex, "TeacherInfo service error");
            return ex.StatusCode switch
            {
                Grpc.Core.StatusCode.NotFound => StatusCode(404),
                _ => StatusCode(500),
            };
        }
        return new JsonResult(result, new JsonSerializerOptions 
        {
            Encoder = JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic),
            WriteIndented = true
        });
    }
}