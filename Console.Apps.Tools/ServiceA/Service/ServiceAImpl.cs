﻿using OpenTracing;
using System;
using System.Threading.Tasks;

namespace ServiceA.Service
{
    public class ServiceAImpl : IServiceA
    {
        private ITracer Tracer;

        public ServiceAImpl(ITracer tracer)
        {
            Tracer = tracer;
        }

        public async Task<string> GetServiceAMessageAsync(string id)
        {

            using (Tracer.BuildSpan("GetServiceAMessageAsync-ActiveSpan2-" + id).StartActive(true))
            {
                Tracer.ActiveSpan.Log("ServiceAImpl before calling inner method");
                await Task.Delay(TimeSpan.FromSeconds(5));
                using (Tracer.BuildSpan("GetServiceAMessageAsync-ActiveSpan3-" + id).StartActive(true))
                {
                    await InnerMethod1(id);
                }
                Tracer.ActiveSpan.Log("ServiceAImpl before calling inner method but within");
            }

            Tracer.ActiveSpan.Log("Out side Using Blocks -" + id);
            return "Waited";
        }

        private async Task<string> InnerMethod1(string id)
        {

            using (Tracer.BuildSpan("InnerMethod1--ActiveSpan4-" + id).StartActive(true))
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                using (Tracer.BuildSpan("InnerMethod1-ActiveSpan5-" + id).StartActive(true))
                {
                    await InnerMethod2(id);
                }
                Tracer.ActiveSpan.Log("InnerMethod1 method");
            }

            Tracer.ActiveSpan.Log("Out side Using Blocks -" + id);
            return "Waited";
        }

        private async Task<string> InnerMethod2(string id)
        {

            using (Tracer.BuildSpan("InnerMethod2-ActiveSpan6-" + id).StartActive(true))
            {
                await Task.Delay(TimeSpan.FromSeconds(5));
                Tracer.ActiveSpan.Log("InnerMethod2 method");
            }

            Tracer.ActiveSpan.Log("Out side Using Blocks -" + id);
            return "Waited";
        }
    }
}
