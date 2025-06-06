﻿using System;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using WebApiClientCore;
using WebApiClientCore.Attributes;

namespace Ray.BiliBiliTool.Agent.Attributes;

public class LogFilterAttribute(bool logError = true) : LoggingFilterAttribute
{
    protected override Task WriteLogAsync(ApiResponseContext context, LogMessage logMessage)
    {
        var loggerFactory = context.HttpContext.ServiceProvider.GetService<ILoggerFactory>();
        if (loggerFactory == null)
        {
            return Task.CompletedTask;
        }

        MethodInfo member = context.ApiAction.Member;
        var strArray = new string[5];
        Type declaringType1 = member.DeclaringType;
        strArray[0] = (object)declaringType1 != null ? declaringType1.Namespace : null;
        strArray[1] = ".";
        Type declaringType2 = member.DeclaringType;
        strArray[2] = (object)declaringType2 != null ? declaringType2.Name : null;
        strArray[3] = ".";
        strArray[4] = member.Name;
        string categoryName = string.Concat(strArray);
        ILogger logger = loggerFactory.CreateLogger(categoryName);

        if (logMessage.Exception == null)
        {
            logger.LogDebug(logMessage.ToString());
        }
        else
        {
            if (logError)
                logger.LogError(logMessage.ToString());
            else
                logger.LogDebug(logMessage.ToString());
        }

        return Task.CompletedTask;
    }
}
