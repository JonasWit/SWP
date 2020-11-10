﻿using Microsoft.Extensions.DependencyInjection;
using SWP.Application.Log;
using SWP.UI.BlazorApp.AdminApp.Stores.Application;
using SWP.UI.BlazorApp.AdminApp.Stores.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SWP.UI.BlazorApp.AdminApp.Stores.Error
{
    public class ErrorState
    {
        public bool Loading { get; set; } = false;
        public string LoadingMessage { get; set; }
        public Exception Exception { get; set; }
    }

    [UIScopedService]
    public class ErrorStore : StoreBase
    {
        private readonly ErrorState _state;
        private readonly CreateLogRecord _createLogRecord;

        public ErrorState GetState() => _state;

        public ErrorStore(IServiceProvider serviceProvider, CreateLogRecord createLogRecord) : base(serviceProvider)
        {
            _state = new ErrorState();
            _createLogRecord = createLogRecord;
        }

        public Task SetException(Exception ex, string userId)
        {
            _state.Exception = ex;
            return _createLogRecord.Create(new CreateLogRecord.Request
            {
                Message = ex.Message,
                UserId = userId,
                StackTrace = ex.StackTrace
            });
        }
    }
}