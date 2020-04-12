using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Ally.Core.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Quartz;
using Quartz.Spi;

namespace Ally.Quartz
{
    public class QuartzConfigurator
    {
        public IServiceCollection Services { get; }
        private NameValueCollection Properties { get; set; }
        private ServiceDescriptor JobFactoryRegistration { get; set; } = new ServiceDescriptor(typeof(IJobFactory), typeof(JobFactory));

        internal QuartzConfigurator(IServiceCollection services)
        {
            Services = services;
        }

        public QuartzConfigurator WithProperties(Dictionary<string, string> properties)
        {
            Properties = properties.ToNameValueCollection();
            return this;
        }
        
        public QuartzConfigurator WithProperties(NameValueCollection properties)
        {
            Properties = properties;
            return this;
        }

        public QuartzConfigurator WithSchedulerListener<TSchedulerLister>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TSchedulerLister : ISchedulerListener
        {
            return WithSchedulerListener(typeof(TSchedulerLister), serviceLifetime);
        }

        public QuartzConfigurator WithTriggerListener<TTriggerListener>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TTriggerListener: ITriggerListener
        {
            return WithTriggerListener(typeof(TTriggerListener), serviceLifetime);
        }
        
        public QuartzConfigurator WithJobListener<TJobListener>(ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
            where TJobListener: IJobListener
        {
            return WithJobListener(typeof(TJobListener), serviceLifetime);
        }
        
        
        public QuartzConfigurator WithSchedulerListener(Type schedulerListener, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            Services.Add(new ServiceDescriptor(typeof(ISchedulerListener),schedulerListener, serviceLifetime));
            return this;
        }
        
        public QuartzConfigurator WithTriggerListener(Type triggerListener, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            Services.Add(new ServiceDescriptor(typeof(ITriggerListener),triggerListener, serviceLifetime));
            return this;
        }
        
        public QuartzConfigurator WithJobListener(Type jobListener, ServiceLifetime serviceLifetime = ServiceLifetime.Singleton)
        {
            Services.Add(new ServiceDescriptor(typeof(IJobListener),jobListener, serviceLifetime));
            return this;
        }

        public QuartzConfigurator WithJobFactory<TJobFactory>()
            where TJobFactory: IJobFactory
        {
            JobFactoryRegistration = new ServiceDescriptor(typeof(IJobFactory), typeof(TJobFactory));
            return this;
        }

        public QuartzConfigurator WithJob<TJob>()
            where TJob: IJob
        {
            Services.Add(new ServiceDescriptor(typeof(IJob), typeof(TJob), ServiceLifetime.Transient));
            return this;
        }

        internal void Configure()
        {
            Services.Add(JobFactoryRegistration);
        }
    }
}