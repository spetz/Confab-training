﻿using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Confab.Shared.Abstractions.Commands;
using Confab.Shared.Abstractions.Messaging;
using Confab.Shared.Abstractions.Modules;

namespace Confab.Shared.Infrastructure.Modules
{
    internal sealed class ModuleClient : IModuleClient
    {
        private readonly ConcurrentDictionary<Type, MessageAttribute> _messageRegistrations = new();
        private readonly IModuleRegistry _moduleRegistry;
        private readonly IModuleSerializer _moduleSerializer;

        public ModuleClient(IModuleRegistry moduleRegistry, IModuleSerializer moduleSerializer)
        {
            _moduleRegistry = moduleRegistry;
            _moduleSerializer = moduleSerializer;
        }

        public async Task<TResult> SendAsync<TResult>(string path, object request) where TResult : class
        {
            var registration = _moduleRegistry.GetRequestRegistration(path);
            if (registration is null)
            {
                throw new InvalidOperationException($"No action has been defined for path: '{path}'.");
            }

            var receiverRequest = TranslateType(request, registration.RequestType);
            var result = await registration.Action(receiverRequest);

            return result is null ? null : TranslateType<TResult>(result);
        }

        public async Task PublishAsync(object message)
        {
            var module = message.GetModuleName();
            var key = message.GetType().Name;
            var registrations = _moduleRegistry
                .GetBroadcastRegistrations(key)
                .Where(r => r.ReceiverType != message.GetType());

            var tasks = new List<Task>();
            
            foreach (var registration in registrations)
            {
                if (!_messageRegistrations.TryGetValue(registration.ReceiverType, out var messageAttribute))
                {
                    messageAttribute = registration.ReceiverType.GetCustomAttribute<MessageAttribute>();
                    if (message is ICommand)
                    {
                        messageAttribute = message.GetType().GetCustomAttribute<MessageAttribute>();
                        module = registration.ReceiverType.GetModuleName();
                    }

                    if (messageAttribute is not null)
                    {
                        _messageRegistrations.TryAdd(registration.ReceiverType, messageAttribute);
                    }
                }

                if (messageAttribute is not null && !string.IsNullOrWhiteSpace(messageAttribute.Module) &&
                    (!messageAttribute.Enabled || messageAttribute.Module != module))
                {
                    continue;
                }
                
                var action = registration.Action;
                var receiverMessage = TranslateType(message, registration.ReceiverType);
                tasks.Add(action(receiverMessage));
            }

            await Task.WhenAll(tasks);
        }

        private T TranslateType<T>(object value)
            => _moduleSerializer.Deserialize<T>(_moduleSerializer.Serialize(value));
        
        private object TranslateType(object value, Type type)
            => _moduleSerializer.Deserialize(_moduleSerializer.Serialize(value), type);
    }
}